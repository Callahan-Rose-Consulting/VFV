using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class Experience_Item : MonoBehaviour
{
    //Author: Austin Greear
    //Purpose: This script loads and stores information from the Scroll_View_Utility script

    public TextMeshProUGUI text_box;

    public Button button;

    public Experience experience;

    public Scroll_View_Utility owner;

    //Pre: None
    //Post: assigns the button variable and adds the click event to the button's unity event
    public void Start() 
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(click_event);
    }

    //Pre: None
    //Post: Finds the instanced Experience_Reactions script and calls load dialogue from it
    public void click_event() 
    {
        if (experience.name != "N/A") 
        {
            if (Selection_Menu.instance != null && Selection_Menu.instance.stored_question_item != null) 
            {
                Selection_Menu.instance.stored_question_item.click_event();

                Selection_Menu.instance.anim.SetTrigger("Close");
            }
        }
    }

    public void set_text(string str) 
    {
        text_box.text = str;
    }
}
