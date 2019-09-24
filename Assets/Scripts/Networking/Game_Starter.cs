using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class Game_Starter : NetworkBehaviour
{
    public int no_players;
    [SyncVar]
    int loaded_players;

    public List<PlayerNetworked> players;

    // Start is called before the first frame update
    void Start()
    {
        no_players = 0;
        loaded_players = 0;
        players = new List<PlayerNetworked>();
    }

    [Command]
    void Cmd_StartGame()
    {
        //players = GameObject.FindGameObjectsWithTag("Player_Networked_Object");

        StartCoroutine(WaitToStart());
    }
    [ClientRpc]
    void Rpc_StartGame()
    {
        GameObject.Find("Controller").GetComponent<Game_Loop_Controller>().Start_Game();
    }
    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(0.25f);
        GameObject.Find("Controller").GetComponent<Game_Loop_Controller>().Start_Game();

        //Tell clients to start the game toos
        Rpc_StartGame();
    }

    public void Register_Player(GameObject player)
    {
        players.Add(player.GetComponent<PlayerNetworked>());
    }

    public void Player_Loaded()
    {
        loaded_players++;

        if (no_players == loaded_players) //if all players load in, staet the game
        {
            //Cmd_StartGame();
        }
    }
}
