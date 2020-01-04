using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBorder : MonoBehaviour
{
    public GameObject playerPointEntityPrefab;
    GameObject[] playerPointEntities;

    private void Awake()
    {
        playerPointEntities = new GameObject[GameObject.FindGameObjectsWithTag("NetworkedPlayerObjects").Length];
    }

    public void AddPlayerPointEntity(int playerID, int points, Material factionMaterial)
    {
        GameObject go = Instantiate(playerPointEntityPrefab);
        go.GetComponent<Renderer>().material = factionMaterial;
        playerPointEntities[playerID] = go;

        MovePlayerPointEntity(playerID, points);
    }

    public void MovePlayerPointEntity(int playerID, int points)
    {
        playerPointEntities[playerID].transform.position = new Vector3(transform.GetChild(points-1).position.x, transform.GetChild(points - 1).position.y + 4, transform.GetChild(points - 1).position.z);
    }
}
