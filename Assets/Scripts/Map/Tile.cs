using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    GameController tc;

    //Terrain related
    public Terrain terrain;
    public Material[] terrains = new Material[7];
    public Terrain.TerrainType starting_terrain;

    //Bridge
    public List<Bridge> Bridges { get; set; }

    //Coordinates
    Coordinate coordinates;
    public int x;
    public int y;

    //Building related
    public Building TileBuilding;
    public bool riverTile;
    public GameObject[] buildings = new GameObject[4]; //5 later 0: dwelling, 1: trading post, 2: fortress, 3: shrine, 4:temple
    TownGroup towngroup; //Just a pointer so towngroup can be accessed through the tile itself

    public int OwnersPlayerID = 64;

    void Awake()
    {
        coordinates = new Coordinate(x,y);
        terrain = new Terrain(starting_terrain);

        if (!riverTile) { Terraform(terrain.GetValue()); }

        tc = GameObject.Find("Controller").GetComponent<GameController>();

        //No building or towngroup to begin with
        towngroup = null; 
        TileBuilding = new Building(Building.Building_Type.NOTHING);

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
        return TileBuilding.GetValue() != 5;
    }

    public Building.Building_Type GetBuildingType()
    {
        return TileBuilding.GetBType();
    }

    public void SetTownGroup(TownGroup towngroup_)
    {
        towngroup = towngroup_;
    }

    //Returns true if this building is a successful upgrade
    public void Build(Building.Building_Type bt, Material mat)
    {
        //Destroy previous building.
        if (HasBuilding()) { Destroy(transform.GetChild(1).gameObject); }

        //Build the building
        TileBuilding.Build(bt);
        
        //Instantiate new building on the map
        GameObject x = Instantiate(buildings[(int)bt], gameObject.transform);
        x.transform.Translate(new Vector3(0, 0.25f, 0), Space.World);
        x.GetComponentInChildren<Renderer>().material = mat;
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
        return OwnersPlayerID;
    }
}
