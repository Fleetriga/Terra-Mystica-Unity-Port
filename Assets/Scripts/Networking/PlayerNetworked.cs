using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerNetworked : NetworkBehaviour {

    public GameObject playerGameObject;
    public GameObject dropDown;

    //Turn Controlling. Only set on host
    public int CurrentPlayer;

    [SyncVar]
    public int Player_ID;
    [SyncVar]
    public bool isHost;
    [SyncVar]
    public Faction.Faction_Type p_faction;

    // Use this for initialization
    void Start() {
        DontDestroyOnLoad(gameObject);

        if (isHost) //New players identify HostPlayer
        {
            name = "HostPlayer";
        }
        if (!isLocalPlayer)
        {
            return;
        }
        if (isServer) // player object becomes TurnController for everyone;
        {
            Cmd_SetHost();
        }

        //Spawn the dropdown and turn controller
        Cmd_SpawnDropdown();
    }

    void Cmd_AddPlayerStatus()
    {
        Rpc_AddPlayerStatus();
    }
    void Rpc_AddPlayerStatus()
    {
        gameObject.AddComponent<PlayerStatus>();
    }

    void Update()
    {
        //If you are the host, scan other players and yourself for turn changes
        if (isHost && hasAuthority && GetComponent<TurnController>() != null)
        {
            if (GetComponent<TurnController>().ScanForTurnChanges()) { Cmd_SetCurrentPlayer(GetComponent<TurnController>().CurrentPlayersID()); }
        }
    }

    //Host methods
    [Command]
    void Cmd_SetCurrentPlayer(int newCurrentPlayersId)
    {
        CurrentPlayer = newCurrentPlayersId;
        Rpc_SetCurrentPlayer(newCurrentPlayersId);
    }
    [ClientRpc]
    void Rpc_SetCurrentPlayer(int newCurrentPlayersId)
    {
        CurrentPlayer = newCurrentPlayersId;
    }
    [Command]
    void Cmd_SetHost()
    {
        name = "HostPlayer";
        isHost = true;
    }

    void OnLevelWasLoaded()
    {
        if (hasAuthority)
        {
            GameObject go = Instantiate(playerGameObject);
            go.GetComponent<Player>().SetUp(p_faction, Player_ID, gameObject);
            GameObject.Find("Controller").GetComponent<GameController>().Start_Game();
        }
    }

    public string Set_Faction(Faction.Faction_Type i)
    {
        Cmd_SetFaction(i);
        return Faction.GetFaction(i);
    }
    [Command]
    public void Cmd_SetFaction(Faction.Faction_Type i)
    {
        p_faction = i;
    }

    [Command]
    void Cmd_SpawnDropdown()
    {
        GameObject go = Instantiate(dropDown);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        go.GetComponent<DropDownMenu>().Cmd_SetOwner(gameObject);
        Rpc_SetIDMoveDropDown(-100, go);
    }
    [ClientRpc]
    void Rpc_SetIDMoveDropDown(int yDifference, GameObject go)
    {
        Player_ID = GameObject.FindGameObjectsWithTag("Player_Networked_Object").Length - 1;
        go.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(0, yDifference * Player_ID);
    }

    public void StartGamePreparations()
    {
        if (!hasAuthority || !isHost) { return; } //If the player is not the host
        gameObject.AddComponent<TurnController>();
        GetComponent<TurnController>().SetUp(NetworkServer.connections.Count);
        Rpc_ChangeScene("GameScene");
    }
    [ClientRpc]
    void Rpc_ChangeScene(string scenename)
    {
        SceneManager.LoadSceneAsync(scenename);
    }

    public Material GetFactionMaterial()
    {
        return Faction.GetFactionMaterial(p_faction);
    }
}
