using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TooltipUI : MonoBehaviour
{
    [SerializeField] private TooltipType tooltipType;
    [SerializeField] protected TooltipPopUp popUp;

    public TooltipType TooltipType { get { return tooltipType; } }

    public abstract string BuildToolTipText();
}
