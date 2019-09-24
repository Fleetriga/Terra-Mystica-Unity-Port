using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class Turn_Controller : NetworkBehaviour {

    public List<int> next_round;
    public List<int> this_round;
    int current_player;

    List<PlayerNetworked> players;
    public int player_count;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        players = new List<PlayerNetworked>();
    }


    void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Set_Up(player_count);
        }
    }

    public void Set_Up(int no_players)
    {
        this_round = new List<int>();
        for (int i = 0; i < no_players; i++)
        {
            this_round.Add(i);
        }
        next_round = new List<int>();
        current_player = 0;
    }

    [Command]
    public void Cmd_Next_Player()
    {
        Rpc_Next_Player();
    }
    [Command]
    public void Cmd_Retire_Player()
    {
        Rpc_Retire_Player();
    }
    [Command]
    public void Cmd_Next_Phase()
    {
        Rpc_Next_Phase();
    }
    [ClientRpc]
    void Rpc_Next_Player()
    {
        if (isServer)
        {
            return;
        }
        current_player = (current_player + 1) % this_round.Count;
    }
    [ClientRpc]
    void Rpc_Retire_Player()
    {
        if (isServer)
        {
            return;
        }
        next_round.Add(this_round[current_player]); //adds to the end of the next round order
        this_round.Remove(this_round[current_player]);
    }
    [ClientRpc]
    void Rpc_Next_Phase()
    {
        if (isServer)
        {
            return;
        }
        this_round = next_round;
        next_round = new List<int>();
    }

    public int Current_Player()
    {
        return this_round[current_player];
    }

    //Returns true when no players left this round
    public bool All_Players_Retired()
    {
        return this_round.Count == 0;
    }

    [Command]
    public void Add_Player(GameObject pl)
    {
        players.Add(pl.GetComponent<PlayerNetworked>());
    }

    [ClientRpc]
    void Set_No_Players(int amount)
    {

    }


}
