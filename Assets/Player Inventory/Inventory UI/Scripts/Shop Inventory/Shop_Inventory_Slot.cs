using UnityEngine;
using UnityEngine.UI;

public class Shop_Inventory_Slot : Inventory_Slot
{
    //Author: Austin Greear
    //Puprpose: This script is used to handle the visualization of the inventory data from the Shop_InventoryUI script

    //Pre: None
    //Post: Simply put in place to override the functionality of the Inventory_Slot script
    public override void OnRemoveButton() 
    {

    }

    //Pre: None
    //Post: Simply put in place to override the functionality of the Inventory_Slot script
    public override void UseItem() 
    {

    }

    //Pre: None
    //Post: Calls the purchase item from the owner variable after casting it to the type of Shop_InventoryUI
    public void Purchase_Item() 
    {
        if (item != null)
        {
            Shop_InventoryUI shop_owner = (Shop_InventoryUI)owner;

            if (shop_owner != null) 
            {
                shop_owner.purchase_item(item.value, item);
            }
        }
    }
}
