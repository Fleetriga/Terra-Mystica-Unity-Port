using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell {

    SingleIncome spell_income;
    int mana_cost;
    int shovels;


    public Spell(int gold, int worker, int priest, int magic, int cost, int shovels)
    {
        spell_income = new SingleIncome(gold, worker, priest, magic);
        mana_cost = cost;
    }

    public SingleIncome GetIncome()
    {
        return spell_income;
    }

    public int GetCost()
    {
        return mana_cost;
    }


}
