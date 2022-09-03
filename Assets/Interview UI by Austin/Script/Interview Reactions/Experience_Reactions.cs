using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public class Experience_Reactions : MonoBehaviour
{
    //Author: Austin Greear
    //Purpose: This script is primarily responsible for loading the win/mid/lose reactions for the interview.
    //Originally i planned to use it for experience based questions but as development progressed we ended up straying from those types of questions

    public static Experience_Reactions instance;

    public Selection_Menu menu;

    public TalkToNPC talkToNPC;

    [System.Serializable]
    public class Experience_Response
    {
        public string experience_name;

        public string[] Reaction;

        public float meter_gain;

        public Experience_Response(string e, string[] r, float mg)
        {
            experience_name = e;

            Reaction = r;

            meter_gain = mg;
        }
    }

    [System.Serializable]
    public class Experience_Question
    {
        public string question;

        public Experience_Response[] responses;

        public Experience_Response default_response;

        public Experience_Question(string q, Experience_Response[] er, Experience_Response dr)
        {
            question = q;

            responses = er;

            default_response = dr;
        }
    }

    public Influence_Meter influence_Meter;

    public Experience_Question current_question;

    public List<Experience_Question> experience_questions = new List<Experience_Question>();


    [TextArea(5, 10)]
    public string[] win_reaction;
    [TextArea(5, 10)]
    public string[] mid_reaction;
    [TextArea(5, 10)]
    public string[] lose_reaction;



    //Pre: None
    //Post: Initializes instance and and talkToNPC variable
    void Start()
    {
        instance = this;

        talkToNPC = GetComponent<TalkToNPC>();
    }

    //Pre: the 2 experiences to compare
    //Post: true if exp1 is greater than exp2
    public bool check_experience(Experience exp1, Experience exp2) 
    {
        return (sum_items(exp1) >= sum_items(exp2));
    }

    //Pre: Experience to sum all of its attributes
    //Post: an integer equal to the sum of all the experiences attributes
    public int sum_items(Experience exp) 
    {
        return exp.personality + exp.teamwork + exp.independence + exp.leadership + exp.empathy + exp.problemSolving + exp.timeManagement + exp.communication + exp.food + exp.art + exp.science + exp.technology;
    }



    //Pre: Experience to load dialogue from
    //Post: Loads the proper reaction dialogue from the given experience
    public void load_dialogue(int index) 
    {
        //Ensures that the talkToNPC message is done and that the variable active is true before changing the dialogue.
        if (talkToNPC.messageDone && index >= 0 && index < experience_questions.Count) 
        {
            talkToNPC.messages[talkToNPC.messageCount] = experience_questions[index].question;

            current_question = experience_questions[index];

            talkToNPC.enabled = false;

            menu.anim.SetTrigger("Toggle");
        }
    }

    //Pre: the name of the experience to check for
    //Post: Loads the proper reaction dialogue from the given experience name loads the default response if it isn't found
    public void load_response(string name) 
    {
        menu.anim.SetTrigger("Toggle");

        for (int i = 0; i < current_question.responses.Length; i++) 
        {
            Experience_Response er = current_question.responses[i];

            if (er.experience_name == name) 
            {
                influence_Meter.increment_meter(er.meter_gain);

                talkToNPC.enabled = true;

                talkToNPC.insert_messages(er.Reaction);

                talkToNPC.DecideWhichDialogueToShow();

                return;
            }
        }

        influence_Meter.increment_meter(current_question.default_response.meter_gain);

        talkToNPC.enabled = true;

        talkToNPC.insert_messages(current_question.default_response.Reaction);

        talkToNPC.DecideWhichDialogueToShow();

        return;
    }

    public float testo = 0.0f;

    //Pre: None
    //Post: Adds the proper dialogue to the end of the TalkToNPC dialgue.
    public void load_feed_back() 
    {
        float value = influence_Meter.meter_percentage();

        testo = value;

        if (value >= 0.7f)
        {
            talkToNPC.insert_message_at_index(win_reaction, talkToNPC.messageCount);
        }
        else if (value >= 0.4f)
        {
            talkToNPC.insert_message_at_index(mid_reaction, talkToNPC.messageCount);
        }
        else 
        {
            talkToNPC.insert_message_at_index(lose_reaction, talkToNPC.messageCount);
        }
    }
}
