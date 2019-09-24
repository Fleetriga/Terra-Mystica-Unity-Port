using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Updater : MonoBehaviour {

    //Start of the game
    public Text dwellingWorkerCost;
    public Text dwellingMoneyCost;

    public Text tpWorkerCost;
    public Text tpMoneyCost;

    public Text strongholdWorkerCost;
    public Text strongholdMoneyCost;

    public Text templeWorkerCost;
    public Text templeMoneyCost;

    public Text sanctuaryWorkerCost;
    public Text sanctuaryMoneyCost;

    //During Gameplay
    //Resource Updates
    public Text moneyCountUI;
    public Text workerCountUI;
    public Text priestCountUI;
    public Text shovelCountUI;
    public Text[] magicCountUI;
    public Text pointsCountUI;

    //Cults
    public Text fireCult;
    public Text waterCult;
    public Text earthCult;
    public Text airCult;

    //Income Updates
    public Text moneyIncomeUI;
    public Text workerIncomeUI;
    public Text priestIncomeUI;

    //UI Components
    public Remaining_Building_UI_Interface remainingBuildingUI;

    public void UpdateResourceText(int[] counts, int[] incomes)
    {
        moneyCountUI.text = counts[0].ToString();
        workerCountUI.text = counts[1].ToString();
        priestCountUI.text = counts[2].ToString();
        shovelCountUI.text = counts[3].ToString();

        moneyIncomeUI.text = incomes[0].ToString();
        workerIncomeUI.text = incomes[1].ToString();
        priestIncomeUI.text = incomes[2].ToString();
    }

    public void UpdateMagicTiers(int[] tiers)
    {
        magicCountUI[0].text = tiers[0].ToString();
        magicCountUI[1].text = tiers[1].ToString();
        magicCountUI[2].text = tiers[2].ToString();
    }

    public void UpdatePoints(int points_)
    {
        pointsCountUI.text = points_.ToString();
    }

    public void UpdatePlayerBuildingCosts(int[] dwl, int[] tp, int[] tem, int[] san, int[] str )
    {
        dwellingMoneyCost.text = dwl[0].ToString();
        dwellingWorkerCost.text = dwl[1].ToString();

        tpMoneyCost.text = tp[0].ToString();
        tpWorkerCost.text = tp[1].ToString();

        strongholdMoneyCost.text = str[0].ToString();
        strongholdWorkerCost.text = str[1].ToString();

        templeMoneyCost.text = tem[0].ToString();
        templeWorkerCost.text = tem[1].ToString();

        sanctuaryMoneyCost.text = san[0].ToString();
        sanctuaryWorkerCost.text = san[1].ToString();
    }
    
    public void UpdatePlayerCultTrack(CultData cd)
    {
        fireCult.text = cd.GetLevels()[0].ToString();
        waterCult.text = cd.GetLevels()[1].ToString();
        earthCult.text = cd.GetLevels()[2].ToString();
        airCult.text = cd.GetLevels()[3].ToString();
    }

    //Disables/Enables interactivity in the child button components
    public void InteractivityEnDisable(GameObject parent, bool endisable)
    {
        Button[] buttons = parent.GetComponentsInChildren<Button>();

        foreach (Button b in buttons)
        {
            b.interactable = endisable;
        }
    }

    public void DisplayRemaining(Building.Building_Type bt_, int building_index)
    {
        remainingBuildingUI.Display_Remaining(bt_, building_index);
    }
}
