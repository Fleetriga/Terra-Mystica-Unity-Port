using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Faction {

    public enum Faction_Type { Mermaids, Test};

    //***********************************Other faction variables
    protected Terrain.TerrainType habitat;
    protected Material building_material;

    //Player Income Map
    protected Income_Map dwelling_income;
    protected Income_Map tp_income;
    protected Income_Map fortress_income;
    protected Income_Map temple_income;
    protected Income_Map sanctuary_income;

    //Player starting counts
    public int Starting_gold;
    public int Starting_worker;
    public int Starting_priest;
    public int Starting_shovels;

    //Player Shovel cost
    public Income_Map Cost_Terraform { get; set; }
    protected int max_upgrade_terraform;

    //Max number of buildings
    public readonly int max_dwelling = 8;
    public readonly int max_tp = 4;
    public readonly int max_fortress = 1;
    public readonly int max_temple = 3;
    public readonly int max_sanctuary = 1;
    protected int starting_dwellings;

    //Shipping
    protected int defaultShipping;
    protected int[] defaultCult;

    //Cost to build by Faction
    protected int[] cost_build_dwelling;
    protected int[] cost_build_TP;
    protected int[] cost_build_stronghold;
    protected int[] cost_build_temple;
    protected int[] cost_build_sanctuary;

    //Upgrade tracks
    public SingleIncome cost_upgrade_shipping;
    public SingleIncome cost_upgrade_terraforming;

    //Magic start and current magic
    protected Magic magic_default;

    //Set IncomeMap
    abstract protected void SetIncomeMap(); //To be inherited
    abstract public void SetUp(); //To be inherited

    //upgrade terraforming and get cost of terraforming
    public SingleIncome Get_Cost_Terraform(int upgrade_terraform)
    {
        return Cost_Terraform.GetTotalIncomeAsSingleIncome(upgrade_terraform);
    }
    public bool Max_Upgrade_Terraform(int upgrade_terraform)
    {
        return upgrade_terraform == max_upgrade_terraform-1;
    }

    //Get Resources
    abstract public Sprite GetDefaultPieceSprite();

    //Accessor Methods
    public int GetRemainingStartingDwellings()
    {
        starting_dwellings--;
        return starting_dwellings;
    }

    //Building Cost
    public int[] Get_cost_build_dwelling()
    {
        return cost_build_dwelling;
    }
    public int[] Get_cost_build_TP()
    {
        return cost_build_TP;
    }
    public int[] Get_cost_build_stronghold()
    {
        return cost_build_stronghold;
    }
    public int[] Get_cost_build_temple()
    {
        return cost_build_temple;
    }
    public int[] Get_cost_build_sanctuary()
    {
        return cost_build_sanctuary;
    }

    //Magic
    public Magic GetDefaultMagic()
    {
        return magic_default;
    }

    //Building Income map
    public Income_Map Get_dwelling_income()
    {
        return dwelling_income;
    }
    public Income_Map Get_tp_income()
    {
        return tp_income;
    }
    public Income_Map Get_fortress_income()
    {
        return fortress_income;
    }
    public Income_Map Get_temple_income()
    {
        return temple_income;
    }
    public Income_Map Get_sanctuary_income()
    {
        return sanctuary_income;
    }

    //Habitat and build material
    public Terrain.TerrainType GetHabitat()
    {
        return habitat;
    }

    public Material Get_Building_Material()
    {
        return building_material;
    }
    //Shipping
    public int GetDefaultShipping()
    {
        return defaultShipping;
    }
    //Cult default
    public int[] Get_Default_CultData()
    {
        return defaultCult;
    }

    public static string GetFaction(Faction_Type ft)
    {
        switch (ft)
        {
            case Faction_Type.Mermaids: return Mermaids.FactionName;
            case Faction_Type.Test: return "TestFaction not implemented at all lad";
        }
        return "fucked if i know how you even managed this";
    }

    public static Faction GetNewFaction(Faction_Type ft)
    {
        switch (ft)
        {
            case Faction_Type.Mermaids: return new Mermaids();
            case Faction_Type.Test: return new Mermaids();
        }
        return null;
    }

    public static Material GetFactionMaterial(Faction_Type ft)
    {
        switch (ft)
        {
            case Faction_Type.Mermaids: return Resources.Load<Material>(Mermaids.FactionMaterial);
            case Faction_Type.Test: return Resources.Load<Material>(TestFaction.FactionMaterial);
        }
        return Resources.Load<Material>(Mermaids.FactionMaterial);
    }

}
