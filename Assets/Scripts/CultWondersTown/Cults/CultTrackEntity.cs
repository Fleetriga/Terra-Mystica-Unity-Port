using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultTrackEntity : PlayerPiece //Inherits to save effort making a new BinUI 
{
    public PlayerStatistics LinkedPlayer { get; set; }
    public MultiProgressTrack LinkedTrack { get; set; }
    public int TrackID { get; set; }

    int trackProgress;

    private void Awake()
    {
        trackProgress = 0;
    }

    private void Update()
    {
        if (LinkedPlayer.CultStanding[TrackID] != trackProgress)
        {
            trackProgress = LinkedPlayer.CultStanding[TrackID];
            LinkedTrack.AddProgressToTrack(this, trackProgress);
        }
    }
}
