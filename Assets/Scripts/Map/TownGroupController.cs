using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGroupController {

    List<TownGroup> groups;

    public TownGroupController()
    {
        groups = new List<TownGroup>();
    }

    /* Take the tile that was built upon and all surrounding tiles with townGroups from the same player.
    // Take also the type of building being built, sadly the tile is joined to a group before the building is actually built so this is requried
    */
    public bool JoinTownGroup(Tile builtUpon, Building.Building_Type newBuildingType, List<Tile> neighbours, Player player)
    {
        List<TownGroup> surroundingGroups = GetSurroundingGroups(neighbours, player);

        //if (surroundingGroups.Count < 1 && tile_.GetTownGroup() == null) { tile_.GetTownGroup().AddTownProgress(Building.GetBuildingValue);} //If theres no surrounding groups but it is already part of a group (i.e. upgrading an isolated building)
        if (surroundingGroups.Count < 1 && builtUpon.GetTownGroup() == null) { AddToTownGroup(builtUpon, CreateTownGroup(builtUpon, player)); } //If it's on it's own, just create a new group for it
        else if (surroundingGroups.Count == 1) { AddToTownGroup(builtUpon, surroundingGroups[0]); } //If it's surrounded by only a single TownGroup then join the tile to that one
        else if (surroundingGroups.Count > 1) { AddToTownGroup(builtUpon, MergeTownGroups(surroundingGroups, CreateTownGroup(builtUpon, player))); } //If surrounded by 2 or more then all townGroups need to be merged into one.

        //Now that it's been added to the group, add it's points on
        builtUpon.GetTownGroup().AddTownProgress(Building.GetBuildingValue(newBuildingType));

        Debug.Log(builtUpon.GetTownGroup().GetProgress());

        return CheckNewTown(builtUpon.GetTownGroup()); //Return true or false depending on whether a new town is built or not
    }

    public void AddToTownGroup(Tile t_, TownGroup tg_)
    {
        t_.SetTownGroup(tg_);
        tg_.AddTile(t_);
    }

    public TownGroup CreateTownGroup(Tile t_, Player player)
    {
        TownGroup newboy = new TownGroup(groups.Count, player);
        groups.Add(newboy);
        return newboy;
    }

    public TownGroup MergeTownGroups(List<TownGroup> groups, TownGroup newGroup)
    {
        bool newgrouptown = false;
        foreach (TownGroup tg in groups)
        {
            if (tg.HasATown()) { newgrouptown = true; }
            foreach (Tile t in tg.GetCoordinates())
            {
                AddToTownGroup(t, newGroup);
            }
            newGroup.AddTownProgress(tg.GetProgress());
        }
        if (newgrouptown) { newGroup.SetTownAvailability(true); }

        return newGroup;
    }

    List<TownGroup> GetSurroundingGroups(List<Tile> tiles, Player player)
    {
        List<TownGroup> groups = new List<TownGroup>(); 
        foreach (Tile t in tiles)
        {
            if(t.GetTownGroup() != null && t.GetTownGroup().GetOwner() == player.GetID())
            {
                if (!groups.Contains(t.GetTownGroup())) { groups.Add(t.GetTownGroup()); }
            }
        }
        return groups;
    }

    public bool CheckNewTown(TownGroup tg_)
    {
        if (tg_.HasATown())
        {
            return false;
        }
        else if(tg_.GetProgress() >= tg_.Get_PointsForTown())
        {
            tg_.SetTownAvailability(true);
            return true;
        }
        return false; //IE no town yet and doesnt have enough points to build one
    }
}
