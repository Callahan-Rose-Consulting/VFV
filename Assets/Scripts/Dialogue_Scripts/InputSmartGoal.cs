using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputSmartGoal : MonoBehaviour
{
    public string smartGoal;
    public GameObject inputBox;
    public GameObject inputSmartGoal;

    private void Start()
    {
        inputBox.SetActive(false);
    }
    public void handleDisplay(ref bool displayInputBox, ref bool changeState)
    {
        inputBox.SetActive(true);
        //inputButton.SetActive(true);
        Debug.Log("for movement");
        // prevent player movement while user types into box
        displayInputBox = true;
        changeState = false;
        GameManager.instance.change_game_state("Dialogue");
    }
    public void StoreSmartGoal()
    {
        smartGoal = inputSmartGoal.GetComponent<Text>().text;
        //textDisplay.GetComponent<Text>().text = "Your goal: " + smartGoal;
        Debug.Log("SmartGoal" + smartGoal);
        

        inputBox.SetActive(false);
        GameManager.instance.change_game_state("Normal");
    }
}