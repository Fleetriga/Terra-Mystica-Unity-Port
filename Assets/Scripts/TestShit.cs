using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShit : MonoBehaviour {

    Player p;

    void Awake()
    {
        p = GetComponent<Player>();    
    }

    public void AddAMagic()
    {
        p.AddSingleIncome(new SingleIncome(0,0,0,1,0));
        p.UpdateResourceText();
    }
}
