using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour {

    public GameObject unused_abilities;
    public GameObject personal_abilities;
    public GameObject world_abilities;

    public AbilityBackground back;

    private void Awake()
    {
        //Set up the background first
        back.SetUp(personal_abilities, world_abilities);
    }

    public void AddAbility<T>() where T: UseableAbility
    {
        T ua = unused_abilities.GetComponentInChildren<T>(); //Gets the ability we're looking for and saves it for us
        ua.transform.SetParent(personal_abilities.transform); //Set parent

        //Recalculate background and then position the boys
        back.PositionAbilities(personal_abilities, world_abilities);
    }

    public void RemoveAbility<T>() where T : UseableAbility
    {
        T ua = personal_abilities.GetComponentInChildren<T>();
        //Remove from list and set parent back to "unused abilities" in case it's picked up again later.
        ua.transform.SetParent(unused_abilities.transform);
        //Reposition the ability so that it's out of the way 
        ua.GetComponent<RectTransform>().anchoredPosition = new Vector2();

        //Reformat abilities bar as something has been removed
        back.PositionAbilities(personal_abilities, world_abilities);
    }

    private void Update()
    {
        //Adding an ability test
        if (Input.GetKeyDown(KeyCode.Y))
        {
            AddAbility<Ability_AddToCult>();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            RemoveAbility<Ability_AddToCult>();
        }
    }


}
