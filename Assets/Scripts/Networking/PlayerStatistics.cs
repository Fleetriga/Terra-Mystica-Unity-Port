using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class PlayerStatistics : NetworkBehaviour
{

    PlayerSheetsInterface playerSheetUpdater;
    PointBorder pointBorder;

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
    //Cults
    [SerializeField] [SyncVar] int fireCult;
    [SerializeField] [SyncVar] int waterCult;
    [SerializeField] [SyncVar] int earthCult;
    [SerializeField] [SyncVar] int airCult;

    public int[] CultStanding { get { return new int[] { fireCult, waterCult, earthCult, airCult }; } }


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
        pointBorder = GameObject.Find("PointBorder").GetComponent<PointBorder>();
        pointBorder.AddPlayerPointEntity(GetComponent<PlayerNetworked>().PlayerID, PlayerPoints, GetComponent<PlayerNetworked>().GetFactionMaterial());
    }

    #region SyncVar Setters
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
    [Command]
    public void CmdSetCultStandings(int[] cultData)
    {
        fireCult = cultData[0];
        waterCult = cultData[1];
        earthCult = cultData[2];
        airCult = cultData[3];
    }
    #endregion

    #region PlayerSheet Updaters
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
        pointBorder.MovePlayerPointEntity(GetComponent<PlayerNetworked>().PlayerID, value);
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
    #endregion

    #region Other Reactive Changes

    #endregion
}
