using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlags : MonoBehaviour {

    Building.Building_Type build_flag;
    Terrain.TerrainType terra_flag;
    MagicController.SpellType magic_cast_flag;
    RoundBonusManager.RoundBonusType round_bonus_flag;
    bool build_for_free;

    //Properties
    public Building.Building_Type BuildFlag { get { return build_flag; } }
    public Terrain.TerrainType Terraform_Flag { get { return terra_flag; } }
    public MagicController.SpellType MagicCastFlag { get { return magic_cast_flag; } }
    public RoundBonusManager.RoundBonusType RoundBonusFlag { get { return round_bonus_flag; } }
    public bool FreeBuildFlag { get { return build_for_free; } set { build_for_free = value; } }

    public bool DebugRAISENEIGHBOURS = false;

    public void Set_Up()
    {
        ResetFlags();
    }

    public void ResetFlags()
    {
        build_flag = Building.Building_Type.NOTHING;
        terra_flag = Terrain.TerrainType.NOTHING;
        magic_cast_flag = MagicController.SpellType.NOTHING;
        round_bonus_flag = RoundBonusManager.RoundBonusType.NOTHING;
        build_for_free = false;
    }

    public void Set_Building_Flag(int flag_)
    {
        ResetFlags();
        build_flag = (Building.Building_Type)flag_;
    }

    public void Set_Terraform_Flag(int flag_)
    {
        ResetFlags();
        terra_flag = (Terrain.TerrainType)flag_;
    }

    public void Set_Magic_Flag(int flag_)
    {
        ResetFlags();
        magic_cast_flag = (MagicController.SpellType)flag_;
    }

    public void Set_Round_Bonus_Flag(int flag_)
    {
        ResetFlags();
        round_bonus_flag = (RoundBonusManager.RoundBonusType)flag_;
    }
}
