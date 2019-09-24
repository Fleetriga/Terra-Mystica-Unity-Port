using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerStatus : NetworkBehaviour
{
    public enum PlayerStatusEnum {Standby, TakingTurn, EndingTurn, Retiring};
    [SyncVar]
    public PlayerStatusEnum CurrentPlayerStatus;


    PlayerNetworked playerHost;

    void Start()
    {
        playerHost = GameObject.Find("HostPlayer").GetComponent<PlayerNetworked>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority) { return; }

        if (playerHost.CurrentPlayer == GetComponent<PlayerNetworked>().Player_ID && CurrentPlayerStatus == PlayerStatusEnum.Standby)
        {
            Cmd_SetPlayerStatus(PlayerStatusEnum.TakingTurn);
        }
        if (playerHost.CurrentPlayer != GetComponent<PlayerNetworked>().Player_ID)
        {
            Cmd_SetPlayerStatus(PlayerStatusEnum.Standby);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Cmd_SetPlayerStatus(PlayerStatusEnum.EndingTurn);
        }
        if (NetworkServer.connections.Count == 1) //If theres only 1 player allow them to always take turns
        {
            Cmd_SetPlayerStatus(PlayerStatusEnum.TakingTurn);
        }
    }

    public void SetPlayerStatus(PlayerStatusEnum status)
    {
        Cmd_SetPlayerStatus(status);
    }

    [Command]
    public void Cmd_SetPlayerStatus(PlayerStatusEnum status)
    {
        CurrentPlayerStatus = status;
    }
}
