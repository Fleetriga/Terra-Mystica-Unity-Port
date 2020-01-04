using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class TakenTurnData : NetworkBehaviour
{
    public int CoordinateX;
    public int CoordinateY;

    public Building.Building_Type Built;
    public Terrain.TerrainType Terraformed;
    public TownFoundingBonusManager.TownTiletype FoundedCityBonus;
    public RoundBonusManager.RoundBonusType PickedRoundBonus;
    public RoundBonusManager.RoundBonusType ReturnedRoundBonus;
    public MagicController.SpellType CastedSpell;
    public int MineTaken = 0;

    public enum ChangeFlag { Build, Terraform, RoundBonus, FoundedCity, FavourBonus, CastWorldMagic, BuildBridge, MineTaken, NOTHING };
    public ChangeFlag Change = ChangeFlag.NOTHING;


    void Awake()
    {
        ResetData();     
    }

    public void SendBuildData(int[] coordinate, Building.Building_Type newBuilding)
    {
        Cmd_ChangeBuildFlagData(newBuilding);
        Cmd_ChangeCoordinateData(coordinate[0], coordinate[1]);
        Cmd_SetChangeFlag(ChangeFlag.Build);
    }

    public void SendMineTakenData(int track)
    {
        Cmd_SetMineTakenData(track);
    }

    public void SendTerraformData(int[] coordinate, Terrain.TerrainType newTerrain)
    {
        Cmd_ChangeTerraformFlagData(newTerrain);
        Cmd_ChangeCoordinateData(coordinate[0], coordinate[1]);
        Cmd_SetChangeFlag(ChangeFlag.Terraform);
    }

    public void SendCityFoundingData(TownFoundingBonusManager.TownTiletype foundedBonus)
    {
        Cmd_ChangeFoundedCityData(foundedBonus);
        Cmd_SetChangeFlag(ChangeFlag.FoundedCity);
    }

    public void SendRoundBonusPicked(RoundBonusManager.RoundBonusType bonus)
    {
        Cmd_ChangeRoundBonusPickedData(bonus);
        Cmd_SetChangeFlag(ChangeFlag.RoundBonus);
    }

    public void SendRoundBonusReturned(RoundBonusManager.RoundBonusType returnedBonus)
    {
        Cmd_ChangeReturnedRoundBonusPickedData(returnedBonus);
    }

    public void SendPickedFavourData(int track, int tier)
    {
        Cmd_ChangeFavourPickedData(track, tier);
        Cmd_SetChangeFlag(ChangeFlag.FavourBonus);
    }

    public void SendCastedSpellData(MagicController.SpellType casted)
    {
        Cmd_ChangeCastedSpellData(casted);
        Cmd_SetChangeFlag(ChangeFlag.CastWorldMagic);
    }

    public void SendBuiltBridgeData(int bridgeID)
    {
        CoordinateX = bridgeID;
        Cmd_ChangeCoordinateData(bridgeID, 0);
        Cmd_SetChangeFlag(ChangeFlag.BuildBridge);
    }

    //Only resets locally, prevents turns being executed twice.
    public void ResetData()
    {
        CoordinateX = 0; CoordinateY = 0;
        Built = Building.Building_Type.NOTHING;
        Terraformed = Terrain.TerrainType.NOTHING;
        PickedRoundBonus = RoundBonusManager.RoundBonusType.NOTHING;
        ReturnedRoundBonus = RoundBonusManager.RoundBonusType.NOTHING;
        FoundedCityBonus = TownFoundingBonusManager.TownTiletype.NOTHING;
        CastedSpell = MagicController.SpellType.NOTHING;
        MineTaken = 20;

        Change = ChangeFlag.NOTHING;
    }

    #region Data Set Commands
    [Command]
    void Cmd_ChangeCoordinateData(int x, int y)
    {
        Rpc_ChangeCoordinateData(x, y);
    }
    [ClientRpc]
    void Rpc_ChangeCoordinateData(int x, int y)
    {
        CoordinateX = x;
        CoordinateY = y;
    }

    [Command]
    void Cmd_SetMineTakenData(int track)
    {
        Rpc_SetMineTakenData(track);
    }
    [ClientRpc]
    void Rpc_SetMineTakenData(int track)
    {
        Change = ChangeFlag.MineTaken;
        MineTaken = track;
    }

    [Command]
    void Cmd_ChangeBuildFlagData(Building.Building_Type newBuilding)
    {
        Rpc_ChangeBuildFlagData(newBuilding);
    }
    [ClientRpc]
    void Rpc_ChangeBuildFlagData(Building.Building_Type newBuilding)
    {
        Built = newBuilding;
    }

    [Command]
    void Cmd_ChangeTerraformFlagData(Terrain.TerrainType newTerrain)
    {
        Rpc_ChangeTerraformFlagData(newTerrain);
    }
    [ClientRpc]
    void Rpc_ChangeTerraformFlagData(Terrain.TerrainType newTerrain)
    {
        Terraformed = newTerrain;
    }

    [Command]
    void Cmd_ChangeFoundedCityData(TownFoundingBonusManager.TownTiletype bonus)
    {
        Rpc_ChangeFoundedCityData(bonus);
    }
    [ClientRpc]
    void Rpc_ChangeFoundedCityData(TownFoundingBonusManager.TownTiletype bonus)
    {
        FoundedCityBonus = bonus;
    }

    [Command]
    void Cmd_ChangeRoundBonusPickedData(RoundBonusManager.RoundBonusType bonus)
    {
        Rpc_ChangeRoundBonusPickedData(bonus);
    }
    [ClientRpc]
    void Rpc_ChangeRoundBonusPickedData(RoundBonusManager.RoundBonusType bonus)
    {
        PickedRoundBonus = bonus;
    }

    [Command]
    void Cmd_ChangeFavourPickedData(int track, int tier)
    {
        Rpc_ChangeFavourPickedData(track, tier);
    }
    [ClientRpc]
    void Rpc_ChangeFavourPickedData(int track, int tier)
    {
        CoordinateX = track; CoordinateY = tier;
    }

    [Command]
    void Cmd_ChangeCastedSpellData(MagicController.SpellType casted)
    {
        Rpc_ChangeCastedSpellData(casted);
    }
    [ClientRpc]
    void Rpc_ChangeCastedSpellData(MagicController.SpellType casted)
    {
        CastedSpell = casted;
    }

    [Command]
    public void Cmd_ChangeReturnedRoundBonusPickedData(RoundBonusManager.RoundBonusType returnedBonus)
    {
        Rpc_ChangeReturnedRoundBonusPickedData(returnedBonus);
    }
    [ClientRpc]
    public void Rpc_ChangeReturnedRoundBonusPickedData(RoundBonusManager.RoundBonusType returnedBonus)
    {
        ReturnedRoundBonus = returnedBonus;
    }

    [Command]
    void Cmd_SetChangeFlag(ChangeFlag newFlag)
    {
        Rpc_SetChangeFlag(newFlag);
    }
    [ClientRpc]
    void Rpc_SetChangeFlag(ChangeFlag newFlag)
    {
        Change = newFlag;
    }
    #endregion
}
