using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour {

    public List<int> next_round;
    public List<int> thisRound;
    int current_player;

    GameObject[] playersStatuses;

    public void SetUp(int noPlayers)
    {
        thisRound = new List<int>();
        for (int i = 0; i < noPlayers; i++)
        {
            thisRound.Add(i);
        }
        next_round = new List<int>();
        current_player = 0;
    }

    public void NextPlayer()
    {
        current_player = (current_player + 1) % thisRound.Count;
    }
    public void RetirePlayer()
    {
        next_round.Add(thisRound[current_player]); //adds to the end of the next round order
        thisRound.Remove(thisRound[current_player]);

        if (thisRound.Count == 0) { NextPhase(); } //Go to next phase when no one is left
    }
    public void NextPhase()
    {
        thisRound = next_round;
        next_round = new List<int>();

        //Set Current player to the first player in this new phase
        current_player = 0;
    }

    public int CurrentPlayersID()
    {
        return thisRound[current_player];
    }

    //Returns true when no players left this round
    public bool AllPlayersRetired()
    {
        return thisRound.Count == 0;
    }
    //Returns true if the current player has changed
    public bool ScanForTurnChanges()
    {
        if (playersStatuses == null)
        {
            playersStatuses = GameObject.FindGameObjectsWithTag("Player_Networked_Object");
        }

        foreach (GameObject go in playersStatuses)
        {
            if(go.GetComponent<PlayerNetworked>().Player_ID == CurrentPlayersID())
            {
                switch (go.GetComponent<PlayerStatus>().CurrentPlayerStatus)
                {
                    case PlayerStatus.PlayerStatusEnum.TakingTurn: return false;
                    case PlayerStatus.PlayerStatusEnum.Retiring: RetirePlayer(); return true;
                    case PlayerStatus.PlayerStatusEnum.EndingTurn: NextPlayer(); return true;
                }
            }
        }

        return false; //Will never be reached
    }

}
