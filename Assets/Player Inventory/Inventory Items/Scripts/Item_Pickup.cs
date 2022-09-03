using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Pickup : MonoBehaviour
{
    //Author: Austin Greear
    //Puprpose: Script to be used when the player picks up an item from the map possibly

    public Inventory_Item item;

    public TalkToNPC t;

    public bool Purchaseable_Item = false;


    public List<string> make_purchase_start_array() 
    {
        var message_array = new List<string>();

        message_array.Add("#INNER_DIALOGUE_BEGIN##YES_NO# Purchase this " + item.name + " for " + "10$?");

        message_array.Add("");

        message_array.Add("#INVOKE_EVENT#");

        return message_array;
    }
    
    public void Start() 
    {
        if (t != null && Purchaseable_Item) 
        {
            t.messages = make_purchase_start_array().ToArray();
        }
    }

    public virtual void add_to_inventory() 
    {
        bool collected = Player_Inventory.instance.Add(item);

        if (collected) 
        {
            AudioSource audio = gameObject.AddComponent<AudioSource>();
            audio.PlayOneShot((AudioClip)Resources.Load("CollectSFX"));
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            Destroy(this.gameObject, 0.5f);
        }
    }

    public void check_for_input() 
    {
        if (Input.GetButton("Interact"))
        {
            add_to_inventory();
        }
    }

    public void add_currency(float amount)
    {
        bool collected = Player_Inventory.instance.increment_currency(amount);

        if (collected)
        {
            AudioSource audio = gameObject.AddComponent<AudioSource>();
            audio.PlayOneShot((AudioClip)Resources.Load("CollectSFX"));
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            Destroy(this.gameObject, 0.5f);
        }
    }

    public void purchase_item(float amount)
    {
        bool collected = item.purchase(amount);

        if (t != null) 
        {
            add_purchase_dialogue(t, collected);
        }

        destroy_item(collected);
    }

    public void add_purchase_dialogue(TalkToNPC t, bool collected)
    {
        var message_array = new List<string>();

        if (collected)
        {
            message_array.Add(item.name + " purchased. #MULTI_END#");

            t.insert_messages(message_array.ToArray());
        }
        else 
        {
            t.messageCount = 0;

            message_array.Add("You lack the sufficient funds... #MULTI_END#");

            message_array.AddRange(make_purchase_start_array());

            t.messages = message_array.ToArray();
        }
    }



    public void destroy_item(bool b) 
    {
        if (b)
        {
            AudioSource audio = gameObject.AddComponent<AudioSource>();
            audio.PlayOneShot((AudioClip)Resources.Load("CollectSFX"));
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            Destroy(this.gameObject, 0.5f);
        }
    }
}
