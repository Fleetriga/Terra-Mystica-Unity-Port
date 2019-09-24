using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Networked_UI_Updater : MonoBehaviour
{
    //panels
    public GameObject wonderTilePanel;
    public GameObject townTilePanel;
    public GameObject round_bonus_panel;

    //Panels PROPERTIES
    public GameObject WonderTilePanel { get { return wonderTilePanel; }}
    public GameObject TownTilePanel { get { return townTilePanel; }}
    public GameObject Round_Bonus_Panel { get { return round_bonus_panel; }}


    //Does the necessary set up for the start of the game
    public void SetUpUI()
    {
        PanelInteractivityEnDisable(WonderTilePanel, false);
        PanelInteractivityEnDisable(TownTilePanel, false);
    }

    //Disables/Enables interactivity in the child button components
    public void PanelInteractivityEnDisable(GameObject parent, bool endisable)
    {
        Button[] buttons = parent.GetComponentsInChildren<Button>();

        foreach (Button b in buttons)
        {
            b.interactable = endisable;
        }
    }
    //Disables/Enables interactivity in the child button components
    public void InteractivityEnDisable(Button button, bool endisable)
    {
        button.interactable = endisable;
    }

    public void Show_Hide_Round_Bonuses(bool show_or)
    {
        round_bonus_panel.SetActive(show_or);
    }

    internal void UpdateWonderTile(int track, int tier)
    {
    }

    public void UpdateTownTile()
    {

    }

    internal GameObject GetTownTilePanel()
    {
        throw new NotImplementedException();
    }

    internal void Show_Hide_RoundMain_Panels(bool v)
    {
        throw new NotImplementedException();
    }
}
