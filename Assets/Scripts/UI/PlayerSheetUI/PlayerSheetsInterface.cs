using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSheetsInterface : MonoBehaviour
{
    public Transform characterSheetContainerTransform;
    public GameObject characterSheetPrefab;
    GameObject[] players;
    PlayerSheetUI[] sheetsGroup;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        players = GameObject.FindGameObjectsWithTag("Player_Networked_Object");
        CreateSheets();
    }


    void CreateSheets()
    {
        sheetsGroup = new PlayerSheetUI[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            GameObject temp = Instantiate(characterSheetPrefab);
            sheetsGroup[i] = temp.GetComponent<PlayerSheetUI>();

            //Move sheet around and into place
            temp.transform.parent = characterSheetContainerTransform;
            temp.GetComponent<RectTransform>().localPosition = new Vector2(0, (temp.GetComponent<RectTransform>().sizeDelta.y + 30) * -i);
            SetUp(i);
        }

        //Update when updating size of character sheets
        //characterSheetContainerTransform.GetComponent<RectTransform>().localPosition = new Vector2(-40, (100+30)*(players.Length-1));
    }

    public void SetUp(int playerID)
    {
        if (sheetsGroup == null) { return; }
        sheetsGroup[playerID].RegularUpdate(players[playerID].GetComponent<PlayerStatistics>());
        sheetsGroup[playerID].TierMagicUpdate(players[playerID].GetComponent<PlayerStatistics>());
        sheetsGroup[playerID].PointsUpdate(players[playerID].GetComponent<PlayerStatistics>());
    }

    public void RegularUpdate(int playerID)
    {
        if (sheetsGroup == null) { return; }
        sheetsGroup[playerID].RegularUpdate(players[playerID].GetComponent<PlayerStatistics>());
    }

    public void MagicUpdate(int playerID)
    {
        if (sheetsGroup == null) { return; }
        sheetsGroup[playerID].TierMagicUpdate(players[playerID].GetComponent<PlayerStatistics>());
    }

    public void PointsUpdate(int playerID)
    {
        if (sheetsGroup == null) { return; }
        sheetsGroup[playerID].PointsUpdate(players[playerID].GetComponent<PlayerStatistics>());
    }

}
