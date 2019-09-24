using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBonus {

    public enum Action_Type {Build_Dwelling, Build_Temple, Build_TP, Build_Sanctuary, Build_Stronghold, BuildT3, Terraform, FoundTown, //Reactive
                            Have_Dwelling, Have_TP, Have_Temple, Have_Sanctuary, Have_Stronghold, Have_Tier3Buildings, //Cumulative
                            NOTHING
                            };

    public int pointBonus { get; set; }
    Action_Type at;
    //Newly made for arrays of bonuses
    public int[] PointBonusAr { get; set; }
    public bool Array { get; set; }

    //Default point bonus
    public PointBonus(Action_Type at_, int pointBonus_)
    {
        pointBonus = pointBonus_;
        at = at_;
        Array = false;
    }
    //Point bonus with varying points given.
    public PointBonus(Action_Type at_, int[] pointBonus_)
    {
        PointBonusAr = pointBonus_;
        pointBonus = 1;
        at = at_;
        Array = true;
    }

    public Action_Type GetActionType()
    {
        return at;
    }

    public void AddPointBonus(int pointBonus_)
    {
        pointBonus += pointBonus_;
    }

    public bool Equals(Action_Type at_)
    {
        return (int)at == (int)at_;
    }


    /**
     * Returns true if the Action_Type is classified as a reactive action
     */ 
    public static bool CheckReactive(Action_Type at_)
    {
        return (int)at_ <= 7; //5 and below are all reactive
    }

    public static Action_Type MapBuildingToAction(Building.Building_Type bt)
    {
        Action_Type at_ = Action_Type.NOTHING;
        switch (bt)
        {
            case Building.Building_Type.Dwelling:
                at_ = Action_Type.Build_Dwelling;
                break;
            case Building.Building_Type.Trading_Post:
                at_ = Action_Type.Build_TP;
                break;
            case Building.Building_Type.Stronghold:
                at_ = Action_Type.Build_Stronghold;
                break;
            case Building.Building_Type.Temple:
                at_ = Action_Type.Build_Temple;
                break;
            case Building.Building_Type.Sanctuary:
                at_ = Action_Type.Build_Sanctuary;
                break;
        }

        return at_;
    }

    public static Building.Building_Type MapActionToBuilding(Action_Type at_)
    {
        Building.Building_Type bt = Building.Building_Type.NOTHING;
        switch (at_)
        {
            case Action_Type.Have_Dwelling:
                bt = Building.Building_Type.Dwelling; break;
            case Action_Type.Have_TP:
                bt = Building.Building_Type.Trading_Post; break;
            case Action_Type.Have_Temple:
                bt = Building.Building_Type.Temple; break;
            case Action_Type.Have_Sanctuary:
                bt = Building.Building_Type.Sanctuary; break;
            case Action_Type.Have_Stronghold:
                bt = Building.Building_Type.Stronghold; break;
        }
        return bt;
    }
}
