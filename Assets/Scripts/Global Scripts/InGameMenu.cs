using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class InGameMenu : MonoBehaviour
{
    private bool menuOpen = false;
    private GameObject menu;
    public GameObject player;
    public GameObject bulletinBoard;
    public Animator anim;

    public UnityEvent pause_event;
    public UnityEvent unpause_event;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void LeaveGame()
    {

        // Reset the inventory items to inital state
        if (Player_Inventory.instance != null)
        {
            Player_Inventory.instance.items.Clear();
            Player_Inventory.instance.currency = 0.0f;
            Player_Inventory.instance.UI.update_currency(Player_Inventory.instance.currency);
            Player_Inventory.instance.UI.Update_UI();
        }

        Calendar.SetCurrentWeek(1);
        Calendar.CurrentDay = 0;
        Calendar.SetSignificationActions(0);
        Calendar.InterviewAccepted = false;
        Calendar.DayOfInterview = -1;

        Work_Event_Manager.instance.current_job_index = -1;

        // Resume.cs
        Resume.ResumeExperiences1.Clear();
        Resume.ResumeExperiences2.Clear();
        Resume.ResumeExperiences3.Clear();
        Experience NullSkill = new Experience();
        NullSkill.name = "N/A";
        Resume.ResumeExperiences1.Add(NullSkill);
        Resume.ResumeExperiences2.Add(NullSkill);
        Resume.ResumeExperiences3.Add(NullSkill);

        Resume.current_job_progress = 0;
        Resume.current_job = null; ;
        Resume.dream_job = null;
        Resume.failed_interview_job = null;

        // Misc
        GameManager.instance.update_job();

        SceneManager.LoadScene("NewMainMenu");
    }


    // Update is called once per frame
    void Update()
    {
        if ((bulletinBoard == null || bulletinBoard.GetComponent<bulletinBoard>().bulletinBoardOpen == false) && (GameManager.instance == null || GameManager.instance.game_state == "Normal"))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (menuOpen)
                {
                    //Reactivating menu selectables
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }

                    menuOpen = false;

                    anim.SetTrigger("Toggle");

                    Time.timeScale = 1.0f;

                    //Old method
                    //gameObject.GetComponent<Canvas>().enabled = false;

                    //player.GetComponent<Player_Movement>().canMove = true;
                }
                else
                {
                    //@CS deactivating menu selectables to not allow player to use arrow keys to browse over to them when going thru a dialogue choice
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }

                    menuOpen = true;

                    anim.SetTrigger("Toggle");

                    Time.timeScale = 0.0f;

                    //Old method
                    //gameObject.GetComponent<Canvas>().enabled = true;

                    //player.GetComponent<Player_Movement>().canMove = false;
                }
            }
        }
    }

    public GameObject first_selected_object;

    public GameObject last_selected_object;

    public void trigger_event_system()
    {
        last_selected_object = EventSystem.current.firstSelectedGameObject;

        if (first_selected_object != null) 
        {
            EventSystem.current.firstSelectedGameObject = first_selected_object;

            EventSystem.current.SetSelectedGameObject(first_selected_object);
        }
    }

    public void restore_last_selected()
    {
        EventSystem.current.firstSelectedGameObject = last_selected_object;

        EventSystem.current.SetSelectedGameObject(last_selected_object);
    }

    public void invoke_pause_event() 
    {
        pause_event.Invoke();
    }

    public void invoke_unpause_event()
    {
        unpause_event.Invoke();
    }
}
