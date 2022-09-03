using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;


public class Work_Event_Manager : MonoBehaviour
{
    //Author: Austin Greear
    //Purpose: This script essetialy handles the start and end of work days based off of what the Job variable is in the player's Resume.

    public static Work_Event_Manager instance;

    public UnityEvent free_day_event;

    public UnityEvent generic_start_day_event;

    public UnityEvent generic_end_day_event;

    public List<TalkToNPC> generic_dialogue_to_modify = new List<TalkToNPC>();

    [TextArea(5, 10)]
    public string[] modified_message;

    List<dialogue_modifier> generic_npc_dialogue = new List<dialogue_modifier>();

    [TextArea(5, 10)]
    public string[] free_day_message;

    [TextArea(5, 10)]
    public string[] end_day_message;

    public TalkToNPC talkToNPC;

    //Enumerator used to take the current celendars day and convert it into a int.
    public enum Week_Days
    {
        Sunday = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }

    [System.Serializable]
    public class Job_Days
    {
        public string name;

        public Job job;

        public GameObject days_object;

        public List<Work_Event> days = new List<Work_Event>();

        public UnityEvent scene_start_event;

        public Job_Days(Job j, List<Work_Event> d)
        {
            job = j;
            days = d;
        }
    }

    public List<Job_Days> job_days = new List<Job_Days>();

    public int current_job_index = 0;
    public void day_index(int index)
    {
        //Resume.current_job_progress = index;
    }

    //Awake and start initialize components and instances.
    public enum CareerPaths { PizzaMaker = 0, PizzaManager, IT_Engineer, SofwareDeveloper, HR_Generalist };
    public void Awake()
    {
        instance = this;

        // Pizza Maker
        // Create a list to hold all the Pizza Maker work events
        List<Work_Event> days = new List<Work_Event>();
        // Get and Store all of the Pizza Maker work events
        days.AddRange(job_days[(int)CareerPaths.PizzaMaker].days_object.GetComponentsInChildren<Work_Event>());
        job_days[(int)CareerPaths.PizzaMaker].days = days;

        // Pizza Manager
        days = new List<Work_Event>();
        days.AddRange(job_days[(int)CareerPaths.PizzaManager].days_object.GetComponentsInChildren<Work_Event>());
        job_days[(int)CareerPaths.PizzaManager].days = days;

        // IT Engineer
        days = new List<Work_Event>();
        days.AddRange(job_days[(int)CareerPaths.IT_Engineer].days_object.GetComponentsInChildren<Work_Event>());
        job_days[(int)CareerPaths.IT_Engineer].days = days;

        // Software Developer
        days = new List<Work_Event>();
        days.AddRange(job_days[(int)CareerPaths.SofwareDeveloper].days_object.GetComponentsInChildren<Work_Event>());
        job_days[(int)CareerPaths.SofwareDeveloper].days = days;

        // HR Generalist
        days = new List<Work_Event>();
        days.AddRange(job_days[(int)CareerPaths.HR_Generalist].days_object.GetComponentsInChildren<Work_Event>());
        job_days[(int)CareerPaths.HR_Generalist].days = days;

        /*
        instance = this;

        Debug.Log("Job Days: " + job_days.Count);

        for (int i = 0; i < job_days.Count; i++)
        {
            List<Work_Event> days = new List<Work_Event>();

            days.AddRange(job_days[i].days_object.GetComponentsInChildren<Work_Event>());

            Debug.Log("Job: " + job_days[i].name + " Days: " + days.Count);

            job_days[i].days = days;
        }
        */
    }
    
     
    public void Start() 
    {

        Debug.Log("generic_dialogue_to_modify.Count: " + generic_dialogue_to_modify.Count);
        for (int i = 0; i < generic_dialogue_to_modify.Count; i++)
        {
            generic_npc_dialogue.Add(new dialogue_modifier(generic_dialogue_to_modify[i], generic_dialogue_to_modify[i].messages, 0, false, modified_message));
        }

        talkToNPC = GetComponent<TalkToNPC>();
    }

    public void insert_free_day_message() 
    {
        talkToNPC.messages = free_day_message;
    }

    /*
     * Added getWorkDayEvent function which will get a randomized workday event for the job
     */ 
    public void insert_end_day_message() 
    {
        if (job_days[current_job_index].days[Resume.current_job_progress].end_day_message.Length > 0)
        {
            talkToNPC.messages = job_days[current_job_index].days[Resume.current_job_progress].end_day_message;
        }
        else
        {
            talkToNPC.messages = end_day_message;
        }
    }

    public void start_work_day() 
    {
        Week_Days myStatus;
        Enum.TryParse(Calendar.GetCurrentDay(), out myStatus);

        find_current_job_index();

        Debug.Log((int)myStatus);

        if (current_job_index >= 0 && job_days[current_job_index].job.Schedule[(int)myStatus])
        {
            insert_end_day_message();

            generic_start_day_event.Invoke();

            start_modify_dialogue(generic_npc_dialogue);

            job_days[current_job_index].days[Resume.current_job_progress].start_event();

            talkToNPC.trigger_dialogue();
        }
        else 
        {
            free_day_event.Invoke();
        }
    }

    //Finds the players current job in the job days list
    public void find_current_job_index() 
    {
        if (Resume.current_job != null)
        {
            for (int i = 0; i < job_days.Count; i++)
            {
                if (Resume.current_job.Company == job_days[i].job.Company && Resume.current_job.Title == job_days[i].job.Title)
                {
                    job_days[i].job = Resume.current_job;
                    current_job_index = i;
                    return;
                }
            }
        }

        current_job_index = -1;
    }

    //Function used to end the work day in game. Calls the end_event on the current work day event, so make sure you start and end the same event.
    public void end_work_day()
    {
        job_days[current_job_index].days[Resume.current_job_progress].end_event();

        generic_end_day_event.Invoke();

        end_modify_dialogue(generic_npc_dialogue);

        if (Resume.current_job_progress + 1 < job_days[current_job_index].days.Count)
        {
            Resume.current_job_progress++;
        }

        //Resume.current_job_progress += 6;  //  Added for debugging Ed, 05/27/2021
    }

    public string[] new_job_message;
    
    public void scene_start_event(TalkToNPC dialogue) 
    {
        find_current_job_index();

        if (current_job_index >= 0)
        {
            dialogue.trigger_dialogue();
        }
    }

    //Modifies dialogue for every single work day event. This is useful for locking the player out of interacting with other NPC when a work day is active.
    public void start_modify_dialogue(List<dialogue_modifier> dialogue)
    {
        for (int i = 0; i < dialogue.Count; i++)
        {
            if (!dialogue[i].override_old_message)
            {
                dialogue[i].old_messages = dialogue[i].npc.messages;
                dialogue[i].old_message_count = dialogue[i].npc.messageCount;
            }

            dialogue[i].npc.messages = dialogue[i].new_messages;
            dialogue[i].npc.messageCount = 0;
        }
    }

    public void end_modify_dialogue(List<dialogue_modifier> dialogue)
    {
        for (int i = 0; i < dialogue.Count; i++)
        {
            if (!dialogue[i].override_old_message)
            {
                dialogue[i].npc.messages = dialogue[i].old_messages;
                dialogue[i].npc.messageCount = dialogue[i].old_message_count;
            }
        }
    }


}
