using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Book", menuName = "Inventory/Book")]
public class Book_Item : Inventory_Item
{
    GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

    //public static bool isRead = false;
    public bool isRead = false;
    //Pre: None
    //Post: Simply just used as a test put out a debug log when the item is used
    public override void Use()
    {

        Debug.Log("Skill Increased!");
        //add details to the book and match it here 10/27/22 mohsen update 
        if (base.details == "Crit")
        {
            gameManager.CritThinking.LevelUp();
        }
        //else if for the rest of the books/skills
        else if (base.details == "Lead")
        {
            gameManager.Leadership.LevelUp();
        }
        else if (base.details == "Team")
        {
            gameManager.Teamwork.LevelUp();
        }
        else if (base.details == "Prof")
        {
            gameManager.Professionalism.LevelUp();
        }
        else if (base.details == "Com")
        {
            gameManager.Communication.LevelUp();
        }

        //add switch book type increment skill?
    }


}