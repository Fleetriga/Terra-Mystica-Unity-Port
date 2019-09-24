using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileLine : MonoBehaviour {
    public Tile[] line;

    public Tile GetTile(int i)
    {
        return line[i];
    }
}
