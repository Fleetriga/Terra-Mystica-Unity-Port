using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Income_Map{

    SingleIncome[] building_income_map;
    int current_index;


    public Income_Map(int no_buildings)
    {
        building_income_map = new SingleIncome[no_buildings + 1]; //+1 accounts for having 0 buildings of a certain type built
        current_index = 0;
    }

    public void AddIncome(SingleIncome si)
    {
        building_income_map[current_index] = si;
        current_index++;
    }

    int GetNextIndex()
    {
        int i = 0;
        foreach (SingleIncome si in building_income_map){
            if (si != null)
            {
                return i;
            }
            i++;
        }
        return building_income_map.Length;
    }

    public SingleIncome GetIncome(int i)
    {
        return building_income_map[i];
    }

    public int[] GetTotalIncome(int no_built)
    {
        int[] incomes = new int[5];
        for (int i = 0; i < no_built + 1; i++) //+1 includes having 0 buildings of a given type built
        {
            incomes[0] += building_income_map[i].Gold;
            incomes[1] += building_income_map[i].Worker;
            incomes[2] += building_income_map[i].Priest;
            incomes[3] += building_income_map[i].Magic;
            incomes[4] += building_income_map[i].Shovel;
        }

        return incomes;
    }

    public SingleIncome GetTotalIncomeAsSingleIncome(int no_built)
    {
        int[] temp = GetTotalIncome(no_built+1);
        return new SingleIncome(temp[0], temp[1], temp[2], temp[3], temp[4]);

    }


}
