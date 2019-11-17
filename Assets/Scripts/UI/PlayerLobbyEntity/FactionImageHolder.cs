using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactionImageHolder : MonoBehaviour
{
    [SerializeField] private Sprite[] factionImages;

    public Sprite[] FactionImages { get { return factionImages; } }
}
