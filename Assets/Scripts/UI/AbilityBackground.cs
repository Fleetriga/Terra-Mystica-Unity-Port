using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBackground : MonoBehaviour {

    public GameObject background_tile;
    public GameObject end_tile;
    public GameObject divider;

    //World and Personal background panels
    public GameObject personal_background_panel;
    public GameObject world_background_panel;

    public GameObject personal_panel;
    public GameObject world_panel;

    GameObject[] personal_backgrounds;
    GameObject[] world_backgrounds;

    int length_of_button;
    int length_of_divide;
    int length_of_ends;

    public void SetUp(GameObject personal_ability_panel_, GameObject world_ability_panel_)
    {
        //world_panel.transform.SetParent(transform);
        //personal_panel.transform.SetParent(transform);
        world_background_panel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        length_of_divide = 20;
        length_of_button = 100;
        length_of_ends = 20;

        SetUpWorldMagic(world_ability_panel_.transform.childCount);
        PositionAbilities(personal_ability_panel_, world_ability_panel_);
    }

    public void PositionAbilities(GameObject personal_ability_panel_, GameObject world_ability_panel_)
    {
        //First calculate background
        Refactor_Personal_Panel(personal_ability_panel_.transform.childCount);
        GlueTogether(personal_ability_panel_.transform.childCount);

        //Then stick the boys into the right places
        for (int i = 0; i < personal_ability_panel_.transform.childCount; i++)
        {
            personal_ability_panel_.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = personal_backgrounds[i].GetComponent<RectTransform>().anchoredPosition;
        }

        for (int i = 0; i < world_ability_panel_.transform.childCount; i++)
        {
            world_ability_panel_.transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition = world_backgrounds[i].GetComponent<RectTransform>().anchoredPosition;
        }
    }

    void GlueTogether(int no_personal)
    {
        world_panel.GetComponent<RectTransform>().anchoredPosition = new Vector2((no_personal * length_of_button) + length_of_ends, 0);
    }

    void SetUpWorldMagic(int worldLength)
    {
        world_backgrounds = new GameObject[worldLength];
        int current = 0;

        GameObject temp;

        //Divide
        temp = Instantiate(divider, world_background_panel.transform);
        temp.GetComponent<RectTransform>().anchoredPosition = new Vector2();

        //World Magic
        for (int i = 0; i < worldLength; i++) //Will be for each
        {
            temp = Instantiate(background_tile, world_background_panel.transform); //Instantiate to background panel
            temp.GetComponent<RectTransform>().localPosition = new Vector2((length_of_button * current) + length_of_divide, 0);
            world_backgrounds[current] = temp;
            current++;
        }

        //End tile
        temp = Instantiate(end_tile, world_background_panel.transform);
        temp.GetComponent<RectTransform>().localPosition = new Vector2((length_of_button * current) + length_of_divide, 0);

    }

    void Refactor_Personal_Panel(int no_personal)
    {
        //delete all current backgrounds, leaving out the buttons
        foreach (Image i in personal_background_panel.GetComponentsInChildren<Image>())
        {
            if (i.GetComponent<UseableAbility>() == null)
            {
                Destroy(i.gameObject);
            }
        }

        int current = 0;

        personal_backgrounds = new GameObject[no_personal];

        //Start Tile
        GameObject temp = Instantiate(end_tile, personal_background_panel.transform);
        temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

        //Personal Abilities
        for (int i = 0; i < no_personal; i++) //Will be for each
        {
            temp = Instantiate(background_tile, personal_background_panel.transform); //Instantiate to personal abilities background panel
            temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(length_of_button * current + length_of_ends, 0);
            personal_backgrounds[current] = temp;
            current++;
        }
    }

}