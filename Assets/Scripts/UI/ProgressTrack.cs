using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressTrack : MonoBehaviour {

    [SerializeField] protected BinUI[] bins;
    protected PlayerPiece containing;

    public void SetUp()
    {
        CreateTrack(bins.Length);
    }

    public void CreateTrack(int size)
    {
        CreateBins(size);
    }

    public virtual void AddPlayerPiece(int playerID, PlayerPiece pp_, int index)
    {
        containing = pp_;
        bins[index].Add(containing);
    }

    void CreateBins(int size)
    {
        for (int i = 0; i < bins.Length; i++)
        {
            bins[i].SetUp(5);
        }
    }

    //Moves the chess peice of the player along its track
    public void AddProgressToTrack(int playerID, int index)
    {
        containing.CurrentBin.Remove(containing);
        bins[index].Add(containing);
    }
}
