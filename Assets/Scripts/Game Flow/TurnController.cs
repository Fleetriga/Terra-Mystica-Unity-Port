using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class TurnController : NetworkBehaviour {

    List<int> nextRoundTurnOrder;
    List<int> currentRoundTurnOrder;
    List<int> currentPhaseTurnOrder;
    int currentPlayerIndex;

    [SyncVar]
    public int CurrentPhase;
    [SyncVar]
    public int CurrentPlayerID;
    [SyncVar]
    public int CurrentRoundNumber;
    public bool ReactToPhaseChange; //Networked via RPC so players can unflag this when they react


    GameObject[] playersStatuses;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
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
        SetUp(NetworkServer.connections.Count);
    }

    public void SetUp(int noPlayers)
    {
        currentRoundTurnOrder = new List<int>();
        for (int i = 0; i < noPlayers; i++)
        {
            currentRoundTurnOrder.Add(i);
        }
        currentPhaseTurnOrder = new List<int>(currentRoundTurnOrder);
        nextRoundTurnOrder = new List<int>();

        currentPlayerIndex = 0;
        Cmd_SetPhase(0);
    }

    void Update()
    {
        //If you are the host, scan other players and yourself for turn changes
        if (hasAuthority)
        {
            if (ScanForTurnChanges()) { Cmd_SetCurrentPlayer(); }
        }
    }

    void SetPhaseToNextPhase()
    {
        switch (CurrentPhase)
        {
            case 0: Cmd_SetPhase(1); break; //First to second dwelling
            case 1: Cmd_SetPhase(2); Cmd_SetReactToPhaseChange(); break; //Second dwelling to nomad dwelling
            case 2: Cmd_SetPhase(3); Cmd_SetReactToPhaseChange(); break; //Nomad dwelling to Chaos Magician dwelling
            case 3: Cmd_SetPhase(4); Cmd_SetReactToPhaseChange(); break; //initial dwellings placed to initial round bonus picking
            case 4: Cmd_SetPhase(5); Cmd_SetReactToPhaseChange(); break; //intial round bonnus picking to income phase
            case 5: Cmd_SetPhase(6); Cmd_SetReactToPhaseChange(); break; //income phase to action phase
            case 6: Cmd_SetPhase(7); Cmd_SetReactToPhaseChange(); break; //action phase to point calculation phase
            case 7: Cmd_SetPhase(8); Cmd_SetReactToPhaseChange(); break; //point calculation phase to cult bonuses phase
            case 8: Cmd_SetPhase(5); Cmd_SetReactToPhaseChange(); Cmd_SetRound(CurrentRoundNumber+1); break; //cult bonuses phase to income phase
        }

        //Set new round turn order based off who retired first last round
        currentRoundTurnOrder = nextRoundTurnOrder;
        nextRoundTurnOrder = new List<int>();

        SetCurrentPhaseTurnOrder();
        currentPlayerIndex = 0;
    }

    void SetCurrentPhaseTurnOrder()
    {

        currentPhaseTurnOrder = new List<int>(currentRoundTurnOrder); //Create a new list for the phase
        if (CurrentPhase == 1 || CurrentPhase == 4) //If it's a phase that requires a reversal then reverse the list. Required when picking initial round bonus and when placing second dwelling
        {
            currentPhaseTurnOrder.Reverse();
        }
    }

    public void NextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % currentRoundTurnOrder.Count;
    }
    public void RetirePlayer()
    {
        nextRoundTurnOrder.Add(currentPhaseTurnOrder[currentPlayerIndex]); //adds to the end of the next round order. This correctly dictates the turn order for next round.
        currentPhaseTurnOrder.RemoveAt(currentPlayerIndex);

        if (currentPhaseTurnOrder.Count == 0) { SetPhaseToNextPhase(); } //Go to next phase when no one is left
    }

    public int CurrentPlayersID()
    {
        if (currentPhaseTurnOrder == null) { return 80; }
        return currentPhaseTurnOrder[currentPlayerIndex];
    }

    void CollectPlayerStatuses()
    {
        playersStatuses = GameObject.FindGameObjectsWithTag("Player_Networked_Object");
    }

    //Returns true if the current player has changed
    public bool ScanForTurnChanges()
    {
        if (playersStatuses == null || playersStatuses.Length != NetworkServer.connections.Count)
        {
            CollectPlayerStatuses();
        }

        foreach (GameObject go in playersStatuses)
        {
            if (go.GetComponent<PlayerNetworked>().PlayerID ==  CurrentPlayerID)
            {
                switch (go.GetComponent<PlayerStatus>().CurrentPlayerStatus)
                {
                    case PlayerStatus.PlayerStatusEnum.TakingTurn: return false;
                    case PlayerStatus.PlayerStatusEnum.Retiring: RetirePlayer();
                        go.GetComponent<PlayerStatus>().CurrentPlayerStatus = PlayerStatus.PlayerStatusEnum.Standby; return true; //Change their status locally, avoids network sync issues
                    case PlayerStatus.PlayerStatusEnum.EndingTurn: NextPlayer(); return true;
                }
            }
        }

        return false; //Will never be reached
    }

    //Networked Methods
    [Command]
    void Cmd_SetCurrentPlayer()
    {
        CurrentPlayerID = CurrentPlayersID();
    }
    [Command]
    void Cmd_SetPhase(int phase)
    {
        CurrentPhase = phase;
    }
    [Command]
    void Cmd_SetRound(int v)
    {
        CurrentRoundNumber = v;
    }
    [Command]
    void Cmd_SetReactToPhaseChange()
    {
        ReactToPhaseChange = true;
        Rpc_SetReactToPhaseChange();
    }
    [ClientRpc]
    void Rpc_SetReactToPhaseChange()
    {
        ReactToPhaseChange = true;
    }

}
