using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSimulator : MonoBehaviour
{
    [SerializeField] Board tileMap;
    [SerializeField] BridgeManager bridgeManager;
    [SerializeField] CultTrackManager cultTrackManager;
    [SerializeField] SoundEffects soundEffects;
    TownFoundingBonusManager townFoundManager;
    WonderController wonderController;
    MagicController magicController;
    RoundBonusManager roundBonusManager;

    //For identifying current player. Saves computation time.
    GameObject[] players;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("NetworkedPlayerObjects");
        townFoundManager = GameObject.Find("Controller").GetComponent<TownFoundingBonusManager>();
        wonderController = GameObject.Find("Controller").GetComponent<WonderController>();
        magicController = GameObject.Find("Controller").GetComponent<MagicController>();
        roundBonusManager = GameObject.Find("Controller").GetComponent<RoundBonusManager>();
    }

    //Scan for player turns taken. Simulate them
    void Update()
    {
        foreach (GameObject go in players)
        {
            TakenTurnData ttd = go.GetComponent<TakenTurnData>();
            if (ttd.Change != TakenTurnData.ChangeFlag.NOTHING)
            {
                switch (ttd.Change)
                {
                    case TakenTurnData.ChangeFlag.Build: SimulateBuild(new int[] { ttd.CoordinateX, ttd.CoordinateY }, ttd.Built, go.GetComponent<PlayerNetworked>().GetFactionMaterial(), go.GetComponent<PlayerNetworked>().PlayerID); break;
                    case TakenTurnData.ChangeFlag.Terraform: SimulateTerraform(new int[] { ttd.CoordinateX, ttd.CoordinateY }, ttd.Terraformed); break;
                    case TakenTurnData.ChangeFlag.RoundBonus: SimulateRoundBonusPicked(ttd.PickedRoundBonus, ttd.ReturnedRoundBonus); break;
                    case TakenTurnData.ChangeFlag.FoundedCity: SimulateCityFounding(ttd.FoundedCityBonus); break;
                    case TakenTurnData.ChangeFlag.FavourBonus: SimulateFavourTaken(ttd.CoordinateX, ttd.CoordinateY); break;
                    case TakenTurnData.ChangeFlag.CastWorldMagic: CastWorldMagic(ttd.CastedSpell); break;
                    case TakenTurnData.ChangeFlag.BuildBridge: BuildBridge(ttd.CoordinateX); break;
                    case TakenTurnData.ChangeFlag.MineTaken: TakeMine(ttd.MineTaken); break;
                }

                //Reset data locally afterwards so that this is body isn't triggered again until the player does something else.
                ttd.ResetData();
            }
        }
    }

    //Takes a coordinate, type of building and player information and simulates
    //the building of given building_type. Also records this to the Board class.
    public void SimulateBuild(int[] coordinate, Building.Building_Type typeToBuild, Material playersBuildingMaterial, int playerID)
    {
        tileMap.GetTile(coordinate).Build(typeToBuild, playersBuildingMaterial);
        tileMap.GetTile(coordinate).OwnersPlayerID = playerID;

        soundEffects.PlaySFX(SoundEffects.SFX.build_SFX);
    }

    //Takes a coordinate, type of terrain and player information and simulates
    //the terraforming of the given tile. Ownership of this tile is not given at this point
    public void SimulateTerraform(int[] coordinate, Terrain.TerrainType terrain)
    {
        tileMap.GetTile(coordinate).Terraform(terrain);

        soundEffects.PlaySFX(SoundEffects.SFX.terraform_SFX);
    }

    //Takes city founding bonus tile. Reduces the amount of the given tile. All bonuses are calculated locally.
    public void SimulateCityFounding(TownFoundingBonusManager.TownTiletype takenBonus)
    {
        townFoundManager.ReduceTownTile(takenBonus);
    }

    //Takes favor tile. Reduces the amount of the given tile. All bonuses are calculated locally.
    public void SimulateFavourTaken(int track, int tier)
    {
        wonderController.ReduceRemainingFavours(track, tier);
    }

    //Takes world magic type. Disables given magic until next turn. All bonuses are calculated locally.
    public void CastWorldMagic(MagicController.SpellType castedSpell)
    {
        magicController.DisableWorldMagic(castedSpell);
    }

    void BuildBridge(int id)
    {
        magicController.DisableWorldMagic(MagicController.SpellType.World_Bridge); //Removes need to worry about having 2 sets of data sent in 1 frame
        bridgeManager.SetBridgeGameObjectActive(id);
    }

    //Takes round bonus. Disables the picking of that round bonus tile.
    void SimulateRoundBonusPicked(RoundBonusManager.RoundBonusType bonusType, RoundBonusManager.RoundBonusType returnedBonus)
    {
        roundBonusManager.TakeRoundBonus(bonusType);
        roundBonusManager.ReturnRoundBonus(returnedBonus);
    }
    void TakeMine(int mineTaken)
    {
        cultTrackManager.TakeMine(mineTaken);
    }

}
