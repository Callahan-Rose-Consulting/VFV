using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    //Author: Austin Greear
    //Puprpose: This script is used to handle the visualization of the inventory data from the Player_Inventory script

    public Transform itemsParent;

    public GameObject inventoryUI;

    public Inventory_Slot[] slots;

    public static InventoryUI instance;

    public  Player_Inventory inventory;

    public GameObject Inventory_Slot;

    public Animator anim;

    public Animator resume_anim;

    public TalkToNPC dialogue_system;

    public TextMeshProUGUI currency_text;

    public RectTransform detail_panel;

    public TextMeshProUGUI detail_text;

    public string[] null_Dialogue = { "Item Used" };

    //CS
    public GameObject bulletinBoard;

    //Pre: None
    //Post: Initializes the instance of Inventory_UI, assigns the dialogue system variable and initializes the dialogue systems message.
    public virtual void Awake()
    {
        instance = this;

        dialogue_system = GetComponent<TalkToNPC>();

        if (dialogue_system != null) 
        {
            dialogue_system.messages = null_Dialogue;
        }
    }


    //Pre: None
    //Post: Assigns many other component variables, instantiates the slots in the inventory UI
    public virtual void Start()
    {
        anim = GetComponent<Animator>();

        inventory = Player_Inventory.instance;

        update_currency(inventory.currency);

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
    }

    public string inventory_button_name = "Jump1";
    public string inventory_button_name2 = "Jump";

    public Animator stats_anim;

    //Pre: None
    //Post: Checks for input to toggle the inventory's animator. Checks to make sure the bullitenboard is not open and that the inventory's state isn't sell.
    public virtual void Update()
    {
        if (bulletinBoard.GetComponent<bulletinBoard>().bulletinBoardOpen == false)
        {
            if (Input.GetButtonDown(inventory_button_name))
            {
                if (GameManager.instance.game_state == "Normal" || GameManager.instance.game_state == "Inventory")
                {
                    if (open)
                    {
                        GameManager.instance.change_game_state("Normal");
                        close_inventory();
                    }
                    else 
                    {
                        GameManager.instance.change_game_state("Inventory");
                        anim.SetTrigger("Open");
                        stats_anim.SetTrigger("Open");
                        resume_anim.SetTrigger("Open");
                    }
                }
            }
        }
    }

    //Pre: None
    //Post: Updates the UI of the inventory depending on the items in the instantiated player inventory script
    public void Update_UI()
    {
        Debug.Log("Updating UI");

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }

    }


    public int columns = 6;

    //Pre: Takes an Inventory slot to display the detail of as input
    //Post: Displays the item associated with that item slot.
    public virtual void display_detail(Inventory_Slot item) 
    {
        if (detail_panel != null && detail_text != null) 
        {
            detail_panel.gameObject.SetActive(true);
            //code to crop title name for books/items for display box

            //string itemNameShort = item.item.name;
            //if (item.item.name.Length > 28)
            //    itemNameShort = item.item.name.Substring(0, 30) + "...";
            //detail_text.text = item.item.name + " Value: " + "$" + item.item.value;

            //displays full name + value of item
            detail_text.text = item.item.name + "\nValue: " + "$" + item.item.value;

            /*
             * original code for description that moves with item selection
            if (item.index % columns == 0) //For the left hand side of the inventory
            {
                detail_panel.anchoredPosition = item.UI_pos.anchoredPosition + new Vector2(96.0f, 0.0f);
            }
            else if (item.index % columns == columns - 1) //For the right hand side of the inventory 
            {
                detail_panel.anchoredPosition = item.UI_pos.anchoredPosition + new Vector2(-96.0f, 0.0f);
            }
            else //For the middle of the inventory
            {
                detail_panel.anchoredPosition = item.UI_pos.anchoredPosition;
            }
            */
        }
    }

    //Pre: None
    //Post: Removes the detail by disabling the gameObject attached to the detail_panel
    public virtual void remove_detail()
    {
        if (detail_panel != null && detail_text != null)
        {
            detail_panel.gameObject.SetActive(false);
        }
    }

    public EventSystem m_EventSystem;


    //Pre: None
    //Post: Sets the event system to assign the currently selected object to be the first in the player's inventory.
    public void trigger_event_system() 
    {
        m_EventSystem = EventSystem.current;

        EventSystem.current.firstSelectedGameObject = slots[0].click_button.gameObject;

        EventSystem.current.SetSelectedGameObject(slots[0].click_button.gameObject);
    }

    //Pre: Takes a float to update the currency with
    //Post: Updates the currency UI
    public void update_currency(float currency)
    {
        currency_text.text = "USD: " + "$" + currency;
    }

    //Pre: None
    //Post: Deactives the inventoryUI gameObject
    public void deactivate_UI() 
    {
        inventoryUI.SetActive(false);
    }

    //Pre: None
    //Post: Activates the inventoryUI gameObject
    public void activate_UI()
    {
        inventoryUI.SetActive(true);
    }

    public bool open = false;

    public void close_inventory() 
    {
        if (open) 
        {
            anim.SetTrigger("Close");
            stats_anim.SetTrigger("Close");
            resume_anim.SetTrigger("Close");
        }
    }
}
