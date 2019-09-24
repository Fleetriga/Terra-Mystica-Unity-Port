using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerraformController : MonoBehaviour {

    Terrain.TerrainType terrain_flag;

    void Awake()
    {
        terrain_flag = Terrain.TerrainType.NOTHING;
    }

    public void SetTerraformFlag(int terrain_type)
    {
        terrain_flag = Terrain.ParseInt(terrain_type);
    }

    public Terrain.TerrainType GetFlag()
    {
        return terrain_flag;
    }

    public void ResetFlag()
    {
        terrain_flag = Terrain.ParseInt(7);
    }
}
