/*
 This class holds the attributes and qualities that an experience will have that a player can receive by interacting with characters.
 EX - Player attends Stefano's pizza workshop, that experience holds different qualties that are within this class.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Experience
{
    //series of "tags" saying the level of each experience in the resume (or possibly otherwise) the
    //player has to be checked during the interview dialogue
    //I have in mind that each experience variable goes from 0-5
    //0 being the min and 5 being the max that the game will check for
    //-BB

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


    /*public Experience(int personality, int teamwork, int independence, int empathy, int problemSolving,
        int timeManagement, int communication)
    {
        this.personality = personality;
        this.teamwork = teamwork;
        this.independence = independence;
        this.empathy = empathy;
        this.problemSolving = problemSolving;
        this.timeManagement = timeManagement;
        this.communication = communication;
    }*/

    public Experience()
    {
        name = "";

        //Behavioral Experience
        personality = 0;
        teamwork = 0;
        independence = 0;
        leadership = 0;
        empathy = 0;

        //Situational Experience
        problemSolving = 0;
        timeManagement = 0;
        communication = 0;

        //Skill Experience
        food = 0;
        art = 0;
        science = 0;
        technology = 0;

        

    }




    


}
