using UnityEngine;
using Mirror;

public class PlayerStatus : NetworkBehaviour
{
    public enum PlayerStatusEnum {Standby, TakingTurn, EndingTurn, Retiring};
    [SyncVar]
    public PlayerStatusEnum CurrentPlayerStatus;


    TurnController turnC;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority || turnC == null) { return; }

        if (turnC.CurrentPlayerID == GetComponent<PlayerNetworked>().PlayerID && CurrentPlayerStatus == PlayerStatusEnum.Standby)
        {
            Cmd_SetPlayerStatus(PlayerStatusEnum.TakingTurn);
        }
        if (turnC.CurrentPlayerID != GetComponent<PlayerNetworked>().PlayerID)
        {
            Cmd_SetPlayerStatus(PlayerStatusEnum.Standby);
        }
        if (turnC.PlayersLeftThisPhase == 1 && CurrentPlayerStatus == PlayerStatusEnum.EndingTurn && turnC.CurrentPlayerID == GetComponent<PlayerNetworked>().PlayerID) 
            //Single player should always be the one taking his turn. However if he's trying to retire bloody let him.
        {
            Cmd_SetPlayerStatus(PlayerStatusEnum.TakingTurn);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Cmd_SetPlayerStatus(PlayerStatusEnum.EndingTurn);
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

    public void EnableStatusCheck()
    {
        turnC = GameObject.Find("TurnController(Clone)").GetComponent<TurnController>();
    }
}
