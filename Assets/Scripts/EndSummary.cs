//This script generates the player end game summary based on the player's stats and skill levels

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EndSummary : MonoBehaviour
{
    public GameObject pnlSummary;
    Text txtSummary;
    
    // Start is called before the first frame update
    void Start()
    {
        //SceneChange.PreSceneChange();
        txtSummary = pnlSummary.transform.Find("summarytxt").GetComponent<Text>();
        Button exitBtn = pnlSummary.transform.Find("btnExit").GetComponent<Button>();
        exitBtn.Select();

        print(Resume.current_job.Title);
        

        txtSummary.text = "Gameplay Summary for " + PlayerPrefs.GetString("PlayerName") + "\n\n" +
            "Final Occupation: " + Resume.current_job.Title +
            "\nMoney: " + SceneChange.currency.ToString() +
            "\nTotal Money Earned: " + SceneChange.totalEarnings.ToString() +
            "\nLeadership " + SceneChange.leadership.Level.ToString() +
            "\nTeamwork " + SceneChange.teamwork.Level.ToString() +
            "\nTechnology " + SceneChange.technology.Level.ToString() +
            "\nProfessionalism " + SceneChange.professionalism.Level.ToString() +
            "\nCommunication " + SceneChange.communication.Level.ToString() +
            "\nCritical Thinking " + SceneChange.critThinking.Level.ToString() +
            "\n\nThank you for playing!";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void returnToMenu()
    {
        SceneManager.LoadScene("NewMainMenu");
    }
}
