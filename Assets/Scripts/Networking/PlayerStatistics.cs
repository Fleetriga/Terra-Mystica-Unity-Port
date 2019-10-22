using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class PlayerStatistics : NetworkBehaviour
{

    PlayerSheetsInterface playerSheetUpdater;
    //Player resources
    [SyncVar (hook=nameof(PlayerSheetGoldUpdate))]
    public int PlayerGold;
    [SyncVar(hook = nameof(PlayerSheetWorkerUpdate))]
    public int PlayerWorkers;
    [SyncVar(hook = nameof(PlayerSheetPriestUpdate))]
    public int PlayerPriests;
    [SyncVar(hook = nameof(PlayerSheetPointsUpdate))]
    public int PlayerPoints;
    //Magic
    [SyncVar(hook = nameof(PlayerSheetTierOneMagicUpdate))]
    public int tierOne;
    [SyncVar(hook = nameof(PlayerSheetTierTwoMagicUpdate))]
    public int tierTwo;
    [SyncVar(hook = nameof(PlayerSheetTierThreeMagicUpdate))]
    public int tierThree;

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
        playerSheetUpdater = GameObject.Find("UI").GetComponent<PlayerSheetsInterface>();
    }

    //Setting the syncvars on the server
    [Command]
    public void CmdSetGold(int gold)
    {
        PlayerGold = gold;
    }
    [Command]
    public void CmdSetWorkers(int workers)
    {
        PlayerWorkers = workers;
    }
    [Command]
    public void CmdSetPriests(int priests)
    {
        PlayerPriests = priests;
    }
    [Command]
    public void CmdSetPoints(int points)
    {
        PlayerPoints = points;
    }
    [Command]
    public void CmdSetMagics(int[] magics)
    {
        tierOne = magics[0];
        tierTwo = magics[1];
        tierThree = magics[2];
    }

    //From here are hook methods. They tell the UI to update with the new values.
    void PlayerSheetGoldUpdate(int value)
    {
        PlayerGold = value;
        if (playerSheetUpdater == null) { return; }
        playerSheetUpdater.RegularUpdate(GetComponent<PlayerNetworked>().PlayerID);
    }
    void PlayerSheetWorkerUpdate(int value)
    {
        PlayerWorkers = value;
        if (playerSheetUpdater == null) { return; }
        playerSheetUpdater.RegularUpdate(GetComponent<PlayerNetworked>().PlayerID);
    }
    void PlayerSheetPriestUpdate(int value)
    {
        PlayerPriests = value;
        if (playerSheetUpdater == null) { return; }
        playerSheetUpdater.RegularUpdate(GetComponent<PlayerNetworked>().PlayerID);
    }
    void PlayerSheetPointsUpdate(int value)
    {
        PlayerPoints = value;
        if (playerSheetUpdater == null) { return; }
        playerSheetUpdater.PointsUpdate(GetComponent<PlayerNetworked>().PlayerID);
    }
    void PlayerSheetTierOneMagicUpdate(int value)
    {
        tierOne = value;
        if(playerSheetUpdater == null) { return; }
        playerSheetUpdater.MagicUpdate(GetComponent<PlayerNetworked>().PlayerID);
    }
    void PlayerSheetTierTwoMagicUpdate(int value)
    {
        tierTwo = value;
        if (playerSheetUpdater == null) { return; }
        playerSheetUpdater.MagicUpdate(GetComponent<PlayerNetworked>().PlayerID);
    }
    void PlayerSheetTierThreeMagicUpdate(int value)
    {
        tierThree = value;
        if (playerSheetUpdater == null) { return; }
        playerSheetUpdater.MagicUpdate(GetComponent<PlayerNetworked>().PlayerID);
    }
}
