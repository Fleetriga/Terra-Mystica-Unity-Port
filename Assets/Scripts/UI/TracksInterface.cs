using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksInterface : MonoBehaviour {

    public ProgressTrack[] offlineTracks;

    //UI references
    UI_Updater ui;
    Networked_UI_Updater nui;

    public enum ProgressTrackRef { Shipping, Terraform };


    public void SetUp()
    {
        foreach (ProgressTrack pt in offlineTracks)
        {
            pt.SetUp();
        }
    }

    /** Moves a players counter within the track
     */
    public void AddProgressToTrack(ProgressTrackRef ptr, int value, int playerID)
    {
        offlineTracks[ParseReference(ptr)].AddProgressToTrack(playerID, value);
    }

    /**
     * Adds a players counter to the track
     */ 
    public void AddPieceToTrack(int playerID, PlayerPiece pp_, ProgressTrackRef ptr, int index)
    {
        offlineTracks[ParseReference(ptr)].AddPlayerPiece(playerID, pp_, index);
    }

    int ParseReference(ProgressTrackRef ptr)
    {
        switch (ptr)
        {
            case ProgressTrackRef.Shipping: //Shipping
                return 0;
            case ProgressTrackRef.Terraform: //terraform
                return 1;
        }
        return 0;
    }

}
