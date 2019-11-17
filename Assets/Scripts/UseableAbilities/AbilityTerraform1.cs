using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTerraform1 : UseableAbility
{
    public GameController controller;
    public GlobalFlags global;

    public void UseAbility(int magicType)
    {
        controller.AddShovel();
    }
}
