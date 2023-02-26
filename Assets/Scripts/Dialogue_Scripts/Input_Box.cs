using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Input_Box : MonoBehaviour
{

    public GameObject inputField;
    public GameObject inputTextBox;
    public GameObject inputButton;
    public Image personalityImage;
    public Text userFeedback;

    Sprite mySprite;

    Texture2D tex = null;
    byte[] fileData;

    public virtual void Start() {
        inputField.SetActive(false);
        inputButton.SetActive(false);
        personalityImage.enabled = false;
    }

    public void handleDisplay(ref bool displayInputBox, ref bool changeState) {
        inputField.SetActive(true);
        inputButton.SetActive(true);

        // prevent player movement while user types into box
        displayInputBox = true;
        changeState = false;
        GameManager.instance.change_game_state("Dialogue");
    }

    private void hideDisplay() {
        inputField.SetActive(false);
        inputButton.SetActive(false);

        GameManager.instance.change_game_state("Normal");
    }

    public void handleSubmit() {
        string userInput = inputTextBox.GetComponent<Text>().text.ToUpper();

        if (userInput.Length != 4) {
            userFeedback.text = "The code needs to be 4 letters.";
            return;
        }

        string filePath = "Assets/UI/PersonalityResults/" + userInput + ".png";

        // create image based on personality type user entered
        if (File.Exists(filePath)) {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            personalityImage.sprite = mySprite;

        }

        else {
            userFeedback.text = "Invalid";
            Debug.Log("NOOOOO"); // tell user that the code is invalid
        }
        
        // personalityImage = infjImage;
        personalityImage.enabled = true;
        
        hideDisplay(); // change game state back to normal
    }

}
