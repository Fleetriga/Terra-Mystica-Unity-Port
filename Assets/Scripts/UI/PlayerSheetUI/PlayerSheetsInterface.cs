using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSheetsInterface : MonoBehaviour
{
    public Transform characterSheetContainerTransform;
    public GameObject characterSheetPrefab;
    PlayerSheetUI[] sheetsGroup;

    // Start is called before the first frame update
    void Start()
    {
        CreateSheets();
    }

    void CreateSheets()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player_Networked_Object");
        sheetsGroup = new PlayerSheetUI[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            GameObject temp = Instantiate(characterSheetPrefab);
            sheetsGroup[i] = temp.GetComponent<PlayerSheetUI>();

            //Move sheet around and into place
            temp.transform.parent = characterSheetContainerTransform;
            temp.GetComponent<RectTransform>().localPosition = new Vector2(20, (temp.GetComponent<RectTransform>().sizeDelta.y * i) + 20);
        }
    }

}
