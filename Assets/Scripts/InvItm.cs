using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class InvItm 
{
    public string name = "New Item";
    public float value = 0.0f;
    public string icon = null;
    public bool isDefaultItem = false;
    public string[] Use_Dialogue = { "Item used" };
    public string details = "";

    public InvItm(string p_name, float p_value, string p_icon, bool p_isDefaultItem, string[] p_Use_Dialogue, string p_details)
    {
        name = p_name;
        value = p_value;
        icon = p_icon;
        isDefaultItem = p_isDefaultItem;
        Use_Dialogue = p_Use_Dialogue;
        details = p_details;
    }
}
