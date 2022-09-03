using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Book", menuName = "Inventory/Book")]
public class Book_Item : Inventory_Item
{
    //public static bool isRead = false;
    public  bool isRead = false;
    //Pre: None
    //Post: Simply just used as a test put out a debug log when the item is used
    public override void Use() 
    {

        Debug.Log("Skill Increased!");

        //add switch book type increment skill?
    }


}