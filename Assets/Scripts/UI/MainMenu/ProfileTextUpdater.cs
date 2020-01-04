using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProfileTextUpdater : MonoBehaviour
{
    [SerializeField] ProfileManager profileManager;
    [SerializeField] TextMeshProUGUI profileName;

    private void OnEnable()
    {
        if (profileName == null) { return;  }
        UpdateText();
    }

    public void UpdateText()
    {
        profileName.text = profileManager.PlayerProfile.ProfileName;
    }
}
