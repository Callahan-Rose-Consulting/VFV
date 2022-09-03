using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue
{
    /*public string name;
    public bool gotogood1;
    public bool gotobad1;*/

    [TextArea(3, 10)]//makes the input field in Unity bigger, to make easier to work with
    public string[] sentenceInput;
    public Sentences[] sentences;
    public bool[] stop;//tells which sentence the interviewer is requesting a response on, has to match the size of SentenceInput, is set in the Inspector
    //I know it's shitty but I'm running out of time and something needs to be working
    
    

    public void assignSentence()
    {
        Debug.Log("assignSentence");
        Debug.Log(sentences.Length);

        sentences = new Sentences[sentenceInput.Length]; //so you dont have to set sentences to the same length as sentenceInput in unity

        int i = 0;
        foreach (string newSentence in sentenceInput)
        {
            Debug.Log(i);
            Debug.Log(newSentence);
            sentences[i] = new Sentences();
            sentences[i].sentence = newSentence;
            sentences[i].stop = stop[i];
            i++;
        }
        Debug.Log("beep boop");
    }
}


/*
 * Play around with using bools to traverse different dialogue paths with different queues
 * depending on what the player is responding with
 */