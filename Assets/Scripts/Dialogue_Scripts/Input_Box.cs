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
    public GameObject exitButton;
    public Image personalityImage;
    public Text userFeedback;
    public Image feedbackBackground;

    Sprite mySprite;

    Texture2D tex = null;
    byte[] fileData;

    public virtual void Start() {
        inputField.SetActive(false);
        inputButton.SetActive(false);
        // personalityImage.enabled = false;
        exitButton.SetActive(false);
        feedbackBackground.enabled = false;
    }

    public void handleDisplay(ref bool displayInputBox, ref bool changeState) {
        inputField.SetActive(true);
        inputButton.SetActive(true);

        // prevent player movement while user types into box
        displayInputBox = true;
        changeState = false;
        GameManager.instance.change_game_state("Dialogue");
    }

    public void hideDisplay() {
        personalityImage.enabled = false;
        exitButton.SetActive(false);

        GameManager.instance.change_game_state("Normal");

    }

    public void handleSubmit() {
        string userInput = inputTextBox.GetComponent<Text>().text.ToUpper();

        if (userInput.Length != 4) {
            updateFeedback("The code needs to be 4 letters", Color.red, true);
            return;
        }

        string filePath = "Assets/UI/PersonalityResults/" + userInput + ".jpg";

        if (!File.Exists(filePath)) {
            updateFeedback("Invalid Code. Try again", Color.red, true); 
            return;
        }


        // create image based on personality type user entered
        fileData = File.ReadAllBytes(filePath);
        tex = new Texture2D(2, 2);
        tex.LoadImage(fileData);
        mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        personalityImage.sprite = mySprite;
        // personalityImage.enabled = true;
        personalityImage.GetComponent<Canvas>().enabled = true;

        updateFeedback("", Color.white, false);
        exitButton.SetActive(true);
        inputField.SetActive(false);
        inputButton.SetActive(false);

        // hideDisplay(); // change game state back to normal
        
    }

    public void updateFeedback(string text, Color color, bool enabled) {
        userFeedback.text = text;
        inputField.GetComponent<Image>().color = color;
        feedbackBackground.enabled = enabled;
    }

}
