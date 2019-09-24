using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBonus {

    public enum Action_Type {Build_Dwelling, Build_Temple, Build_TP, Build_Sanctuary, Build_Stronghold, Terraform,
                            Have_Dwelling, Have_TP, Have_Temple, Have_Sanctuary, Have_Stronghold,
                            NOTHING
                            };

    public int pointBonus { get; set; }
    Action_Type at;
    //Newly made for arrays of bonuses
    public int[] PointBonusAr { get; set; }
    public bool Array { get; set; }

    public PointBonus(Action_Type at_, int pointBonus_)
    {
        pointBonus = pointBonus_;
        at = at_;
        Array = false;
    }
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
        return (int)at_ <= 5; //5 and below are all reactive
    }

    public static Action_Type MapBuildingToAction(Building.Building_Type bt)
    {
        Action_Type at_ = Action_Type.NOTHING;
        switch ((int)bt)
        {
            case 0:
                at_ = Action_Type.Build_Dwelling;
                break;
            case 1:
                at_ = Action_Type.Build_TP;
                break;
            case 2:
                at_ = Action_Type.Build_Stronghold;
                break;
            case 3:
                at_ = Action_Type.Build_Temple;
                break;
            case 4:
                at_ = Action_Type.Build_Sanctuary;
                break;
        }

        return at_;
    }

    public static Building.Building_Type MapActionToBuilding(Action_Type at_)
    {
        Building.Building_Type bt = Building.Building_Type.NOTHING;
        switch ((int)at_)
        {
            case 6:
                bt = Building.Building_Type.Dwelling; break;
            case 7:
                bt = Building.Building_Type.Trading_Post; break;
            case 8:
                bt = Building.Building_Type.Temple; break;
            case 9:
                bt = Building.Building_Type.Sanctuary; break;
            case 10:
                bt = Building.Building_Type.Fortress; break;
        }
        return bt;
    }
}
