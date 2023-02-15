using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;
using System;
using UnityEngine.UI;

public class Interview_Questions : MonoBehaviour
{
    //Author: Austin Greear
    //Purpose: This script is primarily responsible for loading interview questions and storing answers to said questions the player can respond with.

    [System.Serializable]
    public class Answer
    {
        [TextArea(5, 10)]
        public string entry;

        public float meter_gain;

        [TextArea(5, 10)]
        public string[] Reaction;

        public Answer(string e, string[] r, float mg)
        {
            entry = e;

            Reaction = r;

            meter_gain = mg;
        }
    }

    [System.Serializable]
    public class Question
    {
        public string question;

        public Answer[] answers;

        //public float points_earned;

        public Question(string q, Answer[] a)
        {
            question = q;

            answers = a;
        }
    }

    public Animator menu_anim;

    public List<Question> questions = new List<Question>();

    public int question_index = 0;

    public static Interview_Questions instance;

    public TalkToNPC talkToNPC;

    public GameObject content_object;

    public GameObject question_item;

    public int question_slots = 5;

    public List<Interview_Question_Item> items = new List<Interview_Question_Item>();

    public Scrollbar ScrollbarScript;

    public string company_name = "GENERIC COMPANY";

    public string job_title = "GENERIC JOB TITLE";

    //Pre: None
    //Post: Initializes instance and and talkToNPC variable, initializes the slots for the UI and such
    void Start()
    {
        instance = this;

        for (int i = 0; i < question_slots; i++) 
        {
            GameObject g_object = Instantiate(question_item, content_object.transform);

            Interview_Question_Item iqi = g_object.GetComponent<Interview_Question_Item>();

            iqi.owner = this;

            items.Add(iqi);

            g_object.SetActive(false);
        }

        string[] data = {company_name, job_title};

        TalkToNPC.UpdateInterviewResults("NEWINTERVIEW", TalkToNPC.playerFileName, data);

        /*  ON friday i will rewrite the questions to add more STAR/VALUE responses. This is just so you have an idea of what to expect per question.
         *                   *Good response*                                                 *Mid Response*
         * Question 2: Good: "ALLIGN" "UNDERSTANDING"                                     Mid: "ALLIGNMENT"
         * Question 6: Good: "VISION" "UNDERSTANDING" "ALLIGNMENT" "ENACTED"              Mid: (There is no mid)
         * Question 12:Good: "VISION" "UNDERSTANDING"                                     Mid: (There is no mid)
         * Question 4: Good: "SITUATION" "ACTION" "RESULT"                                Mid: (There is no mid)
         * Question 13:Good: "VISION"                                                     Mid: (There is no mid)
         * Question 9: Good: "SITUATION" "ACTION" "RESULT"                                Mid: (There is no mid)
         * Question 5: Good: "SITUATION" "ACTION" "RESULT"                                Mid: (There is no mid)
         * 
         * On friday i wil add more to the "Good" to make it a VERY good response which wil include all STAR/VALUE and in the "Mid" i will add about 1-2 to make it a somewhat decent response.
         * 
       */

        // Debug.Log(company_name);
        // Debug.Log(job_title);
    }

    public int current_question_index = 0;


