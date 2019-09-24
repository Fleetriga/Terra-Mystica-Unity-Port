using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round_Bonus {

    int bonusGold;
    public PointBonus RoundPointbonus { get; set; }
    public SingleIncome Round_Income { get; set; }
    public RoundBonusManager.RoundBonusType Type;

    public Round_Bonus()
    {
        RoundPointbonus = null;
    }

    public Round_Bonus(PointBonus pb_, RoundBonusManager.RoundBonusType type_)
    {
        RoundPointbonus = pb_;
        Type = type_;
    }

    public Round_Bonus(PointBonus pb_, SingleIncome income_, RoundBonusManager.RoundBonusType type_)
    {
        RoundPointbonus = pb_;
        Round_Income = income_;
        Type = type_;
    }

    public void Pass_Up()
    {
        bonusGold++;
    }

    public int Take()
    {
        int temp = bonusGold;
        bonusGold = 0;
        return temp;
    }


}
