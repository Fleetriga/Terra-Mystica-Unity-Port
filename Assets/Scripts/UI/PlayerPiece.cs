using UnityEngine;
using UnityEngine.UI;

public class PlayerPiece : MonoBehaviour {

    BinUI currentBin;
    int playerID;

    public void SetUp(int playerID_, Sprite rep_)
    {
        playerID = playerID_;
        GetComponent<Image>().sprite = rep_;
    }

    public void SetCurrentBin(BinUI bin)
    {
        currentBin = bin;
    }

    public BinUI Get_BinUI()
    {
        return currentBin;
    }

    public int Get_PlayerID()
    {
        return playerID;
    }
}
