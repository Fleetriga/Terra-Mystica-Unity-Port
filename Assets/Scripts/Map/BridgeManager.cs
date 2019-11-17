using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeManager : MonoBehaviour {

    bool allowbridge;
    int allowedID;
    public GameController gameController;
    public Bridge[] bridges;

    void Awake()
    {
        for (int i = 0; i < bridges.Length; i++)
        {
            bridges[i].BridgeID = i;
        }    
    }

    public void BuildBridge(int ID)
    {
        gameController.BuildBridge(ID);
    }

    public void SetBridgeGameObjectActive(int ID)
    {
        bridges[ID].SetBridgeGameObjectActive();
    }

    public bool BridgeAllowed()
    {
        return allowbridge;
    }

    public int GetAllowedPlayerID()
    {
        return allowedID;
    }

    public void AllowBridge(int playerID)
    {
        allowedID = playerID;
        allowbridge = true;
    }

    public void DisallowBridge()
    {
        allowbridge = false;
    }

    public void MergeTowns(TownGroup tg1, TownGroup tg2)
    {
        gameController.Merge_Towns(tg1, tg2);
    }
}
