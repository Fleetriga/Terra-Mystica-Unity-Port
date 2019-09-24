using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultController 
{
    int[] max;

    public CultController()
    {
        max = new int[4] { 10, 10, 10, 10};
    }

    internal static int[] ParseInt(int tracktier)
    {
        int track = int.Parse(tracktier.ToString().ToCharArray()[0].ToString());
        int tier = int.Parse(tracktier.ToString().ToCharArray()[1].ToString());

        return new int[] { track, tier };
    }

    public int Add_To_Track_Level(CultIncome income, Player pl)
    {
        if (pl.Add_To_Track_Level(income, max))
        {
            //Rework the max
            for (int i = 0; i < pl.Get_CultData().Get_Levels().Length; i++)
            {
                if (pl.Get_CultData().Get_Levels()[i] == 10) { max[i] = 9;}
            }
        }
        return pl.Get_Owed_CultMagic();
    }
}