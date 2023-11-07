using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    //Instance for easy access
    public static GameManager instance;

    //Player script for ease of access
    public Player_Movement player_character;

    public Skill selfAssessment { get; set; }
    public Skill Entre_thinking { get; set; }
    public Skill SAF { get; set; }
    public Skill Brand { get; set; }
    public Skill PFP { get; set; }
    public Skill Under_Need { get; set; }
    public Skill HumanResources { get; private set; }
    public Skill IT { get; private set; }
    public Skill SoftwareDevelopment { get; private set; }

    public string Name { get; private set; }
    
    public TextMeshProUGUI CalendarText;

    public TextMeshProUGUI JobText;

    public GameObject SeminarGuy;
    public GameObject StartInterviewGuy;

    public GameObject EventRobert;
    public GameObject NormalRobert;
    public GameObject EventGutsy;
    public GameObject NormalGutsy;
    public GameObject EventDrSphinx;
    public GameObject NormalDrSphinx;
    public GameObject EventStefano;
    public GameObject NormalStefano;
    public GameObject EventSyed;
    public GameObject NormalSyed;
    public GameObject EventJane;
    public GameObject NormalJane;
    public GameObject EventDujon;
    public GameObject NormalDujon;

    public GameObject EventRobertBranding;
    public GameObject EventDrSphinxBranding;
    public GameObject EventJaneBranding;

    private bool teleport;
    public static bool watchedBranding;


    public static List <Vector3> locations = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        //Initialization of instance
        instance = this;
        teleport = true;

        watchedBranding = false;

        //Initialization of character
        player_character = GameObject.Find("Player").GetComponent<Player_Movement>();

        EventRobert = GameObject.Find("EventRobert");
        NormalRobert = GameObject.Find("Robert");
        EventGutsy = GameObject.Find("EventGutsy");
        NormalGutsy = GameObject.Find("Gutsy");
        EventDrSphinx = GameObject.Find("EventDrSphinx");
        NormalDrSphinx = GameObject.Find("Dr Sphinx");
        EventStefano = GameObject.Find("EventStefano");
        NormalStefano = GameObject.Find("Stefano");
        EventSyed = GameObject.Find("EventSyed");
        NormalSyed = GameObject.Find("Syed");
        EventJane = GameObject.Find("EventJane");
        NormalJane = GameObject.Find("Jane");
        EventDujon = GameObject.Find("EventDujon");
        NormalDujon = GameObject.Find("Dujon");
        EventRobertBranding = GameObject.Find("EventRobertBranding");
        EventDrSphinxBranding = GameObject.Find("EventDrSphinxBranding");
        EventJaneBranding = GameObject.Find("EventJaneBranding");

        selfAssessment = new Skill("Leadership", PlayerPrefs.GetInt("LeadershipLevel"));
        Entre_thinking = new Skill("Teamwork", PlayerPrefs.GetInt("TeamworkLevel"));
        SAF = new Skill("Technology", PlayerPrefs.GetInt("TechnologyLevel"));
        Brand = new Skill("Professionalism", PlayerPrefs.GetInt("ProfessionalismLevel"));
        PFP = new Skill("Communication", PlayerPrefs.GetInt("CommunicationLevel"));
        Under_Need = new Skill("Critical Thinking", PlayerPrefs.GetInt("CritThinkingLevel"));
        //job skills added here -Rachel
        HumanResources = new Skill("Human Resources", 0);
        IT = new Skill("IT", 0);
        SoftwareDevelopment = new Skill("Software Development", 0);

        Name = PlayerPrefs.GetString("PlayerName");

        SceneChange.Initialize();
        SceneChange.PostSceneChange();

        /*
        if (SceneChange.initialized == false)
        {
            //SceneChange.playerLoc = gameObject.GetComponent<InGameMenu>().player.transform.position;
            SceneChange.currency = Player_Inventory.instance.currency;
            SceneChange.items = Player_Inventory.instance.items;
            SceneChange.initialized = true;
        }
        else
        {
            //gameObject.GetComponent<InGameMenu>().player.transform.position = SceneChange.playerLoc;
            Player_Inventory.instance.currency = SceneChange.currency;
            Player_Inventory.instance.items = SceneChange.items;

        }
        */
        update_job();
    }

    public void update_job() 
    {
        if (JobText != null)
        {
            if (Resume.current_job != null)
            {
                JobText.text = "Current Job: \n" + Resume.current_job.Title + " at " + Resume.current_job.Company;
            }
            else
            {
                JobText.text = "Current Job: \n" + "None";
            }
        }
    }

    // Update is called once per frame
    void Update()

        
    {
        
        // Update the inventory screen after the scene reverts back to the world scene.
        SceneChange.RefreshInventoryUI();
        //Player_Inventory.instance.UI.update_currency(Player_Inventory.instance.currency);
        //Player_Inventory.instance.UI.Update_UI();

       // CalendarText.text = "Week: " + Calendar.GetCurrentWeek() + " - " + Calendar.GetCurrentDay() + "\n";
        //CalendarText.text += "Actions Left: " + (2 - Calendar.GetSignificationActions());

        if(watchedBranding)
        {
            Debug.Log("The branding video was watched");
        }
        
        if(NormalRobert != null && EventRobert != null && EventGutsy != null && NormalGutsy != null
            && EventDrSphinx != null && NormalDrSphinx != null && EventStefano != null &&
            NormalStefano != null && EventSyed != null && NormalSyed != null && EventJane != null
            && NormalJane != null && EventDujon != null && NormalDujon != null && EventRobertBranding != null
            && EventDrSphinxBranding != null && EventJaneBranding != null)
        {

            if (Calendar.GetCurrentDayIndex() == Calendar.DayOfEvent)
            {
                SeminarGuy.SetActive(false);
                //EventRobert.SetActive(true);
                NormalRobert.SetActive(false);
                //EventGutsy.SetActive(true);
                //NormalGutsy.SetActive(false);
                //EventDrSphinx.SetActive(true);
                NormalDrSphinx.SetActive(false);
               // EventStefano.SetActive(true);
               // NormalStefano.SetActive(false);
                //EventSyed.SetActive(true);
                //NormalSyed.SetActive(false);
                //EventJane.SetActive(true);
                NormalJane.SetActive(false);
                //EventDujon.SetActive(true);
                //NormalDujon.SetActive(false);
                if(watchedBranding)
                {
                    EventRobertBranding.SetActive(true);
                    EventDrSphinxBranding.SetActive(true);
                    EventJaneBranding.SetActive(true);
                    EventRobert.SetActive(false);
                    EventDrSphinx.SetActive(false);
                    EventJane.SetActive(false);
                }
                else
                {
                    EventRobertBranding.SetActive(false);
                    EventDrSphinxBranding.SetActive(false);
                    EventJaneBranding.SetActive(false);
                    EventRobert.SetActive(true);
                    EventDrSphinx.SetActive(true);
                    EventJane.SetActive(true);
                }
            }
            else
            {
                SeminarGuy.SetActive(true);
                EventRobert.SetActive(false);
                NormalRobert.SetActive(true);
                EventGutsy.SetActive(false);
                NormalGutsy.SetActive(true);
                EventDrSphinx.SetActive(false);
                NormalDrSphinx.SetActive(true);
                EventStefano.SetActive(false);
                NormalStefano.SetActive(true);
                EventSyed.SetActive(false);
                NormalSyed.SetActive(true);
                EventJane.SetActive(false);
                NormalJane.SetActive(true);
                EventDujon.SetActive(false);
                NormalDujon.SetActive(true);
                EventRobertBranding.SetActive(false);
                EventDrSphinxBranding.SetActive(false);
                EventJaneBranding.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Something is wrong with one of the NPCs");
        }

        if (SeminarGuy != null && StartInterviewGuy != null) 
        {
            if (Calendar.GetCurrentDayIndex() == Calendar.DayOfInterview) // this is just for the first interview for now
            {
                SeminarGuy.SetActive(false);
                StartInterviewGuy.SetActive(true);
                current_objective = StartInterviewGuy;
                float offset = -1;
                if (teleport)
                {
                    player_character.gameObject.transform.position = new Vector3(StartInterviewGuy.transform.position.x, StartInterviewGuy.transform.position.y + offset, player_character.gameObject.transform.position.z);
                    teleport = false;
                }
                //((TalkToNPC)SeminarGuy).reset_message_count();
                //GameObject o = GameObject.Find("Robert").GetComponent<Button>()
            }
            else
            {
                // Don't show Robert the seminar guy if the player has a job
                if (Resume.current_job == null && Calendar.CurrentDay != Calendar.DayOfEvent)
                {

                    SeminarGuy.SetActive(true);

                    // If the player misses the interview,  Show Robert the Seminar guy again but start with the inteview proposa;
                    if (Calendar.GetCurrentWeek() + Calendar.CurrentDay > Calendar.GetCurrentWeek() + Calendar.DayOfInterview 
                        && Calendar.DayOfInterview != -1)
                    { 

                        TalkToNPC sg = GameObject.Find("Robert").GetComponent<TalkToNPC>();
                        if (sg.messageCount >= sg.messages.Count()-1)
                           sg.set_message_count(11);
                    }
                }
                else
                {
                    // This is required since his defailt state is enable and after a scene change he will be enabled.
                    SeminarGuy.SetActive(false);
                }
                    
                StartInterviewGuy.SetActive(false);
            }
        }
    }
    public void ShowOptions() {
        GameObject.Find("OptionsPanel").transform.localScale = new Vector3(0.5f, 0.5f, 1);
    }
    public void HideOptions() {
        GameObject.Find("OptionsPanel").transform.localScale = new Vector3(0, 0, 0);
    }
    public void ToggleFullScreen() {
        Screen.fullScreen = !Screen.fullScreen;
    }
    //Changed by Sly for Internet stability check
    public void navigate_to_url(string url) 
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) {
            GameObject.Find("NetworkErrPanel").transform.localScale = new Vector3(0.25f, 0.25f, 1);
        }
        //Check if the device can reach the internet via a carrier data network
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork) {
            Application.OpenURL(url);
        }
        //Check if the device can reach the internet via a LAN
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) {
            Application.OpenURL(url);
        }
        
    }
    public void HideNetworkErr() {
        GameObject.Find("NetworkErrPanel").transform.localScale = new Vector3(0, 0, 0);
    }

    public Skill highest_skill(int place) 
    {
        List<Skill> skills = new List<Skill>();

        skills.Add(selfAssessment);

        skills.Add(Entre_thinking);

        skills.Add(SAF);

        skills.Add(Brand);

        skills.Add(PFP);

        skills.Add(Under_Need);
        //job skills added here -Rachel
        skills.Add(IT);

        skills.Add(SoftwareDevelopment);

        skills.Add(HumanResources);


        List<Skill> SortedList = skills.OrderBy(o => o.Level).ToList();

        int index = SortedList.Count - place;

        if (index >= SortedList.Count)
        {
            return SortedList[SortedList.Count - 1];
        }
        else if (index < 0)
        {
            return SortedList[0];
        }
        else
        {
            return SortedList[index];
        }
    }

    public string prev_state = "Normal";

    public string game_state = "Normal";

    public GameObject current_objective;

    public int story_progress = 0;

    public void Set_Objective(GameObject o)
    {
        current_objective = o;
    }
    public void Clear_Objective()
    {
        current_objective = null;
    }

    public void reset_game_state() 
    {
        change_game_state(prev_state);
    }

    public void change_game_state(string state) 
    {
        prev_state = game_state;

        game_state = state;

        switch (state) 
        {
            case ("Dialogue"):

                if (InventoryUI.instance != null) 
                {
                    InventoryUI.instance.close_inventory();
                }

                break;

            case ("Inventory"):

                if (prev_state == "Inventory")
                {
                    change_game_state("Normal");
                }
                else 
                {
                    player_character.canMove = false;
                }

                break;

            case ("Normal"):

                player_character.canMove = true;

                break;

            default:

                break;
        }
    }
}
