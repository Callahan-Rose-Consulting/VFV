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
        inputField.SetActive(true);
        inputButton.SetActive(true);
        displayInputBox = true;
        changeState = false;
    }

    public void handleSubmit() {
        Debug.Log(inputTextBox.GetComponent<Text>().text);

        inputField.SetActive(false);
        inputButton.SetActive(false);

        GameManager.instance.change_game_state("Normal");
    }

}
