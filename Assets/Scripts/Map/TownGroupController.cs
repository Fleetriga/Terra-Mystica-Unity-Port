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

    public bool JoinTownGroup(Tile tile_, List<Tile> tiles, Player player)
    {
        List<TownGroup> surroundingGroups = GetSurroundingGroups(tiles, player);

        //if (surroundingGroups.Count < 1 && tile_.GetTownGroup() == null) { tile_.GetTownGroup().AddTownProgress(Building.GetBuildingValue);} //If theres no surrounding groups but it is already part of a group (i.e. upgrading an isolated building)
        if (surroundingGroups.Count < 1 && tile_.GetTownGroup() == null) { AddToTownGroup(tile_, CreateTownGroup(tile_, player)); } 
        else if (surroundingGroups.Count == 1) { AddToTownGroup(tile_, surroundingGroups[0]); }
        else if (surroundingGroups.Count > 1) { AddToTownGroup(tile_, MergeTownGroups(surroundingGroups, CreateTownGroup(tile_, player))); } //Merge then add the connecting one

        //Now that it's been added to the group, add it's points on
        tile_.GetTownGroup().AddTownProgress(Building.GetBuildingValue(tile_.GetBuildingType()));

        return CheckNewTown(tile_.GetTownGroup()); //Return true or false depending on whether a new town is built or not
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
