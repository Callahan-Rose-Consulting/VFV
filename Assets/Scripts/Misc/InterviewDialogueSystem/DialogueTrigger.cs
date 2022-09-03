using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    public Dialogue GoodDialogue;
    public Dialogue BadDialogue;
    private bool Good = false;
    public Text text;

    /*
     * I edited this for the demo, basically just put in something where either good ending or bad ending triggers.  you can get rid of this
     * once the interview system is more functional.  This is only for a grade, so we have an ending for Maxim and Mike.
     * -Justin Paul <3
     */
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
    }

    public void Start()
    {
        //GetComponent<Selection_Menu>().enabled = false; ;
        
        foreach(Experience experience in Resume.ResumeExperiences3)
        {
            if (experience.name == "Worked with Stefano on making pizzas")
            {
                TriggerDialogue(GoodDialogue);
                Good = true;
            }
        }

        if (!Good)
        {
            TriggerDialogue(BadDialogue);
        }
    }

    public void TriggerDialogue(Dialogue dialogue)
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    
}
/*
 * 
 * Maybe run this at the start of an interview just to get the script going? 
 * Attach to a button and make the interview on click?
 */