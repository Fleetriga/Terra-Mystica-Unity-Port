using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerNetworked : NetworkBehaviour {

    [SerializeField] GameObject LocalPlayerObject;
    [SerializeField] GameObject PlayerLobbyEntityPrefab;
    [SerializeField] GameObject turnControllerPrefab;

    GameLobbyManager lobbyManager;

    public static int playerID;

    [SyncVar] public int PlayerID;
    [SyncVar] [SerializeField] bool isReady; //Ready check before game starts
    [SyncVar] bool isHost;
    [SyncVar] public Faction.FactionType PlayerFaction = Faction.FactionType.NOFACTION;
    [SyncVar (hook=nameof(InitialisePlayerLobbyEntity))] public string ProfileName;

    public bool IsHost { get { return isHost; } }
    public bool IsReady { get { return isReady; } }

    // Use this for initialization
    void Start() {
        DontDestroyOnLoad(gameObject);

        PlayerID = playerID++;

        if (isHost) //To identify host in Hierachy, this conditional is only fired for clients, isHost is false for the host itself until later.
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
        Cmd_SetProfileName(GameObject.Find("IO").GetComponent<ProfileManager>().PlayerProfile.ProfileName);
    }

    void InitialisePlayerLobbyEntity(string proname)
    {
        if (proname == "") { return; }
        lobbyManager = GameObject.Find("GameLobby").GetComponent<GameLobbyManager>();
        lobbyManager.PlayerConnected(this, isLocalPlayer, isServer, proname); //Behaviour changes for local players own lobby entity.
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
            GameObject go = Instantiate(LocalPlayerObject);
            go.GetComponent<Player>().SetUp(PlayerFaction, PlayerID, gameObject);
            GameObject.Find("Controller").GetComponent<GameController>().StartGame();
        }
    }

    public string SetFaction(Faction.FactionType i)
    {
        Cmd_SetFaction(i);
        return Faction.GetFactionName(i);
    }
    [Command]
    public void Cmd_SetFaction(Faction.FactionType i)
    {
        PlayerFaction = i;
    }

    [Command]
    void Cmd_SpawnTurnController()
    {
        GameObject go = Instantiate(turnControllerPrefab);
        NetworkServer.Spawn(go);
    }

    [Command]
    void Cmd_SetProfileName(string proname)
    {
        ProfileName = proname;
    }

    [Command]
    public void Cmd_SetPlayerReadyUnready()
    {
        isReady = !isReady;
    }

    /// <summary>
    /// Will no longer use this bastard.
    /// </summary>
    [Command]
    void Cmd_SpawnPlayerLobbyEntity()
    {
        GameObject go = Instantiate(PlayerLobbyEntityPrefab);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
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
        return Faction.GetFactionMaterial(PlayerFaction);
    }
}
