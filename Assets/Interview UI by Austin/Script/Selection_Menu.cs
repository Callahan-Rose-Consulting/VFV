using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selection_Menu : MonoBehaviour
{
    //Author: Austin Greear
    //Purpose: This script handles the initialization of the selection menu.
    //The selection menu is used to display the players various experiences that the earn throughout the game.

    public static Selection_Menu instance;

    public Scroll_View_Utility subfolder1;

    public Scroll_View_Utility subfolder2;

    public Scroll_View_Utility subfolder3;

    public List<Experience> subfolder1_items = new List<Experience>();

    public List<Experience> subfolder2_items = new List<Experience>();

    public List<Experience> subfolder3_items = new List<Experience>();

    public Animator anim;

    public bool open = false;

    public Interview_Question_Item stored_question_item;

    //Pre: None
    //Post: assigns the component variables of the script and initializes/populates the resume skills UI
    void Start()
    {
        instance = this;

        anim = GetComponent<Animator>();

        subfolder1_items = populate_empty_list(Resume.ResumeExperiences1);

        subfolder2_items = populate_empty_list(Resume.ResumeExperiences2);

        subfolder3_items = populate_empty_list(Resume.ResumeExperiences3);

        populate_content_folders();
    }

    public List<Experience> populate_empty_list(List<Experience> empty_list)
    {
        if (empty_list.Count == 0) 
        {
            Experience NullSkill = new Experience();

            NullSkill.name = "N/A";

            empty_list.Add(NullSkill);
        }

        return empty_list;
    }

    //Pre: None
    //Post: populates the given subfolders with static resume class experiences lists
    public void populate_content_folders()
    {
        if (Resume.ResumeExperiences1 != null)
        {
            subfolder1.populate_content(Resume.ResumeExperiences1);
        }

        if (Resume.ResumeExperiences2 != null)
        {
            subfolder2.populate_content(Resume.ResumeExperiences2);
        }

        if (Resume.ResumeExperiences3 != null)
        {
            subfolder3.populate_content(Resume.ResumeExperiences3);
        }
    }

    public GameObject experience_item;

    //Pre: None
    //Post: triggers the animator of all of the subfolder scripts
    public void trigger_end() 
    {
        if (subfolder1.open) 
        {
            subfolder1.trigger_end();
        }

        if (subfolder2.open)
        {
            subfolder2.trigger_end();
        }

        if (subfolder3.open)
        {
            subfolder3.trigger_end();
        }
    }

    public void trigger_event_system()
    {
        if (subfolder1.button != null) 
        {
            EventSystem.current.firstSelectedGameObject = subfolder1.button.gameObject;

            EventSystem.current.SetSelectedGameObject(subfolder1.button.gameObject);
        }
    }

    public void Set_x_Scale(float scale) 
    {
        this.gameObject.transform.localScale = new Vector3(scale, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
    }
}
