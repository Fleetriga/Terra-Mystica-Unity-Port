using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Faction {

    //Blue, yellow, grey, red, black, Green, Brown, Others
    public enum FactionType { Mermaids, Swarmlings, Nomads, Fakirs, Dwarves, Gnomes, Chaos_Magicians, Giants, Darklings, Alchemists,
                                Witches, Auren, Halflings, Cultists, Test, NOFACTION};

    //***********************************Other faction variables
    public FactionType factionType;
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
    public Income_Map GetDwellingIncome()
    {
        return dwelling_income;
    }
    public Income_Map GetTpIncome()
    {
        return tp_income;
    }
    public Income_Map GetFortressIncome()
    {
        return fortress_income;
    }
    public Income_Map GetTempleIncome()
    {
        return temple_income;
    }
    public Income_Map GetSanctuaryIncome()
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

    public static string GetFactionName(FactionType ft)
    {
        switch (ft)
        {
            case FactionType.Mermaids: return Mermaids.FactionName;
            case FactionType.Swarmlings: return Swarmlings.FactionName;
            case FactionType.Alchemists: return Alchemists.FactionName;
            case FactionType.Auren: return Auren.FactionName;
            case FactionType.Chaos_Magicians: return ChaosMagicians.FactionName;
            case FactionType.Cultists: return Cultists.FactionName;
            case FactionType.Darklings: return Darklings.FactionName;
            case FactionType.Dwarves: return Dwarves.FactionName;
            case FactionType.Fakirs: return Fakirs.FactionName;
            case FactionType.Giants: return Giants.FactionName;
            case FactionType.Gnomes: return Gnomes.FactionName;
            case FactionType.Halflings: return Halflings.FactionName;
            case FactionType.Nomads: return Nomads.FactionName;
            case FactionType.Witches: return Witches.FactionName;

            case FactionType.Test: return "TestFaction not implemented at all lad";
        }
        return "fucked if i know how you even managed this";
    }

    public static Faction GetNewFaction(FactionType ft)
    {
        switch (ft)
        {
            case FactionType.Mermaids: return new Mermaids();
            case FactionType.Swarmlings: return new Swarmlings();
            case FactionType.Alchemists: return new Alchemists();
            case FactionType.Auren: return new Auren();
            case FactionType.Chaos_Magicians: return new ChaosMagicians();
            case FactionType.Cultists: return new Cultists();
            case FactionType.Darklings: return new Darklings();
            case FactionType.Dwarves: return new Dwarves();
            case FactionType.Fakirs: return new Fakirs();
            case FactionType.Giants: return new Giants();
            case FactionType.Gnomes: return new Gnomes();
            case FactionType.Halflings: return new Halflings();
            case FactionType.Nomads: return new Nomads();
            case FactionType.Witches: return new Witches();

            case FactionType.Test: return new TestFaction();
        }
        return null;
    }

    public static Material GetFactionMaterial(FactionType ft)
    {
        switch (ft)
        {
            case FactionType.Mermaids: return Resources.Load<Material>(Mermaids.FactionMaterial);
            case FactionType.Swarmlings: return Resources.Load<Material>(Swarmlings.FactionMaterial);
            case FactionType.Alchemists: return Resources.Load<Material>(Alchemists.FactionMaterial);
            case FactionType.Auren: return Resources.Load<Material>(Auren.FactionMaterial);
            case FactionType.Chaos_Magicians: return Resources.Load<Material>(ChaosMagicians.FactionMaterial);
            case FactionType.Cultists: return Resources.Load<Material>(Cultists.FactionMaterial);
            case FactionType.Darklings: return Resources.Load<Material>(Darklings.FactionMaterial);
            case FactionType.Dwarves: return Resources.Load<Material>(Dwarves.FactionMaterial);
            case FactionType.Fakirs: return Resources.Load<Material>(Fakirs.FactionMaterial);
            case FactionType.Giants: return Resources.Load<Material>(Giants.FactionMaterial);
            case FactionType.Gnomes: return Resources.Load<Material>(Gnomes.FactionMaterial);
            case FactionType.Halflings: return Resources.Load<Material>(Halflings.FactionMaterial);
            case FactionType.Nomads: return Resources.Load<Material>(Nomads.FactionMaterial);
            case FactionType.Witches: return Resources.Load<Material>(Witches.FactionMaterial);

            case FactionType.Test: return Resources.Load<Material>(TestFaction.FactionMaterial);
        }
        return Resources.Load<Material>(Mermaids.FactionMaterial);
    }

}
