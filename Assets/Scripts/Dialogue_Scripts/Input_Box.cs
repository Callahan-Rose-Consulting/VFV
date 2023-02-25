using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Input_Box : MonoBehaviour
{

    public GameObject inputField;
    public GameObject inputTextBox;

    public virtual void Start() {
        // inputField.SetActive(false);
    }

    public void handleDisplay() {
        Debug.Log("I'VE BEEN CALLED IN INPUTBOX");
        bool isDisplayed = inputField.activeSelf;

        inputField.SetActive(!isDisplayed);
    }

    public void handleSubmit() {
        Debug.Log(inputTextBox.GetComponent<Text>().text);
    }

}
