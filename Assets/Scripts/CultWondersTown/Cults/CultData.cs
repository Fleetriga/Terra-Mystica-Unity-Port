using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultData {

    int[] levels;
    int owed_magic;

    public CultData(int start_fi, int start_wa, int start_ea, int start_air)
    {
        levels = new int[4];
        levels[0] = start_fi;
        levels[1] = start_wa;
        levels[2] = start_ea;
        levels[3] = start_air;
    }
    public CultData(int[] default_Levels)
    {
        levels = new int[4];
        levels[0] = default_Levels[0];
        levels[1] = default_Levels[1];
        levels[2] = default_Levels[2];
        levels[3] = default_Levels[3];
    }

    //Returns true if 10 is reached
    public bool Add_To_Track_Level(CultIncome income, int[] max)
    {
        int temp_value; //Just for comparing before/after

        for (int i = 0; i < 4; i++)
        {
            temp_value = levels[i];
            levels[i] += income.Get_Income()[i];
            Check_magic_owed(temp_value, levels[i]); //if the cult level passes a certain point then the player recieves magic;
        }

        //Make everything the max
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] > max[i]) { levels[i] = max[i]; }
        }

        //Check the max
        if (levels[0] >= 10 && max[0] == 10) { return true; }
        if (levels[0] >= 10 && max[0] == 10) { return true; }
        if (levels[0] >= 10 && max[0] == 10) { return true; }
        if (levels[0] >= 10 && max[0] == 10) { return true; }

        return false;
    }

    public int Get_Owed_Magic()
    {
        int temp = owed_magic;
        owed_magic = 0;
        return temp;
    }

    //doesn't use ELSE IF because a player may infact pass 2 checkpoints
    void Check_magic_owed(int before, int after)
    {
        if (before < 3 && after >= 3) //Rose above 2 and is owed 1 magic
        {
            owed_magic += 1;
        }
        if (before < 5 && after >= 5) //Rose above 4 and is owed 2 magic
        {
            owed_magic += 2;
        }
        if (before < 7 && after >= 7) //Rose above 6 and is owed 2 magic
        {
            owed_magic += 2;
        }
        if (before < 10 && after >= 10) //Rose above 9 and is owed 3 magic
        {
            owed_magic += 3;
        }
    }

    public int[] Get_Levels()
    {
        return levels;
    }


}
