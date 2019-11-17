using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Send_To_DropDownMenu : MonoBehaviour
{
    public void SendToMenu(int new_faction)
    {
        transform.parent.parent.GetComponent<DropDownMenu>().OnChangeText(new_faction);
    }
}
