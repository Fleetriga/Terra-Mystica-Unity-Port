using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round_Bonus_Manager : MonoBehaviour {

    Round_Bonus[] round_bonuses;
    public GameObject[] round_bonus_objects;
    List<Round_Bonus_Types> taken;

    public enum Round_Bonus_Types { test, test2, NOTHING };

	public void Set_Up()
    {
        round_bonuses = new Round_Bonus[] { new Round_Bonus(new PointBonus(PointBonus.Action_Type.Have_Dwelling, 1), new SingleIncome(0,2,0,0)), new Round_Bonus(new PointBonus(PointBonus.Action_Type.Build_Dwelling, 2), new SingleIncome(3,0,0,0))};
        taken = new List<Round_Bonus_Types>();
    }

    public Round_Bonus Get_Round_Bonus(Round_Bonus_Types rbt)
    {
        return round_bonuses[(int)rbt];
    }

    public void Take_Round_Bonus(Round_Bonus_Types rbt)
    {
        round_bonus_objects[(int)rbt].gameObject.SetActive(false);
        taken.Add(rbt);
    }

    public void Reset_Available_Bonuses()
    {
        foreach (Round_Bonus_Types t in taken)
        {
            round_bonus_objects[(int)t].SetActive(true);
        }
        taken = new List<Round_Bonus_Types>();
    }

}
