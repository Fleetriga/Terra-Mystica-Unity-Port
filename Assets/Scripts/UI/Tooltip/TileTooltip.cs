using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileTooltip : TooltipUI
{
    Tile tile;
    bool hoveredOver;
    float hoveredOverTime;

    private void Awake()
    {
        tile = GetComponent<Tile>();
    }

    public override string BuildToolTipText()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("<size=36>").Append(TooltipType.ColourHTMLTag).Append(tile.terrain.ToString() + " ").Append(TooltipType.TypeTitle).Append("</color></size>").AppendLine();
        builder.Append("Building: ").Append(tile.TileBuilding.ToString()).AppendLine();
        if (tile.OwnersPlayerID != 64) //if the tile has an owner apend that too.
        {
            builder.Append("Owner: Player ").Append(tile.OwnersPlayerID).AppendLine();
            builder.Append("Town Progress: ").Append(tile.GetTownGroup().GetProgress()).AppendLine();
        }
        else
        {
            builder.Append("Owner: Uncolonised ").AppendLine();
            builder.Append("Town Progress: 0").AppendLine();
        }
        return builder.ToString();
    }

    private void Update()
    {
        if (hoveredOver)
        {
            hoveredOverTime += 1 * Time.deltaTime;
        }
        if (hoveredOverTime >= 0.5 && hoveredOver)
        {
            popUp.DisplayInfo(this);
            hoveredOver = false; //Stops tooltip being spammed open
        }
        
    }

    private void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            hoveredOver = true;
        }
    }

    private void OnMouseExit()
    {
        hoveredOver = false;
        popUp.HideInfo();
    }


}
