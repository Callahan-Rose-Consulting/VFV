/*
 This script is used in the Character Selection scene, it allows the player to distribute their points into different skills.
 
 Currently the player starts with 5 points in each stat with 8 extra points to allocate,
 however, it might be a good idea to start each stat at 1 and give the player less points to allocate, perhaps none.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    Skill Leadership= new Skill("Leadership", 5);
    Skill Teamwork = new Skill("Teamwork", 5);
    Skill Technology = new Skill("Technology", 5);
    Skill Professionalism = new Skill("Professionalism", 5);
    Skill Communication = new Skill("Communication", 5);
    Skill CritThinking = new Skill("Critical Thinking", 5);
    int Points = 8;
    Canvas canvas;
    AudioSource click;
    Text LeadershipTxt;
    Text TeamworkTxt;
    Text TechnologyTxt;
    Text ProfessionalismTxt;
    Text CommunicationTxt;
    Text CritThinkingTxt;
    Text PointsTxt;
    InputField NameInputField;
    Job CurrentJob;
    // Start is called before the first frame update
    void Start()
    {
        click = GetComponent<AudioSource>();
        canvas = GameObject.Find("CharacterCreation").GetComponent<Canvas>();
        LeadershipTxt = canvas.transform.GetChild(1).GetComponent<Text>();
        TeamworkTxt = canvas.transform.GetChild(2).GetComponent<Text>();
        TechnologyTxt = canvas.transform.GetChild(3).GetComponent<Text>();
        ProfessionalismTxt = canvas.transform.GetChild(4).GetComponent<Text>();
        CommunicationTxt = canvas.transform.GetChild(5).GetComponent<Text>();
        CritThinkingTxt = canvas.transform.GetChild(6).GetComponent<Text>();
        PointsTxt = canvas.transform.GetChild(7).GetComponent<Text>();
        NameInputField = canvas.transform.GetChild(8).GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        LeadershipTxt.text = "Leadership: " + Leadership.Level;
        TeamworkTxt.text = "Teamwork: " + Teamwork.Level;
        TechnologyTxt.text = "Technology: " + Technology.Level;
        ProfessionalismTxt.text = "Professionalism: " + Professionalism.Level;
        CommunicationTxt.text = "Communication: " + Communication.Level;
        CritThinkingTxt.text = "Critical Thinking: " + CritThinking.Level;
        PointsTxt.text = "Points Left: " + Points;
    }

    public void PlayRandomClickSound()
    {
        click.pitch = Random.Range(0.75f, 1f);
        click.Play();
    }

    public void SaveStats()
    {
        if (NameInputField.text == "")
        {
            NameInputField.text = "Player";
        }
        PlayerPrefs.SetInt("LeadershipLevel", Leadership.Level);
        PlayerPrefs.SetInt("TeamworkLevel", Teamwork.Level);
        PlayerPrefs.SetInt("TechnologyLevel", Technology.Level);
        PlayerPrefs.SetInt("ProfessionalismLevel", Professionalism.Level);
        PlayerPrefs.SetInt("CommunicationLevel", Communication.Level);
        PlayerPrefs.SetInt("CritThinkingLevel", CritThinking.Level);
        PlayerPrefs.SetString("PlayerName", NameInputField.text);
        PlayerPrefs.Save();
        SceneManager.LoadScene("World Map");
    }

    public void IncreaseSkill(Button button)
    {
        if (Points > 0)
        {
            switch (button.name)
            {
                case "IncreaseLeadership":
                    Leadership.Level++;
                    break;
                case "IncreaseTeamwork":
                    Teamwork.Level++;
                    break;
                case "IncreaseTechnology":
                    Technology.Level++;
                    break;
                case "IncreaseProfessionalism":
                    Professionalism.Level++;
                    break;
                case "IncreaseCommunication":
                    Communication.Level++;
                    break;
                case "IncreaseCritThinking":
                    CritThinking.Level++;
                    break;
            }
            Points--;
        }
        click.pitch = 1;
        click.Play();
    }

    public void DecreaseSkill(Button button)
    {
        switch (button.name)
        {
            case "DecreaseLeadership":
                if ((Leadership.Level - 1) > 0)
                {
                    Leadership.Level--;
                    Points++;
                }
                break;
            case "DecreaseTeamwork":
                if ((Teamwork.Level - 1) > 0)
                {
                    Teamwork.Level--;
                    Points++;
                }
                break;
            case "DecreaseTechnology":
                if ((Technology.Level - 1) > 0)
                {
                    Technology.Level--;
                    Points++;
                }
                break;
            case "DecreaseProfessionalism":
                if ((Professionalism.Level - 1) > 0)
                {
                    Professionalism.Level--;
                    Points++;
                }
                break;
            case "DecreaseCommunication":
                if ((Communication.Level - 1) > 0) {
                    Communication.Level--;
                    Points++;
                }
                break;
            case "DecreaseCritThinking":
                if ((CritThinking.Level - 1) > 0) {
                    CritThinking.Level--;
                    Points++;
                }
                break;
        }
        click.pitch = 0.75f;
        click.Play();
    }
    
    public void randomly_attribute_stats() 
    {
        int point_amount = Points;

        for (int i = 0; i < point_amount; i++) 
        {
            int rand_int = Random.Range(0, 6);

            switch (rand_int)
            {
                case 0: 
                    Leadership.Level++;
                    break;
                case 1:
                    Teamwork.Level++;
                    break;
                case 2: 
                    Technology.Level++;
                    break;
                case 3: 
                    Professionalism.Level++;
                    break;
                case 4: 
                    Communication.Level++;
                    break;
                case 5: 
                    CritThinking.Level++;
                    break;
            }

            Points--;

        }
    }
}
