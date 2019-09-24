using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
public class Coordinate
{
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()

    int x;
    int y;

	public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int GetX()
    {
        return x;
    }

    public int GetY()
    {
        return y;
    }

    public int[] GetXY()
    {
        return new int[] {x,y};
    }

    public override bool Equals(object obj)
    {
        return (x == ((Coordinate)obj).GetX() && y == ((Coordinate)obj).GetY());
    }
}
