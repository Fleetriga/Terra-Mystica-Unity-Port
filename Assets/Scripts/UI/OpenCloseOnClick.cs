using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseOnClick : MonoBehaviour
{
    bool open;
    public UIOpenCloseGroup groupManager;

    public void OnClick()
    {
        if (open)
        {
            open = false;
        }
        else
        {
            groupManager.CloseAll();
            open = true;
        }
    }

    void Update()
    {
        Vector3 scale = transform.GetChild(0).localScale;
        scale.y = Mathf.Lerp(open ? 1 : 0, open ? 1 : 0, Time.deltaTime);
        transform.GetChild(0).localScale = scale;
    }

    public void Close()
    {
        open = false;
    }

    public void Open()
    {
        open = true;
    }
}
