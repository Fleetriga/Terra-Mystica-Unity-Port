using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleIncome{

    public int Gold { get; set;}
    public int Worker { get; set;  }
    public int Priest { get; set; }
    public int Magic { get; set; }

    public SingleIncome(int g, int w, int p, int ma)
    {
        Gold = g;
        Worker = w;
        Priest = p;
        Magic = ma;
    }

    public SingleIncome()
    {
        Gold = 0;
        Worker = 0;
        Priest = 0;
        Magic = 0;
    }

    public int[] Income_AsArray()
    {
        return new int[] { Gold, Worker, Priest, Magic};
    }
    
    public SingleIncome Reciprocal()
    {
        return new SingleIncome(Gold*-1, Worker*-1, Priest*-1, Magic*-1);
    }
}
