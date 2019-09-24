using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class GameController : MonoBehaviour {
    MagicController magicController;
    WonderController wonderController;
    TownFoundingBonusManager townFoundManager;
    Networked_UI_Updater networkedUI;
    CultController cultController;
    TracksInterface progressTrackManager;
    RoundBonusManager roundBonusManager;
    RoundEndBonusManager roundEndBonusManager;
    Global_Flags global;

    Player localPlayer; //Found later



    public Board board;
    public AbilityManager am;
    public BridgeManager bm;

    public int phase;
    public int round;
    bool endTurnOrBuild;

    // Use this for initialization
    public void Start_Game() {
        GameObject ui_holder = GameObject.Find("UI");

        //Find Component classes
        magicController = GetComponent<MagicController>();
        roundBonusManager = GetComponent<RoundBonusManager>();
        global = GetComponent<Global_Flags>();
        networkedUI = ui_holder.GetComponent<Networked_UI_Updater>();
        progressTrackManager = ui_holder.GetComponent<TracksInterface>();
        townFoundManager = GetComponent<TownFoundingBonusManager>();
        wonderController = GetComponent<WonderController>();
        roundEndBonusManager = GetComponent<RoundEndBonusManager>();

        //Create component classes
        cultController = new CultController();
        
        phase = 0;
        round = 0;

        localPlayer = GameObject.FindWithTag("Player_Object").GetComponent<Player>();

        wonderController.SetUp();
        magicController.SetUpSpells();
        networkedUI.SetUpUI();
        progressTrackManager.SetUp();
        townFoundManager.SetUp();
        roundBonusManager.SetUp();
        roundEndBonusManager.SetUp();
        SetUpTracks();
        global.Set_Up();
    }

    void SetUpTracks()
    {
        progressTrackManager.AddPieceToTrack(localPlayer.GetID(), localPlayer.GetPiece(0), TracksInterface.ProgressTrackRef.Cult_fire, localPlayer.GetCultData().GetLevels()[0]);
        progressTrackManager.AddPieceToTrack(localPlayer.GetID(), localPlayer.GetPiece(1), TracksInterface.ProgressTrackRef.Cult_water, localPlayer.GetCultData().GetLevels()[1]);
        progressTrackManager.AddPieceToTrack(localPlayer.GetID(), localPlayer.GetPiece(2), TracksInterface.ProgressTrackRef.Cult_earth, localPlayer.GetCultData().GetLevels()[2]);
        progressTrackManager.AddPieceToTrack(localPlayer.GetID(), localPlayer.GetPiece(3), TracksInterface.ProgressTrackRef.Cult_air, localPlayer.GetCultData().GetLevels()[3]);
        progressTrackManager.AddPieceToTrack(localPlayer.GetID(), localPlayer.GetPiece(4), TracksInterface.ProgressTrackRef.Shipping, 0);
        progressTrackManager.AddPieceToTrack(localPlayer.GetID(), localPlayer.GetPiece(5), TracksInterface.ProgressTrackRef.Terraform, 0);
    }

    public void UpgradeTrack(int track)
    {
        if (track == 0)
        {
            if (localPlayer.UpgradeShipping())
            {
                progressTrackManager.Progress_Track(TracksInterface.ProgressTrackRef.Shipping, localPlayer.GetShipping(), localPlayer.GetID());
            }
        }
        else {
            if (localPlayer.UpgradeTerraforming())
            {
                progressTrackManager.Progress_Track(TracksInterface.ProgressTrackRef.Terraform, localPlayer.Current_Upgrade_Terraform(), localPlayer.GetID());
            }
        }
    }

    //Merge towns when a bridge is built
    public void Merge_Towns(TownGroup tg1, TownGroup tg2)
    {
        if (tg1 != null && tg2 != null)
        {
            if (tg1.Merge_Town_Group(tg2, localPlayer.PointsForTown))
            {
                networkedUI.PanelInteractivityEnDisable(networkedUI.TownTilePanel, true);
                networkedUI.DisEnableTownTiles();
                localPlayer.CheckPoints(PointBonus.Action_Type.FoundTown);
            }
        }
    }

    //Phase related Methods
    void SetUpPhase()
    {
        global.ResetFlags(); //Reset flags before setting up a phase. Redundancy.
        //Set up the next phase, moving UI elements and stuff
        switch (phase)
        {
            case 1: SetUpIncomePhase(); break;
            case 2: SetUpRoundMain(); break;
            case 3: SetUpRoundEnd(); break;
        }
    }

    void SetUpIncomePhase()
    {
        networkedUI.DisEnableRoundBonuses(true);
        roundBonusManager.AppearRoundBonuses(); //Show round bonuses 
        magicController.ResetWorldMagic();
    }

    void SetUpRoundMain()
    {
        roundBonusManager.ResetAvailableBonuses(); //Reset them for next round now.
        localPlayer.AddPointBonus(roundEndBonusManager.GetRoundEndBonus(round).RoundEndPointBonus, true); //Get this phases RoundEndBonuses reactive point bonus
        networkedUI.DisEnableRoundBonuses(false); //Then take them off the screen
    }

    void SetUpRoundEnd()
    {
        localPlayer.AddSingleIncome(roundEndBonusManager.GetRoundEndBonus(round).GetTotalBouns(localPlayer.GetCultData()));
        RemoveRoundBonusAbilities();
        roundEndBonusManager.ResetBonuses();
        round++;
        phase = 1;
        SetUpPhase(); //Temporary
    }

    void RemoveRoundBonusAbilities()
    {
        if (localPlayer.CurrentRoundBonus.Type == RoundBonusManager.RoundBonusType.OneCult_FourGold)
        {
            am.RemoveAbility<AbilityAddToCult>();
        }
        if (localPlayer.CurrentRoundBonus.Type == RoundBonusManager.RoundBonusType.OneTerraform_TwoGold)
        {
            am.RemoveAbility<AbilityTerraform1>();
        }
    }

    public void ParseFlags(Tile t)
    {
        if (!CheckLocalTurn())
        {
            return;
        }
        switch (phase)
        {
            case 0: ParseFlagsSetUp(t); break;
            case 1: ParseFlagsRoundIncomePhase(); break;
            case 2: ParseFlagsRoundMainPhase(t); break;
            case 3: ParseFlagsRoundEnd(); break;
        }
    }

    void ParseFlagsSetUp(Tile t) //phase 0
    {
        //If the building flag is set to dwelling AND the terrain matches the players habitat
        if (Building.Equals(global.BuildFlag, Building.Building_Type.Dwelling) &&
            t.GetTerrainType() == localPlayer.GetHabitat())
        {
            FreeBuild(global.BuildFlag, t);
            localPlayer.localPlayerObject.GetComponent<TakenTurnData>().SendBuildData(t.GetCoordinates().GetXY(), global.BuildFlag);

            if (localPlayer.GetRemainingStartingDwellings() <= 0) { phase = 1; RetirePlayer(); } //Next phase after set up is round income, phase 1
            else {
                localPlayer.EndTurn(); 
            }
        }

        global.ResetFlags();
    }

    void ParseFlagsRoundIncomePhase() //phase 1 triggers for each player when they chose a bonus
    {
        if (global.RoundBonusFlag != RoundBonusManager.RoundBonusType.NOTHING)
        {
            //give player bonus income
            if (roundBonusManager.GetRoundBonus(global.RoundBonusFlag).Round_Income != null)
            {
                localPlayer.CurrentRoundBonus = roundBonusManager.GetRoundBonus(global.RoundBonusFlag);
                localPlayer.AddToNonBuildingIncome(roundBonusManager.GetRoundBonus(global.RoundBonusFlag).Round_Income.IncomeAsArray());
            }

            //If there's a point bonus
            if (roundBonusManager.GetRoundBonus(global.RoundBonusFlag).RoundPointbonus != null)
            {
                
                localPlayer.AddPointBonus(roundBonusManager.GetRoundBonus(global.RoundBonusFlag).RoundPointbonus, true); //It will sort itself out at the end since it's temporary
            }

            //Special round bonuses require abilities to be added
            if (global.RoundBonusFlag == RoundBonusManager.RoundBonusType.OneTerraform_TwoGold)
            {
                am.AddAbility<AbilityTerraform1>();
            }
            if(global.RoundBonusFlag == RoundBonusManager.RoundBonusType.OneCult_FourGold)
            {
                am.AddAbility<AbilityAddToCult>();
            }
            
            localPlayer.IncreaseCountByIncome();

            localPlayer.localPlayerObject.GetComponent<TakenTurnData>().SendRoundBonusPicked(global.RoundBonusFlag); //Tell other players that 

            phase = 2; //Next phase after Income is Main game phase, phase 2
            RetirePlayer();
        } 
    }

    void ParseFlagsRoundMainPhase(Tile t) //phase 2 - Main game phase
    {
        //If the building flag is set AND 
        //if player is upgrading OR the tile is unoccupied 
        //AND player has resources AND the terrain matches the players habitat
        //AND It's correct in the building upgrade path 
        //AND the player is only building a dwelling if they've terraformed.
        if (global.BuildFlag != Building.Building_Type.NOTHING &&
            ((global.BuildFlag != Building.Building_Type.Dwelling && t.OwnersPlayerID==localPlayer.playerID) || (global.BuildFlag == Building.Building_Type.Dwelling && t.OwnersPlayerID == 64)) &&
            (localPlayer.CheckCanBuild(global.BuildFlag)) &&
            (t.TileBuilding.IsUpgrade(global.BuildFlag)) && //Ignore need of neighbour if you're upgrading a building                               VVVVVVVVV
            (board.CheckNeighbourSettleable(localPlayer.GetBuildingLocs(), t, localPlayer.GetHabitat(), localPlayer.GetShipping()) || global.BuildFlag != Building.Building_Type.Dwelling) &&
            ((endTurnOrBuild && global.BuildFlag == Building.Building_Type.Dwelling) || !endTurnOrBuild))
        {                                                                                                                                
            localPlayer.localPlayerObject.GetComponent<TakenTurnData>().SendBuildData(t.GetCoordinates().GetXY(), global.BuildFlag);
            if (Build(global.BuildFlag, t)) { localPlayer.EndTurn(); }; //End turn unless player gets a town tile or favour tile
            endTurnOrBuild = false; //If it was set, it isn't now.
        }
        //If the terrain flag is(isn't not) set AND there is no building on this tile AND and this is not a river tile AND player validation passes(resources OK)
        if ((!Terrain.TerrainEquals(global.Terraform_Flag, Terrain.TerrainType.NOTHING)) &&
            (!t.HasBuilding()) && (!t.IsRiverTile()) &&
            localPlayer.CheckCanTerraform(global.Terraform_Flag, t.GetTerrainType()))
        {
            Terraform(global.Terraform_Flag, t);
            localPlayer.localPlayerObject.GetComponent<TakenTurnData>().SendTerraformData(t.GetCoordinates().GetXY(), global.Terraform_Flag);
        }

        global.ResetFlags();
    }

    void ParseFlagsRoundEnd() //phase 3 add accumulative points
    {
        localPlayer.CalculateAccumulativeBonuses();
        localPlayer.CurrentRoundBonus = null;
        RetirePlayer();
        phase = 1; //Next phase after roundEnd is Income phase, phase 1
    }

    //End a players RoundMain phase, triggered by user via UI
    public void End_Round()
    {
        localPlayer.CalculateAccumulativeBonuses(); //Calc points
        localPlayer.AddToNonBuildingIncome(localPlayer.CurrentRoundBonus.Round_Income.Reciprocal().IncomeAsArray()); //Calc new income and update UI
        localPlayer.ResetTemporaryPointBonuses(); //Reset temp bonuses so they dont get carried over
        RetirePlayer();
    }

    //Retire a player when hes finished his phase
    public void RetirePlayer()
    {
        localPlayer.Retire();
        SetUpPhase();
    }

    public void CastMagic()
    {
        //If Magic has been invoked
        if ((!MagicController.Equals(global.MagicCastFlag, MagicController.SpellType.NOTHING)) &&
            localPlayer.CheckCanCastSpell(magicController.GetSpell((int)global.MagicCastFlag)))
        {
            if (global.MagicCastFlag == MagicController.SpellType.BURN)
            {
                localPlayer.BurnMagic();
            }
            if((int)global.MagicCastFlag < 3)//If it's a regular spell
            {
                localPlayer.CastSpell(magicController.GetSpell((int)global.MagicCastFlag));
            }
            //If it's a world spell we need an extra step.
            //However if the player terraformed this round, deny the use of world magic
            if ((int)global.MagicCastFlag <= 8 && !magicController.CheckWorldMagicUsed(global.MagicCastFlag) && !endTurnOrBuild)
            {
                if (global.MagicCastFlag == MagicController.SpellType.World_Bridge)
                {
                    localPlayer.CastSpell(magicController.GetSpell((int)global.MagicCastFlag)); //Cast spell to remove magic from tier 3 pool
                    bm.Allow_Bridge(localPlayer.GetID());
                }
                else
                {
                    //If it's successfully casted and not a bridge, send that data to the server and gain its effects
                    localPlayer.localPlayerObject.GetComponent<TakenTurnData>().SendCastedSpellData(global.MagicCastFlag);
                    localPlayer.CastSpell(magicController.GetSpell((int)global.MagicCastFlag));
                }

                //Turn ends after global magic is casted
                localPlayer.EndTurn();
            }

        }
    }

    void Terraform(Terrain.TerrainType t_type, Tile t)
    {
        localPlayer.Terraform(t_type, t.GetTerrainType());
        endTurnOrBuild = true;
    }

    /* Builds a building without costing any player resouces and returns true if you should end turn */
    bool FreeBuild(Building.Building_Type newBuildingType, Tile builtUpon)
    {
        localPlayer.FreeBuild(newBuildingType);
        localPlayer.AddBuiltCoordinate(builtUpon);
        
        bool endTurn = true;

        //Enable favour tile building if 
        if (localPlayer.CheckWonderTile(global.BuildFlag))
        {
            networkedUI.PanelInteractivityEnDisable(networkedUI.favourButtons, true);
            networkedUI.DisEnableFavourTiles();
            endTurn = false;
        }

        //Enable town tile picking if a city was founded
        if (board.JoinTileToTownGroup(builtUpon, newBuildingType, localPlayer))
        {
            networkedUI.PanelInteractivityEnDisable(networkedUI.townButtons, true);
            networkedUI.DisEnableTownTiles();
            endTurn = false;
        }

        return endTurn;
    }

    //Returns true if you should end turn
    bool Build(Building.Building_Type newBuildingType, Tile builtUpon)
    {
        //Change local player resources
        localPlayer.Build(newBuildingType);
        localPlayer.AddBuiltCoordinate(builtUpon);

        bool endTurn = true;

        //Enable favour tile building if 
        if (localPlayer.CheckWonderTile(global.BuildFlag))
        {
            networkedUI.PanelInteractivityEnDisable(networkedUI.favourButtons, true);
            networkedUI.DisEnableFavourTiles();
            endTurn = false;
        }

        //Enable town tile picking if a city was founded
        if (board.JoinTileToTownGroup(builtUpon, newBuildingType, localPlayer))
        {
            networkedUI.PanelInteractivityEnDisable(networkedUI.townButtons, true);
            networkedUI.DisEnableTownTiles();
            endTurn = false;
        }

        return endTurn;
    }

    public void BuildBridge(int iD)
    {
        localPlayer.localPlayerObject.GetComponent<TakenTurnData>().SendBuiltBridgeData(iD);
    }

    /* USED PURELY FOR THE +1 CULT ABILITY */
    public void IncreaseCult(int track)
    {
        switch (track)
        {
            case 0:
                cultController.AddToTrackLevel(new CultIncome(1, 0, 0, 0), localPlayer); break;
            case 1:
                cultController.AddToTrackLevel(new CultIncome(0, 1, 0, 0), localPlayer); break;
            case 2:
                cultController.AddToTrackLevel(new CultIncome(0, 0, 1, 0), localPlayer); break;
            case 3:
                cultController.AddToTrackLevel(new CultIncome(0, 0, 0, 1), localPlayer); break;
        }
        localPlayer.EndTurn();
    }
    //Used purely for terraform ability
    public void AddShovel()
    {
        localPlayer.AddSingleIncome(new SingleIncome(0,0,0,0,1));
        endTurnOrBuild = true;
    }

    public void TakeTownTile(int type)
    {
        FavourBonus tc = townFoundManager.Get_TownTile((TownFoundingBonusManager.TownTiletype)type); //TOWN FOUNDING TILES USE FavourBonus's class as a base
        if (tc != null)
        {
            //Increase players income and points
            localPlayer.AddSingleIncome(tc.GetIncome());
            localPlayer.AddSinglePointValue(tc.Points);

            //Increase players cult standing. Tell the track to increase too
            AddCultIncome(tc.Get_CultIncome());

            //turn off buying ability
            networkedUI.PanelInteractivityEnDisable(networkedUI.townButtons, false);

            //Send turn data to server and end turn
            localPlayer.localPlayerObject.GetComponent<TakenTurnData>().SendCityFoundingData((TownFoundingBonusManager.TownTiletype)type);
            localPlayer.EndTurn();
        }
    }

    public void AddCultIncome(CultIncome ci_)
    {
        if (ci_ != null)
        {
            localPlayer.AddSingleIncome(new SingleIncome(0,0,0, cultController.AddToTrackLevel(ci_, localPlayer),0)); //Add to the cult track and then add any owed magic to player
            progressTrackManager.Progress_Track(TracksInterface.ProgressTrackRef.Cult_fire, localPlayer.GetCultData().GetLevels()[0], localPlayer.GetID());
            progressTrackManager.Progress_Track(TracksInterface.ProgressTrackRef.Cult_water, localPlayer.GetCultData().GetLevels()[1], localPlayer.GetID());
            progressTrackManager.Progress_Track(TracksInterface.ProgressTrackRef.Cult_earth, localPlayer.GetCultData().GetLevels()[2], localPlayer.GetID());
            progressTrackManager.Progress_Track(TracksInterface.ProgressTrackRef.Cult_air, localPlayer.GetCultData().GetLevels()[3], localPlayer.GetID());
        }
    }

    public void TakeFavourTile(int tracktier)
    {
        int[] temp = CultController.ParseInt(tracktier); //Parse e.g. 12 into track 1 tier 2
        int track = temp[0]-1; int tier = temp[1];

        FavourBonus wt = wonderController.GetWonderTile(track, tier);

        if (track == 0 && tier == 1) //If its fire 2 or W2 then it requires special treatment
        {
            localPlayer.PointsForTown = 6;
            localPlayer.EndTurn();
        }
        else if (track == 1 && tier == 1)
        {
            am.AddAbility<AbilityAddToCult>();
            localPlayer.EndTurn();
        }
        else if (wt != null)
        {
            //Increase players income and points
            localPlayer.AddToNonBuildingIncome(wt.GetIncome().IncomeAsArray());
            if (wt.GetPointBonus() != null) { localPlayer.AddPointBonus(wt.GetPointBonus(), false); }

            //Increase players cult standing. Tell the track to increase too
            AddCultIncome(wt.Get_CultIncome());

            networkedUI.PanelInteractivityEnDisable(networkedUI.favourButtons, false);

            localPlayer.EndTurn();
        } //If no options are picked for whatever reason return, otherwise send the data to the server
        else { return; } localPlayer.localPlayerObject.GetComponent<TakenTurnData>().SendPickedFavourData(track, tier);
    }

    public void EndTurnRetire()
    {
        if (endTurnOrBuild) { localPlayer.EndTurn(); endTurnOrBuild = false; }
        else { phase=3; localPlayer.Retire(); SetUpPhase(); }
    }

    public void DebugAddMagic()
    {
        localPlayer.AddSingleIncome(new SingleIncome(0,0,0,1,0));
    }

    bool CheckLocalTurn()
    {
        return localPlayer.TakingTurn();
    }
}
