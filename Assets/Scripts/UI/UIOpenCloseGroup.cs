using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOpenCloseGroup : MonoBehaviour
{

    public OpenCloseOnClick[] groupMembers;

    public void CloseAll()
    {
        foreach (OpenCloseOnClick member in groupMembers)
        {
            member.Close();
        }
    }
}
