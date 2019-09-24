using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonderController : MonoBehaviour {

    int[] remaining_fire;
    int[] remaining_water;
    int[] remaining_earth;
    int[] remaining_air;

    FavourBonus[] fire;
    FavourBonus[] water;
    FavourBonus[] earth;
    FavourBonus[] air;

    public WonderController()
    {
        remaining_fire = new int[] { 3, 3, 1 };
        remaining_water = new int[] { 3, 3, 1 };
        remaining_earth = new int[] { 3, 3, 1 };
        remaining_air = new int[] { 3, 3, 1 };
    }

    public void SetUp()
    {
        fire = new FavourBonus[] {  new FavourBonus(new SingleIncome(3,0,0,0,0), null, new CultIncome(1, 0, 0, 0)), //3g 1cult
                                   new FavourBonus(new SingleIncome(), null, new CultIncome(2, 0, 0, 0)), //NEEDS TOBE LOOKED AT
                                   new FavourBonus(new SingleIncome(), null, new CultIncome(3, 0, 0, 0))}; //3cult

        water = new FavourBonus[] { new FavourBonus(new SingleIncome(), new PointBonus(PointBonus.Action_Type.Build_TP, 3), new CultIncome(0, 1, 0, 0)), //tp=>3 1cult
                                   new FavourBonus(new SingleIncome(), null, new CultIncome(0, 2, 0, 0)), //NEEDS TO BE LOOKED AT
                                   new FavourBonus(new SingleIncome(), null, new CultIncome(0, 3, 0, 0))}; //3cult

        earth = new FavourBonus[] { new FavourBonus(new SingleIncome(), new PointBonus(PointBonus.Action_Type.Build_Dwelling, 2), new CultIncome(0, 0, 1, 0)), //dwel => 2 1cult
                                   new FavourBonus(new SingleIncome(1,1,0,0,0), null, new CultIncome(0, 0, 2, 0)), //1g 1w 1cult
                                   new FavourBonus(new SingleIncome(), null, new CultIncome(0, 0, 3, 0))}; //3cult

        air = new FavourBonus[] {   new FavourBonus(new SingleIncome(), new PointBonus(PointBonus.Action_Type.Have_TP, new int[]{ 2, 3, 3, 4}), new CultIncome(0, 0, 0, 1)), //2,3,3,4 points for 1,2,3,4 TP at end of turn
                                   new FavourBonus(new SingleIncome(0,0,0,4,0), null, new CultIncome(0, 0, 0, 2)), //4ma 1cult
                                   new FavourBonus(new SingleIncome(), null, new CultIncome(0, 0, 0, 3))}; //3cult
    }

    public FavourBonus GetWonderTile(int cult_track, int tier)
    {
        switch (cult_track)
        {
            case 0:
                if (remaining_fire[tier] > 0) { return fire[tier]; } break;
            case 1:
                if (remaining_water[tier] > 0) { return water[tier]; } break;
            case 2:
                if (remaining_earth[tier] > 0) { return earth[tier]; } break;
            case 3:
                if (remaining_air[tier] > 0) { return air[tier]; } break;
        }
        return null;
    }

    public void ReduceRemainingFavours(int track, int tier)
    {
        switch (track)
        {
            case 0:
                remaining_fire[tier]--; break;
            case 1:
                remaining_water[tier]--; break;
            case 2:
                remaining_earth[tier]--; break;
            case 3:
                remaining_air[tier]--; break;
        }
    }

    public int GetRemaining(int cult_track, int tier)
    {
        switch (cult_track)
        {
            case 0:
                if (remaining_fire[tier] > 0) { return remaining_fire[tier]--; }
                break;
            case 1:
                if (remaining_water[tier] > 0) { return remaining_water[tier]--; }
                break;
            case 2:
                if (remaining_earth[tier] > 0) { return remaining_earth[tier]--; }
                break;
            case 3:
                if (remaining_air[tier] > 0) { return remaining_air[tier]--; }
                break;
        }
        return 0;
    }
	
	
}
