using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Inventory_Item : ScriptableObject
{
    //Base parent for Inventory Items

    new public string name = "New Item";

    public float value = 0.0f;

    public Sprite icon = null;

    public bool isDefaultItem = false;

    public string[] Use_Dialogue = { "Item used" };

    public string details = "";

    public int Dialogue_Progress;

    //added by Don murphy. Inventory_Items are also used in Bookstre.cs.
    //bookUpdateStats is used to store the string that will be called when the book is used.
    //This string gets passed into Inventory_Slot.cs when the player uses an item.
    //Within Inventory_Slot.cs the ReplaceKeyWord function from TalkToNPCs.cs is called
    public string bookStats; 

    //Pre: None
    //Post: Simply just sends out a debug log notifying that the item was used. Should be overridden on children scripts for actual functionality
    public virtual void Use() 
    {
        Debug.Log("Using " + name);
    }

    //Pre: Takes a float as the cost of the item
    //Post: calls the purchase_item function on the instantiated Player_Inventory script
    public bool purchase(float cost)
    {
        if (Player_Inventory.instance != null) 
        {
            return Player_Inventory.instance.purchase_item(cost, this);
        }

        return false;
    }

    //Pre: Takes a float as the sell value of the item
    //Post: calls the sell_item function on the instantiated Player_Inventory script
    public bool sell(float cost)
    {
        if (Player_Inventory.instance != null)
        {
            return Player_Inventory.instance.sell_item(cost, this);
        }

        return false;
    }
}
