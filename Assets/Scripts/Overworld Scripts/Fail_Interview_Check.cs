using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fail_Interview_Check : MonoBehaviour
{
    //Author: Austin Greear
    //Purpose: Checks to see if the player has failed a specific interview (only checking by name and title)
    //If it finds it, it alters the TalkToNPC accordingly 

    public Job job;

    public TalkToNPC talkToNPC;

    [TextArea(5, 10)]
    public string[] succeeded_dialogue;

    [TextArea(5, 10)]
    public string[] failed_dialogue;

    // Start is called before the first frame update
    void Start()
    {
        talkToNPC = GetComponent<TalkToNPC>();
    }

    public bool debug_win;

    public bool debug_fail;

    public void check_fail_dialogue() 
    {
        var message_array = new List<string>();

        if (Resume.current_job != null && Resume.current_job.Company == job.Company && Resume.current_job.Title == job.Title || debug_win)
        {
            insert_messages(succeeded_dialogue);

            succeeded_dialogue = message_array.ToArray();
        }
        else if (Resume.failed_interview_job != null && Resume.failed_interview_job.Company == job.Company && Resume.failed_interview_job.Title == job.Title || debug_fail)
        {
            insert_messages(failed_dialogue);

            failed_dialogue = message_array.ToArray();
        }
    }

    public bool insert_message_at_front = true;

    public void insert_messages(string[] str_array)
    {
        if (insert_message_at_front)
        {
            var message_array = new List<string>();

            message_array.AddRange(str_array);

            message_array.AddRange(talkToNPC.messages);

            talkToNPC.messages = message_array.ToArray();
        }
        else 
        {
            talkToNPC.messages = str_array;
        }
    }
}
