using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mermaids : Faction {
    public static string FactionName = "Mermaids";
    public static string FactionMaterial = "Factions/Blue/Material";

    public override void SetUp()
    {
        //Building cost and update income costs on UI
        cost_build_dwelling = new int[] { 2, 1 };
        cost_build_TP = new int[] { 3, 2 };
        cost_build_stronghold = new int[] { 6, 4 };
        cost_build_temple = new int[] { 6, 4 };
        cost_build_sanctuary = new int[] { 6, 4 };

        defaultShipping = 0;
        defaultCult = new int[] { 0,2,0,0};

        magic_default = new Magic(7, 5, 0);

        habitat = Terrain.TerrainType.Swamp;

        max_upgrade_terraform = 3;

        cost_upgrade_shipping = new SingleIncome(4,0,1,0,0);
        cost_upgrade_terraforming = new SingleIncome(5,2,1,0,0);

        starting_dwellings = 2;

        Starting_gold = 50;
        Starting_worker = 50;
        Starting_priest = 0;
        Starting_shovels = 0;

        SetIncomeMap();
    }

    protected override void SetIncomeMap()
    {
        //Set up Income map
        dwelling_income = new Income_Map(max_dwelling);
        tp_income = new Income_Map(max_tp);
        fortress_income = new Income_Map(max_fortress);
        temple_income = new Income_Map(max_temple);
        sanctuary_income = new Income_Map(max_sanctuary);
        Cost_Terraform = new Income_Map(max_upgrade_terraform+1);

        //Dwellings
        dwelling_income.AddIncome(new SingleIncome(0, 1, 0, 0, 0)); //1
        dwelling_income.AddIncome(new SingleIncome(0, 1, 0, 0, 0)); //2
        dwelling_income.AddIncome(new SingleIncome(0, 1, 0, 0, 0)); //3
        dwelling_income.AddIncome(new SingleIncome(0, 1, 0, 0, 0)); //4
        dwelling_income.AddIncome(new SingleIncome(0, 1, 0, 0, 0)); //5
        dwelling_income.AddIncome(new SingleIncome(0, 1, 0, 0, 0)); //6
        dwelling_income.AddIncome(new SingleIncome(0, 1, 0, 0, 0)); //7 
        dwelling_income.AddIncome(new SingleIncome(0, 1, 0, 0, 0)); //8

        //tps
        tp_income.AddIncome(new SingleIncome(2, 0, 0, 1, 0));
        tp_income.AddIncome(new SingleIncome(2, 0, 0, 1, 0));
        tp_income.AddIncome(new SingleIncome(2, 0, 0, 2, 0));
        tp_income.AddIncome(new SingleIncome(2, 0, 0, 2, 0));

        //fortress
        fortress_income.AddIncome(new SingleIncome(0, 0, 0, 4, 0));

        //Temple
        temple_income.AddIncome(new SingleIncome(0, 0, 1, 0, 0));
        temple_income.AddIncome(new SingleIncome(0, 0, 1, 0, 0));
        temple_income.AddIncome(new SingleIncome(0, 0, 1, 0, 0));

        //Sanctuary
        sanctuary_income.AddIncome(new SingleIncome(0, 0, 1, 0, 0));

        //terraform cost
        Cost_Terraform.AddIncome(new SingleIncome(0, 3, 0, 0, 0));
        Cost_Terraform.AddIncome(new SingleIncome(0, -1, 0, 0, 0)); //when upgraded it becomes 1 cheaper
        Cost_Terraform.AddIncome(new SingleIncome(0, -1, 0, 0, 0));
    }

    public override Sprite GetDefaultPieceSprite()
    {
        return Resources.Load<Sprite>("Factions/Blue/Sprites/PlayerPiece");
    }
}
