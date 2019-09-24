using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavourBonus {

    //income
    SingleIncome income { get; set;}
    CultIncome cult_income;
    //point bonus
    PointBonus pointBonus;
    public int Points { get; set; }

    /**For favour tiles */
    public FavourBonus(SingleIncome i_, PointBonus pb_, CultIncome ci_)
    {
        income = i_;
        pointBonus = pb_;
        cult_income = ci_;
    }

    /**For town tiles */
    public FavourBonus(SingleIncome i_, int points_, CultIncome ci_)
    {
        income = i_;
        Points = points_;
        cult_income = ci_;
    }

    public SingleIncome GetIncome()
    {
        return income;
    }

    public PointBonus GetPointBonus()
    {
        return pointBonus;
    }

    public CultIncome Get_CultIncome()
    {
        return cult_income;
    }
}
