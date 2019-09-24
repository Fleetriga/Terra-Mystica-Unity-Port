using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTrack : MonoBehaviour {

    BinUI[] bins;
    PlayerPiece[] containing;
    public GameObject[] offsets;
    public GameObject[] panels;

    public void SetUp()
    {
        bins = new BinUI[panels.Length];
        containing = new PlayerPiece[5];
        Create_Track(panels.Length);
    }

    public void Create_Track(int size)
    {
        CreateBins(size);
    }

    public void AddPlayerPiece(int playerID, PlayerPiece pp_, int index)
    {
        containing[playerID] = pp_;
        bins[index].Add(containing[playerID]);
        RefactorPeices(index);
    }

    void CreateBins(int size)
    {
        for (int i = 0; i < bins.Length; i++)
        {
            bins[i] = new BinUI(containing.Length); ;
        }
    }

    void RefactorPeices(int index) //Give all peices the correct offset
    {
        GameObject offsetTemp = offsets[bins[index].GetSize()-1];
        RectTransform[] transforms = offsetTemp.GetComponentsInChildren<RectTransform>();

        int i = 0;
        foreach (PlayerPiece pp_ in bins[index].Get_PlayerPieces())
        {
            pp_.transform.SetParent(panels[index].transform); //First make it a child of that track square
            pp_.GetComponent<RectTransform>().localPosition = transforms[i].localPosition; //Then set it's local position to the correct offset
            i++; //and increase the offset by one
        }
    }

    //Moves the chess peice of the player along its track
    public void Progress_To_Track(int playerID, int index)
    {
        containing[playerID].Get_BinUI().Remove(containing[playerID]);
        bins[index].Add(containing[playerID]);
        RefactorPeices(index);
    }
}
