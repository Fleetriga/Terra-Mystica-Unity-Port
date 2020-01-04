using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionAbilityHolder : MonoBehaviour
{
    [SerializeField] private GameObject[] abilityPrefabs;

    public GameObject[] AbilityPrefabs { get { return abilityPrefabs; } }
}
