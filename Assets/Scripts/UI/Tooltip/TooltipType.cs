using UnityEngine;
using System.Collections;

[CreateAssetMenu]
public class TooltipType : ScriptableObject
{
    [SerializeField] private string title;
    [SerializeField] private Color titleColour;

    public string TypeTitle { get { return title; } }

    public string ColouredTooltipType {
        get
        {
            string colourHex = ColorUtility.ToHtmlStringRGB(titleColour);
            return $"<color=#{colourHex}>{title}</color>";
        }
    }
    public string ColourHTMLTag
    {
        get
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGB(titleColour) + ">";
        }
    }

    public Color TitleColour { get { return titleColour; } }
    
}
