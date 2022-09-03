using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //This is a very old script that is no longer used. Can delete.
    static public DialogueManager Instance;

    public Experience selected_experience;
    public int questionCount = 0;

    //public Text nameText;
    public Text dialogueText;
    //public Animator animator;//Placeholder in the case we decide to animate any textboxes

    private Queue<Sentences> sentences;

    void Start()
    {
        Instance = this;
        //sentences = new Queue<string>();
    }


    public void StartDialogue (Dialogue dialogue)
    {
        Debug.Log("StartDialogue");
        //Animator.SetBool("IsOpen", true);

        // nameText.text = dialogue.name;
        sentences = new Queue<Sentences>();
        dialogue.assignSentence();

        this.sentences.Clear();
        int loopnum = 0;
        foreach (Sentences sentence in dialogue.sentences)//puts all filled in members of sentence array in the queue
        {
            Debug.Log(loopnum);
            loopnum++;
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
        
    }

    public void DisplayNextResponse(Experience_Item experience)
    {
        Sentences sentence = new Sentences(); 
        sentence.sentence = experience.experience.name;
        StopAllCoroutines();//Helps go to the next page of text
        StartCoroutine(TypeSentence(sentence));//starts the next page of text
    }

    public void DisplayNextSentence()
    {
        Debug.Log("DisplayNextSentence");
        if (sentences.Count == 0)
        {
            //EndDialogue();//animator bs
            return;
        }

        if (sentences.Peek().stop == true)
        {
            ExperienceCheck(sentences.Peek());//looks at the current active sentence 
        }        
        Sentences sentence = sentences.Dequeue();
        StopAllCoroutines();//Helps go to the next page of text
        StartCoroutine(TypeSentence(sentence));//starts the next page of text
    }

    IEnumerator TypeSentence(Sentences sentence)//the fancy type by character feature
    {
        Debug.Log("TypeSentence");

        dialogueText.text = "";//clears the dialogue box

        foreach(char letter in sentence.sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

    }

    public void ExperienceCheck(Sentences sentence)
    {
        //Reactivate UI
        //Take response from selected_experience and evaluate below



        if (selected_experience.problemSolving >= 1 && questionCount == 0)
        { 
            sentence.goToGood = true;
        }
        else if (questionCount == 0)
        {
            sentence.goToBad = true;
        }
        else
        {
            Debug.Log("Skill check condition error 0");
        }

        if (selected_experience.empathy >= 1 && questionCount == 1)
        {
            sentence.goToGood = true;
        }
        else if (questionCount == 1)
        {
            sentence.goToBad = true;
        }
        else
        {
            Debug.Log("Skill check condition error 1");
        }

        if (selected_experience.food >= 1 && questionCount == 2)
        {
            sentence.goToGood = true;
        }
        else if (questionCount == 2)
        {
            sentence.goToBad = true;
        }
        else
        {
            Debug.Log("Skill check condition error 2");
        }





        if (sentence.goToGood == true)
        {
            dialogueText.text = "Wow, that's impressive!";
            //interviewerExpression = good
        }
        else if (sentence.goToBad == true)
        {
            dialogueText.text = "That's not exactly what we are looking for...";
            //interviewerExpression = bad
        }
        else
        {
            dialogueText.text = "Something went wrong here: DialogueManager.ExperienceCheck()";
            Debug.Log("Something went wrong here: DialogueManager.ExperienceCheck()");
        }
    }

    //more animator bs that will likely go unused
   /* void EndDialogue()
    {
        Animator.SetBool("IsOpen", false);
    }*/
}
