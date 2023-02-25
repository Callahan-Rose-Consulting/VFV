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

    public void handleDisplay() {
        bool fieldIsDisplayed = inputField.activeSelf;
        bool buttonIsDisplayed = inputButton.activeSelf;

        inputField.SetActive(!fieldIsDisplayed);
        inputButton.SetActive(!buttonIsDisplayed);
    }

    public void handleSubmit() {
        Debug.Log(inputTextBox.GetComponent<Text>().text);
    }

}
