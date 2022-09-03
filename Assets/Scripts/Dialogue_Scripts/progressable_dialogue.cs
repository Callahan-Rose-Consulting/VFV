using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class progressable_dialogue : Dialogue_Sequencer
{
    //Name of programmer: Austin Greear
    //Purpose: This is a component that is used to handle the modifications of NPCs dialogue based off of the index variable.
    //By storing an array of Dialogue_Messages this component can use an integer to access and progress the messages in it at will.

    public int index = 0;

    public override void Start()
    {
        talkToNPC = GetComponent<TalkToNPC>();
    }

    public bool progress = false;

    public void progress_dialogue() 
    {
        progress = true;
    }

    public override void load_new_dialogue() 
    {
        if (progress && index >= 0 && index < messages.Count) 
        {
            progress = false;

            talkToNPC.messages = messages[index].messages;

            talkToNPC.messageCount = 0;

            index++;
        }
    }
}
