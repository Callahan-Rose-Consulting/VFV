using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddExperience : MonoBehaviour
{
    //Author: Austin Greear
    //Purpose: The purpose of this script is to add an experience to the player's resume.

    public enum categoryType {Skill, Work, Education}
    [SerializeField]
    private categoryType category = categoryType.Skill;

    public new string name;

    //Behavioral Experience
    public int personality;
    public int teamwork;
    public int independence;
    public int leadership;
    public int empathy;

    //Situational Experience
    public int problemSolving;
    public int timeManagement;
    public int communication;

    //Skill Experience
    public int food;
    public int art;
    public int science;
    public int technology;

    public Experience experience;

    public categoryType Category { get => category; set => category = value; }

    // Start is called before the first frame update
    void Start()
    {
        experience.name = name;
        
        experience.personality = personality;
        
        experience.teamwork = teamwork;

        experience.independence = independence;

        experience.leadership = leadership;

        experience.empathy = empathy;

        experience.problemSolving = problemSolving;

        experience.timeManagement = timeManagement;

        experience.communication = communication;

        experience.food = food;

        experience.art = art;

        experience.science = science;

        experience.technology = technology;
    }

    public void add_exp() 
    {
        switch (Category) 
        {
            case (categoryType.Skill):
                final_add_exp(Resume.ResumeExperiences1);
                break;

            case (categoryType.Work):
                final_add_exp(Resume.ResumeExperiences2);
                break;

            case (categoryType.Education):
                final_add_exp(Resume.ResumeExperiences3);
                break;

            default:
                final_add_exp(Resume.ResumeExperiences1);
                break;
        }
    }

    public void final_add_exp(List<Experience> experience_list) 
    {
        if (experience_list.Count > 0)
        {
            if (experience_list[0].name == "N/A")
            {
                experience_list[0] = experience;
            }
            else 
            {
                experience_list.Add(experience);
            }
        }
        else 
        {
            experience_list.Add(experience);
        }
    }

    public void add_from_event() 
    {
        add_exp();

        if (Selection_Menu.instance != null)
        {
            Debug.Log("Added and Updated");
            Selection_Menu.instance.populate_content_folders();
        }
    }
}
