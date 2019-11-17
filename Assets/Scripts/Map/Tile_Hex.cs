using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Hex : Tile {


    public override Renderer GetRenderer()
    {
        return transform.GetChild(0).GetChild(0).GetComponent<Renderer>();
    }
}
