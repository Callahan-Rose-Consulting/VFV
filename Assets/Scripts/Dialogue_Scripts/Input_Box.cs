using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Input_Box : MonoBehaviour
{

    public GameObject inputField;

    public virtual void Start() {
        inputField.gameObject.SetActive(false);
    }

    public void handleDisplay() {
        bool isDisplayed = inputField.gameObject.activeSelf;

        inputField.gameObject.SetActive(!isDisplayed);
    }

}
