using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class GoToGame : NetworkBehaviour
{

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    [Command]
    public void Cmd_LoadGame()
    {
        
    }

    [ClientRpc]
    void Rpc_LoadGame()
    {
        SceneManager.LoadScene(1);
    }
}
