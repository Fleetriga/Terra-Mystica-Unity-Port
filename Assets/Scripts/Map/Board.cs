using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    public TileLine[] map;
    TownGroupController tgc;

    // Use this for initialization
    void Start()
    {
        tgc = new TownGroupController();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            List<Coordinate> temp = new List<Coordinate>();
            temp.Add(new Coordinate(0, 6));
            TestRaise(GetAllNeighbours(temp, 0));
        }
    }

    private void TestRaise(HashSet<Tile> HashSet)
    {
        foreach (Tile t in HashSet)
        {
            t.gameObject.transform.Translate(Vector3.up * 5);
        }
    }

    public List<Tile> GetNeighbours(int[] xy)
    {
        int offset = 0;
        List<Tile> neighbours = new List<Tile>();
        if (xy[1] % 2 == 0)
        {
            offset = -1;
        }
        if (xy[0] - 1 >= 0) { neighbours.Add(map[xy[0] - 1].GetTile(xy[1] + 0)); } //Left //if left of tile is on the board
        if (xy[0] + 1 <= 12) { neighbours.Add(map[xy[0] + 1].GetTile(xy[1] + 0)); } // right //if right of tile is on the board
        
        if (xy[1] + 1 <= 8) //If there are tiles above
        {
            if (xy[0] + 0 + offset >= 0) { neighbours.Add(map[xy[0] + 0 + offset].GetTile(xy[1] + 1)); }//Up left
            if (xy[0] + 1 + offset <= 12) { neighbours.Add(map[xy[0] + 1 + offset].GetTile(xy[1] + 1)); }//Up right 
        }
        if (xy[1] - 1 >= 0) //If there are tiles below
        {
            if (xy[0] + 1 + offset <= 12) { neighbours.Add(map[xy[0] + 1 + offset].GetTile(xy[1] - 1)); }//down right 
            if (xy[0] + 0 + offset >= 0) { neighbours.Add(map[xy[0] + 0 + offset].GetTile(xy[1] - 1)); }//down left
        }
        return neighbours;
    }

    /**
    HashSet<Tile> GetSettleable(Terrain.TerrainType habitat, Coordinate[] buildingLocs)
    {
        HashSet<Tile> allNeighbours = GetAllNeighbours(buildingLocs);
        HashSet<Tile> settleable = new HashSet<Tile>();
        foreach (Tile t in allNeighbours)
        {
            if (Terrain.TerrainEquals(habitat, t.GetTerrainType()))
            {
                settleable.Add(t);
            }
        }

        return settleable;
    }
    */

    HashSet<Tile> GetAllNeighbours(List<Coordinate> buildingLocs, int shipping)
    {
        HashSet<Tile> allNeighbours = new HashSet<Tile>();
        HashSet<Tile> riverTiles = new HashSet<Tile>();

        //Find neighbours around all buildings. 
        foreach (Coordinate c in buildingLocs)
        {
            Tile current = map[c.GetX()].GetTile(c.GetY());
            //Find all buildings with bridges
            if (current.Bridges.Count > 0) //First check it has bridges
            {
                foreach (Bridge b in current.Bridges)
                {
                    allNeighbours.Add(b.CrossBridge(current)); //Add the other side of the bridge
                }
            }

            List<Tile> temp = GetNeighbours(c.GetXY());
            foreach (Tile t in temp)
            {
                if (t.IsRiverTile()) { riverTiles.Add(t); } else { allNeighbours.Add(t); }
            }
        }

        if (shipping > 0) //stop process being done when theres no need
        {
            //From here check river crossings
            riverTiles = GetTraversableRiverTiles(riverTiles, new HashSet<Tile>(riverTiles), shipping - 1)[0];

            //Then add neighbours from these river tiles to the total neighbours
            foreach (Tile t in riverTiles)
            {
                List<Tile> temp = GetNeighbours(t.GetCoordinates().GetXY());
                foreach (Tile t2 in temp)
                {
                    if (!t2.IsRiverTile()) { allNeighbours.Add(t2); }
                }
            }
        }

        return allNeighbours;
    }

    HashSet<Tile>[] GetTraversableRiverTiles(HashSet<Tile> startingTiles, HashSet<Tile> returnSet, int remainingShipping)
    {
        if (remainingShipping >= 1)
        {
            //RECURSIVE
            HashSet<Tile>[] temp = GetTraversableRiverTiles(startingTiles, returnSet, remainingShipping - 1);
            HashSet<Tile> nextTimesStarting = new HashSet<Tile>();
            returnSet = temp[0]; startingTiles = temp[1];


            //Actual Body
            foreach (Tile t in startingTiles) //For each river tile added this round
            {
                List<Tile> temp2 = GetNeighbours(t.GetCoordinates().GetXY()); //Get the neighbours
                foreach (Tile t2 in temp2)
                {
                    if (t2.IsRiverTile()) //Seperate and add river tiles
                    {
                        if (returnSet.Add(t2)) //If it's unique
                        {
                            nextTimesStarting.Add(t2); //Newly added are used as starting set for next round
                        }
                    }
                }
            }
            return new HashSet<Tile>[] { returnSet, nextTimesStarting }; //Return while still recursing
        }
        return new HashSet<Tile>[] { returnSet, startingTiles }; //Last return 
    }

    public bool CheckNeighbourSettleable(List<Coordinate> buildingLocs, Tile target, Terrain.TerrainType habitat, int shipping)
    {
        HashSet<Tile> neighbours = GetAllNeighbours(buildingLocs, shipping);
        foreach (Tile t in neighbours)
        {
            if (t.GetCoordinates().Equals(target.GetCoordinates()) && Terrain.TerrainEquals(t.GetTerrainType(), habitat))
            {
                return true;
            }
        }
        return false;

    }

    public bool CheckTileIsNeighbour(List<Coordinate> buildingLocs, Tile target, int shipping)
    {
        HashSet<Tile> neighbours = GetAllNeighbours(buildingLocs, shipping);
        foreach (Tile t in neighbours)
        {
            if (t.GetCoordinates().Equals(target.GetCoordinates()))
            {
                return true;
            }
        }
        return false;
    }

    //Return true if a new town can be founded
    public bool JoinTileToTownGroup(Tile builtUpon, Building.Building_Type newBuildingType, Player player)
    {
        return tgc.JoinTownGroup(builtUpon, newBuildingType, GetNeighbours(builtUpon.GetCoordinates().GetXY()), player);//Since it was just added we can access towngroup straight from the given tile
    }

    public Tile GetTile(int[] coordinates)
    {
        return map[coordinates[0]].GetTile(coordinates[1]);
    }
}

