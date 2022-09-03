using UnityEngine;
using UnityEngine.UI;

public class Inventory_Slot : MonoBehaviour
{
    //Author: Austin Greear
    //Puprpose: This script loads the information of a given item through the InventoryUI script.

    public Inventory_Item item;

    public Image icon;

    public Button click_button;

    public Button removeButton;

    public InventoryUI owner;

    public int index = 0;

    public RectTransform UI_pos;

    //Pre: None
    //Post: The UI_pos variable is assigned
    public virtual void Start() 
    {
        UI_pos = GetComponent<RectTransform>();
    }

    //Pre: Takes an inventory item as input
    //Post: Initialized the slot with the inventory_item's information.
    public void AddItem(Inventory_Item newItem) 
    {
        item = newItem;

        icon.sprite = item.icon;

        icon.enabled = true;

        removeButton.interactable = true;
    }

    //Pre: None
    //Post: Clears out the inventory slot
    public void ClearSlot()
    {
        item = null;

        // Ed - June 13th, 2021  Code was trying to assign values to a null icon.
        if (null != icon)
        {
            icon.sprite = null;

            icon.enabled = false;
        }


        removeButton.interactable = false;
    }

    //Pre: None
    //Post: Finds the Player_Inventory instance and removes the item from it
    public virtual void OnRemoveButton() 
    {
        Player_Inventory.instance.Remove(item);
    }

    //Pre: None
    //Post: Uses the item that is stored within this script if the item isn't null and the inventory's state isn't sell
    public virtual void UseItem() 
    {
        if (item != null) 
        {
            if (Player_Inventory.instance.state != "Sell")
            {
                if (!InventoryUI.instance.dialogue_system.getIsTalking())
                {
                    InventoryUI.instance.dialogue_system.messageCount = item.Dialogue_Progress;

                    InventoryUI.instance.dialogue_system.messages = item.Use_Dialogue;

                    InventoryUI.instance.dialogue_system.trigger_dialogue();

                    item.Use();

                    item.Dialogue_Progress = item.Use_Dialogue.Length - 1;

                    
                    // Add book to education list
                    AddToExperienceList(Resume.ResumeExperiences3, item.name);
                    /*
                    Experience experience = new Experience();
                    experience.name = item.name;

                    if (Resume.ResumeExperiences3.Count == 1 && Resume.ResumeExperiences3[0].name == "N/A")
                    {
                        Resume.ResumeExperiences3[0] = experience;
                    }
                    else if (!Resume.ResumeExperiences3.Exists(x => x.name == experience.name))
                    {
                        Resume.ResumeExperiences3.Add(experience);
                    }

                    if (Selection_Menu.instance != null)
                    {
                        Selection_Menu.instance.populate_content_folders();
                    }
                    */

                    // Check to see if criteria for career fair has been met.
                    int iSE = 0, iHR = 0, iIT = 0;
                    foreach(Experience Book in Resume.ResumeExperiences3 )
                    {
                        BookStoreItem b = System.Array.Find(Bookstore.BS_Items, p => p.Title == Book.name);
                        if (b.Flags.Contains("SE")) iSE++;
                        if (b.Flags.Contains("IT")) iIT++;
                        if (b.Flags.Contains("HR")) iHR++;
                    }

                    if (iSE >= 4) AddToExperienceList(Resume.ResumeExperiences1, "Software Engineering");
                    if (iIT >= 4) AddToExperienceList(Resume.ResumeExperiences1, "IT Engineer");
                    if (iHR >= 4) AddToExperienceList(Resume.ResumeExperiences1, "HR Managment");
                }
            }
            else 
            {
                bool sold = false;

                sold = Player_Inventory.instance.sell_item(item.value, item);

                if (sold) 
                {
                    owner.remove_detail();
                }
            }
        }
    }

    //Pre: None
    //Post: Calls the function display_detail from the owner variable of this script
    public virtual void Display_Detail()
    {
        if (owner != null && item != null)
        {
            owner.display_detail(this);
        }
        else 
        {
            owner.remove_detail();
        }
    }

    private void AddToExperienceList(System.Collections.Generic.List<Experience> p_aoExperience, string p_oItemName)
    {
        Experience experience = new Experience();
        experience.name = p_oItemName;

        if (p_aoExperience.Count == 1 && p_aoExperience[0].name == "N/A")
        {
            p_aoExperience[0] = experience;
        }
        else if (!p_aoExperience.Exists(x => x.name == experience.name))
        {
            p_aoExperience.Add(experience);
        }

        if (Selection_Menu.instance != null)
        {
            Selection_Menu.instance.populate_content_folders();
        }
    }

    public void AddAllCareerChoices()
    {
        AddToExperienceList(Resume.ResumeExperiences1, "Software Engineering");
        AddToExperienceList(Resume.ResumeExperiences1, "IT Engineer");
        AddToExperienceList(Resume.ResumeExperiences1, "HR Managment");
    }
}
