using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.EventSystems;

public class DropDownMenu : NetworkBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text text;
    RectTransform container;
    public GameObject options;
    bool isOpen;

    PlayerNetworked player_owner;

    void Start()
    {
        isOpen = false;
        GameObject go = Instantiate(options, transform.GetChild(0));
        container = go.transform.GetComponent<RectTransform>();
        go.GetComponent<RectTransform>().localPosition = new Vector2(-8, -22);
    }

    public void ChangeTextOn(int faction)
    {
        Cmd_ChangeText(player_owner.Set_Faction((Faction.FactionType)faction));
    }

    [Command]
    public void Cmd_SetOwner(GameObject networked_player_object)
    {
        player_owner = networked_player_object.GetComponent<PlayerNetworked>();
        Rpc_SetOwner(networked_player_object);
    }

    [ClientRpc]
    void Rpc_SetOwner(GameObject networked_player_object)
    {
        player_owner = networked_player_object.GetComponent<PlayerNetworked>();
    }

    void OnMouseOver()
    {
        container.localScale = new Vector3(1,1,1);
    }

    private void Update()
    {
        if (!hasAuthority)
        {
            return;
        }
        Vector3 scale = container.localScale;
        scale.y = Mathf.Lerp(scale.y, isOpen ? 1:0, Time.deltaTime * 12);
        container.localScale = scale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isOpen = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOpen = false;
    }

    [Command]
    void Cmd_ChangeText(string newText)
    {
        text.text = newText;
        Rpc_ChangeText(newText);
    }

    [ClientRpc]
    void Rpc_ChangeText(string newText)
    {
        text.text = newText;
    }
}
