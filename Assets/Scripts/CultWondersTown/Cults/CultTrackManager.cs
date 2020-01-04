using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CultTrackManager : MonoBehaviour
{
    [SerializeField] MultiProgressTrack[] tracks;
    [SerializeField] GameObject CultTrackEntity;
    [SerializeField] MineShaftManager[] mineShafts;

    void Awake()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("NetworkedPlayerObjects");
        foreach (MultiProgressTrack track in tracks)
        {
            track.SetUp(go.Length); //Get NoPlayers
            foreach (GameObject g in go) 
            {
                GameObject newEntity = Instantiate(CultTrackEntity); //Instantiate an entity for the given player for the given track
                newEntity.GetComponent<CultTrackEntity>().LinkedPlayer = g.GetComponent<PlayerStatistics>(); //Link to a player
                newEntity.GetComponent<CultTrackEntity>().LinkedTrack = track; //Link to a track
                //Change colour

                //Change entity according to faction colour
                track.AddPlayerPiece(newEntity.GetComponent<CultTrackEntity>());
            }
        }   
    }

    public void TakeMine(int track)
    {
        mineShafts[track].TakeMine();
    }
}
