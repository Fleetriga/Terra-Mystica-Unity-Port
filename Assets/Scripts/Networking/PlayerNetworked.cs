using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerNetworked : NetworkBehaviour {

    public GameObject player_game_object;
    public GameObject dropDown;
    public Turn_Controller turn_synced;

    public int Player_ID;

    public Faction.Faction_Type p_faction;

    //dirty bit
    bool gameStarted = true;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);

        //If youre the server, register the player
        if (isServer)
        {
            //Register the player to the server
            Cmd_Register_Player();
            Cmd_Spawn_TurnSync();
        }
        if (!isLocalPlayer)
        {
            return;
        }

        //Instantiate it, then send it to the server to get spawned to all players
        Cmd_spawn_Dropdown();
	}

    void Update()
    {
        if (!gameStarted)
        {
            //Cmd_Player_Loaded();
            GameObject.Find("Controller").GetComponent<Game_Loop_Controller>().Start_Game();
            gameStarted = true;
        }
    }

    void OnLevelWasLoaded()
    {
        if (isLocalPlayer && SceneManager.GetActiveScene().buildIndex == 1)
        {
            GameObject go = Instantiate(player_game_object);
            go.GetComponent<Player>().SetUp(p_faction, Player_ID);
            gameStarted = false;
        }
    }

    public string Set_Faction(Faction.Faction_Type i)
    {
        Cmd_Set_Faction(i);
        return Faction.GetFaction(i);
    }

    [Command]
    public void Cmd_Set_Faction(Faction.Faction_Type i)
    {
        p_faction = i;
    }

    [Command]
    void Cmd_spawn_Dropdown()
    {
        GameObject go = Instantiate(dropDown);
        NetworkServer.SpawnWithClientAuthority(go, connectionToClient);
        go.GetComponent<DropDownMenu>().Cmd_SetOwner(gameObject);
        Rpc_Move_DropDown(-100, go);
    }

    [ClientRpc]
    void Rpc_Move_DropDown(int yDifference, GameObject go)
    {
        Player_ID = GameObject.FindGameObjectsWithTag("Player_Networked_Object").Length-1;
        go.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector2(0, yDifference * Player_ID);
    }

    [Command]
    public void Cmd_Register_Player()
    {
        turn_synced.Add_Player(this);
    }

    [Command]
    void Cmd_Player_Loaded()
    {
        Rpc_Player_Loaded();
    }

    [ClientRpc]
    void Rpc_Player_Loaded()
    {
        GameObject.Find("GameStarter").GetComponent<Game_Starter>().Player_Loaded();
    }


}
