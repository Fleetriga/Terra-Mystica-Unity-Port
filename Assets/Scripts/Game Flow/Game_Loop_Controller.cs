using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Game_Loop_Controller : MonoBehaviour {
    MagicController mc;
    WonderController wc;
    TownTileController ttc;
    Networked_UI_Updater nui;
    CultController ct;
    TracksInterface ti;
    Round_Bonus_Manager rbm;
    Global_Flags global;

    Player local_player; //Found later

    public GameObject player_game_object;
    public Board board;
    public AbilityManager am;
    public Bridge_Manager bm;
    public Turn_Controller turn_c; //Networked

    int phase;

    // Use this for initialization
    public void Start_Game() {
        GameObject ui_holder = GameObject.Find("UI");

        mc = GetComponent<MagicController>();
        rbm = GetComponent<Round_Bonus_Manager>();
        global = GetComponent<Global_Flags>();

        wc = new WonderController();
        ct = new CultController();
        ttc = new TownTileController();

        nui = ui_holder.GetComponent<Networked_UI_Updater>();
        ti = ui_holder.GetComponent<TracksInterface>();

        local_player = GameObject.FindWithTag("Player_Object").GetComponent<Player>();
        turn_c = GameObject.Find("NetworkSync").GetComponent<Turn_Controller>();

        wc.SetUp();
        mc.SetUpSpells();
        nui.SetUpUI();
        ti.SetUp();
        SetUpTracks();
        ttc.SetUp();
        rbm.Set_Up();
        global.Set_Up();
    }

    void SetUpTracks()
    {
        ti.AddPieceToTrack(local_player.GetID(), local_player.GetPiece(0), TracksInterface.ProgressTrackRef.Cult_fire, local_player.Get_CultData().Get_Levels()[0]);
        ti.AddPieceToTrack(local_player.GetID(), local_player.GetPiece(1), TracksInterface.ProgressTrackRef.Cult_water, local_player.Get_CultData().Get_Levels()[1]);
        ti.AddPieceToTrack(local_player.GetID(), local_player.GetPiece(2), TracksInterface.ProgressTrackRef.Cult_earth, local_player.Get_CultData().Get_Levels()[2]);
        ti.AddPieceToTrack(local_player.GetID(), local_player.GetPiece(3), TracksInterface.ProgressTrackRef.Cult_air, local_player.Get_CultData().Get_Levels()[3]);
        ti.AddPieceToTrack(local_player.GetID(), local_player.GetPiece(4), TracksInterface.ProgressTrackRef.Shipping, 0);
        ti.AddPieceToTrack(local_player.GetID(), local_player.GetPiece(5), TracksInterface.ProgressTrackRef.Terraform, 0);
    }

    public void UpgradeTrack(int track)
    {
        if (track == 0)
        {
            if (local_player.UpgradeShipping())
            {
                ti.Progress_Track(TracksInterface.ProgressTrackRef.Shipping, local_player.GetShipping(), local_player.GetID());
            }
        }
        else {
            if (local_player.UpgradeTerraforming())
            {
                ti.Progress_Track(TracksInterface.ProgressTrackRef.Terraform, local_player.Current_Upgrade_Terraform(), local_player.GetID());
            }
        }
    }

    //Merge towns when a bridge is built
    public void Merge_Towns(TownGroup tg1, TownGroup tg2)
    {
        if (tg1 != null && tg2 != null)
        {
            if (tg1.Merge_Town_Group(tg2, local_player.PointsForTown))
            {
                nui.PanelInteractivityEnDisable(nui.TownTilePanel, true);
            }
        }
    }

    //Phase related Methods
    void Set_Up_Phase()
    {
        //Set up the next phase, moving UI elements and stuff
        switch (phase)
        {
            case 1: Set_Up_Income_Phase(); break;
            case 2: Set_Up_RoundMain(); break;
            case 3: Set_Up_RoundEnd(); break;
        }
    }

    void Set_Up_Income_Phase()
    {
        nui.Show_Hide_Round_Bonuses(true);
        rbm.Reset_Available_Bonuses();
    }

    void Set_Up_RoundMain()
    {
        nui.Show_Hide_Round_Bonuses(false);
    }

    void Set_Up_RoundEnd()
    {
    }

    public void ParseFlags(Tile t)
    {
        if (!Check_Local_Turn())
        {
            return;
        }
        switch (phase)
        {
            case 0: Parse_Flags_SetUp(t); break;
            case 1: Parse_Flags_RoundIncome_Phase(); break;
            case 2: Parse_Flags_RoundMain_Phase(t); break;
            case 3: Parse_Flags_RoundEnd(); break;
        }
    }

    void Parse_Flags_SetUp(Tile t) //phase 0
    {
        //If the building flag is set to dwelling AND the terrain matches the players habitat
        if (Building.Equals(global.Build_Flag, Building.Building_Type.Dwelling) &&
            t.GetTerrainType() == local_player.GetHabitat())
        {
            FreeBuild(global.Build_Flag, t);
            if (local_player.Get_Remaining_Starting_Dwellings() == 0) { End_Phase(); }
            else {
                EndTurn();
            }
        }
    }

    void Parse_Flags_RoundIncome_Phase() //phase 1 triggers for each player when they chose a bonus
    {
        if (global.Round_Bonus_Flag != Round_Bonus_Manager.Round_Bonus_Types.NOTHING)
        {
            //give player bonus income
            if (rbm.Get_Round_Bonus(global.Round_Bonus_Flag).Round_Income != null)
            {
                local_player.Current_Round_Bonus = rbm.Get_Round_Bonus(global.Round_Bonus_Flag);
                local_player.Add_To_NonBuilding_Income(rbm.Get_Round_Bonus(global.Round_Bonus_Flag).Round_Income.Income_AsArray());
            }

            //If there's a point bonus
            if (rbm.Get_Round_Bonus(global.Round_Bonus_Flag).Round_Point_bonus != null)
            {
                local_player.Add_Point_Bonus(rbm.Get_Round_Bonus(global.Round_Bonus_Flag).Round_Point_bonus, true); //It will sort itself out at the end since it's temporary
            }

            rbm.Take_Round_Bonus(global.Round_Bonus_Flag);
            local_player.Increase_Count_By_Income();

            End_Phase();
        }
    }

    void Parse_Flags_RoundMain_Phase(Tile t) //phase 2 - Main game phase
    {
        //If the building flag is set AND player has resources AND the terrain matches the players habitat
        if (!Building.Equals(global.Build_Flag, Building.Building_Type.NOTHING) &&
            (local_player.CheckCanBuild(global.Build_Flag) &&
            (board.CheckNeighbourSettleable(local_player.GetBuildingLocs(), t, local_player.GetHabitat(), local_player.GetShipping()) || global.Build_Flag != Building.Building_Type.Dwelling)))
        //Ignore need of neighbour if you're upgrading a building
        {
            Build(global.Build_Flag, t);
        }
        //If the terrain flag is(isn't not) set AND there is no building on this tile AND and this is not a river tile AND player validation passes(resources OK)
        if ((!Terrain.TerrainEquals(global.Terraform_Flag, Terrain.TerrainType.NOTHING)) &&
            (!t.HasBuilding()) && (!t.IsRiverTile()) &&
            local_player.CheckCanTerraform(global.Terraform_Flag, t.GetTerrainType()))
        {
            Terraform(global.Terraform_Flag, t);
        }
    }

    void Parse_Flags_RoundEnd() //phase 3 add accumulative points
    {
        local_player.CalculateAccumulativeBonuses();
        local_player.Current_Round_Bonus = null;
        End_Phase();
    }

    //End a players turn, if he's the last remaining player and he retires this turn, go to next phase
    public void EndTurn()
    {
        if (Check_End_Phase())
        {
            phase = (phase + 1) % 3;
            if (phase == 0) { phase++; } //Do not go back to set up phase
            turn_c.Cmd_Next_Phase();
            Set_Up_Phase();
        }
        else
        {
            local_player.EndTurn();
            turn_c.Cmd_Next_Player();
        }
    }

    //End a players RoundMain phase, triggered by user via UI
    public void End_Round()
    {
        local_player.CalculateAccumulativeBonuses(); //Calc points
        local_player.Add_To_NonBuilding_Income(local_player.Current_Round_Bonus.Round_Income.Reciprocal().Income_AsArray()); //Calc new income and update UI
        local_player.Reset_Temporary_Point_Bonuses(); //Reset temp bonuses so they dont get carried over
        End_Phase();
    }
    //Check if all players are retired this phase
    bool Check_End_Phase()
    {
        if (turn_c.All_Players_Retired())
        {
            return true;
        }
        return false;
    }
    //Retire a player when hes finished his phase
    public void End_Phase()
    {
        turn_c.Cmd_Retire_Player();
        EndTurn();
    }

    public void CastMagic()
    {
        //If Magic has been invoked
        if ((!MagicController.Equals(global.Magic_Cast_Flag, MagicController.Spell_Type.NOTHING)) &&
            local_player.CheckCanCastSpell(mc.GetSpell((int)global.Magic_Cast_Flag)))
        {
            if (global.Magic_Cast_Flag == MagicController.Spell_Type.BURN)
            {
                local_player.BurnMagic();
            }
            if((int)global.Magic_Cast_Flag < 3)//If it's a regular spell
            {
                local_player.CastSpell(mc.GetSpell((int)global.Magic_Cast_Flag));
            }
            //If it's a world spell we need an extra step
            if ((int)global.Magic_Cast_Flag <= 8 && !mc.Check_WorldMagic_Used(global.Magic_Cast_Flag))
            {
                if (global.Magic_Cast_Flag == MagicController.Spell_Type.World_Bridge)
                {
                    local_player.CastSpell(mc.GetSpell((int)global.Magic_Cast_Flag)); //Cast spell to remove magic from tier 3 pool
                    bm.Allow_Bridge(local_player.GetID());
                }
                else
                {
                    local_player.CastSpell(mc.GetSpell((int)global.Magic_Cast_Flag));
                }
            }

        }
    }

    void Terraform(Terrain.TerrainType t_type, Tile t)
    {
        local_player.Terraform(t_type, t.GetTerrainType());
        t.Terraform(t_type);
        global.Reset_Flags();
        EndTurn();
    }

    /* Builds a building without costing any player resouces */
    void FreeBuild(Building.Building_Type bt, Tile t)
    {
        //if the final Building class checks pass then build and finally tax the player.
        if (t.Build(bt, local_player.Get_Building_Material()))
        {
            local_player.FreeBuild(bt);
            local_player.AddBuiltCoordinate(t);

            Building.Building_Type temp = global.Build_Flag;
            global.Reset_Flags();

            if (board.JoinTileToTownGroup(t, local_player))
            {
                nui.PanelInteractivityEnDisable(nui.TownTilePanel, true);
                if (local_player.CheckWonderTile(temp))
                {
                    nui.PanelInteractivityEnDisable(nui.WonderTilePanel, true);
                }
            }
            else if (local_player.CheckWonderTile(temp))
            {
                nui.PanelInteractivityEnDisable(nui.WonderTilePanel, true);
            }
        }
    }

    void Build(Building.Building_Type bt, Tile t)
    {
        //if the final Building class checks pass then build and finally tax the player.
        if (t.Build(bt, local_player.Get_Building_Material()))
        {
            local_player.Build(bt);
            local_player.AddBuiltCoordinate(t);
            local_player.CheckPoints(PointBonus.MapBuildingToAction(global.Build_Flag));

            Building.Building_Type temp = global.Build_Flag;
            global.Reset_Flags();

            if (board.JoinTileToTownGroup(t, local_player))
            {
                nui.PanelInteractivityEnDisable(nui.TownTilePanel, true);
                if (local_player.CheckWonderTile(temp))
                {
                    nui.PanelInteractivityEnDisable(nui.WonderTilePanel, true);
                }
            }

            else if (local_player.CheckWonderTile(temp))
            {
                nui.PanelInteractivityEnDisable(nui.WonderTilePanel, true);
            }
        }
    }

    /* USED PURELY FOR THE +1 CULT ABILITY */
    public void IncreaseCult(int track)
    {
        switch (track)
        {
            case 0:
                ct.Add_To_Track_Level(new CultIncome(1,0,0,0), local_player); break;
            case 1:
                ct.Add_To_Track_Level(new CultIncome(0, 1, 0, 0), local_player); break;
            case 2:
                ct.Add_To_Track_Level(new CultIncome(0, 0, 1, 0), local_player); break;
            case 3:
                ct.Add_To_Track_Level(new CultIncome(0, 0, 0, 1), local_player); break;
        }
        
    }

    public void Add_Town_Tile(int type)
    {
        WonderTile tc = ttc.Get_TownTile((TownTileController.TownTile_type)type);
        if (tc != null)
        {
            nui.UpdateTownTile();

            //Increase players income and points
            local_player.AddSingleIncome(tc.GetIncome());
            local_player.AddSinglePointValue(tc.Points);

            //Increase players cult standing. Tell the track to increase too
            Add_To_Track(tc.Get_CultIncome());

            //turn off buying ability
            nui.PanelInteractivityEnDisable(nui.TownTilePanel, false);

            EndTurn();
        }

    }

    public void Add_To_Track(CultIncome ci_)
    {
        if (ci_ != null)
        {
            local_player.AddSingleIncome(new SingleIncome(0,0,0, ct.Add_To_Track_Level(ci_, local_player))); //Add to the cult track and then add any owed magic to player
            ti.Progress_Track(TracksInterface.ProgressTrackRef.Cult_fire, local_player.Get_CultData().Get_Levels()[0], local_player.GetID());
            ti.Progress_Track(TracksInterface.ProgressTrackRef.Cult_water, local_player.Get_CultData().Get_Levels()[1], local_player.GetID());
            ti.Progress_Track(TracksInterface.ProgressTrackRef.Cult_earth, local_player.Get_CultData().Get_Levels()[2], local_player.GetID());
            ti.Progress_Track(TracksInterface.ProgressTrackRef.Cult_air, local_player.Get_CultData().Get_Levels()[3], local_player.GetID());
        }
    }

    public void Add_Wonder_Tile(int tracktier)
    {
        int[] temp = CultController.ParseInt(tracktier); //Parse e.g. 12 into track 1 tier 2
        int track = temp[0]-1; int tier = temp[1];

        WonderTile wt = wc.GetWonderTile(track, tier);

        if (track == 0 && tier == 1) //If its fire 2 or W2 then it requires special treatment
        {
            local_player.PointsForTown = 6;
        }
        else if (track == 1 && tier == 1)
        {
            am.AddAbility<Ability_AddToCult>();
        }
        if (wt != null)
        {
            nui.UpdateWonderTile(track, tier);

            //Increase players income and points
            local_player.Add_To_NonBuilding_Income(wt.GetIncome().Income_AsArray());
            if (wt.GetPointBonus() != null) { local_player.Add_Point_Bonus(wt.GetPointBonus(), false); }

            //Increase players cult standing. Tell the track to increase too
            Add_To_Track(wt.Get_CultIncome());

            nui.PanelInteractivityEnDisable(nui.WonderTilePanel, false);

            EndTurn();
        }
    }

    public void DebugAddMagic()
    {
        local_player.AddSingleIncome(new SingleIncome(0,0,0,1));
    }

    bool Check_Local_Turn()
    {
        return local_player.GetID() == turn_c.Current_Player();
    }
}
