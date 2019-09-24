using UnityEngine;
using UnityEngine.UI;

public class Remaining_Building_UI_Interface : MonoBehaviour {
    public GameObject[] remaining_buildings;

	public void Display_Remaining(Building.Building_Type bt_, int building_index_)
    {
        remaining_buildings[(int)bt_].transform.GetChild(building_index_).gameObject.SetActive(
                                                                                                #pragma warning disable CS0618 // Type or member is obsolete
                                                                                                !remaining_buildings[(int)bt_].transform.GetChild(building_index_).gameObject.active //Set to the opposite of what it is now
                                                                                                #pragma warning restore CS0618 // Type or member is obsolete
                                                                                               );
    }
}
