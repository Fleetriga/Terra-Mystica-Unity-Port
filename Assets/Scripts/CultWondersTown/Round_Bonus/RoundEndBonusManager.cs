using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundEndBonusManager : MonoBehaviour
{
    RoundEndBonus[] roundEndBonuses;
    public Sprite finishedRoundEndBonus;
    public GameObject[] roundEndBonusGameObjects;
    public Transform activeBonuses;
    public Transform inactiveBonuses;
    //List<RoundBonusType> usedBonuses;

    public void SetUp()
    {
        UnityEngine.Random.InitState(GameObject.Find("RandomSeed").GetComponent<NetworkedRandomSeed>().randomSeed);

        roundEndBonuses = new RoundEndBonus[] { new RoundEndBonus(new PointBonus(PointBonus.Action_Type.Build_Dwelling, 2), new CultIncome(0,4,0,0), new SingleIncome(0,0,1,0,0)),
                                                new RoundEndBonus(new PointBonus(PointBonus.Action_Type.Build_Dwelling, 2), new CultIncome(4,0,0,0), new SingleIncome(0,0,0,4,0)),
                                                new RoundEndBonus(new PointBonus(PointBonus.Action_Type.Build_TP, 3), new CultIncome(0,0,0,4), new SingleIncome(0,0,0,0,1)),
                                                new RoundEndBonus(new PointBonus(PointBonus.Action_Type.Build_TP, 3), new CultIncome(0,4,0,0), new SingleIncome(0,0,0,0,1)),
                                                new RoundEndBonus(new PointBonus(PointBonus.Action_Type.BuildT3, 5), new CultIncome(0,0,0,2), new SingleIncome(0,1,0,0,0)),
                                                new RoundEndBonus(new PointBonus(PointBonus.Action_Type.BuildT3, 5), new CultIncome(2,0,0,0), new SingleIncome(0,1,0,0,0)),
                                                new RoundEndBonus(new PointBonus(PointBonus.Action_Type.Terraform, 2), new CultIncome(0,0,1,0), new SingleIncome(1,0,0,0,0)),
                                                new RoundEndBonus(new PointBonus(PointBonus.Action_Type.FoundTown, 5), new CultIncome(0,0,4,0), new SingleIncome(0,0,0,0,1)),};
        //
        for (int i = 0; i < 6; i++) //pick 6 random round end bonuses. Set them as active
        {
            inactiveBonuses.GetChild(UnityEngine.Random.Range(0, inactiveBonuses.transform.childCount)).SetParent(activeBonuses);
        }
        for (int i = 0; i < activeBonuses.childCount; i++) //Reposition this games bonuses
        {
            activeBonuses.GetChild(i).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (activeBonuses.GetChild(i).GetComponent<RectTransform>().rect.height * i) + 2);
        }

        inactiveBonuses.gameObject.SetActive(false); //Disappear the bonuses not in this game

    }

    public RoundEndBonus GetRoundEndBonus(int round)
    {
        return roundEndBonuses[(int)activeBonuses.GetChild(round).GetComponent<RoundEndBonus>().Identifier];
    }

    public void ResetBonuses()
    {

    }

    public void GreyOutRoundEndBonus(int currentRoundNumber)
    {
        activeBonuses.GetChild(currentRoundNumber).GetComponent<Image>().sprite = finishedRoundEndBonus;
    }
}
