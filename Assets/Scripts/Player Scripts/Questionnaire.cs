/* 
 * This script is used to determine the player's starting stats from a set of questions they pick
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Questionnaire : MonoBehaviour
{

    Skill SelfAssessment = new Skill("Self Assessment", 0);
    Skill EntreprenurealMindset = new Skill("Entreprenureal Mindset", 0);
    Skill SAF = new Skill("Survive, Adapt, Flourish", 0);
    Skill PersonalBrand = new Skill("Personal Brand", 0);
    Skill PFP = new Skill("PFP", 0);
    Skill UnderservedNeed = new Skill("Underserved Need", 0);
    Skill WrittenForm = new Skill("Written Form", 0);
    Skill OralForm = new Skill("Oral Form", 0);
    Skill SocialMedia = new Skill("Social Media", 0);

    /*
    float agreeLeadership = 0;
    float agreeTeamwork = 0;
    float agreeTechnology = 0;
    float agreeProfessionalism = 0;
    float agreeCommunication = 0;
    float agreeCritThinking = 0;
    */
    Canvas canvas;
    public GameObject SkillsQuestion;
    public GameObject SkipQuestion;
    public GameObject SkillEntry;
    public GameObject Summary;
    AudioSource click;

    InputField NameInputField;
    TextMeshProUGUI NameTxt;
    Job CurrentJob;
    Text Question;
    int QuestionInt = 0;


    //for Summary Display
    Text SelfAssessmentTxt;
    Text EntreprenurealMindsetTxt;
    Text SAFTxt;
    Text PersonalBrandTxt;
    Text PFPTxt;
    Text UnderservedNeedTxt;
    Text WrittenFormTxt;
    Text OralFormTxt;
    Text SocialMediaTxt;
    Text SkillsTxt;
    //Text PointsTxt;
    Toggle HSDTog;
    Toggle GEDTog;
    Toggle TwoYrDegreeTog;
    Toggle FourYrDegreeTog;
    Toggle TourofDutyTog;
    Toggle VolunteerTog;
    InputField TourInput;
    InputField TourBranchInput;
    InputField VolunteerInput;

    //This function is called once after the player enters their name to remove characters that aren't allowed in file names.
    //This funciton also removes characters to the in-game player name and can be checked by talking to Rob in the center of the world map at game start.
    //created by Don Murphy
    public static string CleanString(string dirtyString)
    {
        string removeChars = "\"!@#$%^&*()_+={}[]|:;<>?/~`,\\";
        string result = dirtyString;

        foreach (char c in removeChars)
        {
            result = result.Replace(c.ToString(), string.Empty);
        }

        return result;
    }

    public static string PrintStartingStat(string choice)
    {
        if (choice == "self assessment")
        {
            return PlayerPrefs.GetInt("SelfAssessmentLevel").ToString();
        }
        else if (choice == "entreprenureal mindset")
        {
            return PlayerPrefs.GetInt("EntreprenurealMindsetLevel").ToString();
        }
        else if (choice == "SAF")
        {
            return PlayerPrefs.GetInt("SAFLevel").ToString();
        }
        else if (choice == "personal brand")
        {
            return PlayerPrefs.GetInt("PersonalBrandLevel").ToString();
        }
        else if (choice == "PFP")
        {
            return PlayerPrefs.GetInt("PFPLevel").ToString();
        }
        else if (choice == "underserved need")
        {
            return PlayerPrefs.GetInt("UnderservedNeedLevel").ToString();
        }
        else if (choice == "written form")
        {
            return PlayerPrefs.GetInt("WrittenFormLevel").ToString();
        }
        else if (choice == "oral form")
        {
            return PlayerPrefs.GetInt("OralFormLevel").ToString();
        }
        else if (choice == "social media")
        {
            return PlayerPrefs.GetInt("SocialMediaLevel").ToString();
        }
        else
        {
            return "Error occured. Check Questionnaire.cs PrintStartingStat()";
        }
    }

    /*
    //Questionnaire Questions
    const string Leadership1 = "I set high standards for myself, and expect others to meet those standards too";
    const string Leadership2 = "I’m good at helping people to change direction";
    const string Teamwork1 = "I like to make sure that everyone understands where we’re going";
    const string Teamwork2 = "I enjoy working in a team environment";
    const string Tech1 = "I am able to use technology, such as computers and mobile devices, well";
    const string Tech2 = "I know how to use basic word processing applications";
    const string Professional1 = "I am organized and have accountability to myself, my colleagues, and the organization";
    const string Professional2 = "I typically arrive on time and am prepared";
    const string Communication1 = "I am able to succinctly instruct, inform, or explain things to people";
    const string Communication2 = "I understand the importance of and demonstrate verbal, written, and non-verbal abilities";
    const string CritThink1 = "I enjoy solving problems and puzzles";
    const string CritThink2 = "I like to observe and reflect on my observations";

  */

    // Start is called before the first frame update
    void Start()
    {
        click = GetComponent<AudioSource>();
        canvas = GameObject.Find("CharacterCreation").GetComponent<Canvas>();
        SkipQuestion.SetActive(true);
        SkillsQuestion.SetActive(false);
        SkillEntry.SetActive(false);
        Summary.SetActive(false);

        Button btnCancel = SkillsQuestion.transform.Find("MostlyAgree").GetComponent<Button>();
        btnCancel.Select();

        SelfAssessmentTxt = Summary.transform.Find("SelfAssessmentTxt").GetComponent<Text>();
        EntreprenurealMindsetTxt = Summary.transform.Find("EntreprenurealMindsetTxt").GetComponent<Text>();
        SAFTxt = Summary.transform.Find("SAFTxt").GetComponent<Text>();
        PersonalBrandTxt = Summary.transform.Find("PersonalBrandTxt").GetComponent<Text>();
        PFPTxt = Summary.transform.Find("PFPTxt").GetComponent<Text>();
        UnderservedNeedTxt = Summary.transform.Find("UnderservedNeedTxt").GetComponent<Text>();
        WrittenFormTxt = Summary.transform.Find("WrittenFormTxt").GetComponent<Text>();
        OralFormTxt = Summary.transform.Find("OralFormTxt").GetComponent<Text>();
        SocialMediaTxt = Summary.transform.Find("SocialMediaTxt").GetComponent<Text>();

        //PointsTxt = canvas.transform.GetChild(2).GetComponent<Text>();

        NameInputField = SkillEntry.transform.Find("NameInput").GetComponent<InputField>();
        NameTxt = Summary.transform.Find("NameTxt").GetComponent<TextMeshProUGUI>();
        Question = SkillsQuestion.transform.Find("Question").GetComponent<Text>();

        SkillsTxt = Summary.transform.Find("SkillsTxt").GetComponent<Text>();
        HSDTog = SkillEntry.transform.Find("HSDTog").GetComponent<Toggle>();
        GEDTog = SkillEntry.transform.Find("GEDTog").GetComponent<Toggle>(); 
        TwoYrDegreeTog = SkillEntry.transform.Find("TwoYrDegreeTog").GetComponent<Toggle>();
        FourYrDegreeTog = SkillEntry.transform.Find("FourYrDegreeTog").GetComponent<Toggle>();
        TourofDutyTog = SkillEntry.transform.Find("TourofDutyTog").GetComponent<Toggle>();
        VolunteerTog = SkillEntry.transform.Find("VolunteerTog").GetComponent<Toggle>();
        TourInput = SkillEntry.transform.Find("TourInput").GetComponent<InputField>();
        TourBranchInput = SkillEntry.transform.Find("TourBranchInput").GetComponent<InputField>();
        VolunteerInput = SkillEntry.transform.Find("VolunteerInput").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
        SelfAssessmentTxt.text = "Self Assessment: " + SelfAssessment.Level;
        EntreprenurealMindsetTxt.text = "Entreprenureal Mindset: " + EntreprenurealMindset.Level;
        SAFTxt.text = "Survive, Adapt, Flourish: " + SAF.Level;
        PersonalBrandTxt.text = "PersonalBrand: " + PersonalBrand.Level;
        PFPTxt.text = "Personal Value Proposition: " + PFP.Level;
        UnderservedNeedTxt.text = "Underserved Need: " + UnderservedNeed.Level;
        WrittenFormTxt.text = "Written Form: " + WrittenForm.Level;
        OralFormTxt.text = "Oral Form: " + OralForm.Level;
        SocialMediaTxt.text = "Social Media: " + SocialMedia.Level;
        //PointsTxt.text = "Points Left: " + Points;

    }

    public void SaveStats()
    {
        print("debug: start saveStats");

      
        if (NameInputField.text == "")
        {
            NameInputField.text = "Player";
        }
        PlayerPrefs.SetInt("SelfAssessmentLevel", SelfAssessment.Level);
        PlayerPrefs.SetInt("EntreprenurealMindsetLevel", EntreprenurealMindset.Level);
        PlayerPrefs.SetInt("SAFLevel", SAF.Level);
        PlayerPrefs.SetInt("PersonalBrandLevel", PersonalBrand.Level);
        PlayerPrefs.SetInt("PFPLevel", PFP.Level);
        PlayerPrefs.SetInt("UnderservedNeedLevel", UnderservedNeed.Level);
        PlayerPrefs.SetInt("WrittenFormLevel", WrittenForm.Level);
        PlayerPrefs.SetInt("OralFormLevel", OralForm.Level);
        PlayerPrefs.SetInt("SocialMediaLevel", SocialMedia.Level);
        PlayerPrefs.SetString("PlayerName", CleanString(NameInputField.text)); //change by Don Murphy to clean name input
        PlayerPrefs.Save();
        print("Saved Player Data");
        SceneManager.LoadScene("World Map");
    }

    public void PlayRandomClickSound()
    {
        click.pitch = Random.Range(0.75f, 1f);
        click.Play();
    }
    /*
    public void MostlyAgree()
    {
        //add 1 to the tally for that stat
        switch (Question.text)
        {
            case Leadership1:
                agreeLeadership++;
                print("debug: agreeleadership1 /n");
                print(agreeLeadership + Leadership.Level);
                break;
            case Teamwork1:
                agreeTeamwork++;
                print("debug: agreeteamwork1 /n");
                print(agreeTeamwork + Teamwork.Level);
                break;
            case Tech1:
                agreeTechnology++;
                print("debug: tech1 /n");
                print(agreeTechnology + Technology.Level);
                break;
            case Professional1:
                agreeProfessionalism++;
                print("debug: agreeprofessional1");
                print(agreeProfessionalism + Professionalism.Level);
                break;
            case Communication1:
                agreeCommunication++;
                print("debug: agreecommunication1");
                print(agreeCommunication + Communication.Level);
                break;
            case CritThink1:
                agreeCritThinking++;
                print("debug: agreecritthink1");
                print(agreeCritThinking + CritThinking.Level);
                break;
            case Leadership2:
                agreeLeadership++;
                print("debug: agreeleadership2");
                print(agreeLeadership);
                break;
            case Teamwork2:
                agreeTeamwork++;
                break;
            case Tech2:
                agreeTechnology++;
                break;
            case Professional2:
                agreeProfessionalism++;
                break;
            case Communication2:
                agreeCommunication++;
                break;
            case CritThink2:
                agreeCritThinking++;
                break;
        }
        QuestionInt++;
        SetQuestion();
        click.pitch = 1;
        click.Play();
    }*/
    /*
    public void MostlyDisagree()
    {
        //add 0 then move to next screen
        print("debug: disagree");
        QuestionInt++;
        SetQuestion();
        click.pitch = 0.75f;
        click.Play();
    }*/

    /* public void CalculateStats()          // temporarily commented out to use later for the new Nine Metrics by Abdallah Abulaban
     {

         int points = 8;
         //if they have the same stats for all of them then give each 1 more points
         if (agreeLeadership == agreeTeamwork
             && agreeTeamwork == agreeTechnology
             && agreeTechnology == agreeProfessionalism
             && agreeProfessionalism == agreeCommunication
             && agreeCommunication == agreeCritThinking)
         {
             Leadership.Level = 0;
             Teamwork.Level = 0;
             Technology.Level = 0;
             Professionalism.Level = 0;
             Communication.Level = 0;
             CritThinking.Level = 0;
             points = 2;
             while (points > 0)
             {
                 points--;
                 RandomStatAssign();
             }
         }
         else
         {
             float total = agreeLeadership + agreeTeamwork + agreeTechnology + agreeProfessionalism + agreeCommunication + agreeCritThinking;

             Leadership.Level += Mathf.RoundToInt((agreeLeadership / total) * points);
             Teamwork.Level += Mathf.RoundToInt(agreeTeamwork / total * points);
             Technology.Level += Mathf.RoundToInt(agreeTechnology / total * points);
             Professionalism.Level += Mathf.RoundToInt(agreeProfessionalism / total * points);
             Communication.Level += Mathf.RoundToInt(agreeCommunication / total * points);
             CritThinking.Level += Mathf.RoundToInt(agreeCritThinking / total * points);
         }
        /* points = points - Leadership.Level - Teamwork.Level - Technology.Level - Communication.Level - Professionalism.Level - CritThinking.Level;
         while(points > 0)
         {
             points--;
             RandomStatAssign();
         }

     }*/
    
    public void FinishWithoutQuestionnaire()    // used for the sticky note in the main page
    {
        SkillsQuestion.SetActive(false);
        SkillEntry.SetActive(true);
        Summary.SetActive(false);
    }
    /*
    public void SetQuestion()
    {
        string[] Questions = new string[12] {Leadership1, Leadership2, Teamwork1, Teamwork2, Tech1, Tech2,
            Professional1, Professional2, Communication1, Communication2, CritThink1, CritThink2};

        if (QuestionInt < 12)//less than number of questions
        {
            Question.text = (Questions[QuestionInt].ToString());
        }
        else
        {
            //CalculateStats();                 // temporarily commented out to use later for the new Nine Metrics by Abdallah Abulaban
            //disable Questionnaire
            SkillsQuestion.SetActive(false);
            //enable skill entry page
            SkillEntry.SetActive(true);

            NameInputField.Select();
        }
    }
    */
    public void SaveSkills()
    {
        if (HSDTog.isOn)
        {
            SkillsTxt.text += "\nHigh School Diploma";
            Resume.HighSchoolDiplomaBool = true;
        }
        else if(GEDTog.isOn)
        {
            SkillsTxt.text += "\nGED";
            Resume.GEDBool = true;
        }

        if (TwoYrDegreeTog.isOn)
        {
            SkillsTxt.text += "\n2 Year Degree";
            Resume.TwoYearDegree = true;
        }

        if (FourYrDegreeTog.isOn)
        {
            SkillsTxt.text += "\n4 Year Degree";
            Resume.FourYearDegree = true;
        }

        if (TourofDutyTog.isOn)
        {
            SkillsTxt.text += "\nTour of Duty " + TourInput.text + "\nBranch: " + TourBranchInput.text;
            Resume.TourofDutyBool = true;
            Resume.TourName = TourInput.text;
            Resume.TourBranch = TourBranchInput.text;
        }

        if (VolunteerTog.isOn)
        {
            SkillsTxt.text += "\nVolunteer as " + VolunteerInput.text;
            Resume.VolunteerBool = true;
            Resume.VolunteerTitle = VolunteerInput.text;
        }
        
        

        DisplaySkills();
    }

    public void DisplaySkills()
    {
        //save skills entered


        //disable skill entry page
        SkillEntry.SetActive(false);
        //enable summary page
        Summary.SetActive(true);
        Button btnCancel = Summary.transform.Find("FinishButton2").GetComponent<Button>();
        btnCancel.Select();

        NameTxt.text = NameInputField.text.ToString();
        if (NameTxt.text == "")//if no name provided
            NameTxt.text = "Player";

    }

    
}
