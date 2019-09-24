using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Updater : MonoBehaviour {

    //Start of the game
    public Text dwelling_worker_cost;
    public Text dwelling_money_cost;

    public Text tp_worker_cost;
    public Text tp_money_cost;

    public Text stronghold_worker_cost;
    public Text stronghold_money_cost;

    public Text temple_worker_cost;
    public Text temple_money_cost;

    public Text sanctuary_worker_cost;
    public Text sanctuary_money_cost;

    //During Gameplay
    //Resource Updates
    public Text money_count_UI;
    public Text worker_count_UI;
    public Text priest_count_UI;
    public Text shovel_count_UI;
    public Text[] magic_count_UI;
    public Text points_count_UI;

    //Cults
    public Text fire_cult;
    public Text water_cult;
    public Text earth_cult;
    public Text air_cult;

    //Income Updates
    public Text money_income_UI;
    public Text worker_income_UI;
    public Text priest_income_UI;

    //UI Components
    public Remaining_Building_UI_Interface remaining_building_UI;

    public void UpdateResourceText(int[] counts, int[] incomes)
    {
        money_count_UI.text = counts[0].ToString();
        worker_count_UI.text = counts[1].ToString();
        priest_count_UI.text = counts[2].ToString();
        shovel_count_UI.text = counts[3].ToString();

        money_income_UI.text = incomes[0].ToString();
        worker_income_UI.text = incomes[1].ToString();
        priest_income_UI.text = incomes[2].ToString();
    }

    public void UpdateMagicTiers(int[] tiers)
    {
        magic_count_UI[0].text = tiers[0].ToString();
        magic_count_UI[1].text = tiers[1].ToString();
        magic_count_UI[2].text = tiers[2].ToString();
    }

    public void UpdatePoints(int points_)
    {
        points_count_UI.text = points_.ToString();
    }

    public void UpdatePlayerBuildingCosts(int[] dwl, int[] tp, int[] tem, int[] san, int[] str )
    {
        dwelling_money_cost.text = dwl[0].ToString();
        dwelling_worker_cost.text = dwl[1].ToString();

        tp_money_cost.text = tp[0].ToString();
        tp_worker_cost.text = tp[1].ToString();

        stronghold_money_cost.text = str[0].ToString();
        stronghold_worker_cost.text = str[1].ToString();

        temple_money_cost.text = tem[0].ToString();
        temple_worker_cost.text = tem[1].ToString();

        sanctuary_money_cost.text = san[0].ToString();
        sanctuary_worker_cost.text = san[1].ToString();
    }

    public GameObject GetResourcePanel()
    {
        return null;
    }

    public void UpdatePlayerCultTrack(CultData cd)
    {
        fire_cult.text = cd.Get_Levels()[0].ToString();
        water_cult.text = cd.Get_Levels()[1].ToString();
        earth_cult.text = cd.Get_Levels()[2].ToString();
        air_cult.text = cd.Get_Levels()[3].ToString();
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
        remaining_building_UI.Display_Remaining(bt_, building_index);
    }
}
