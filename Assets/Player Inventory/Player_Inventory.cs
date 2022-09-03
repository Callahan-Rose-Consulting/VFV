using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
{
    //Author: Austin Greear
    //Puprpose: This script is used to store and manage the core data of the players inventory.

    public static Player_Inventory instance;

    TalkToNPC talktoNPC;

    public string state = "Use";

    //Pre: Takes a string as input
    //Post: Assigns the state variable with the given variable
    public void set_state(string new_state) 
    {
        state = new_state;
    }

    //Pre: None
    //Post: Assigns the instance of this script
    public virtual void Awake() 
    {
        instance = this;
    }

    //Pre: None
    //Post: Assigns the UI variable by finding the instantiated inventoryUI script
    public virtual void Start() 
    {
        UI = InventoryUI.instance;
    }

    public delegate void OnItemChanged();

    public OnItemChanged onItemChangedCallback;

    public int space = 20;

    public float currency = 0;
    public float totalEarnings = 0f;

    public InventoryUI UI;

    //Pre: takes a float as an amount to increment the currency by
    //Post: Modifies the currency amount by the given variable
    public bool increment_currency(float amount) 
    {
        currency += amount;

        UI.update_currency(currency);

        return true;
    }

    public List<Inventory_Item> items = new List<Inventory_Item>();

    //Pre: Takes an inventory item as input
    //Post: Adds said inventory item to the inventory if there is enough space available in the inventory
    public bool Add(Inventory_Item item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= space) 
            {
                return false;
            }

            items.Add(item);

            if (onItemChangedCallback != null) 
            {
                onItemChangedCallback.Invoke();
            }
        }

        return true;
    }

    //Pre: Takes an inventory item as input
    //Post: Removes the first found instance of the given item from the inventory
    public bool Remove(Inventory_Item item)
    {
        bool removed = items.Remove(item);

        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }

        return removed;
    }

    //Pre: Takes an item as input
    //Post: Returns true if the given inventory item was found in the inventory.
    public bool contains(Inventory_Item item) 
    {
        return items.Contains(item);
    }

    //Pre: Takes a float as the cost of the item, and an inventory item as the actual item to be purchased
    //Post: If the item can be purchesed (There is enough space in the player's inventory and they have enough currency) then the item will be purchased
    public bool purchase_item(float cost, Inventory_Item item) 
    {
        if (currency >= cost) 
        {
            if (Add(item)) 
            {
                increment_currency(-cost);

                return true;
            }
        }
        
        //tell player they do not have enough currency?
        Debug.Log("Not enough money!");

        return false;
    }

    //Pre: Takes a float as the cost of the item, and an inventory item as the actual item to be sold
    //Post: if the item can be removed, then the player's currency is incremented by the given amount
    public bool sell_item(float cost, Inventory_Item item)
    {
        if (Remove(item))
        {
            increment_currency(cost);

            return true;
        }

        return false;
    }

    public List<Inventory_Item> getItems()
    {
        return items;
    }
}
