using UnityEngine;
using Mirror;

public class Game_Starter : MonoBehaviour
{
    public void StartGame()
    {
        GameObject.Find("HostPlayer").GetComponent<PlayerNetworked>().StartGamePreparations();
    }
}
