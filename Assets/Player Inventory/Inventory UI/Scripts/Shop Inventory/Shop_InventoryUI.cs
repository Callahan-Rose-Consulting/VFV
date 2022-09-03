using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Shop_InventoryUI : InventoryUI
{
    //Author: Austin Greear
    //Puprpose: This script is used to handle the visualization of the inventory data from the Shop_Inventory script

    //Pre: None
    //Post: Simply put in place to override the functionality of the InventoryUI script
    public override void Awake()
    {

    }

    //Pre: None
    //Post: 
    public override void Start()
    {
        anim = GetComponent<Animator>();

        inventory.onItemChangedCallback += Update_UI;

        for (int i = 0; i < inventory.space; i++) 
        {
            Instantiate(Inventory_Slot, itemsParent);
        }

        slots = itemsParent.GetComponentsInChildren<Inventory_Slot>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].index = i;
            slots[i].owner = this;
        }

        Update_UI();
    }

    //Pre: An item to be used in the formatting of the purchase dialogue array
    //Post: Returns a list of strings containing the purchase dialogue with the given item
    public List<string> make_purchase_start_array(Inventory_Item item)
    {
        var message_array = new List<string>();

        message_array.Add("#INNER_DIALOGUE_BEGIN##YES_NO# Purchase this " + item.name + " for " + item.value);

        return message_array;
    }

    //Pre: Take a float as an amount to purchase the given inventory_item for.
    //Post: Changes the dialogue to display the purchase dialogue
    public void purchase_item(float amount, Inventory_Item item)
    {
        bool collected = item.purchase(amount);

        if (dialogue_system != null)
        {
            add_purchase_dialogue(dialogue_system, collected, item);
        }

        if (collected) 
        {
            inventory.Remove(item);
        }
    }

    //Pre: Takes a talktoNPC script as reference to add the dialogue messages to, takes a boolean to keep track if the item was purchased or not, takes an inventory item as the item to purchase
    //Post: Adds the purchased dialogue to the TalkToNPC script. 
    public void add_purchase_dialogue(TalkToNPC t, bool collected, Inventory_Item item)
    {
        var message_array = new List<string>();

        t.messageCount = 0;

        if (collected)
        {
            t.messages[0] = item.name + " purchased.";
        }
        else
        {
            t.messages[0] = "You lack the sufficient funds...";
        }

        t.StopAllCoroutines();

        t.DecideWhichDialogueToShow();
    }

    public List<UnityEvent> stored_events = new List<UnityEvent>();

    public int stored_event_index = 0;

    //Pre: Takes an integer
    //Post: Sets the stored_event_index to the given integer
    public void set_event_index(int i) 
    {
        stored_event_index = i;
    }

    //Pre: None
    //Post: Invokes the event at the stored_event_index index of the stored_events list
    public void invoke_stored_event() 
    {
        if (stored_event_index >= 0 && stored_event_index < stored_events.Count) 
        {
            if (stored_events[stored_event_index] != null) 
            {
                stored_events[stored_event_index].Invoke();
            }
        }
    }

    //Pre: None
    //Post: Restarts the TalkToNPCs first message so that the player may reinitialize the shop
    public void reset_shop_dialogue() 
    {
        if (dialogue_system != null) 
        {
            dialogue_system.messages[0] = "#MULTI_START# #INVOKE_EVENT#Welcome to the book store! What would you like to do?";
        }
    }

    //Pre: None
    //Post: Simply used to override the events of the InventoryUI script
    public override void Update()
    {

    }

    public GameObject first_selected_object;

    //Pre: None
    //Post: Sets the event system to assign the currently selected object to be the first in the player's inventory.
    public void shop_trigger_event_system()
    {
        if (first_selected_object != null) 
        {
            EventSystem.current.firstSelectedGameObject = first_selected_object;

            EventSystem.current.SetSelectedGameObject(first_selected_object);
        }
    }
}
