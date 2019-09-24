using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    Game_Loop_Controller tc;

    //Terrain related
    public Terrain terrain;
    public Material[] terrains = new Material[7];
    public Terrain.TerrainType starting_terrain;

    //Bridge
    public List<Bridge> Bridges { get; set; }

    //Coordinate
    Coordinate coordinates;
    public int x;
    public int y;

    //Building related
    Building building;
    public bool riverTile;
    public GameObject[] buildings = new GameObject[4]; //5 later 0: dwelling, 1: trading post, 2: fortress, 3: shrine, 4:temple
    TownGroup towngroup; //Just a pointer so towngroup can be accessed through the tile itself


    void Awake()
    {
        coordinates = new Coordinate(x,y);
        terrain = new Terrain(starting_terrain);
        if (!riverTile) { Terraform(terrain.GetValue()); }
        building = new Building(Building.Building_Type.NOTHING);
        tc = GameObject.Find("Controller").GetComponent<Game_Loop_Controller>();
        towngroup = null; //does not belong to any group to begin with
        Bridges = new List<Bridge>();
    }

    void OnMouseDown()
    {
        tc.ParseFlags(this);
    }

    public void Terraform(Terrain.TerrainType t_type)
    { 
        GetRenderer().material = terrains[(int)t_type];
        terrain.Set(t_type);
    }

    public virtual Renderer GetRenderer()
    {
        return GetComponent<Renderer>();
    }


    public bool HasBuilding()
    {
        return building.GetValue() != 5;
    }

    public Building.Building_Type GetBuildingType()
    {
        return building.GetBType();
    }

    public void SetTownGroup(TownGroup towngroup_)
    {
        towngroup = towngroup_;
    }

    /**
     * This is probably a redundant method with the Validation used in TurnController. Check later.
     */
    public bool Build(Building.Building_Type bt, Material mat)
    {
        bool temp = HasBuilding();
        if (building.Upgrade(bt))
        {
            if (temp) { Destroy(transform.GetChild(1).gameObject); }
            GameObject x = Instantiate(buildings[building.GetValue()], gameObject.transform);
            x.transform.Translate(new Vector3(0, 0.25f, 0), Space.World);
            x.GetComponentInChildren<Renderer>().material = mat;
            return true;
        }

        //Else it failed to build (probably a building already there.
        return false;
    }

    public Terrain.TerrainType GetTerrainType()
    {
        return terrain.GetValue();
    }

    public bool IsRiverTile()
    {
        return riverTile;
    }

    public Coordinate GetCoordinates()
    {
        return coordinates;
    }

    public TownGroup GetTownGroup()
    {
        return towngroup;
    }

    public int GetOwner()
    {
        if (towngroup != null)
        {
            return towngroup.GetOwner();
        }
        else return 64;
    }
}
