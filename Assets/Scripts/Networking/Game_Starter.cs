using UnityEngine;
using Mirror;

public class Game_Starter : MonoBehaviour
{
    public void StartGame()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("NetworkedPlayerObjects");

        foreach (GameObject go in players) //If any players arent ready ignore the call.
        {
            if (!go.GetComponent<PlayerNetworked>().IsReady)
            {
                return;
            }
        }

        GameObject.Find("HostPlayer").GetComponent<PlayerNetworked>().StartGamePreparations();
    }
}
