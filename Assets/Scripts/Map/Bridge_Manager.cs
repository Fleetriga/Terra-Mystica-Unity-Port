using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge_Manager : MonoBehaviour {

    bool allow_bridge;
    int allowed_ID;
    public Game_Loop_Controller gfc;

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
        gfc.Merge_Towns(tg1, tg2);
    }
}
