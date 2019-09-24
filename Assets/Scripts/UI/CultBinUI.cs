using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BinUI {

    PlayerPiece[] containingPieces;
    int size;

    public BinUI(int maxPlayers)//Create without providing a next, i.e this bin is the end of the track
    {
        containingPieces = new PlayerPiece[maxPlayers];
    }

    public void Add(PlayerPiece pp_) //Add a player by its playerID
    {
        size++;
        containingPieces[pp_.Get_PlayerID()] = pp_;
        pp_.SetCurrentBin(this);
    }

    public void Remove(PlayerPiece pp_) //remove the playerID, should only ever be 1 per playerID so its safe to not organise the list
    {
        containingPieces[pp_.Get_PlayerID()] = null; size--;
    }

    /**
     * Returns a list of player pieces contained in this thing, no nulls included
     */
    public PlayerPiece[] Get_PlayerPieces()
    {
        PlayerPiece[] temp = new PlayerPiece[size];
        int i = 0;
        foreach (PlayerPiece pp in containingPieces)
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
