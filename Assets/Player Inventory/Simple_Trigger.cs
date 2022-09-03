using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Simple_Trigger : MonoBehaviour
{
    //Author: Austin Greear
    //Puprpose: This script is simply used to trigger a multitude of different things from the player colliding with it.
    //This script is primarily used to allow the player to enter/exit buildings

    public string collideTag = "Player";

    public Collider2D last_other;

    public UnityEvent onTriggerEnter;

    public UnityEvent onTriggerStay;

    public UnityEvent onTriggerExit;

    public bool entered = false;

    public bool invoke_stay_on_update = false;

    void Update() 
    {
        if (invoke_stay_on_update && entered) 
        {
            onTriggerStay.Invoke();
        }
    }

    public string interact_key = "e";

    public UnityEvent interactEvent;

    public void check_interaction_input() 
    {
        if (!DayChanger.endOfDay)
        {
            if (Input.GetKeyDown(interact_key))
            {
                interactEvent.Invoke();
            }
        }
    }

    public void trigger_fade_with_interact_event() 
    {
        if (!DayChanger.endOfDay)
        {
            if (Input.GetKeyDown(interact_key))
            {
                simple_trigger_fade_event();
            }
        }
    }

    public void simple_trigger_fade_event()
    {
        Fade.instance.FadeOut_With_Event(interactEvent);
    }

    public void instant_teleport(GameObject location) 
    {
        GameManager.instance.player_character.gameObject.transform.position = location.transform.position;
    }

    //Pre: This function simply occurs when an object that has the tag, collideTag enters the objects collision
    //Post: It invokes the OnTriggerEnter event (This event is user defined it can do whatever the user defines in the inspectior)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == collideTag)
        {
            entered = true;

            last_other = other;

            onTriggerEnter.Invoke();
        }
    }

    //Pre: This function simply occurs when an object that has the tag, collideTag stays in the objects collision
    //Post: It invokes the OnTriggerStay event (This event is user defined it can do whatever the user defines in the inspectior)
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == collideTag)
        {
            last_other = other;

            onTriggerStay.Invoke();
        }
    }

    //Pre: This function simply occurs when an object that has the tag, collideTag exits the objects collision
    //Post: It invokes the onTriggerExit event (This event is user defined it can do whatever the user defines in the inspectior)
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == collideTag)
        {
            entered = false;

            last_other = other;

            onTriggerExit.Invoke();
        }
    }

    //Pre: None
    //Post: Simply invokes the onTriggerEnter event. Could be used for testing and such.
    public void invoke_on_enter_event() 
    {
        onTriggerEnter.Invoke();
    }

    public void EnterBookStore()
    {
        SceneChange.PreSceneChange();
        SceneManager.LoadScene("BookStore");
    }
}
