using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkedRandomSeed : NetworkBehaviour
{
    [SyncVar]
    public int randomSeed;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);
        if (hasAuthority) //Only the server host will ahve this. Therefore will only be fired once
        {
            randomSeed = Random.Range(0, 64);
        }
    }
}
