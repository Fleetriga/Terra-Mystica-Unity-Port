using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.EventSystems;
using System;

public class UIElementTooltip : TooltipUI, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string subtitle;
    [TextArea(3, 10)] [SerializeField] protected string tooltipDescription;

    public string Subtitle { get { return subtitle; } }
    public string TooltipText { get { return tooltipDescription; } }


    public string ColouredSubtitle
    { get
        {
            string colourHex = ColorUtility.ToHtmlStringRGB(TooltipType.TitleColour);
            return $"<color=#{colourHex}>{Subtitle}</color>";
        }
    }
    public override string BuildToolTipText()
    {
        try
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<size=36>").Append(TooltipType.ColouredTooltipType).Append("</size>").AppendLine();
            builder.Append("<size=30>").Append(ColouredSubtitle).Append("</size>").AppendLine();
            builder.Append(tooltipDescription);

            return builder.ToString();
        }
        catch (Exception e)
        {
            Debug.Log(name);
            return "";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        popUp.DisplayInfo(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        popUp.HideInfo();
    }
}
