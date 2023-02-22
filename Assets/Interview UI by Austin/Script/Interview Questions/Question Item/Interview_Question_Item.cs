using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Interview_Question_Item : MonoBehaviour
{
    //Author: Austin Greear
    //Purpose: This script is primarily responsible for loading and storing question data.
    //i.e the reaction and the meter gain from each answer

    public Interview_Questions owner;

    public Animator anim;

    public TextMeshProUGUI text_box;

    public string[] Reaction;

    public string[] properties;

    public float meter_amount = 0.0f;

    public Button button;

    public void Awake() 
    {
        button = GetComponent<Button>();

        Reset_Event();
    }

    public void load_text(string str) 
    {
        text_box.text = str;
    }

    //Pre: None
    //Post: Loads the reaction and meter gain of this script by finding the instanced Interview_Questions script and calling the load_anser function on it.
    public void click_event() 
    {
        if (Reaction.Length != 0) 
        {
            Interview_Questions.instance.load_answer(Reaction, properties, meter_amount);
            
            if (Selection_Menu.instance.open) 
            {
                Selection_Menu.instance.anim.SetTrigger("Close");
            }
        }
    }

    public float value = 0;

    public void update_scroll_position() 
    {
        if (owner != null) 
        {
            owner.ScrollbarScript.value = value;
        }
    }

    public void Reset_Event() 
    {
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(click_event);
    }

    public void SetEvent(UnityAction call) 
    {
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(call);
    }
}
