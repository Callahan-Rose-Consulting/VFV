using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Dialogue_Sequencer : MonoBehaviour
{
    //Name of programmer: Austin Greear
    //Purpose: This is a component that is used to handle the modifications of NPCs dialogue based off of days.

    [System.Serializable]
    public class Dialogue_Messages
    {
        public string name;

        public int day;
        [TextArea(5, 10)]
        public string[] messages;
    }

    public TalkToNPC talkToNPC;

    public List<Dialogue_Messages> messages = new List<Dialogue_Messages>();

    public virtual void Start()
    {
        talkToNPC = GetComponent<TalkToNPC>();
    }

    public virtual void load_new_dialogue()
    {
        int index = messages.FindIndex(i => i.day == Calendar.CurrentDay);

        if (index >= 0) 
        {
            talkToNPC.messages = messages[index].messages;
        }
    }
}