    //Pre: an integer to access to specific index in questions list
    //Post: populates the Answers UI with the corresponding answers from the given question
    public void populate_answers(int index) 
    {
        
        if (index < questions.Count && index >= 0) 
        {
            current_question_index = index;

            for (int i = 0; i < items.Count; i++)
            {
                if (i < questions[index].answers.Length)
                {

                    //Replacing keywords
                    questions[index].answers[i].entry = questions[index].answers[i].entry.Replace("<COMPANY_NAME>", company_name);

                    questions[index].answers[i].entry = questions[index].answers[i].entry.Replace("<JOB_TITLE>", job_title);

                    //Assigns scroll view value to each experience item
                    if (i != 0)
                    {
                        float scroll_value = ((questions[index].answers.Length - 1.0f) - i) / (questions[index].answers.Length - 1);

                        items[i].value = scroll_value;
                    }
                    else
                    {
                        items[i].value = 1;
                    }

                    //Resets the click event for the answers... Useful for the inclusion of 1 answering mechanic

                    items[i].Reset_Event();

                    //Could possibly be put inside the TalkToNPC script for convenience later on...
                    Regex getMessage = new Regex(@"\*.[^_]*\*");

                    Match x = getMessage.Match(questions[index].answers[i].entry);

                    if (x.Success)
                    {
                        string x_str = x.Value.Replace("*", String.Empty);

                        string[] splitStr = x_str.Split(' ');

                        switch (splitStr[0])
                        {
                            case "highestskill":

                                int skill_index = int.Parse(splitStr[1]);

                                questions[index].answers[i].entry = questions[index].answers[i].entry.Replace(x.Value, GameManager.instance.highest_skill(skill_index).Name);

                                break;

                            case "resumeExp":
                                //Another method that simply just opens/closes the experience menu

                                questions[index].answers[i].entry = questions[index].answers[i].entry.Replace(x.Value, String.Empty);

                                items[i].SetEvent(toggle_experience_menu);

                                Selection_Menu.instance.stored_question_item = items[i];

                                break;

                            default:
                                Debug.Log("Default case");



                                break;
                        }
                    }

                    //__________________________________________________________________________

                    getMessage = new Regex(@"\*.[^_]*\*");

                    x = getMessage.Match(questions[index].answers[i].entry);

                    if (x.Success)
                    {
                        bool found = false;

                        string x_str = x.Value.Replace("*", String.Empty);

                        for (int j = 0; j < Resume.ResumeExperiences1.Count; j++)
                        {
                            if (found)
                            {
                                break;
                            }
                            if (Resume.ResumeExperiences1[j].name == x_str)
                            {
                                found = true;
                            }
                        }

                        for (int j = 0; j < Resume.ResumeExperiences2.Count; j++)
                        {
                            if (found)
                            {
                                break;
                            }
                            if (Resume.ResumeExperiences2[j].name == x_str)
                            {
                                found = true;
                            }
                        }

                        for (int j = 0; j < Resume.ResumeExperiences3.Count; j++)
                        {
                            if (found)
                            {
                                break;
                            }
                            if (Resume.ResumeExperiences3[j].name == x_str)
                            {
                                found = true;
                            }
                        }



                        //NACE level checking
                        getMessage = new Regex(@"\#.[^_]*\#");

                        Match level = getMessage.Match(questions[index].answers[i].entry);

                        if (level.Success)
                        {
                            int level_str = int.Parse(level.Value.Replace("#", String.Empty));

                            questions[index].answers[i].entry = questions[index].answers[i].entry.Replace(level.Value, String.Empty);

                            found = skill_check(x_str, level_str);
                        }



                        if (found)
                        {
                            questions[index].answers[i].entry = questions[index].answers[i].entry.Replace(x.Value, String.Empty);
                            
                            load_answer(items[i], questions[index].answers[i]);
                        }
                        else 
                        {
                            items[i].gameObject.SetActive(false);
                        }
                    }
                    else 
                    {
                        load_answer(items[i], questions[index].answers[i]);
                    }
                }
                else
                {
                    items[i].gameObject.SetActive(false);
                }
            }

        }
    }

    public bool skill_check(string str, int value) 
    {
        bool passed = false;

        Debug.Log(str + "   " + value);

        switch (str)
        {
            case "Leadership":

                if (value <= GameManager.instance.Leadership.Level) 
                {
                    passed = true;
                }

                break;

            case "Teamwork":

                if (value <= GameManager.instance.Teamwork.Level)
                {
                    passed = true;
                }

                break;

            case "Technology":

                if (value <= GameManager.instance.Technology.Level)
                {
                    passed = true;
                }

                break;

            case "Professionalism":

                if (value <= GameManager.instance.Professionalism.Level)
                {
                    passed = true;
                }

                break;

            case "Communication":

                if (value <= GameManager.instance.Communication.Level)
                {
                    passed = true;
                }

                break;

            case "CritThinking":

                if (value <= GameManager.instance.CritThinking.Level)
                {
                    passed = true;
                }

                break;

            default:

                break;
        }

        return passed;
    }

