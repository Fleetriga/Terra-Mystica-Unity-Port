using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class NetworkAddressSetter : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] NetworkManager manager;
    [SerializeField] GameObject[] connectScreenAndLobbyScreen;

    public void SetNetworkAddress()
    {
        //Set address
        manager.networkAddress = inputField.text.ToString();

        //connect as client
        manager.StartClient();

        connectScreenAndLobbyScreen[0].SetActive(false);
        connectScreenAndLobbyScreen[1].SetActive(true);
    }
}
