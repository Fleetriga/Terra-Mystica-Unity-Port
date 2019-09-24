using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUIElement : MonoBehaviour {
    bool down;

    private void Awake()
    {
        down = true;
    }

    public void MoveUI(int amount)
    {
        if (down)
        {
            transform.Translate(new Vector3(0, amount, 0));
            down = false;
        }
        else
        {
            transform.Translate(new Vector3(0, amount*-1, 0));
            down = true;
        }
        
    }
}
