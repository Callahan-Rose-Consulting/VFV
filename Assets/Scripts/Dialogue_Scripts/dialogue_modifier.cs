using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class dialogue_modifier
{
    //Name of programmer: Austin Greear
    //Purpose: This is a class that is used to handle the modifications of a NPCs dialogue script during specific work days.

    public string name;

    public TalkToNPC npc;
    [TextArea(5, 10)]
    public string[] old_messages;

    public int old_message_count;

    public bool override_old_message;
    [TextArea(5, 10)]
    public string[] new_messages;

    public dialogue_modifier(TalkToNPC n, string[] om, int omc, bool oom, string[] nm)
    {
        npc = n;

        name = n.name;

        old_messages = om;

        old_message_count = omc;

        override_old_message = oom;

        new_messages = nm;
    }
}