using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputSmartGoal : MonoBehaviour
{
    public string smartGoal;
    public GameObject inputField;
    public GameObject textDisplay;
    public PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    public void GetSmartGoal()
    {
        // Disable player movement
        playerMovement.enabled = false;

        smartGoal = inputField.GetComponent<Text>().text;
        textDisplay.GetComponent<Text>().text = "You entered: " + smartGoal;
    }

    public void OnInputFieldEndEdit()
    {
        // Enable player movement
        playerMovement.enabled = true;
    }
}
