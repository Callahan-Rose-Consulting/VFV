using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Input_Box : MonoBehaviour
{

    public GameObject inputField;
    public GameObject inputTextBox;
    public GameObject inputButton;

    public virtual void Start() {
        inputField.SetActive(false);
        inputButton.SetActive(false);
    }

    public void handleDisplay(ref bool displayInputBox, ref bool changeState) {
        // display textbox and put dialouge mode so player input doesn't affect sorroundings 
        if (inputField.activeSelf) {
            inputField.SetActive(false);
            inputButton.SetActive(false);
        }

        else {
            inputField.SetActive(true);
            inputButton.SetActive(true);
            displayInputBox = true;
            changeState = false;
        }

    }

    public void handleSubmit() {
        Debug.Log(inputTextBox.GetComponent<Text>().text);
    }

}
