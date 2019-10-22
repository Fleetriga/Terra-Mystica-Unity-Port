using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    static readonly float CAMERASPEED = 0.05f;

    float translateX;
    float translateZ;

    bool translate;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            translateX = -1f * CAMERASPEED;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            translateX = 1f * CAMERASPEED;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            translateZ = 1f * CAMERASPEED;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            translateZ = -1f * CAMERASPEED;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            translateZ = 0;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
                translateX = 0;
        }

        if (translateX != 0 || translateZ != 0) //If theres a need to translate. IE one of the translate coefficients is not set to 0 (not move)
        {
            transform.Translate(new Vector3(translateX, 0, translateZ));
        }

    }
}
