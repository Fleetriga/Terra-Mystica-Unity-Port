using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour {

    Building.Building_Type to_be_built;

    void Awake()
    {
        to_be_built = Building.Building_Type.NOTHING;
    }

    public bool GetBuildOk()
    {
        return (int)to_be_built != 5;
    }

    public void ResetFlag()
    {
        to_be_built = Building.ParseInt(5);
    }
}
