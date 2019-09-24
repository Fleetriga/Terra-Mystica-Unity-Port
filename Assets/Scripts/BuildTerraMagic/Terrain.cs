using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain {

    public enum TerrainType { Forest = 0, Mountain = 1, Rocklands = 2, Desert = 3, Farmland = 4, Badlands = 5, Swamp = 6, NOTHING=7 };
    public enum Clockwise_Value { Clockwise = 1, AntiClockwise =-1 };

    TerrainType t_type;

    public Terrain(TerrainType t)
    {
        t_type = t;
    }

    public Terrain()
    {
        t_type = (TerrainType)((int)Random.Range(0, 5));
    }

    public void Next(int distance, Clockwise_Value clockwise)
    {
        t_type = (TerrainType)(((int)t_type + (distance*(int)clockwise)) % 7);
    }

    /**
     *  Get the distance between terrain a and b
     */
    public static int GetDistance(TerrainType a, TerrainType b)
    {
        int distance = (int)a - (int)b;
        if (distance < 0) { distance *= -1; }
        if (distance >= 4) { distance = 7 - distance; }
        return distance;
    }

    public void Set(TerrainType given_t_type)
    {
        t_type = given_t_type;
    }

    public TerrainType GetValue()
    {
        return t_type;
    }

    public static TerrainType ParseInt(int i)
    {
        return (TerrainType)i;
    }

    public static bool TerrainEquals(TerrainType t, TerrainType t2)
    {
        return (int)t == (int)t2;
    }
}
