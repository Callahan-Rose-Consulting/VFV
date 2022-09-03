using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Dialogue_Randomizer : Dialogue_Sequencer
{
    //Name of programmer: Austin Greear
    //Purpose: This is a class that is used to handle the randomization of dialogue for some town functions.
    //This was mainly created to handle the randomization of school lecture events

    [System.Serializable]
    public class Dialogue_Messages
    {
        public string name;

        public float chance;
        [TextArea(5, 10)]
        public string[] messages;
    }

    public List<Dialogue_Messages> rand_messages = new List<Dialogue_Messages>();

    public override void Start()
    {
        talkToNPC = GetComponent<TalkToNPC>();

        rand_messages = rand_messages.OrderBy(o => o.chance).ToList();
    }

    public override void load_new_dialogue() 
    {
        for (int i = 0; i < rand_messages.Count; i++) 
        {
            if (UnityEngine.Random.Range(0f, 1f) < rand_messages[i].chance) 
            {
                talkToNPC.messageCount = 0;
                talkToNPC.messages = rand_messages[i].messages;
                break;
            }
        }
    }
}
