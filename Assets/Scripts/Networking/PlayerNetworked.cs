using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerNetworked : NetworkBehaviour {

    public GameObject playerGameObject;
    public GameObject dropDown;
    public GameObject turnControllerPrefab;

    [SyncVar]
    public int PlayerID;
    [SyncVar]
    public bool isHost;
    [SyncVar]
    public Faction.FactionType p_faction;

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
            Cmd_SpawnTurnController();
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

    //Host methods
    [Command]
    void Cmd_SetHost()
    {
        name = "HostPlayer";
        isHost = true;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (hasAuthority)
        {
            GetComponent<PlayerStatus>().EnableStatusCheck();
            GameObject go = Instantiate(playerGameObject);
            go.GetComponent<Player>().SetUp(p_faction, PlayerID, gameObject);
            GameObject.Find("Controller").GetComponent<GameController>().StartGame();
        }
    }

    public string Set_Faction(Faction.FactionType i)
    {
        Cmd_SetFaction(i);
        return Faction.GetFaction(i);
    }
    [Command]
    public void Cmd_SetFaction(Faction.FactionType i)
    {
        p_faction = i;
    }

    [Command]
    void Cmd_SpawnTurnController()
    {
        GameObject go = Instantiate(turnControllerPrefab);
        NetworkServer.Spawn(go);
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
        PlayerID = GameObject.FindGameObjectsWithTag("Player_Networked_Object").Length - 1;
        go.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(0, yDifference * PlayerID);
    }

    public void StartGamePreparations()
    {
        if (!hasAuthority || !isHost) { return; } //If the player is not the host
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
