using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeManager : MonoBehaviour {

    bool allow_bridge;
    int allowed_ID;
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

    public bool Bridge_Allowed()
    {
        return allow_bridge;
    }

    public int Get_Allowed_PlayerID()
    {
        return allowed_ID;
    }

    public void Allow_Bridge(int playerID)
    {
        allowed_ID = playerID;
        allow_bridge = true;
    }

    public void Disallow_Bridge()
    {
        allow_bridge = false;
    }

    public void Merge_Towns(TownGroup tg1, TownGroup tg2)
    {
        gameController.Merge_Towns(tg1, tg2);
    }
}
