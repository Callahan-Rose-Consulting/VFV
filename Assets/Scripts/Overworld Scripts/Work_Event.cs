using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Work_Event : MonoBehaviour
{
    //Author: Austin Greear
    //Purpose: This script contains information that the Work event manager uses when starting and ending a work day.

    public UnityEvent work_event_start;

    public UnityEvent work_event_end;

    public List<dialogue_modifier> npc_dialogue = new List<dialogue_modifier>();

    public bool event_active = false;

    [TextArea(5, 10)]
    public string[] end_day_message;

    public void Start() 
    {
        this.gameObject.SetActive(false);
    }

    //Starts the work day by invoking the work_event_start event and modifying the dialogue accordingly (This is useful for locking the player out of interacting with other NPC when a work day is active.)
    public void start_event()
    {
        start_modify_dialogue(npc_dialogue);

        work_event_start.Invoke();

        event_active = true;
    }

    //Boolean used to decide whether or not you want fade the end of the day out.
    public bool fade_out_end = false;

    //Invokes the end event. Also modifies the dialogue to be what it was previously.
    public void end_event()
    {
        if (event_active) 
        {
            end_modify_dialogue(npc_dialogue);

            if (fade_out_end)
            {
                Fade.instance.FadeOut_With_Event(work_event_end);
            }
            else
            {
                work_event_end.Invoke();
            }

            event_active = false;
        }
    }

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

    public void set_position(string vector3)
    {
        Vector3 vector = transform.localPosition;

        string[] splitStr = vector3.Split(' ');

        for (int i = 0; i < splitStr.Length; i++)
        {
            if (i == 0)
            {
                vector.x = float.Parse(splitStr[i]);
            }
            else if (i == 1)
            {
                vector.y = float.Parse(splitStr[i]);
            }
            else
            {
                vector.z = float.Parse(splitStr[i]);
            }
        }

        transform.localPosition = vector;
    }
}
