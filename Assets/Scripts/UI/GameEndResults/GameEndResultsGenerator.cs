using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndResultsGenerator : MonoBehaviour
{
    PlayerStatistics[] players;
    [SerializeField] GameObject resultsboardEntity;

    private void OnEnable()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("NetworkedPlayerObjects");
        players = new PlayerStatistics[go.Length];
        for (int i = 0; i < go.Length; i++)
        {
            players[i] = go[i].GetComponent<PlayerStatistics>();
        }
        GenerateResults();
    }

    public void GenerateResults()
    {
        PlayerStatistics[] sorted = ShuttleSort(players, new PlayerStatistics[players.Length]);

        //Order players in list by points
        for (int i = 0; i < sorted.Length; i++)
        {
            GameObject go = Instantiate(resultsboardEntity, transform);
            Result r = go.GetComponent<Result>();
            r.FactionImage.sprite = GetComponent<FactionImageHolder>().FactionImages[(int)sorted[i].GetComponent<PlayerNetworked>().PlayerFaction];
            r.Playername.text = sorted[i].GetComponent<PlayerNetworked>().ProfileName;
            r.Points.text = sorted[i].PlayerPoints.ToString();
            r.Rank.text = (i + 1).ToString();
        }
    }

    //Recursive function returns a list of PlayerStatistics in order of points. Highest to lowest.
    public PlayerStatistics[] ShuttleSort(PlayerStatistics[] playerList, PlayerStatistics[] organisedList)
    {
        if (playerList.Length == 0) { return organisedList; }

        PlayerStatistics[] nextList = new PlayerStatistics[playerList.Length-1];

        PlayerStatistics nextHighestPoints = playerList[0];
        for (int i = 1; i < playerList.Length; i++) //Start from second player in the list
        {
            if (playerList[i].PlayerPoints < nextHighestPoints.PlayerPoints)
            {
                nextList[i-1] = nextHighestPoints;
                nextHighestPoints = playerList[i];
            }
            else
            {
                nextList[i-1] = playerList[i];
            }
        }

        organisedList[playerList.Length - 1] = nextHighestPoints;

        return ShuttleSort(nextList, organisedList);
    }
}
