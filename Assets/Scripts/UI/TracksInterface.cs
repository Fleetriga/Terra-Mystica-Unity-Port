using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksInterface : MonoBehaviour {

    public ProgressTrack[] networked_tracks;
    public ProgressTrack[] offline_tracks;

    //UI references
    UI_Updater ui;
    Networked_UI_Updater nui;

    public enum ProgressTrackRef { Cult_fire, Cult_water, Points, Cult_earth, Cult_air, Shipping, Terraform };


    public void SetUp()
    {
        foreach (ProgressTrack pt in networked_tracks)
        {
            pt.SetUp();
        }
        foreach (ProgressTrack pt in offline_tracks)
        {
            pt.SetUp();
        }
    }

    /** Moves a players counter within the track
     */
    public void Progress_Track(ProgressTrackRef ptr, int value, int playerID)
    {
        if ((int)ptr < 5) //If its networked
        {
            networked_tracks[ParseReference(ptr)].Progress_To_Track(playerID, value);
        }
        else //Else use non networked
        {
            offline_tracks[ParseReference(ptr)].Progress_To_Track(playerID, value);
        }
    }

    /**
     * Adds a players counter to the track
     */ 
    public void AddPieceToTrack(int playerID, PlayerPiece pp_, ProgressTrackRef ptr, int index)
    {
        if ((int)ptr < 5) //If its networked
        {
            networked_tracks[ParseReference(ptr)].AddPlayerPiece(playerID, pp_, index);
        }
        else //Else use non networked
        {
            offline_tracks[ParseReference(ptr)].AddPlayerPiece(playerID, pp_, index);
        }
    }

    int ParseReference(ProgressTrackRef ptr)
    {
        switch (ptr)
        {
            case ProgressTrackRef.Cult_fire: // Cult_fire
                return 0;
            case ProgressTrackRef.Cult_water: //Water
                return 1;
            case ProgressTrackRef.Cult_earth: //Earth
                return 2;
            case ProgressTrackRef.Cult_air: //Air
                return 3;
            case ProgressTrackRef.Shipping: //Shipping
                return 0;
            case ProgressTrackRef.Terraform: //terraform
                return 1;
            case ProgressTrackRef.Points: //points
                return 4;
        }
        return 0;
    }

}
