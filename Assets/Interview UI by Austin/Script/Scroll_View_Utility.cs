using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll_View_Utility : MonoBehaviour
{
    //Author: Austin Greear
    //Purpose: This script loads and stores information from the Selection Menu script
    //The populate_content function is used by the Selection Menu script to load information on it.

    public GameObject experience_item;

    public Button button;

    public List<Experience_Item> experience_items = new List<Experience_Item>();

    public int max_amount = 5;

    public LayoutElement layout_element;

    public float current_scale_percent = 0.0f;

    public float max_scale = 100.0f;

    public float item_height = 25;

    public Animator anim;

    public bool open = false;

    //Pre: None
    //Post: Assigns the component variables of this script initializes the slots in the transform of this script
    void Awake() 
    {
        layout_element = GetComponent<LayoutElement>();

        anim = GetComponent<Animator>();

        for (int i = 0; i < max_amount; i++)
        {
            GameObject item = Instantiate(experience_item, transform);

            Experience_Item ei = item.GetComponent<Experience_Item>();

            ei.owner = this;

            experience_items.Add(ei);

            item.SetActive(false);
        }
    }

    public int current_exp_count = 0;

    //Pre: Takes a list of experiences to populate the transform with
    //Post: Populates the UI with the given experiences
    public void populate_content(List<Experience> experiences) 
    {
        max_scale = 0;

        current_exp_count = experiences.Count;

        for (int i = 0; i < experience_items.Count; i++)
        {
            if (i < experiences.Count)
            {
                experience_items[i].set_text("-" + experiences[i].name);

                experience_items[i].experience = experiences[i];

                max_scale += item_height;
            }
        }
    }

    //Pre: None
    //Post: Changes the preferred height of the layout element to match the maxScale * current_scale_percentage. (The current scale percentage is simply used for animator purposes, the maxScale is used to keep track of what should be the maxScale of the given items)
    void FixedUpdate() 
    {
        if (open) 
        {
            Debug.Log("Desired: " + current_scale_percent * max_scale);
        }

        layout_element.preferredHeight = current_scale_percent * max_scale;
    }

    //Pre: None
    //Post: toggles the animator off if the script variable is true. Then deactivates all of the buttons in the experience items list.
    public void trigger_end() 
    {
        if (open) 
        {
            anim.SetTrigger("Toggle");
        }

        for (int i = 0; i < current_exp_count; i++) 
        {
            experience_items[i].gameObject.SetActive(false);
        }
    }

    //Pre: None
    //Post: Sets all of the experience item's buttons to interactable
    public void trigger_start()
    {
        for (int i = 0; i < current_exp_count; i++)
        {
            experience_items[i].gameObject.SetActive(true);
        }
    }
}
