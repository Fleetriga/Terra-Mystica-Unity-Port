﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCastMagic : UseableAbility {
    public GameController controller;
    public Global_Flags global;

	public void CastMagic(int magicType)
    {
        global.Set_Magic_Flag(magicType);
        controller.CastMagic();
    }
}