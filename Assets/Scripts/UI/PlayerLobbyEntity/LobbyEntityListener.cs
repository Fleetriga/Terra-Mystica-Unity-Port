using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyEntityListener : MonoBehaviour
{
    private Faction.FactionType currentFaction = Faction.FactionType.NOFACTION;
    private bool isReady = false;
    private PlayerNetworked correspondingPlayer;

    //Elements to change.
    Image lobbyEntityImage;
    GameObject lobbyEntityAbilities;

    public PlayerNetworked CorrespondingPlayer {get { return correspondingPlayer; } set { correspondingPlayer = value; } }

    void Awake()
    {
        lobbyEntityImage = transform.GetChild(1).GetComponent<Image>();
        lobbyEntityAbilities = transform.GetChild(2).gameObject;
    }

    public void SetCorrespondingPlayer(PlayerNetworked playerNetworked)
    {
        correspondingPlayer = playerNetworked;
        currentFaction = correspondingPlayer.PlayerFaction;
        UpdateLobbyEntity();
    }

    void Update()
    {
        //Check for changes.
        if (correspondingPlayer.PlayerFaction != currentFaction)
        {
            currentFaction = correspondingPlayer.PlayerFaction;
            UpdateLobbyEntity();
        }

        if (correspondingPlayer.IsReady != isReady)
        {
            isReady = correspondingPlayer.IsReady;
            UpdateReadyAesthetic();
        }
    }

    private void UpdateReadyAesthetic()
    {
        //Dont bother for host
        if (correspondingPlayer.IsHost) { return; }
        transform.GetChild(4).gameObject.SetActive(correspondingPlayer.IsReady);
    }

    void UpdateLobbyEntity()
    {
        //Now change players LobbyEntity to reflect the picked faction
        lobbyEntityImage.sprite = GetComponent<FactionImageHolder>().FactionImages[(int)currentFaction];
         //Not implemented yet, no special ability prefabs ahve been made.
    }
}
