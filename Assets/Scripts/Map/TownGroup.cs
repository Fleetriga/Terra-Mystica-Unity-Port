using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownGroup {
    bool isTown;
    int progress_points;
    int groupID;
    Player owner;
    List<Tile> build_coordinates;

    //For newly created groups
    public TownGroup(int ID_, Player playerID_)
    {
        isTown = false;
        groupID = ID_;
        owner = playerID_;
        build_coordinates = new List<Tile>();
    }

    //For groups created from merged groups
    public TownGroup(bool isTown_, int progress_points_, int groupID_, Player playerID_)
    {
        isTown = isTown_;
        progress_points = progress_points_;
        groupID = groupID_;
        owner = playerID_;
        build_coordinates = new List<Tile>();
    }

    //returns true if a town is made
    public bool AddTownProgress(int progress)
    {
        progress_points += progress;
        if (progress_points >= owner.PointsForTown)
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
            if (progress_points >= points_for_town) { return true; }
        }

        return false;
    }

    public void SetTownAvailability(bool alreadybuilt)
    {
        isTown = alreadybuilt;
    }

    public void AddTile(Tile coordinate_)
    {
        build_coordinates.Add(coordinate_);
    }

    public List<Tile> GetCoordinates()
    {
        return build_coordinates;
    }

    public int GetProgress()
    {
        return progress_points;
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
