using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round_Bonus {

    int bonus_gold;
    public PointBonus Round_Point_bonus { get; set; }
    public SingleIncome Round_Income { get; set; }

    public Round_Bonus()
    {
        Round_Point_bonus = null;
    }

    public Round_Bonus(PointBonus pb_)
    {
        Round_Point_bonus = pb_;
    }

    public Round_Bonus(PointBonus pb_, SingleIncome income_)
    {
        Round_Point_bonus = pb_;
        Round_Income = income_;
    }

    public void Pass_Up()
    {
        bonus_gold++;
    }

    public int Take()
    {
        int temp = bonus_gold;
        bonus_gold = 0;
        return temp;
    }


}
