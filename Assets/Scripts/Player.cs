using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    //Player resources
    int money_count;
    int worker_count;
    int priest_count;
    int shovel_count;
    Magic tier_magic;
    public int Points { get; set; }
    int shipping;
    int current_upgrade_terraform;
    CultData cd;

    //Roundly variables
    public Round_Bonus Current_Round_Bonus { get; set; }

    //Player ID
    public int playerID;

    //Player income
    int money_income;
    int worker_income;
    int priest_income;
    int magic_income;

    //Any bonuses to incomes that arent from buildings
    int bonus_gold;
    int bonus_worker;
    int bonus_priest;
    int bonus_magic;

    //Current number of X building built;
    int current_dwelling;
    int current_tp;
    int current_fortress;
    int current_temple;
    int current_sanctuary;

    public int PointsForTown { get; set; }

    //Current Coordinates of buildings built
    List<Coordinate> buildingLocs;

    //A list of the players PlayerPieces
    PlayerPiece[] playerPieces;
    public GameObject playerPeice;

    //Points
    PointKeeper pk;

    UI_Updater ui;

    public Faction faction;

    // Use this for initialization
    public void SetUp (Faction.Faction_Type p_faction, int ID) {
        //Set ID
        playerID = ID;
        
        //Create player UI pieces. 7 pieces.
        playerPieces = new PlayerPiece[] { Instantiate(playerPeice).GetComponent<PlayerPiece>(), Instantiate(playerPeice).GetComponent<PlayerPiece>() , Instantiate(playerPeice).GetComponent<PlayerPiece>(),  Instantiate(playerPeice).GetComponent<PlayerPiece>() ,
                                           Instantiate(playerPeice).GetComponent<PlayerPiece>(), Instantiate(playerPeice).GetComponent<PlayerPiece>()};

        //Set the faction and tell it to set up an income map.
        faction = Faction.GetNewFaction(p_faction);
        faction.SetUp();

        //Setup playerPieces
        SetUpPieces();

        //points
        pk = new PointKeeper();
        Points = 20;

        //Shipping & terraform upgrade levels
        shipping = faction.GetDefaultShipping();
        current_upgrade_terraform = 0;

        //Starting building locations is empty
        buildingLocs = new List<Coordinate>();

        //Starting cult tracks
        cd = new CultData(faction.Get_Default_CultData());

        //Setup UI to start with
        ui = GameObject.Find("UI").GetComponent<UI_Updater>();
        ui.UpdatePlayerBuildingCosts(faction.Get_cost_build_dwelling(), faction.Get_cost_build_TP(), faction.Get_cost_build_temple(), faction.Get_cost_build_sanctuary(), faction.Get_cost_build_stronghold());
        ui.UpdatePlayerCultTrack(cd);

        //Starting resources and income and update UI resources/income
        money_count = faction.Starting_gold;
        worker_count = faction.Starting_worker;
        priest_count = faction.Starting_priest;
        shovel_count = faction.Starting_shovels;

        CalculateIncome();

        tier_magic = faction.GetDefaultMagic();

        PointsForTown = 7;

        //Update the UI after setting up all variables
        UpdateResourceText();

        //Give yourself to the loop controller
        //GameObject.Find("Controller").GetComponent<Game_Loop_Controller>().Add_Player();
	}

    void SetUpPieces()
    {
        foreach(PlayerPiece pp in playerPieces)
        {
            pp.SetUp(playerID, faction.GetDefaultPieceSprite());
        }
    }

    void CalculateIncome()
    {
        ResetIncomes();

        //For dwells
        int[] income = faction.Get_dwelling_income().Get_Total_Income(current_dwelling);
        AddTo_Income(income);
        
        //For tps
        income = faction.Get_tp_income().Get_Total_Income(current_tp);
        AddTo_Income(income);

        //For fortress
        income = faction.Get_fortress_income().Get_Total_Income(current_fortress);
        AddTo_Income(income);

        //For temples
        income = faction.Get_temple_income().Get_Total_Income(current_temple);
        AddTo_Income(income);

        //For sancuary
        income = faction.Get_sanctuary_income().Get_Total_Income(current_sanctuary);
        AddTo_Income(income);

        income = new int[] { bonus_gold, bonus_worker, bonus_priest, bonus_magic };
        AddTo_Income(income);
    }

    //Adds one income to another.
    void AddTo_Income(int[] income)
    {
        money_income += income[0];
        worker_income += income[1];
        priest_income += income[2];
        magic_income += income[3];
    }

    //Adds income as a permanant increase
    public void Add_To_NonBuilding_Income(int[] income)
    {
        bonus_gold += income[0];
        bonus_worker += income[1];
        bonus_priest += income[2];
        bonus_magic += income[3];
        CalculateIncome();
        UpdateResourceText();
    }

    //Adds income as a one time only
    public void AddSingleIncome(SingleIncome income)
    {
        int[] incomes = income.Income_AsArray();
        money_count += incomes[0];
        worker_count += incomes[1];
        priest_count += incomes[2];
        tier_magic.AddMagic(incomes[3]);
        UpdateResourceText();
    }

    void ResetIncomes()
    {
        money_income = 0;
        worker_income = 0;
        priest_income = 0;
        magic_income = 0;
    }

    //Building and Terraforming
    public bool CheckCanBuild(Building.Building_Type bt)
    {
        int[] temp = null;
        switch (bt)
        {
            case Building.Building_Type.Dwelling:
                temp = faction.Get_cost_build_dwelling(); break;
            case Building.Building_Type.Trading_Post:
                temp = faction.Get_cost_build_TP(); break;
            case Building.Building_Type.Fortress:
                temp = faction.Get_cost_build_stronghold(); break; 
            case Building.Building_Type.Sanctuary:
                temp = faction.Get_cost_build_temple(); break; 
            case Building.Building_Type.Temple:
                temp = faction.Get_cost_build_sanctuary(); break; 
        }
        if (money_count - temp[0] >= 0 && worker_count - temp[1] >= 0) { return true; }
        return false;
    }

    public int GetID()
    {
        return playerID;
    }

    public bool CheckCanTerraform(Terrain.TerrainType target, Terrain.TerrainType tile)
    {
        int distance = Terrain.GetDistance(target, tile);

        //If player has either enough shovels or has enough resources to buy shovels
        if (distance <= shovel_count || distance-shovel_count * faction.Get_Cost_Terraform(current_upgrade_terraform).Worker <= worker_count)
        {
            return true;
        }
        return false;
    }

    public bool CheckWonderTile(Building.Building_Type bt)
    {
        return Building.Equals(bt, Building.Building_Type.Sanctuary) || Building.Equals(bt, Building.Building_Type.Temple);
    }

    public void Terraform(Terrain.TerrainType target, Terrain.TerrainType tile)
    {
        int distance = Terrain.GetDistance(target, tile);

        for (int i = 0; i < distance; i++)
        {
            if (shovel_count > 0)
            {
                shovel_count--;
            }
            else //If we still need to terraform, convert workers into shovels and then use them
            {
                AddSingleIncome(faction.Get_Cost_Terraform(current_upgrade_terraform).Reciprocal());
            } 
        }
    }

    public void Build(Building.Building_Type bt)
    {
        switch (bt)
        {
            case Building.Building_Type.Dwelling:
                money_count = money_count - faction.Get_cost_build_dwelling()[0];
                worker_count = worker_count - faction.Get_cost_build_dwelling()[1];
                current_dwelling++;
                break;
            case Building.Building_Type.Trading_Post:
                //First calculate resources and all that
                money_count = money_count - faction.Get_cost_build_TP()[0];
                worker_count = worker_count - faction.Get_cost_build_TP()[1];
                //Then update UI for building which gained another of itself
                ui.DisplayRemaining(Building.Building_Type.Dwelling, Get_Remaining_Index(Building.Building_Type.Dwelling));
                current_tp++; current_dwelling--;
                break;
            case Building.Building_Type.Fortress:
                money_count = money_count - faction.Get_cost_build_stronghold()[0];
                worker_count = worker_count - faction.Get_cost_build_stronghold()[1];
                ui.DisplayRemaining(Building.Building_Type.Trading_Post, Get_Remaining_Index(Building.Building_Type.Trading_Post));
                current_fortress++; current_tp--;
                break;
            case Building.Building_Type.Temple:
                money_count = money_count - faction.Get_cost_build_temple()[0];
                worker_count = worker_count - faction.Get_cost_build_temple()[1];
                ui.DisplayRemaining(Building.Building_Type.Trading_Post, Get_Remaining_Index(Building.Building_Type.Trading_Post));
                current_temple++; current_tp--;
                break;
            case Building.Building_Type.Sanctuary:
                money_count = money_count - faction.Get_cost_build_sanctuary()[0];
                worker_count = worker_count - faction.Get_cost_build_sanctuary()[1];
                ui.DisplayRemaining(Building.Building_Type.Temple, Get_Remaining_Index(Building.Building_Type.Temple));
                current_sanctuary++; current_temple--;
                break;
        }
        //Finally update UI for building which is losing one of itself
        ui.DisplayRemaining(bt, Get_Remaining_Index(bt));
    }

    //Gives you the remaning number of buildings of given type in the players building pool
    int Get_Remaining_Index(Building.Building_Type bt)
    {
        switch (bt)
        {
            case Building.Building_Type.Dwelling: return faction.max_dwelling - current_dwelling;
            case Building.Building_Type.Trading_Post: return faction.max_tp - current_tp;
            case Building.Building_Type.Fortress: return faction.max_fortress - current_fortress;
            case Building.Building_Type.Temple: return faction.max_temple - current_temple;
            case Building.Building_Type.Sanctuary: return faction.max_sanctuary - current_sanctuary;
        }
        return 0;
    }

    internal void AddSinglePointValue(int points)
    {
        Points += points;
        UpdateResourceText();
    }

    public void FreeBuild(Building.Building_Type bt)
    {
        switch (bt)
        {
            case Building.Building_Type.Dwelling:
                current_dwelling++;
                break;
            case Building.Building_Type.Trading_Post:
                ui.DisplayRemaining(Building.Building_Type.Dwelling, Get_Remaining_Index(Building.Building_Type.Dwelling));
                current_tp++; current_dwelling--;
                break;
            case Building.Building_Type.Fortress:
                ui.DisplayRemaining(Building.Building_Type.Trading_Post, Get_Remaining_Index(Building.Building_Type.Trading_Post));
                current_fortress++; current_tp--;
                break;
            case Building.Building_Type.Sanctuary:
                ui.DisplayRemaining(Building.Building_Type.Trading_Post, Get_Remaining_Index(Building.Building_Type.Trading_Post));
                current_temple++; current_tp--;
                break;
            case Building.Building_Type.Temple:
                current_sanctuary++; current_temple--;
                ui.DisplayRemaining(Building.Building_Type.Temple, Get_Remaining_Index(Building.Building_Type.Temple));
                break;
        }
        ui.DisplayRemaining(bt, Get_Remaining_Index(bt));
    }

    public void AddBuiltCoordinate(Tile t)
    {
        buildingLocs.Add(t.GetCoordinates());
    }

    public List<Coordinate> GetBuildingLocs()
    {
        return buildingLocs;
    }

    //Upgrading Terraforming and Shipping
    //Split up in case they will be expanded upon later
    public bool UpgradeShipping()
    {
        SingleIncome cost = faction.cost_upgrade_shipping;
        //First check if it can be done
        if (money_count >= cost.Gold && worker_count >= cost.Worker && priest_count >= cost.Priest && tier_magic.GetTiers()[2] >= cost.Magic && shipping != 4)//and its not at max shipping
        {
            AddSingleIncome(cost.Reciprocal());
            shipping++;
            return true;
        }
        return false;
    }

    public bool UpgradeTerraforming()
    {
        SingleIncome cost = faction.cost_upgrade_terraforming;
        //First check if it can be done
        if (money_count >= cost.Gold && worker_count >= cost.Worker && priest_count >= cost.Priest && tier_magic.GetTiers()[2] >= cost.Magic && !faction.Max_Upgrade_Terraform(current_upgrade_terraform))//and its not at max shipping
        {
            AddSingleIncome(cost.Reciprocal());
            current_upgrade_terraform++;
            return true;
        }
        return false;
    }

    public int Current_Upgrade_Terraform()
    {
        return current_upgrade_terraform;
    }

    //Spells
    public bool CheckCanCastSpell(Spell s)
    {
        return tier_magic.GetTiers()[2] >= s.GetCost();
    }

    public void CastSpell(Spell s)
    {
        tier_magic.UseSpell(s.GetCost());
        AddSingleIncome(s.GetIncome());
    }

    public void BurnMagic()
    {
        tier_magic.BurnMagic(1);
        UpdateResourceText();
    }

    //Cults
    public bool Add_To_Track_Level(CultIncome income, int[] max)
    {
        bool temp = cd.Add_To_Track_Level(income, max);
        ui.UpdatePlayerCultTrack(cd);
        return temp;
    }
    public CultData Get_CultData()
    {
        return cd;
    }
    public int Get_Owed_CultMagic()
    {
        return cd.Get_Owed_Magic();
    }

    //Points
    public void CheckPoints(PointBonus.Action_Type at_)
    {
        Points += pk.GetPointBonus(at_);
    }

    public void UpdateResourceText()
    {
        ui.UpdateResourceText(new int[] { money_count, worker_count, priest_count, shovel_count }, new int[] { money_income, worker_income, priest_income});
        ui.UpdateMagicTiers(tier_magic.GetTiers());
        ui.UpdatePoints(Points);
    }

    public void Add_Point_Bonus(PointBonus pb_, bool temporary)
    {
        pk.AddPointBonus(pb_, temporary);
    }

    public void Reset_Temporary_Point_Bonuses()
    {
        pk.ResetTempBonuses();
    }

    /** First calculates how many buildings of the type specified within the PointBonus there is.
     *  Then 
     */
    public void CalculateAccumulativeBonuses()
    {
        int konkai_points = 0;
        foreach (PointBonus pb in pk.Accumulative_perm)
        {
            if (pb.Array)
            {
                konkai_points += pb.PointBonusAr[Get_No_Buildings(PointBonus.MapActionToBuilding(pb.GetActionType()))-1] * pb.pointBonus;
            }
            else
            {
                konkai_points += Get_No_Buildings(PointBonus.MapActionToBuilding(pb.GetActionType())) * pb.pointBonus;
            }
        }
        foreach (PointBonus pb in pk.Accumulative_temp)
        {
            if (pb.Array)
            {
                konkai_points += pb.PointBonusAr[Get_No_Buildings(PointBonus.MapActionToBuilding(pb.GetActionType()))-1] * pb.pointBonus;
            }
            else
            {
                konkai_points += Get_No_Buildings(PointBonus.MapActionToBuilding(pb.GetActionType())) * pb.pointBonus;
            }
        }
        Points += konkai_points;
    }

    //End Turn and round
    public void EndTurn()
    {
        CalculateIncome();
        UpdateResourceText();
    }

    //Increases the players resources by their roundly income
    public void Increase_Count_By_Income()
    {
        //First we calculate it, just in case it changed
        CalculateIncome();
        //Then we add increase our counts by the income amounts
        money_count += money_income;
        worker_count += worker_income;
        priest_count += priest_income;
        tier_magic.AddMagic(magic_income);

        //Then update the texts
        UpdateResourceText();
    }

    //Accessor Methods from here
    int Get_No_Buildings(Building.Building_Type bt_)
    {
        switch (bt_)
        {
            case Building.Building_Type.Dwelling:
                return current_dwelling;
            case Building.Building_Type.Trading_Post:
                return current_tp;
            case Building.Building_Type.Fortress:
                return current_fortress;
            case Building.Building_Type.Temple:
                return current_temple;
            case Building.Building_Type.Sanctuary:
                return current_sanctuary;
        }
        return 0;
    }
    public Material Get_Building_Material()
    {
        return faction.Get_Building_Material();
    }

    public int Get_Money_Count()
    {
        return money_count;
    }

    public int Get_Worker_Count()
    {
        return worker_count;
    }

    public int Get_Money_Income()
    {
        return money_income;
    }

    public int Get_Worker_Income()
    {
        return worker_income;
    }

    public Terrain.TerrainType GetHabitat()
    {
        return faction.GetHabitat();
    }

    public int GetShovelCount()
    {
        return shovel_count;
    }

    public int GetPointBonus(PointBonus.Action_Type at_)
    {
        return pk.GetPointBonus(at_);
    }

    public int GetShipping()
    {
        return shipping;
    }

    public PlayerPiece GetPiece(int index)
    {
        return playerPieces[index];
    }

    public int Get_Remaining_Starting_Dwellings()
    {
        return faction.Get_Remaining_Starting_Dwellings();
    }
}