using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InputSmartGoal : MonoBehaviour
{
    public string smartGoal;
    public GameObject inputField;
    public GameObject textDisplay;


    public void StoreSmartGoal()
    {
        smartGoal = inputField.GetComponent<Text>().text;
        textDisplay.GetComponent<Text>().text = "Your goal: " + smartGoal;
        
        

    }
}