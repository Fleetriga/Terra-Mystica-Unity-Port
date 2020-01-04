using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLobbyManager : MonoBehaviour
{
    //Local player variables
    Faction.FactionType chosenFaction;
    PlayerNetworked localPlayer;
    GameObject localPlayerLobbyEntity;

    [SerializeField]
    private Transform factionDescriptionsObject;
    GameObject currentDisplayedDescription;

    [SerializeField] private GameObject PlayerLobbyEntityPrefab;

    public Faction.FactionType ChosenFaction { get; }

    //Method for local player to choose their faction.
    public void SetChosenFaction(int faction)
    {
        chosenFaction = (Faction.FactionType)faction;
        localPlayer.SetFaction((Faction.FactionType)faction);

        //Now change UI for player to display chosen faction information
        if (currentDisplayedDescription != null) { currentDisplayedDescription.SetActive(false); }
        currentDisplayedDescription = factionDescriptionsObject.GetChild(faction).gameObject;
        currentDisplayedDescription.SetActive(true);
    }

    public void SetPlayerReadyUnready()
    {
        localPlayer.Cmd_SetPlayerReadyUnready();
    }

    public void PlayerConnected(PlayerNetworked connectedPlayer, bool local, bool host, string playername)
    {
        //spawn a lobby entity corresponding to new player
        GameObject go = Instantiate(PlayerLobbyEntityPrefab, transform.GetChild(0).GetChild(1));
        //Set entities PlayerListener to listen to the player who just joined.
        go.GetComponent<LobbyEntityListener>().SetCorrespondingPlayer(connectedPlayer, playername);

        //If it's the local player it needs to be connected to PlayerNetworked to tell them of faction changes.
        if (local)
        {
            localPlayer = connectedPlayer;

            if (host)
            {
                transform.GetChild(0).GetChild(2).GetChild(1).gameObject.SetActive(true); //Give host the "StartGame button"
                localPlayer.Cmd_SetPlayerReadyUnready(); //Host starts ready.
            }
            else
            {
                transform.GetChild(0).GetChild(2).GetChild(0).gameObject.SetActive(true); //Give clients the "Ready Up button"
            }

        }
    }
}
