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
    public Image infjImage; 

    Sprite mySprite;

    Texture2D tex = null;
    byte[] fileData;

    private static string[] personalityTypes = {
        "ISTJ", "ISFJ",  "INFJ",  "INTJ",  
        "ISTP",  "ISFP",  "INFP",  "INTP", 
        "ESTP", "ESFP", "ENFP",  "ENTP",  
        "ESTJ",  "ESFJ",  "ENTJ", "ENFJ"};

    public virtual void Start() {
        inputField.SetActive(false);
        inputButton.SetActive(false);
        personalityImage.enabled = false;
        infjImage.enabled = false;
    }

    public void handleDisplay(ref bool displayInputBox, ref bool changeState) {
        inputField.SetActive(true);
        inputButton.SetActive(true);
        displayInputBox = true;
        changeState = false;
    }

    private void hideDisplay() {
        inputField.SetActive(false);
        inputButton.SetActive(false);

        GameManager.instance.change_game_state("Normal");
    }

    public void handleSubmit() {
        string userInput = inputTextBox.GetComponent<Text>().text.ToUpper();

        if (userInput.Length != 4 || !personalityTypes.Contains(userInput)) {
            Debug.Log("CAUGHT!");
            return;
        }

        string filePath = "Assets/UI/PersonalityResults/INFJ.png";
 
        if (File.Exists(filePath)) {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..t$$anonymous$$s will auto-resize the texture dimensions.
            mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            personalityImage.sprite = mySprite; // apply the new sprite to the image

        }

        else {
            Debug.Log("NOOOOO");
        }
        
        // personalityImage = infjImage;
        personalityImage.enabled = true;
        
        hideDisplay(); // change game state back to normal
    }

}
