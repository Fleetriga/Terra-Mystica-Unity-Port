using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundEndBonus : MonoBehaviour
{
    public PointBonus RoundEndPointBonus;
    CultIncome cultCostPer; //How much for 1 bonus income i.e. it takes 2 fire to get 1 shovel. If you have 4 fire you get two shovels.
    SingleIncome cultPerBonus; //What you get per bonus income
    public RoundEndBonusType Identifier;

    public enum RoundEndBonusType { Dwelling_WaterPriest, Dwelling_FireMagic, TP_AirShovel, TP_WaterShovel, T3_AirWorker, T3_FireWorker, Terra_EarthGold, Town_EarthShovel};

    public RoundEndBonus(PointBonus pb, CultIncome ci, SingleIncome si)
    {
        RoundEndPointBonus = pb;
        cultCostPer = ci;
        cultPerBonus = si;
    }

    public SingleIncome GetTotalBouns(CultData playersTotal)
    {
        int nankaiDekita = 0;
        if (playersTotal.GetLevels()[0] > 0) { nankaiDekita = cultCostPer.GetIncome()[0] / playersTotal.GetLevels()[0];} //Fire
        if (playersTotal.GetLevels()[1] > 0) { nankaiDekita = cultCostPer.GetIncome()[1] / playersTotal.GetLevels()[1]; } //water
        if (playersTotal.GetLevels()[2] > 0) { nankaiDekita = cultCostPer.GetIncome()[2] / playersTotal.GetLevels()[2]; } //earth
        if (playersTotal.GetLevels()[3] > 0) { nankaiDekita = cultCostPer.GetIncome()[3] / playersTotal.GetLevels()[3]; } //air

        return cultPerBonus.Multiply(nankaiDekita);
    }

}
