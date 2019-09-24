using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultIncome {

    int[] levels;

    public CultIncome(int income_fi, int income_wa, int income_ea, int income_air)
    {
        levels = new int[4];

        levels[0] = income_fi;
        levels[1] = income_wa;
        levels[2] = income_ea;
        levels[3] = income_air;
    }

    public int[] GetIncome()
    {
        return levels;
    }

    //Returns true if this cult income has no value
    public bool GetNull()
    {
        if (levels[0] == 0 && levels[1] == 0 && levels[4] == 0 && levels[3] == 0) { return true; }

        return false;
    }

}
