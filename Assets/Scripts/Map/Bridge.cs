using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour {

    public Tile[] joined_tiles;
    public Bridge_Manager bm;

    public void Build_Bridge()
    {
        //get current player
        if (bm.Get_Allowed_PlayerID() == joined_tiles[0].GetOwner() || bm.Get_Allowed_PlayerID() == joined_tiles[1].GetOwner()) //If the person building it has a building nextdoor
        {
            //Link to tiles
            joined_tiles[0].Bridges.Add(this);
            joined_tiles[1].Bridges.Add(this);

            //change look
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);

            bm.Merge_Towns(joined_tiles[0].GetTownGroup(), joined_tiles[1].GetTownGroup());

            //Then stop other dickheads from building bridges. Build walls not bridges.
            bm.Disallow_Bridge(); 
        }
    }

    void OnMouseDown()
    {
        if (bm.Bridge_Allowed())
        {
            Build_Bridge();
        }
    }

    public Tile Cross_Bridge(Tile other)
    {
        if (joined_tiles[0] == null) { return null; } //No joined boys then no bridge, return nothing. But this should never be used.
        else if (other.GetCoordinates().GetX() == joined_tiles[0].GetCoordinates().GetX() && other.GetCoordinates().GetY() == joined_tiles[0].GetCoordinates().GetY()) { return joined_tiles[1]; } //If other is joined_tiles[0] then return joined_tiles[1]
        else { return joined_tiles[0];  } //Otherwise it's safe to say its joined_tiles[1] so return 0
    }
}
