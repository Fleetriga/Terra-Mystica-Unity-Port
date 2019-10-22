using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPriestToCult : MonoBehaviour
{
    public GameObject cults;
    public GameController controller;
    public GameObject offScreenTransform;

    public void TranportCultOptions()
    {
        if (controller.CheckPlayerCanAfford(new SingleIncome(0,0,1,0,0)))
        {
            cults.transform.SetParent(transform);
            cults.GetComponent<RectTransform>().anchoredPosition = new Vector2(GetComponent<RectTransform>().anchoredPosition.x, GetComponent<RectTransform>().anchoredPosition.y + 100);
        }
    }

    // parses input and adds to the players cult track 
    public void AddToCult(int track)
    {
        CultIncome ci = null;
        switch (track)
        {
            case 0:
                ci = new CultIncome(1, 0, 0, 0); break;
            case 1:
                ci = new CultIncome(0, 1, 0, 0); break;
            case 2:
                ci = new CultIncome(0, 0, 1, 0); break;
            case 3:
                ci = new CultIncome(0, 0, 0, 1); break;
        }
        controller.AddCultIncome(ci);
        controller.AddSingleIncome(new SingleIncome(0,0,-1,0,0));
        controller.EndTurn();

        //Then unpair the 4 cult buttons and send it off
        cults.transform.SetParent(offScreenTransform.transform);
        cults.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

}
