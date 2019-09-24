using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityTerraform1 : UseableAbility
{
    public GameController controller;
    public Global_Flags global;

    public void UseAbility(int magicType)
    {
        controller.AddShovel();
    }
}
