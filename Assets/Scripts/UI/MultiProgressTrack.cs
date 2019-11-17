using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiProgressTrack : MonoBehaviour
{
    [SerializeField]protected BinUI[] entityBins; //1-10
    protected CultTrackEntity[] playerEntities; //Same as the amount of players in the game
    [SerializeField] int trackID;

    public void SetUp(int noPlayers)
    {
        playerEntities = new CultTrackEntity[noPlayers];
        foreach (BinUI bin in entityBins)
        {
            bin.SetUp(5);
        }
    }

    public void AddPlayerPiece(CultTrackEntity entity)
    {
        playerEntities[entity.LinkedPlayer.GetComponent<PlayerNetworked>().PlayerID] = entity;
        entity.CurrentBin = entityBins[0]; //Link entity to it's bin and vice versa
        entity.CurrentBin.Add(entity);
        entity.TrackID = trackID;
        entity.CurrentBin.RefactorPeices();
    }

    public void AddProgressToTrack(CultTrackEntity entity, int newTrackPosition)
    {
        //Get the right track
        entity.CurrentBin.Remove(entity);
        entityBins[newTrackPosition].Add(entity);
        entity.CurrentBin.RefactorPeices();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
