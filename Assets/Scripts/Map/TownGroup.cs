using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGroup {
    bool isTown;
    int progressPoints;
    int groupID;
    Player owner;
    List<Tile> buildCoordinates; //used for iterating over all tiles within the group

    //For newly created groups
    public TownGroup(int ID_, Player playerID_)
    {
        isTown = false;
        groupID = ID_;
        owner = playerID_;
        buildCoordinates = new List<Tile>();
    }

    //For groups created from merged groups
    public TownGroup(bool isTown_, int progress_points_, int groupID_, Player playerID_)
    {
        isTown = isTown_;
        progressPoints = progress_points_;
        groupID = groupID_;
        owner = playerID_;
        buildCoordinates = new List<Tile>();
    }

    //returns true if a town is made
    public bool AddTownProgress(int progress)
    {
        progressPoints += progress;
        if (progressPoints >= owner.PointsForTown)
        {
            return true;
        }return false;
    }

    public bool Merge_Town_Group(TownGroup tg_, int points_for_town)
    {
        foreach (Tile t in tg_.GetCoordinates())
        {
            AddTile(t);
        }

        AddTownProgress(tg_.GetProgress());

        if (tg_.HasATown() || isTown)
        {
            isTown = true;
        }
        else
        {
            if (progressPoints >= points_for_town) { return true; }
        }

        return false;
    }

    public void SetTownAvailability(bool alreadybuilt)
    {
        isTown = alreadybuilt;
    }

    public void AddTile(Tile coordinate_)
    {
        buildCoordinates.Add(coordinate_);
    }

    public List<Tile> GetCoordinates()
    {
        return buildCoordinates;
    }

    public int GetProgress()
    {
        return progressPoints;
    }

    public int GetOwner() //Gives owner by it's playerID
    {
        return owner.GetID();
    }

    public int Get_PointsForTown()
    {
        return owner.PointsForTown;
    }

    public int GetGroupID()
    {
        return groupID;
    }

    public bool HasATown()
    {
        return isTown;
    }
}
