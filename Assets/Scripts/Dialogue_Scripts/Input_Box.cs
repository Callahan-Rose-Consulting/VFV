using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Input_Box : MonoBehaviour
{
    public PersonalityInfo personalityInfo;

    public GameObject inputField;
    public GameObject inputTextBox;
    public GameObject exitButton;

    public GameObject personalityObj;
    public Image personalityImage;

    public Text userFeedback;
    public Image feedbackBackground;

    Sprite mySprite;

    Texture2D tex = null;
    byte[] fileData;

    public virtual void Start() {
        feedbackBackground.enabled = false;
    }

    public void handleDisplay(ref bool displayInputBox, ref bool changeState) {
        inputField.SetActive(true);

        // prevent player movement while user types into box
        displayInputBox = true;
        changeState = false;
        GameManager.instance.change_game_state("Dialogue");
    }

    public void hideDisplay() {
        personalityObj.SetActive(false);

        GameManager.instance.change_game_state("Normal");
    }

    public void hideInput() {
        inputField.SetActive(false);

        GameManager.instance.change_game_state("Normal");
    }

    public void handleSubmit() {
        string userInput = inputTextBox.GetComponent<Text>().text.ToUpper();
        Debug.Log("KAREEM " + userInput);

        if (userInput.Length != 4) {
            updateFeedback("The code needs to be 4 letters", Color.red, true);
            return;
        }

        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "PersonalityResults/" + userInput + ".png");

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

        inputField.SetActive(false);
        personalityObj.SetActive(true);

        updateFeedback("", Color.white, false);
        personalityInfo.OutputPersonalityType(fileData);
    }

    public void updateFeedback(string text, Color color, bool enabled) {
        userFeedback.text = text;
        inputField.GetComponent<Image>().color = color;
        feedbackBackground.enabled = enabled;
    }

}
