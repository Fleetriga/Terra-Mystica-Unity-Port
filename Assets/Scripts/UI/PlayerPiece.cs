using UnityEngine;
using UnityEngine.UI;

public class PlayerPiece : MonoBehaviour {

    public BinUI CurrentBin { get; set; }
    public int PlayerID { get { return playerID; } }
    int playerID;

    public void SetUp(int playerID_, Sprite rep_)
    {
        playerID = playerID_;
        GetComponent<Image>().sprite = rep_;
    }

    public void SetCurrentBin(BinUI bin)
    {
        CurrentBin = bin;
    }
}
