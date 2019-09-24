using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Networked_UI_Updater : MonoBehaviour
{
    //panels
    public GameObject favourTilePanel;
    public GameObject townTilePanel;
    public GameObject roundBonusFrame;

    //Button holders
    public GameObject favourButtons;
    public GameObject townButtons;
    public GameObject roundBonusButtons;

    //Panels PROPERTIES
    public GameObject FavourTilePanel { get { return favourTilePanel; }}
    public GameObject TownTilePanel { get { return townTilePanel; }}

    //Does the necessary set up for the start of the game
    public void SetUpUI()
    {
        PanelInteractivityEnDisable(favourButtons, false);
        PanelInteractivityEnDisable(townButtons, false);
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

    public void DisEnableRoundBonuses(bool activate)
    {
        roundBonusFrame.SetActive(activate);
    }

    public void DisEnableTownTiles()
    {
        townTilePanel.GetComponent<OpenCloseOnClick>().OnClick();
    }

    public void DisEnableFavourTiles()
    {
        favourTilePanel.GetComponent<OpenCloseOnClick>().OnClick();
    }
}
