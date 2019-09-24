using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building {

    public enum Building_Type { Dwelling, Trading_Post, Fortress, Temple, Sanctuary, NOTHING };
    Building_Type b_type;
    bool hasBuilt;

    public static Building_Type ParseInt(int i)
    {
        return (Building_Type)i;
    }

    public Building(Building_Type bt)
    {
        b_type = bt;
        hasBuilt = false;
    }

    public bool Upgrade(Building_Type to)
    {
        if (((int)to - (int)b_type == 1) || ((int)b_type == 1 && (int)to == 3) || ((int)b_type == 5 && (int)to == 0)) //If upgrade is next in path or upgrading from TP to sanctuary or upgrading to a dwelling
        {
            Build(to);
            return true;
        }
        return false;
    }

    public bool HasBuilt()
    {
        return hasBuilt;
    }

    public void Build(Building_Type bt)
    {
        b_type = bt;
        hasBuilt = true;
    }

    public Building_Type GetBType()
    {
        return b_type;
    }

    public int GetValue()
    {
        return (int)b_type;
    }

    public static bool Equals(Building_Type t, Building_Type t2)
    {
        return (int)t == (int)t2;
    }

    public static int GetBuildingValue(Building_Type bt_)
    {
        switch (bt_)
        {
            case Building_Type.Dwelling:
                return 1;
            case Building_Type.Trading_Post:
                return 1;
            case Building_Type.Fortress:
                return 1;
            case Building_Type.Sanctuary:
                return 1;
            case Building_Type.Temple:
                return 0;
        }
        return 0;
    }
}
