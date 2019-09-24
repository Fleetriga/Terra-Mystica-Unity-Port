using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_Flags : MonoBehaviour {

    Building.Building_Type build_flag;
    Terrain.TerrainType terra_flag;
    MagicController.Spell_Type magic_cast_flag;
    Round_Bonus_Manager.Round_Bonus_Types round_bonus_flag;

    //Properties
    public Building.Building_Type Build_Flag { get { return build_flag; } }
    public Terrain.TerrainType Terraform_Flag { get { return terra_flag; } }
    public MagicController.Spell_Type Magic_Cast_Flag { get { return magic_cast_flag; } }
    public Round_Bonus_Manager.Round_Bonus_Types Round_Bonus_Flag { get { return round_bonus_flag; } }

    public void Set_Up()
    {
        Reset_Flags();
    }

    public void Reset_Flags()
    {
        build_flag = Building.Building_Type.NOTHING;
        terra_flag = Terrain.TerrainType.NOTHING;
        magic_cast_flag = MagicController.Spell_Type.NOTHING;
        round_bonus_flag = Round_Bonus_Manager.Round_Bonus_Types.NOTHING;
    }

    public void Set_Building_Flag(int flag_)
    {
        Reset_Flags();
        build_flag = (Building.Building_Type)flag_;
    }

    public void Set_Terraform_Flag(int flag_)
    {
        Reset_Flags();
        terra_flag = (Terrain.TerrainType)flag_;
    }

    public void Set_Magic_Flag(int flag_)
    {
        Reset_Flags();
        magic_cast_flag = (MagicController.Spell_Type)flag_;
    }

    public void Set_Round_Bonus_Flag(int flag_)
    {
        Reset_Flags();
        round_bonus_flag = (Round_Bonus_Manager.Round_Bonus_Types)flag_;
    }
}
