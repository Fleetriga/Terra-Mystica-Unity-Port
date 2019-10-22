using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameObject localPlayerObject;
    PlayerStatistics publicPlayerAttributes;

    //Player resources
    int shovel_count;
    Magic tierMagic;
    int shipping;
    int current_upgrade_terraform;
    CultData cd;

    //Round pick up bonuses
    public Round_Bonus CurrentRoundBonus { get; set; }

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
    public void SetUp (Faction.FactionType p_faction, int ID, GameObject bossObject) {
        //Set ID
        playerID = ID;
        localPlayerObject = bossObject;
        publicPlayerAttributes = bossObject.GetComponent<PlayerStatistics>();

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
        publicPlayerAttributes.CmdSetPoints(20);

        //Shipping & terraform upgrade levels
        shipping = faction.GetDefaultShipping();
        current_upgrade_terraform = 0;

        //Starting list of "built upon" coordinates is empty
        buildingLocs = new List<Coordinate>();

        //Starting cult tracks
        cd = new CultData(faction.Get_Default_CultData());

        //Setup UI to start with
        ui = GameObject.Find("UI").GetComponent<UI_Updater>();
        ui.UpdatePlayerBuildingCosts(faction.Get_cost_build_dwelling(), faction.Get_cost_build_TP(), faction.Get_cost_build_temple(), faction.Get_cost_build_sanctuary(), faction.Get_cost_build_stronghold());
        ui.UpdatePlayerCultTrack(cd);

        //Starting resources and income and update UI resources/income
        publicPlayerAttributes.CmdSetGold(faction.Starting_gold);
        publicPlayerAttributes.CmdSetWorkers(faction.Starting_worker);
        publicPlayerAttributes.CmdSetPriests(faction.Starting_priest);
        shovel_count = faction.Starting_shovels;

        CalculateIncome();

        tierMagic = faction.GetDefaultMagic(); UpdateMagicPublicVariables();

        PointsForTown = 7;

        //Update the UI after setting up all variables
        UpdateResourceText();
	}

    internal bool CheckPlayerCanAfford(SingleIncome price)
    {
        return publicPlayerAttributes.PlayerGold >= price.Gold && publicPlayerAttributes.PlayerWorkers >= price.Worker && publicPlayerAttributes.PlayerPriests >= price.Priest;
    }

    void UpdateMagicPublicVariables()
    {
        publicPlayerAttributes.CmdSetMagics(tierMagic.GetTiers());
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
        int[] income = faction.Get_dwelling_income().GetTotalIncome(current_dwelling);
        AddTo_Income(income);
        
        //For tps
        income = faction.Get_tp_income().GetTotalIncome(current_tp);
        AddTo_Income(income);

        //For fortress
        income = faction.Get_fortress_income().GetTotalIncome(current_fortress);
        AddTo_Income(income);

        //For temples
        income = faction.Get_temple_income().GetTotalIncome(current_temple);
        AddTo_Income(income);

        //For sancuary
        income = faction.Get_sanctuary_income().GetTotalIncome(current_sanctuary);
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
    public void AddToNonBuildingIncome(int[] income)
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
        int[] incomes = income.IncomeAsArray();
        publicPlayerAttributes.CmdSetGold(publicPlayerAttributes.PlayerGold += incomes[0]);
        publicPlayerAttributes.CmdSetWorkers(publicPlayerAttributes.PlayerWorkers += incomes[1]);
        publicPlayerAttributes.CmdSetPriests(publicPlayerAttributes.PlayerPriests += incomes[2]);
        tierMagic.AddMagic(incomes[3]); UpdateMagicPublicVariables();
        shovel_count += incomes[4];
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
            case Building.Building_Type.Stronghold:
                temp = faction.Get_cost_build_stronghold(); break; 
            case Building.Building_Type.Sanctuary:
                temp = faction.Get_cost_build_temple(); break; 
            case Building.Building_Type.Temple:
                temp = faction.Get_cost_build_sanctuary(); break; 
        }
        if (publicPlayerAttributes.PlayerGold - temp[0] >= 0 && publicPlayerAttributes.PlayerWorkers - temp[1] >= 0) { return true; }
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
        if (distance <= shovel_count || distance-shovel_count * faction.Get_Cost_Terraform(current_upgrade_terraform).Worker <= publicPlayerAttributes.PlayerWorkers)
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
                CheckPoints(PointBonus.Action_Type.Terraform);
            } 
        }

        ui.UpdatePoints(publicPlayerAttributes.PlayerPoints);
    }

    public void Build(Building.Building_Type bt)
    {
        switch (bt)
        {
            case Building.Building_Type.Dwelling:
                publicPlayerAttributes.CmdSetGold(publicPlayerAttributes.PlayerGold - faction.Get_cost_build_dwelling()[0]);
                publicPlayerAttributes.CmdSetWorkers(publicPlayerAttributes.PlayerWorkers - faction.Get_cost_build_dwelling()[1]);
                current_dwelling++;
                break;
            case Building.Building_Type.Trading_Post:
                //First calculate resources and all that
                publicPlayerAttributes.CmdSetGold(publicPlayerAttributes.PlayerGold - faction.Get_cost_build_TP()[0]);
                publicPlayerAttributes.CmdSetWorkers(publicPlayerAttributes.PlayerWorkers - faction.Get_cost_build_TP()[1]);
                //Then update UI to display remaining buildings of the type that increased. So when a player builds a TP they regain a dwelling.
                ui.DisplayRemaining(Building.Building_Type.Dwelling, Get_Remaining_Index(Building.Building_Type.Dwelling));
                current_tp++; current_dwelling--;
                break;
            case Building.Building_Type.Stronghold:
                publicPlayerAttributes.CmdSetGold(publicPlayerAttributes.PlayerGold - faction.Get_cost_build_stronghold()[0]);
                publicPlayerAttributes.CmdSetWorkers(publicPlayerAttributes.PlayerWorkers - faction.Get_cost_build_stronghold()[1]);
                ui.DisplayRemaining(Building.Building_Type.Trading_Post, Get_Remaining_Index(Building.Building_Type.Trading_Post));
                current_fortress++; current_tp--;
                CheckPoints(PointBonus.Action_Type.BuildT3); //Special point bonus
                break;
            case Building.Building_Type.Temple:
                publicPlayerAttributes.CmdSetGold(publicPlayerAttributes.PlayerGold - faction.Get_cost_build_temple()[0]);
                publicPlayerAttributes.CmdSetWorkers(publicPlayerAttributes.PlayerWorkers - faction.Get_cost_build_temple()[1]);
                ui.DisplayRemaining(Building.Building_Type.Trading_Post, Get_Remaining_Index(Building.Building_Type.Trading_Post));
                current_temple++; current_tp--;
                break;
            case Building.Building_Type.Sanctuary:
                publicPlayerAttributes.CmdSetGold(publicPlayerAttributes.PlayerGold - faction.Get_cost_build_sanctuary()[0]);
                publicPlayerAttributes.CmdSetWorkers(publicPlayerAttributes.PlayerWorkers - faction.Get_cost_build_sanctuary()[1]);
                ui.DisplayRemaining(Building.Building_Type.Temple, Get_Remaining_Index(Building.Building_Type.Temple));
                current_sanctuary++; current_temple--;
                CheckPoints(PointBonus.Action_Type.BuildT3); //Special point bonus
                break;
        }
        CheckPoints(PointBonus.MapBuildingToAction(bt)); //Check if we gained any points#
        ui.UpdatePoints(publicPlayerAttributes.PlayerPoints);

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
            case Building.Building_Type.Stronghold: return faction.max_fortress - current_fortress;
            case Building.Building_Type.Temple: return faction.max_temple - current_temple;
            case Building.Building_Type.Sanctuary: return faction.max_sanctuary - current_sanctuary;
        }
        return 0;
    }

    internal void AddSinglePointValue(int points)
    {
        publicPlayerAttributes.CmdSetPoints(publicPlayerAttributes.PlayerPoints += points);
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
            case Building.Building_Type.Stronghold:
                ui.DisplayRemaining(Building.Building_Type.Trading_Post, Get_Remaining_Index(Building.Building_Type.Trading_Post));
                current_fortress++; current_tp--;
                CheckPoints(PointBonus.Action_Type.BuildT3); //Special point bonus
                break;
            case Building.Building_Type.Sanctuary:
                ui.DisplayRemaining(Building.Building_Type.Trading_Post, Get_Remaining_Index(Building.Building_Type.Trading_Post));
                current_temple++; current_tp--;
                CheckPoints(PointBonus.Action_Type.BuildT3); //Special point bonus
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
        if (publicPlayerAttributes.PlayerGold >= cost.Gold 
            && publicPlayerAttributes.PlayerWorkers >= cost.Worker 
            && publicPlayerAttributes.PlayerPriests >= cost.Priest 
            && tierMagic.GetTiers()[2] >= cost.Magic && shipping != 4)//and its not at max shipping
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
        if (publicPlayerAttributes.PlayerGold >= cost.Gold 
            && publicPlayerAttributes.PlayerWorkers >= cost.Worker 
            && publicPlayerAttributes.PlayerPriests >= cost.Priest 
            && tierMagic.GetTiers()[2] >= cost.Magic 
            && !faction.Max_Upgrade_Terraform(current_upgrade_terraform))//and its not at max shipping
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
        return tierMagic.GetTiers()[2] >= s.GetCost();
    }

    public void CastSpell(Spell s)
    {
        tierMagic.UseSpell(s.GetCost()); UpdateMagicPublicVariables();
        AddSingleIncome(s.GetIncome());
    }

    public void BurnMagic()
    {
        tierMagic.BurnMagic(1); UpdateMagicPublicVariables();
        UpdateResourceText();
    }

    //Cults
    public bool AddToTrackLevel(CultIncome income, int[] max)
    {
        bool temp = cd.Add_To_Track_Level(income, max);
        ui.UpdatePlayerCultTrack(cd);
        return temp;
    }
    public CultData GetCultData()
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
        publicPlayerAttributes.CmdSetPoints(publicPlayerAttributes.PlayerPoints += pk.GetPointBonus(at_));
    }
    public void UpdateResourceText()
    {
        ui.UpdateResourceText(new int[] { publicPlayerAttributes.PlayerGold, publicPlayerAttributes.PlayerWorkers, publicPlayerAttributes.PlayerPriests, shovel_count }, new int[] { money_income, worker_income, priest_income});
        ui.UpdateMagicTiers(tierMagic.GetTiers());
        ui.UpdatePoints(publicPlayerAttributes.PlayerPoints);
    }
    public void AddPointBonus(PointBonus pb_, bool temporary)
    {
        pk.AddPointBonus(pb_, temporary);
    }
    public void ResetTemporaryPointBonuses()
    {
        pk.ResetTempBonuses();
    }
    // First calculates how many buildings of the type specified within the PointBonus there is.
    public void CalculateAccumulativeBonuses()
    {
        int konkai_points = 0;
        foreach (PointBonus pb in pk.Accumulative_perm)
        {
            if (pb.Array)
            {
                konkai_points += pb.PointBonusAr[GetNoBuildings(PointBonus.MapActionToBuilding(pb.GetActionType()))-1] * pb.pointBonus;
            }
            else
            {
                konkai_points += GetNoBuildings(PointBonus.MapActionToBuilding(pb.GetActionType())) * pb.pointBonus;
            }
        }
        foreach (PointBonus pb in pk.Accumulative_temp)
        {
            if (pb.Array)
            {
                konkai_points += pb.PointBonusAr[GetNoBuildings(PointBonus.MapActionToBuilding(pb.GetActionType()))-1] * pb.pointBonus;
            }
            else
            {
                konkai_points += GetNoBuildings(PointBonus.MapActionToBuilding(pb.GetActionType())) * pb.pointBonus;
            }
        }
        publicPlayerAttributes.CmdSetPoints(publicPlayerAttributes.PlayerPoints += konkai_points);
    }

    //End Turn and round
    public void EndTurn()
    {
        CalculateIncome();
        UpdateResourceText();

        //Tell the network that it's over
        localPlayerObject.GetComponent<PlayerStatus>().Cmd_SetPlayerStatus(PlayerStatus.PlayerStatusEnum.EndingTurn);
    }
    public bool TakingTurn()
    {
        return localPlayerObject.GetComponent<PlayerStatus>().CurrentPlayerStatus == PlayerStatus.PlayerStatusEnum.TakingTurn;
    }
    public void Retire()
    {
        localPlayerObject.GetComponent<PlayerStatus>().Cmd_SetPlayerStatus(PlayerStatus.PlayerStatusEnum.Retiring);
    }

    //Increases the players resources by their roundly income
    public void IncreaseCountByIncome()
    {
        //First we calculate it, just in case it changed
        CalculateIncome();
        //Then we add increase our counts by the income amounts
        publicPlayerAttributes.CmdSetGold(publicPlayerAttributes.PlayerGold += money_income);
        publicPlayerAttributes.CmdSetWorkers(publicPlayerAttributes.PlayerWorkers += worker_income);
        publicPlayerAttributes.CmdSetPriests(publicPlayerAttributes.PlayerPriests += priest_income);
        tierMagic.AddMagic(magic_income); UpdateMagicPublicVariables();

        //Then update the texts
        UpdateResourceText();
    }

    //Accessor Methods from here
    int GetNoBuildings(Building.Building_Type bt_)
    {
        switch (bt_)
        {
            case Building.Building_Type.Dwelling:
                return current_dwelling;
            case Building.Building_Type.Trading_Post:
                return current_tp;
            case Building.Building_Type.Stronghold:
                return current_fortress;
            case Building.Building_Type.Temple:
                return current_temple;
            case Building.Building_Type.Sanctuary:
                return current_sanctuary;
        }
        return 0;
    }

    public Terrain.TerrainType GetHabitat()
    {
        return faction.GetHabitat();
    }

    public int GetShipping()
    {
        return shipping;
    }

    public PlayerPiece GetPiece(int index)
    {
        return playerPieces[index];
    }

    public int GetRemainingStartingDwellings()
    {
        return faction.GetRemainingStartingDwellings();
    }
}