    //Pre: N/A
    //Post: Loads an answer
    public void load_answer(Interview_Question_Item item, Answer answer) 
    {
        item.load_text(talkToNPC.ReplaceKeywords(answer.entry));

        item.Reaction = answer.Reaction;

        item.meter_amount = answer.meter_gain;

        item.gameObject.SetActive(true);

        item.anim.SetTrigger("Open");
    }



    public void add_answers(List<Answer> answers)
    {
        int items_index = 0;

        for (int i = 0; i < answers.Count; i++) 
        {
            if (items_index >= 0 && items_index < items.Count) 
            {

                items[items_index].load_text(talkToNPC.ReplaceKeywords(answers[i].entry));

                items[items_index].Reaction = answers[i].Reaction;

                items[items_index].meter_amount = answers[i].meter_gain;

                items[items_index].gameObject.SetActive(true);

                items[items_index].anim.SetTrigger("Open");
            }
        }
    }



    //Pre: None
    //Post: clears the answers UI
    public void clear_array()
    {
      
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].gameObject.activeSelf)
            {
                items[i].anim.SetTrigger("Close");
            }
        }
        Debug.Log("DEACTIVATED");
    }



    //Pre: an integer corresponding to an index in the questions list
    //Post: loads corresponding question into the interview. (Specifically changing the talktoNPC dialogue then populating the answers UI)
    public void load_question(int index) 
    {
        if (index >= 0 && index < questions.Count) 
        {
            menu_anim.SetTrigger("Open");

            talkToNPC.messages[talkToNPC.messageCount] = questions[index].question;

            talkToNPC.enabled = false;

            question_index = index;
        }
    }

    public void anim_load_question() 
    {
        Debug.Log("ACTIVATED");
        populate_answers(question_index);
    }

    public Influence_Meter influence_Meter;

    //Pre: a float to increment the influence meter by.
    //Post: increments the influence meter by the given float amount
    public void increment_meter(float value) 
    {
        if (influence_Meter != null && talkToNPC.messageDone)
        {
            bool max_influence = influence_Meter.increment_meter(value);
        }
    }

    //Pre: an array of strings used for loading the reaction, and a float for the influence meter gain
    //Post: loads answers into the answer UI and increments the meter by the given amount.
    public void load_answer(string[] Reaction, float meter_gain) 
    {
        if (talkToNPC.messageDone) 
        {
            add_answer_to_player_results(Reaction);

            menu_anim.SetTrigger("Close");

            increment_meter(meter_gain);

            talkToNPC.enabled = true;

            talkToNPC.insert_messages(Reaction);

            talkToNPC.DecideWhichDialogueToShow();
        }
    }

    // pre condition: takes array of strings containing the interviewer reactions
    // post condition: search array of strings for keywords that display STAR/VALUE properties and add them to the player result file

    private void add_answer_to_player_results(string[] Reaction) {
        string[] userWords = {"", "", "", ""};
        var keyWords = new List<string>() {"SITUATION", "ACTION", "RESULT", "VISION", "ALLIGN", "UNDERSTAND", "ENACT"};

        var interviewType = "";

        for (int i = 0; i < Reaction.Length; i++) {
            if (Reaction[i].contains("STAR")) {
                interviewType = "STAR";
            }

            else if (Reaction[i].contains("VALUE")) {
                interviewType = "VALUE";
            }

            for (int j = 0; j < keyWords.Count; j++) {
                if (Reaction[i].Contains(keyWords[j])) {
                    userWords.Add(keyWords[j]); // if the reaction contains a key word, add it to userWords
                }
            }
        }

        // KAREEM

        TalkToNPC.UpdateInterviewResults(interviewType, TalkToNPC.playerFileName, userWords);

        // TalkToNPC.UpdatePlayerResults("Star", TalkToNPC.playerFileName);
    }


    public void load_answer_by_index(int index)
    {
        talkToNPC.insert_message_at_index(questions[current_question_index].answers[index].Reaction, talkToNPC.messageCount);
    }

    public void trigger_event_system()
    {
        EventSystem.current.firstSelectedGameObject = items[0].gameObject;

        EventSystem.current.SetSelectedGameObject(items[0].gameObject);
    }

    public void toggle_experience_menu() 
    {
        Selection_Menu.instance.anim.SetTrigger("Toggle");
    }
}
