using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundBonusManager : MonoBehaviour {

    Round_Bonus[] round_bonuses;
    public GameObject[] roundBonusGameObjects;
    public Transform activeBonuses;
    public Transform inactiveBonuses;
    List<RoundBonusType> takenBonuses;

    public enum RoundBonusType { Priest, OneWorker_ThreeMagic, SixGold, ThreePower_OneShipping, OneTerraform_TwoGold, OneCult_FourGold, DwellingBonus_TwoGold, TPBonus_OneWorker, TierThreeBonus_TwoWorkers, NOTHING };

	public void SetUp()
    {
        int noPlayers = 2;
        Random.InitState(GameObject.Find("RandomSeed").GetComponent<NetworkedRandomSeed>().randomSeed);

        round_bonuses = new Round_Bonus[] { new Round_Bonus(null, new SingleIncome(0,0,1,0,0), RoundBonusType.Priest), //Priest
                                            new Round_Bonus(null, new SingleIncome(0,1,0,3,0), RoundBonusType.OneWorker_ThreeMagic), //1 worker 3 magic
                                            new Round_Bonus(null, new SingleIncome(6,0,0,0,0), RoundBonusType.SixGold), //6 gold
                                            new Round_Bonus(null, new SingleIncome(0,0,0,3,0), RoundBonusType.ThreePower_OneShipping), //3 power 1 shipping
                                            new Round_Bonus(null, new SingleIncome(2,0,0,0,0), RoundBonusType.OneTerraform_TwoGold), //Special action 1 terraform, 2 gold
                                            new Round_Bonus(null, new SingleIncome(4,0,0,0,0), RoundBonusType.OneCult_FourGold), //Special action 1 cult, 4 gold
                                            new Round_Bonus(new PointBonus(PointBonus.Action_Type.Have_Dwelling, 1), new SingleIncome(2,0,0,0,0), RoundBonusType.DwellingBonus_TwoGold),  //Have dwelling point bonus, 2 gold
                                            new Round_Bonus(new PointBonus(PointBonus.Action_Type.Have_TP, 2), new SingleIncome(0,1,0,0,0), RoundBonusType.TPBonus_OneWorker), //Have TP point bonus, 1 worker
                                            new Round_Bonus(new PointBonus(PointBonus.Action_Type.Have_Tier3Buildings, 4), new SingleIncome(0,2,0,0,0), RoundBonusType.TierThreeBonus_TwoWorkers) //Have tier 3 building point bonus, 2 workers
        };

        for (int i = 0; i < noPlayers+3; i++) //pick random round bonuses
        {
            inactiveBonuses.GetChild(Random.Range(0, inactiveBonuses.transform.childCount)).SetParent(activeBonuses);
        }
        for (int i = 0; i < activeBonuses.childCount; i++) //Reposition this games bonuses
        {
            activeBonuses.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector2((activeBonuses.GetChild(i).GetComponent<RectTransform>().rect.width * i) + 5, 0);
        }

        inactiveBonuses.gameObject.SetActive(false); //Disappear the bonuses not in this game

        takenBonuses = new List<RoundBonusType>();
    }

    public Round_Bonus GetRoundBonus(RoundBonusType rbt)
    {
        return round_bonuses[(int)rbt];
    }

    public void TakeRoundBonus(RoundBonusType taken)
    {
        takenBonuses.Add(taken);
        foreach (Transform go in activeBonuses)
        {
            if (go.GetComponent<RoundBonusMono>().type == taken) { go.gameObject.SetActive(false); }
        }
    }

    public void ResetAvailableBonuses()
    {
        takenBonuses = new List<RoundBonusType>();
    }

    public void AppearRoundBonuses()
    {
        foreach (Transform go in activeBonuses)
        {
            go.gameObject.SetActive(true);
            foreach (RoundBonusType taken in takenBonuses) { if (go.GetComponent<RoundBonusMono>().type == taken) { go.gameObject.SetActive(false); } } //If this round bonus was taken before by some other player, Ignore this shit.
        }
    }

}
