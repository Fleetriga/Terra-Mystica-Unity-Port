using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MineShaftManager : MonoBehaviour
{
    [SerializeField]private Image[] Mines;
    [SerializeField]private Sprite TakenMine;
    [SerializeField]private GameController gameController;
    [SerializeField]private int track;

    int remainingMines = 3;

    int[] mineValues = { 2,2,2,3 };

    public void OnMineClicked()
    {
        gameController.MinePicked(ToCultIncome(), track);
    }

    public void TakeMine()
    {
        Mines[remainingMines].sprite = TakenMine;
        remainingMines--;
    }

    CultIncome ToCultIncome()
    {
        switch (track)
        {
            case 0: return new CultIncome(mineValues[remainingMines], 0,0,0);
            case 1: return new CultIncome(0, mineValues[remainingMines], 0, 0);
            case 2: return new CultIncome(0, 0, mineValues[remainingMines], 0);
            case 3: return new CultIncome(0, 0, 0, mineValues[remainingMines]);
        }
        return null;
    }

}
