using System;
using UnityEngine;

public class BinUI : MonoBehaviour {

    public PlayerPiece[] ContainingPieces { get; set; }
    int size;

    public void SetUp(int maxPlayers)//Create without providing a next, i.e this bin is the end of the track
    {
        ContainingPieces = new PlayerPiece[maxPlayers];
    }

    public void Add(PlayerPiece pp_) //Add a player by its playerID
    {
        size++;
        ContainingPieces[pp_.PlayerID] = pp_;
        pp_.CurrentBin = this;

        //Move the player peice here
        RefactorPeices();
    }

    public void Remove(PlayerPiece pp_) //remove the playerID, should only ever be 1 per playerID so its safe to not organise the list
    {
        ContainingPieces[pp_.PlayerID] = null; size--;
    }

    public void RefactorPeices()
    {
        foreach (PlayerPiece pp_ in ContainingPieces)
        {
            if (pp_ != null)
            {
                pp_.transform.parent = transform;
                pp_.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    /**
     * Returns a list of player pieces contained in this thing, no nulls included
     */
    public PlayerPiece[] Get_PlayerPieces()
    {
        PlayerPiece[] temp = new PlayerPiece[size];
        int i = 0;
        foreach (PlayerPiece pp in ContainingPieces)
        {
            if (pp != null) { temp[i] = pp; i++; }
        }
        return temp;
    }

    public int GetSize()
    {
        return size;
    }
}
