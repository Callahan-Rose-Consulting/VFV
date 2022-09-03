using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class stored_dialogue : MonoBehaviour
{
    //Author: Austin Greear
    //Purpose: This component was mainly created to simply hold many different messages that a NPCs dialogue script could be changed to.
    //The assign_dialogue function for example would simply change the TalkToNPC script by assigning its message variable to a message messages array at the given index.

    [System.Serializable]
    public class Stored_Message
    {
        public string name;

        [TextArea(5, 10)]
        public string[] messages;
    }

    public TalkToNPC talkToNPC;

    public virtual void Start()
    {
        talkToNPC = GetComponent<TalkToNPC>();
    }

    public List<Stored_Message> messages = new List<Stored_Message>();

    public void assign_dialogue(int index) 
    {
        if (index >= 0 && index < messages.Count) 
        {
            var message_array = new List<string>();

            message_array.AddRange(messages[index].messages);

            talkToNPC.messages = message_array.ToArray();
        }
    }
}
