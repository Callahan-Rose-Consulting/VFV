using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using TMPro;



public class DayChanger : MonoBehaviour
{
    public static DayChanger instance;
    public Canvas UICanvas;
    public GameObject DaySummaryUI;
    public RawImage textBox;
    public TextMeshProUGUI Text;

    public GameObject EventPopUp;
    public RawImage popUpTextBox;
    public TextMeshProUGUI popUpText;

    public GameObject GameOverText;
    public RawImage GameOverTextBox;

    public static bool endOfDay;
    public static bool triggerEvent;

    public static string[] summary_messages;

    public TextMeshProUGUI CalendarTextBox;
    //public TalkToNPC talkToNPC;
    //public string[] messages;

    void Awake()
    {
        summary_messages = new string[3];
        instance = this;
 
    }

    private Player_Movement player;
    private bool coroutine = false;

    public List<Dialogue_Sequencer> dialogue_modifiers = new List<Dialogue_Sequencer>();

    // Start is called before the first frame update
    void Start()
    {

        CalendarTextBox.text = "Week: " + Calendar.GetCurrentWeek() + " - " + Calendar.GetCurrentDay() + "\n";
        CalendarTextBox.text += "Actions Left: " + (2 - Calendar.GetSignificationActions());
        player = GameObject.Find("Player").GetComponent<Player_Movement>();
        endOfDay = false;
        triggerEvent = true;
        Dialogue_Sequencer[] d = FindObjectsOfType(typeof(Dialogue_Sequencer)) as Dialogue_Sequencer[];
        StartCoroutine(ChangeSizeCoroutine(0.5f, 0f, textBox));
        StartCoroutine(ChangeSizeCoroutine(0.5f, 0f, popUpTextBox));
        StartCoroutine(ChangeSizeCoroutine(0.5f, 0f, GameOverTextBox));

        dialogue_modifiers = d.ToList();
        summary_messages[0] = "";
        summary_messages[1] = "";
        summary_messages[2] = "";
        //talkToNPC = GetComponent<TalkToNPC>();
        //talkToNPC.messages = messages;
    }

    // Update is called once per frame
    void Update()
    {

        CalendarTextBox.text = "Week: " + Calendar.GetCurrentWeek() + " - " + Calendar.GetCurrentDay() + "\n";
        CalendarTextBox.text += "Actions Left: " + (2 - Calendar.GetSignificationActions());
     
        if(TalkToNPC.endGame)
        {
            if(!TalkToNPC.dialogueActive)
            {
                StartCoroutine(WaitForKeyPressGameOver());
            }
        }
        if (player.canMove && Calendar.GetSignificationActions() >= 2)
        {
            if (!coroutine)
            {
                GameObject.Find("FadeToBlack").GetComponent<Fade>().FadeOut();
                // ED GameObject.Find("GameManager").GetComponent<AudioSource>().Play();
                StartCoroutine(WaitForKeyPress());
                
                coroutine = true;
            }
        }
        
        if(triggerEvent && Calendar.GetCurrentDayIndex() == Calendar.DayOfEvent && Resume.current_job == null)
        {
            EventPopUp.SetActive(true);
            StartCoroutine(WaitForKeyPressEvent());
            triggerEvent = false;
        }else if (Calendar.GetCurrentDayIndex() != Calendar.DayOfEvent)
        {
            triggerEvent = true;
        }
 

        if(endOfDay)
        {
            Debug.Log("End of day is true");
        }
        else
        {
            Debug.Log("End of day is false");
        }
       /* if(Input.GetKey("space") || Input.GetKey("enter"))
        {
            Debug.Log("Space is clicked");
            DaySummaryUI.SetActive(false);   
        }*/

    }

