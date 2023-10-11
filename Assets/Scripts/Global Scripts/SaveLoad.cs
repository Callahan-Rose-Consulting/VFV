using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SaveLoad : MonoBehaviour
{
    //private PlayerData Player = new PlayerData();

    private PlayerData data = new PlayerData();
    //public static GameManager instance;
    [SerializeField] public static Vector3 LastPlayerLoc=new Vector3();
    [SerializeField] private string sceneName;

    
    //Canvas UICanvas;

    [SerializeField]
    public static Vector3 lastPlayerLoc=Vector3.zero;
    public void SaveIntoJson()
    {
        string playerjson = JsonUtility.ToJson(data);
        Debug.Log("Wrote save file to "+ Application.persistentDataPath + "/saveData.json");
        System.IO.File.WriteAllText(Application.persistentDataPath + "/saveData.json", playerjson);
    }

    [System.Serializable]
    public struct PlayerData
    {
        // PlayerStats.cs
        public int leadership; 
        public int teamwork;
        public int technology;
        public int professionalism;
        public int communication;
        public int critThinking;
        public string playerName;

        // Player_Inventory.cs
        public List<InvItm> items;
        public float currency;
        public float totalEarnings;

        // Calendar.cs
        public int WeekNumber;
        public int CurrentDay;
        public int SignificantActions;
        public bool InterviewAccepted;
        public int DayOfInterview;

        // Work_Event_Manager.cs
        public int current_job_index;

        // Resume.cs
        public List<Experience> ResumeExperiences1;
        public List<Experience> ResumeExperiences2;
        public List<Experience> ResumeExperiences3;
        public Job current_job;
        public int current_job_progress;
        public Job dream_job;
        public Job failed_interview_job;
        public  bool HighSchoolDiplomaBool;
        public  bool GEDBool;
        public  bool TwoYearDegree;
        public  bool FourYearDegree;
        public  bool TourofDutyBool;
        public  string TourName;
        public  string TourBranch;
        public  bool VolunteerBool1;
        public  string VolunteerTitle1;
        public bool VolunteerBool2;
        public string VolunteerTitle2;
        public bool VolunteerBool3;
        public string VolunteerTitle3;

        // TextSpeedSet.cs
        public float TextSpeed;
        public int TextSpeedIndex;

        // DialogVolumeScript.cs
        public float DialogVolume;

        // MasterVolumeScript.cs
        public float MasterVolumne;

        // MusicVolumeScripts.cs
        public float MusicVolume;

        // SFXVolumeScript.cs
        public float SFXVolume;

        // Misc
        public Vector3 playerLoc;
        public string sceneName;
       
    }

    public void saveGame()
    {
        data.items = new List<InvItm>();

        // PlayerStats.cs
        data.playerName = PlayerPrefs.GetString("PlayerName");
        data.leadership = PlayerPrefs.GetInt("LeadershipLevel");
        data.teamwork = PlayerPrefs.GetInt("TeamworkLevel");
        data.technology = PlayerPrefs.GetInt("TechnologyLevel");
        data.professionalism = PlayerPrefs.GetInt("ProfessionalismLevel");
        data.communication = PlayerPrefs.GetInt("CommunicationLevel");
        data.critThinking = PlayerPrefs.GetInt("CritThinkingLevel");

        // Player_Inventory.cs
        foreach (Inventory_Item li in Player_Inventory.instance.items) {
            InvItm a = new InvItm(li.name,
                            li.value,
                            li.icon.name,
                            li.isDefaultItem,
                            li.Use_Dialogue,
                            li.details);

            data.items.Add(a);
        }

        data.currency = Player_Inventory.instance.currency;
        data.totalEarnings = Player_Inventory.instance.totalEarnings;

        //Calendar.cs
        data.WeekNumber = Calendar.GetCurrentWeek();
        data.CurrentDay = Calendar.GetCurrentDayIndex();
        data.SignificantActions = Calendar.GetSignificationActions();
        data.InterviewAccepted = Calendar.InterviewAccepted;
        data.DayOfInterview = Calendar.DayOfInterview;
        
        //data.playerLoc = LastPlayerLoc;

        // Work_Event_Manager.cs
        data.current_job_index = Work_Event_Manager.instance.current_job_index;

        // Resume.cs
        data.ResumeExperiences1 = Resume.ResumeExperiences1;
        data.ResumeExperiences2 = Resume.ResumeExperiences2;
        data.ResumeExperiences3 = Resume.ResumeExperiences3;
        data.current_job_progress = Resume.current_job_progress;
        data.current_job = Resume.current_job;
        data.dream_job = Resume.dream_job;
        data.failed_interview_job = Resume.failed_interview_job;
        data.HighSchoolDiplomaBool = Resume.HighSchoolDiplomaBool;
        data.GEDBool = Resume.GEDBool;
        data.TwoYearDegree = Resume.TwoYearDegree;
        data.FourYearDegree = Resume.FourYearDegree;
        data.TourofDutyBool = Resume.TourofDutyBool;
        data.TourName = Resume.TourName;
        data.VolunteerBool1 = Resume.VolunteerBool1;
        data.VolunteerTitle1 = Resume.VolunteerTitle1;
        data.VolunteerBool2 = Resume.VolunteerBool2;
        data.VolunteerTitle2 = Resume.VolunteerTitle2;
        data.VolunteerBool3 = Resume.VolunteerBool3;
        data.VolunteerTitle3 = Resume.VolunteerTitle3;

        // TextSpeedSet.cs
        data.TextSpeed = PlayerPrefs.GetFloat("TextSpeed");
        data.TextSpeedIndex = PlayerPrefs.GetInt("TextSpeedIndex");

        // DialogVolumeScript.cs
        data.DialogVolume = PlayerPrefs.GetFloat("DialogVolume");

        // MasterVolumeScript.cs
        data.MasterVolumne = PlayerPrefs.GetFloat("MasterVolume");

        // MusicVolumeScripts.cs
        data.MusicVolume = PlayerPrefs.GetFloat("MusicVolume");

        // SFXVolumeScript.cs
        data.SFXVolume = PlayerPrefs.GetFloat("SFXVolume");

        // Misc
        data.sceneName = "World Map";
        data.playerLoc = gameObject.GetComponent<InGameMenu>().player.transform.position;

        SaveIntoJson();
    }

    public void loadGame()
    {
        // Load save file
        var path = Application.persistentDataPath + "/saveData.json";
        var jsonFile = File.ReadAllText(path);
        data = JsonUtility.FromJson<PlayerData>(jsonFile);

        // Load the scene where that data was saved. (World)
        //SceneManager.LoadScene(data.sceneName);

        // Initialize Scene
        // PlayerStats.cs
        PlayerPrefs.SetString("PlayerName", data.playerName);
        PlayerPrefs.SetInt("LeadershipLevel", data.leadership);
        PlayerPrefs.SetInt("TeamworkLevel", data.teamwork);
        PlayerPrefs.SetInt("TechnologyLevel", data.technology);
        PlayerPrefs.SetInt("ProfessionalismLevel", data.professionalism);
        PlayerPrefs.SetInt("CommunicationLevel", data.communication);
        PlayerPrefs.SetInt("CritThinkingLevel", data.critThinking);

        // Player_Inventory.cs

        if (Player_Inventory.instance != null)
        {
            Player_Inventory.instance.items.Clear();
            foreach (InvItm li in data.items) {
                Inventory_Item ii = Resources.Load<Inventory_Item>("Square");
                //Inventory_Item ii = new Inventory_Item();
                ii.name = li.name;
                ii.icon = Resources.Load<Sprite>(li.icon);
                ii.isDefaultItem = li.isDefaultItem;
                ii.value = li.value;
                
                Player_Inventory.instance.Add(ii);
            }
            Player_Inventory.instance.currency = data.currency;
            Player_Inventory.instance.totalEarnings = data.totalEarnings;
            Player_Inventory.instance.UI.update_currency(Player_Inventory.instance.currency);
            Player_Inventory.instance.UI.Update_UI();
        }
        

        //Calendar.cs
        Calendar.SetCurrentWeek(data.WeekNumber);
        Calendar.CurrentDay = data.CurrentDay;
        Calendar.SetSignificationActions(data.SignificantActions);
        Calendar.InterviewAccepted = data.InterviewAccepted;
        Calendar.DayOfInterview= data.DayOfInterview;

        // Work_Event_Manager.cs
        Work_Event_Manager.instance.current_job_index = data.current_job_index;

        // Resume.cs
        Resume.ResumeExperiences1.Clear();
        Resume.ResumeExperiences2.Clear();
        Resume.ResumeExperiences3.Clear();
        Resume.ResumeExperiences1 = data.ResumeExperiences1;
        Resume.ResumeExperiences2 = data.ResumeExperiences2;
        Resume.ResumeExperiences3 = data.ResumeExperiences3;
        Resume.current_job_progress = data.current_job_progress;
        Resume.current_job = (data.current_job.Title.Length == 0 ? null: data.current_job);
        Resume.dream_job = (data.dream_job.Title.Length == 0 ? null : data.dream_job);
        Resume.failed_interview_job = data.failed_interview_job;
        Resume.HighSchoolDiplomaBool = data.HighSchoolDiplomaBool;
        Resume.GEDBool = data.GEDBool;
        Resume.TwoYearDegree = data.TwoYearDegree;
        Resume.FourYearDegree = data.FourYearDegree;
        Resume.TourofDutyBool = data.TourofDutyBool;
        Resume.TourName = data.TourName;
        Resume.VolunteerBool1 = data.VolunteerBool1;
        Resume.VolunteerTitle1 = data.VolunteerTitle1;
        Resume.VolunteerBool2 = data.VolunteerBool2;
        Resume.VolunteerTitle2 = data.VolunteerTitle2;
        Resume.VolunteerBool3 = data.VolunteerBool3;
        Resume.VolunteerTitle3 = data.VolunteerTitle3;

        // TextSpeedSet.cs
        PlayerPrefs.SetFloat("TextSpeed", data.TextSpeed);
        PlayerPrefs.SetInt("TextSpeedIndex", data.TextSpeedIndex);

        // DialogVolumeScript.cs
        PlayerPrefs.SetFloat("DialogVolume", data.DialogVolume);

        // MasterVolumeScript.cs
        PlayerPrefs.SetFloat("MasterVolume", data.MasterVolumne);

        // MusicVolumeScripts.cs
        PlayerPrefs.SetFloat("MusicVolume", data.MusicVolume);

        // SFXVolumeScript.cs
        PlayerPrefs.SetFloat("SFXVolume", data.SFXVolume);

        // Misc
        GameManager.instance.update_job();
       // transform.localPosition = data.playerLoc;

        gameObject.GetComponent<InGameMenu>().player.transform.position = data.playerLoc;
        //InventoryUI.instance.update_currency();

    }
}

