using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleIncome{

    public int Gold { get; set;}
    public int Worker { get; set;  }
    public int Priest { get; set; }
    public int Magic { get; set; }
    public int Shovel;

    public SingleIncome(int g, int w, int p, int ma, int shovels)
    {
        Gold = g;
        Worker = w;
        Priest = p;
        Magic = ma;
        Shovel = shovels;
    }

    public SingleIncome()
    {
        Gold = 0;
        Worker = 0;
        Priest = 0;
        Magic = 0;
    }

    public int[] IncomeAsArray()
    {
        return new int[] { Gold, Worker, Priest, Magic, Shovel};
    }
    
    public SingleIncome Reciprocal()
    {
        return new SingleIncome(Gold*-1, Worker*-1, Priest*-1, Magic*-1, Shovel*-1);
    }

    public SingleIncome Multiply(int times)
    {
        return new SingleIncome(Gold*times, Worker*times, Priest*times, Magic*times, Shovel*times);
    }
}