    IEnumerator WaitForKeyPress()
    {
        /*WaitForSeconds waitTime = new WaitForSeconds(1);
        yield return waitTime;*/
        while(!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return))
        {
            endOfDay = true;
            player.canMove = false;
            show_summary_dialogue();
            yield return null;
        }
        GameObject.Find("FadeToBlack").GetComponent<Fade>().FadeIn();
        StartCoroutine(ChangeSizeCoroutine(0.5f, 0f, textBox));
        player.canMove = true;
        endOfDay = false;
        //DaySummaryUI.SetActive(false);
        Debug.Log("Day changed beforenew"+ Calendar.CurrentDay);
       
        Calendar.IncreaseDay();
      
        Debug.Log("Day changed after new" + Calendar.CurrentDay);

        load_new_day_dialogue();
        TalkToNPC.interviewTaken = false;

        if (Resume.current_job != null)
        {
            Work_Event_Manager.instance.start_work_day();
        }
        else 
        {
            Debug.Log("Still no job!");
        }
        /* //attempt to add debriefing
         if(TalkToNPC.interviewTaken)
         {
             Debug.Log("InterviewTaken is true!");
             //Trigger debreifing dialogue
             //TalkToNPC.interviewTaken = false;
         }
         else
         {
             Debug.Log("InterviewTaken is false");
         }*/

        //talkToNPC.trigger_dialogue();
        gameObject.transform.position = new Vector3(6.5f, 8.5f, 0f);
        coroutine = false;
    }

    IEnumerator WaitForKeyPressEvent()
    {
        /*WaitForSeconds waitTime = new WaitForSeconds(1);
        yield return waitTime;*/
        while (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return))
        {
            player.canMove = false;
            show_event_popup();
            StartCoroutine(ChangeSizeCoroutine(0.5f, 1f, popUpTextBox));
            yield return null;
        }
        StartCoroutine(ChangeSizeCoroutine(0.5f, 0f, popUpTextBox));
        
        player.canMove = true;
        

        coroutine = false;
    }

    IEnumerator WaitForKeyPressGameOver()
    {
        /*WaitForSeconds waitTime = new WaitForSeconds(1);
        yield return waitTime;*/
        while (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Return))
        {
            player.canMove = false;
            GameOverText.SetActive(true);
            StartCoroutine(ChangeSizeCoroutine(0.5f, 1f, GameOverTextBox));
            yield return null;
        }
        StartCoroutine(ChangeSizeCoroutine(0.5f, 0f, GameOverTextBox));
        SceneChange.PreSceneChange();
        SceneManager.LoadScene("End Summary");
        player.canMove = true;

        coroutine = false;
    }

    public void load_new_day_dialogue() 
    {
        for (int i = 0; i < dialogue_modifiers.Count; i++)
        {
            dialogue_modifiers[i].load_new_dialogue();
        }
    }

    IEnumerator ChangeSizeCoroutine(float time, float scale, RawImage textBox)
    {
        Vector3 originalScale = textBox.transform.localScale;
        Vector3 destinationScale = new Vector3(scale, scale, scale);

        float currentTime = 0.0f;

        do
        {
            textBox.transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
        textBox.transform.localScale = destinationScale;
    }


    public void show_summary_dialogue()
    {
        Debug.Log("HEY YOU!!!!!!");
        bool next = false;
        DaySummaryUI.SetActive(true);
        StartCoroutine(ChangeSizeCoroutine(0.5f, 1f, textBox));
        
        summary_messages[0] = summary_messages[0].Replace("*", String.Empty);
        summary_messages[1] = summary_messages[1].Replace("*", String.Empty);
        summary_messages[2] = summary_messages[2].Replace("*", String.Empty);

        string morning = "In the morning, I ";
        string afternoon = "\n \n In the afternoon, I ";
        string afternoon2 = " and ";

        if (summary_messages[1] == "")
        {
            afternoon = "";
        }

        if (summary_messages[2] == "")
        {
            afternoon2 = "";
        }
        Text.text = morning + summary_messages[0] + "\n" + afternoon + summary_messages[1] + "\n" + afternoon2 + summary_messages[2];
        
       
    }

    public void show_event_popup()
    {
        string text = "The networking event is today. Head to the Wellness Center!";
        popUpText.text = text;
    }
}