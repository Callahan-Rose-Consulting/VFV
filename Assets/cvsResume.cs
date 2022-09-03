using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.EventSystems;

// Get rid of the naming convention complaint and the " 'new' expression can be simplified " message.  It can't due to Unity.   
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "<Pending>")]
#pragma warning disable IDE0044 // Add readonly modifier

public class cvsResume : MonoBehaviour
{
    private enum enlist { DesignSkills, DesignResume, RateResume };

    // Variable for updating text in panels
    public GameObject JobOppertunityPanel;  // For enabling and disabling the Job Opportunity panel
    public GameObject ResumeDesignPanel;    // For enabling and disabling the Resume Design panel
    public GameObject RankAndReviewPanel;   // For enabling and disabling the Rank and ??? panel

    // Variable for updating text in panels
    public TextMeshProUGUI txtJobOpportunityBackStory;
    public TextMeshProUGUI txtJobOpportunityDetail;
    public TextMeshProUGUI txtSkillDescription;
    public TextMeshProUGUI txtResult;
    public TextMeshProUGUI txtThePercent;


    public GameObject SkillsTemplate; // Template for the button list
    public GameObject SkillsContent;  // Content of the skills list
    public GameObject ResumeContent;  // Content of the Resume list
    public GameObject RankContent;  // Content of the Ranking list

 
    public Dropdown.DropdownEvent onValueChanged;
    public List<Dropdown.OptionData> SkillsOptions = new List<Dropdown.OptionData>();
    public List<Dropdown.OptionData> ResumeOptions = new List<Dropdown.OptionData>();
    public List<Dropdown.OptionData> RankOptions = new List<Dropdown.OptionData>();

    private int SelectedSkillIndex = -1;

    private Color selectedColor = Color.yellow; 
    private Color normalColor = Color.white;
    private Color selectedTextColor = Color.white;
    private Color normalTextColor = Color.black;
    
    ResumeJob ApplyingFor;
    string oPlayerName;

    // Start is called before the first frame update
    void Start()
    {
        // Make the "Job Opportunity" panel visible then hide the "Resume Design" and "Rank and Review" panels.
        JobOppertunityPanel.SetActive(true);
        ResumeDesignPanel.SetActive(false);
        RankAndReviewPanel.SetActive(false);

        if (Resume.current_job == null)
        {
            ApplyingFor = PizzaJob;
        }
        else if (Resume.current_job == null)
        {
        }
        else
        {
            ApplyingFor = PizzaJob;
        }

        // Randomize the Resume items so the game is not predictable
        ApplyingFor.Randomize();

        // Ensure that the lists are emtpy before populating.
        ClearSkills(SkillsContent);
        ClearSkills(ResumeContent);
        ClearSkills(RankContent);

        AddSkills(ApplyingFor);

        // Set the text to be displayed in the initial screen
        txtJobOpportunityDetail.SetText(ApplyingFor.m_oJobDetail);
        txtJobOpportunityBackStory.SetText(ApplyingFor.m_oJobBackStory.Replace("<Player Name>", PlayerPrefs.GetString("PlayerName")));

        SelectedSkillIndex = 0;
        EnterSkillsOrResumeButton(SkillsContent, SelectedSkillIndex);
    }




    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            UpdateLists(SelectedSkillIndex);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            EnterSkillsOrResumeButton(SkillsContent, ++SelectedSkillIndex);
            //SkillsContent.transform.GetChild(SelectedSkillIndex).GetComponent<Button>().onClick.Invoke();
            //UnSelectButton();
        }
    }

    public void ResumeDesign()
    {
        JobOppertunityPanel.SetActive(false);
        ResumeDesignPanel.SetActive(true);
    }

    public void RankAndReview()
    {
        ResumeDesignPanel.SetActive(false);
        RankAndReviewPanel.SetActive(true);

        Transform Skill;
        Button b;
        int iCount = 0;
        int iSum = 0;
        for (int i = 0; i < RankContent.transform.childCount; i++)
        {
            Skill = RankContent.transform.GetChild(i);
            b = Skill.gameObject.GetComponent<Button>();
            if(b.IsActive())
            {
                iCount++;
                iSum += ApplyingFor.m_aoRI[i].m_iValue;
            }
        }

        txtThePercent.text = "Ranking Percentage: " + Math.Round((iSum / (iCount * 3.0))*100,2) +"%";
    }


    public void ReturnToGame()
    {
        SceneManager.LoadScene("World Map");
    }

    void AddSkills(ResumeJob p_oRJ)
    {
        var buttons = p_oRJ.GetItemNames()
            .Select(name => new Dropdown.OptionData(name))
            .ToList();

        AddSkills(SkillsOptions, buttons, SkillsContent, true, true, enlist.DesignSkills);
        AddSkills(ResumeOptions, buttons, ResumeContent, false, false, enlist.DesignResume);
        AddSkills(RankOptions, buttons, RankContent, false, true, enlist.RateResume);


    }

    private void AddSkills(List<Dropdown.OptionData> p_oListbox, List<Dropdown.OptionData> p_oItems, GameObject p_oContent, bool p_bActive, bool p_bEnabled, enlist p_enList)
    {
        foreach (var option in p_oItems)
        {
            var copy = Instantiate(SkillsTemplate);
            copy.transform.SetParent(p_oContent.transform);
            copy.transform.localPosition = Vector3.zero;
            copy.transform.localScale = Vector3.one;

            copy.SetActive(p_bActive);
            //copy.GetComponent<Button>().enabled = p_bEnabled;


            copy.GetComponentInChildren<TextMeshProUGUI>().text = option.text;

            int copyOfIndex = p_oListbox.Count;

            // Set event for what happens when the mouse enters the button
            EventTrigger enterTrigger = copy.gameObject.AddComponent<EventTrigger>();
            var pointerEnter = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            

            // Set event for what happens when the mouse exits the button
            EventTrigger exitTrigger = copy.gameObject.AddComponent<EventTrigger>();
            var pointerExit = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };

            switch (p_enList)
            {
                case enlist.DesignSkills:
                    copy.GetComponent<Button>().onClick.AddListener(() => { AddSelectedSkill(copyOfIndex); });

                    pointerEnter.callback.AddListener((e) => EnterSkillsOrResumeButton(p_oContent, copyOfIndex));
                    pointerExit.callback.AddListener((e) => ExitSkillsOrResumeButton(p_oContent, copyOfIndex));
                    break;
                case enlist.DesignResume:
                    copy.GetComponent<Button>().onClick.AddListener(() => { UnSelectButton(); });
                    pointerEnter.callback.AddListener((e) => EnterSkillsOrResumeButton(p_oContent, copyOfIndex));
                    pointerExit.callback.AddListener((e) => ExitSkillsOrResumeButton(p_oContent, copyOfIndex));
                    break;
                case enlist.RateResume:
                    copy.GetComponent<Button>().onClick.AddListener(() => { SelectedRating(copyOfIndex); });

                    pointerEnter.callback.AddListener((e) => EnterRateResumeButton(p_oContent, copyOfIndex));
                    pointerExit.callback.AddListener((e) => ExitRateResumeButton(p_oContent, copyOfIndex));
                    break;
            }

            enterTrigger.triggers.Add(pointerEnter);
            exitTrigger.triggers.Add(pointerExit);


            p_oListbox.Add(option);

        }
    }

    /* =[ Skills List Functionality ]==================================
     *  
     * All functionality related to maniputlation of the skills list
     * on the designer screen will be found below
     * 
     * ================================================================
     */ 

    private void AddSelectedSkill(int p_iIndex)
    {
        UpdateLists(p_iIndex);


        int iCount = 0;
        for (int i = 0; i < ResumeContent.transform.childCount; i++)
        {
            if (ResumeContent.transform.GetChild(i).gameObject.GetComponent<Button>().IsActive())
            {
                iCount++;
            }
        }


        if (iCount >= 5)
            GameObject.Find("btnReview").GetComponent<Button>().interactable = true; 
        else
            GameObject.Find("btnReview").GetComponent<Button>().interactable = false;


    }


    /*
     * Set the selected button color and then display the skill description
     * at the bottom of the screen
     */ 
    public void EnterSkillsOrResumeButton(GameObject p_oContent, int p_iIndex)
    {
        EnterButton(p_oContent, SkillsOptions.Count, p_iIndex);

        txtSkillDescription.SetText(ApplyingFor.GetDescription(p_iIndex));
    }

    /* 
     * Set the button color back to normal
     */
    public void ExitSkillsOrResumeButton(GameObject p_oContent, int p_iIndex)
    {
        ClearItem(p_oContent, p_iIndex);
    }

    /* =[ Resume List Functionality ]==================================
     *  
     * All functionality related to manipulation of the Selected Resume 
     * items list on the designer screen will be found below
     * 
     * ================================================================
     */

    private void UnSelectButton() => EventSystem.current.SetSelectedGameObject(null); 
   
    /*
     * Set the selected button color and then display the skill description
     * at the bottom of the screen
     */
    public void EnterRateResumeButton(GameObject p_oContent, int p_iIndex)
    {
        EnterButton(p_oContent, RankOptions.Count, p_iIndex);

        //txtResult.SetText(ApplyingFor.getDescription(p_iIndex));
    }

    /* 
     * Set the button color back to normal
     */
    public void ExitRateResumeButton(GameObject p_oContent, int p_iIndex) => ClearItem(p_oContent, p_iIndex);

    private void SelectedRating(int index)
    {
        UnSelectButton();
        txtResult.SetText(ApplyingFor.GetWhy(index));
    }


    public void ClearSkills(GameObject p_oContent)
    {
        foreach (Transform t in p_oContent.transform)
        {
            Destroy(t.gameObject);
        }

        SkillsOptions.Clear();
    }


    /* =[ Resume List Functionality ]==================================
     *  
     * All functionality common to all list buttons will be found below.
     * 
     * ================================================================
     */
    public void EnterButton(GameObject p_oContent, int p_iItemCount, int p_iIndex)
    {
        // There are times when leaving the button does not set the
        // background to normal so I clear them all then set the one
        // that the mouse is over.
        for (int iIndex = 0; iIndex < p_iItemCount; iIndex++)
        {
            ClearItem(p_oContent, iIndex);
        }

        SetItem(p_oContent, p_iIndex);

        txtSkillDescription.SetText(ApplyingFor.GetDescription(p_iIndex));
    }

    /* 
     * Set the button color back to normal
     */
    public void ExitButton(GameObject p_oContent, int p_iIndex)
    {
        ClearItem(p_oContent, p_iIndex);
    }
    private void SetItem(GameObject p_oContent, int p_iIndex)
    {
        if (p_iIndex < 0) return;

        SetButtonColor(p_oContent, p_iIndex, selectedColor, selectedTextColor);
    }

    private void ClearItem(GameObject p_oContent, int p_iIndex)
    {
        if (p_iIndex < 0) return;

        SetButtonColor(p_oContent, p_iIndex, normalColor, normalTextColor);
    }

    private void SetButtonColor(GameObject p_oContent, int p_iIndex, Color p_stbackground, Color p_stforeground)
    {
        var oButton = p_oContent.transform.GetChild(p_iIndex).GetComponent<Button>();

        ColorBlock stColorBlock = new ColorBlock()
        {
            normalColor = p_stbackground,
            colorMultiplier = oButton.colors.colorMultiplier,
            disabledColor = oButton.colors.disabledColor,
            fadeDuration = oButton.colors.fadeDuration,
            highlightedColor = p_stbackground,
            pressedColor = p_stbackground
        };

        oButton.colors = stColorBlock;
        oButton.GetComponentInChildren<TextMeshProUGUI>().color = p_stforeground;
    }

    /*
     *  Refresh lists after item movement.
     */
    private void UpdateLists(int index)
    {
        {
            int start = (int)Math.Floor((Double)(index / 3)) * 3;

            for (int i = start; i < start + 3; i++)
            {
                ShowInList(SkillsContent, i);
                HideInList(ResumeContent, i);
                HideInList(RankContent, i);
            }

            HideInList(SkillsContent, index);
            ShowInList(ResumeContent, index);
            ShowInList(RankContent, index);

            SelectedSkillIndex = index;
        }
    }


    private void ShowInList(GameObject p_oContent, int p_iIndex) => ItemState(p_oContent, p_iIndex, true);

    private void HideInList(GameObject p_oContent, int p_iIndex) => ItemState(p_oContent, p_iIndex, false);

    private void ItemState(GameObject p_oContent, int p_iIndex, bool p_bState) => p_oContent.transform.GetChild(p_iIndex).gameObject.SetActive(p_bState);

    class ResumeJob
    {
        public string m_oJobBackStory;
        public string m_oJobDetail;
        public ResumeItem[] m_aoRI;



        public ResumeJob(string p_oJobBackStory, string p_oJobDetail, ResumeItem[] p_aoRI) {
            m_oJobBackStory = p_oJobBackStory;
            m_oJobDetail = p_oJobDetail;
            m_aoRI = p_aoRI;
        }

        public string[] GetItemNames()
        {
            List<string> oButtons = new List<string>();

            foreach (ResumeItem i in m_aoRI)
            {
                oButtons.Add(i.m_oLabel);
            }

            return oButtons.ToArray();
        }
        public string GetDescription(int p_iIndex)
        {
            return m_aoRI[p_iIndex].m_oDescription;
        }

        public string GetWhy(int p_iIndex)
        {
            return m_aoRI[p_iIndex].m_oWhy;
        }

        public void Randomize()
        {
            for(int i=0; i < m_aoRI.Length; i+=3)
            {
                int[] ShuffleIndex = { i, i + 1, i + 2 };

                Reshuffle(ShuffleIndex);

                int[] aiValue = { m_aoRI[i].m_iValue, m_aoRI[i + 1].m_iValue,  m_aoRI[i + 2].m_iValue };
                string[] aoDescription = { m_aoRI[i].m_oDescription, m_aoRI[i + 1].m_oDescription, m_aoRI[i + 2].m_oDescription};
                string[] aooWhy = { m_aoRI[i].m_oWhy, m_aoRI[i + 1].m_oWhy, m_aoRI[i + 2].m_oWhy };

                m_aoRI[ShuffleIndex[0]].m_iValue = aiValue[0];
                m_aoRI[ShuffleIndex[1]].m_iValue = aiValue[1];
                m_aoRI[ShuffleIndex[2]].m_iValue = aiValue[2];

                m_aoRI[ShuffleIndex[0]].m_oDescription = aoDescription[0];
                m_aoRI[ShuffleIndex[1]].m_oDescription = aoDescription[1];
                m_aoRI[ShuffleIndex[2]].m_oDescription = aoDescription[2];

                m_aoRI[ShuffleIndex[0]].m_oWhy = aooWhy[0];
                m_aoRI[ShuffleIndex[1]].m_oWhy = aooWhy[1];
                m_aoRI[ShuffleIndex[2]].m_oWhy = aooWhy[2];
            }
        }

        void Reshuffle(int[] index)
        {
            // Knuth shuffle algorithm :: courtesy of Wikipedia :)
            for (int t = 0; t < index.Length; t++)
            {
                int tmp = index[t];
                int r = UnityEngine.Random.Range(t, index.Length);
                index[t] = index[r];
                index[r] = tmp;
            }
        }
    }
    class ResumeItem
    {
        string m_oSkill;
        public string m_oLabel;
        public string m_oDescription;
        public string m_oWhy;
        public int m_iValue;

        public ResumeItem(string p_oSkill, string p_oLabel, int p_iValue, string p_oDescription, string p_oWhy)
        {
            m_oSkill = p_oSkill;
            m_iValue = p_iValue;
            m_oLabel = p_oLabel;
            m_oDescription = p_oDescription;
            m_oWhy = p_oWhy;


        }
    }

    static string HD = "<size=140%><align=center><b><u>Heading</u></b><align=left>\n" +
                   "<size=120%><b>\nWeak Choice:</b>\n" +
                   "<align=center><size=90%>Emma Kalish\n" +
                   "555.555.1212 | gamemaster@gmail.com<align=left>\n" +
                   "<size=120%><b>\nWhy?</b><size=95%>\n" +
                   "   The heading should contain a contact address along with a professional email address\n" +
                   "<size=120%><b>\n\n\nGood Choice:</b><size=95%>\n" +
                   "<align=center><size=90%>Emma Kalish\n" +
                   "1010 Aztec Drive, Oakland, CA 94619 | 555.555.1212 | EmmaKalish_Resume@gmail.com<align=left>\n" +
                   "<size=120%><b>\nWhy?</b><size=95%>\n" +
                   "   This heading includes the basic contact information including a reputable email\n   address.  Addresses are free, so if necessary, create a new one specifically for your\n   resume." +
                   "<size=120%><b>\n\n\nExcellent Choice:</b><size=95%>\n" +
                   "<align=center><size=90%>Emma Kalish\n" +
                   "1010 Aztec Drive, Oakland, CA 94619 | 555.555.1212 | EmmaKalish_Resume@gmail.com\n" +
                   "LinkedIn: http://linkedin.com/in/EmmaKalish | Portfolio: www.EmmaKalish.com<align=left>\n" +
                   "<size=120%><b>\nWhy?</b><size=95%>\n" +
                   "   This heading is an improvement on the prior version because it adds additional \n" +
                   "   information that may not easily fit on a standard two-page resume.  Make sure the \n" +
                   "   information posted at these locations are items you are willing to share with \n" +
                   "   potential employers.";

    static string EE = "<size=140%><align=center><b><u>Education</u></b><align=left>\n" +
        "<size=120%><b>\nWeak Choice:</b><size=90%>\n" +
        "   BS degree in CS, UofM-Dearborn\n" +
        "<size=120%><b>\nWhy?</b><size=100%>\n" +
        "   This is too vague; recruiter isn't going to take the time to decipher abbreviations.\n\n" +
        "<size=120%><b>\nGood Choice:</b><size=90%>\n" +
        "   Bachelor of Science degree in Computer Science\n   University of Michigan-Dearborn Campus, September 2016 - May 2020\n" +
        "<size=120%><b>\nWhy?</b><size=100%>\n" +
        "   This is much more informative and clearer.\n\n" +
        "<size=120%><b>\nExcellent Choice:</b><size=90%>\n" +
        "   Bachelor of Science degree in Computer Science\n   University of Michigan-Dearborn Campus, September 2016 - May 2020\n   Selected coursework: Software Engineering, Web Application Development,\n<pos=172>Artificial Intelligence\n" +
        "<size=120%><b>\nWhy?</b><size=100%>\n" +
        "   <color=red>This highlights types of skills that were learned as a student.  This is a good option\n   if your career history is short.";

    static string JE = "<size=140%><align=center><b><u>Job Experience</u></b><align=left>\n" +
         "<size=120%><b>\nWeak Choice:</b><size=90%>\n" +
         "   Stock Clerk, WeGotIt Hardware\n      *  Stocked shelves\n      *  Assisted customers\n      *  Operated cash register\n" +
         "<size=120%><b>\nWhy?</b><size=100%>\n" +
         "   This is just very basic information and needs more detail\n\n" +
         "<size=120%><b>\nGood Choice:</b><size=90%>\n" +
         "   Stock Clerk, WeGotIt Hardware in Ortonville, June 2017 to the present time\n      *  Restock shelves with items as the inventory runs low\n      *  Assist customers in finding items and answering questions\n      *  Operate the cash register and electronic payment devices\n" +
         "<size=120%><b>\nWhy?</b><size=100%>\n" +
         "   This is more descriptive of the duties and uses present tense instead of\n   past tense as the job is ongoing\n\n" +
         "<size=120%><b>\nExcellent Choice:</b><size=90%>\n" +
         "   Stock Clerk, WeGotIt Hardware in Ortonville, June 2017 to present time\n      *  Stock and organize shelves so customer can find items easily\n      *  Assist customers in finding items, answer questions and provide\n         recommendations/advice with customer projects\n      *  Operate the cash register and electronic payment devices and troubleshoot/resolve\n         system problems\n      *  Organized the stockroom so items could be located easier which increased efficiency\n         10% and reduced labor cost 5%\n" +
         "<size=120%><b>\nWhy?</b><size=100%>\n" +
         "   This version is more informative regarding the duties, demonstrates greater\n   responsibility and initiative in problem-solving";

    static string EP = "<size=140%><align=center><b><u>Educational Projects</u></b><align=left>\n" +
            "<size=120%><b>\nWeak Choice:</b><size=90%>\n" +
            "   *  Created a chess game using Microsoft C#\n" +
            "   *  Designed a life simulation game using Unity and C#\n" +
            "<size=120%><b>\nWhy?</b><size=100%>\n" +
            "   This entry lacks detail describing what was learned and how it would be of value\n   to an employer\n\n" +
            "<size=120%><b>\nGood Choice:</b><size=90%>\n" +
            "   *  Created an online chess game using C# that allowed participants to play over a\n      peer-to-peer network.\n" +
            "   *  Designed a life simulation game implementing Unity and C# which was intended to assist\n      Veterans in acquiring a job while transitioning to civilian life.\n" +
            "<size=120%><b>\nWhy?</b><size=100%>\n" +
            "   This version improves the description/intent of the projects and complexity but\n   lacks what was learned.\n\n" +
            "<size=120%><b>\nExcellent Choice:</b><size=90%>\n" +
            "   *  Designed and implemented a peer-to-peer chess game using C# and TCP-IP which\n      allowed participants to play online.  This endeavor required understanding the TCP/IP\n      protocol, C# language and software engineering.\n" +
            "   *  Worked with a team of developers to design a life simulation game implementing Unity\n      and C# which was intended to assist Veterans in acquiring a job while getting\n      accustomed to civilian life.  The effort included honing project and time management skills\n      along with emphasizing teamwork.\n" +
            "<size=120%><b>\nWhy?</b><size=100%>\n" +
            "   This version improves the project description while including skills that may be\n   important to a future employer.\n";


    static string VE = "<size=140%><align=center><b><u>Volunteer Experience</u></b><align=left>\n" +
                "<size=120%><b>\nWeak Choice:</b><size=90%>\n" +
                "   Meals on Wheels volunteer\n" +
                "<size=120%><b>\nWhy?</b><size=100%>\n" +
                "   Too general, lacks dates and responsibilities.\n\n" +
                "<size=120%><b>\nGood Choice:</b><size=90%>\n" +
                "   Meals on Wheels, Detroit Chapter, 7/5/20-11/30/20\n" +
                "   Delivered food parcels\n" +
                "<size=120%><b>\nWhy?</b><size=100%>\n" +
                "   Includes location, dates, and responsibility, but responsibility is vague.\n\n" +
                "<size=120%><b>\nExcellent Choice:</b><size=90%>\n" +
                "   Meals on Wheels, Detroit Chapter, 7/5/20-11/30/20\n" +
                "   Organized food delivery route, scheduled other drivers, coordinated with members\n   of the community.\n" +
                "<size=120%><b>\nWhy?</b><size=100%>\n" +
                "   Includes details, shows greater responsibility and community support.\n";



    
 

    ResumeJob PizzaJob = new ResumeJob (
        "<Player Name>, your networking skills are beginning to payoff.  After attending Robert’s Sweet Spot seminar, and actively engaging with both the instructor and several of the attendees, he thought enough of you to inform you that there is an opening at the Pizza Palace.  Opportunities such as this are one of the many advantages of getting out and building your social network.",
        "It is during these networking opportunities that you should be looking for that underserved need and trying to determine how your skills can be used to fill the void.  Keep track of this information as it can be invaluable when designing a resume for a particular prospect.\n\n<align=center><b>Résumé Game</b>\n\n<align=left>   In this mini-game you will attempt to design a robust résumé using options provided in each résumé section. To display the contents of each section button, slide the mouse over the button to show its contents in the lower text box. If you believe that description is the best available choice for that section, click on it to move it over to the résumé. Once all sections have been populated, press the review button to see how you did. The various sections and the ratings for each item can be seen by placing the mouse over the chosen item on the left side of the screen",
        new ResumeItem[]  
        {
            new ResumeItem("Heading", "Heading 1", 1, "<align=center>Emma Kalish\n555.555.1212 | gamemaster@gmail.com", HD),
            new ResumeItem("Heading", "Heading 2", 2, "<align=center>Emma Kalish\n1010 Aztec Drive, Oakland, CA 94619 | 555.555.1212 | EmmaKalish_Resume@gmail.com", HD),
            new ResumeItem("Heading", "Heading 3", 3, "<align=center>Emma Kalish\n1010 Aztec Drive, Oakland, CA 94619 | 555.555.1212 | EmmaKalish_Resume@gmail.com\nLinkedIn: http://linkedin.com/in/EmmaKalish | Portfolio: www.EmmaKalish.co", HD),
            
            new ResumeItem("Education", "Education 1", 1, "<b>Education</b>\n   BS degree in CS, UofM-Dearborn", EE),
            new ResumeItem("Education", "Education 2", 2, "<b>Education</b>\n   Bachelor of Science degree in Computer Science\n   University of Michigan-Dearborn Campus, September 2016 - May 2020", EE),
            new ResumeItem("Education", "Education 3", 3, "<b>Education</b>\n   Bachelor of Science degree in Computer Science\n   University of Michigan-Dearborn Campus, September 2016 - May 2020\n   Selected coursework: Software Engineering, Web Application Development, Artificial Intelligence", EE),
            
            //new ResumeItem("Tour of duty", "Tour of duty 1", 1, "Worst Choice", "Why is this the worst"),
            //new ResumeItem("Tour of duty", "Tour of duty 2", 2, "Ok Choice", "Why is this Ok"),
            //new ResumeItem("Tour of duty", "Tour of duty 3", 3, "Best Choice", "Why is this the Best"),
            
            new ResumeItem("Job Experience", "Job Experience 1", 1, "<b>Job Experience</b>\n   Stock Clerk, WeGotIt Hardware\n      *  Stocked shelves\n      *  Assisted customers\n      *  Operated cash register", JE),
            new ResumeItem("Job Experience", "Job Experience 2", 2, "<b>Job Experience</b>\n   Stock Clerk, WeGotIt Hardware in Ortonville, June 2017 to the present time\n      *  Restock shelves with items as the inventory runs low\n      *  Assist customers in finding items and answering questions\n      *  Operate the cash register and electronic payment devices", JE),
            new ResumeItem("Job Experience", "Job Experience 3", 3, "<b>Job Experience</b>\n   Stock Clerk, WeGotIt Hardware in Ortonville, June 2017 to present time\n      *  Stock and organize shelves so customer can find items easily\n      *  Assist customers in finding items, answer questions and provide recommendations/advice with customer projects\n      *  Operate the cash register and electronic payment devices and troubleshoot/resolve system problems\n      *  Organized the stockroom so items could be located easier which increased efficiency 10% and reduced labor cost 5%", JE),
            
            new ResumeItem("Educational Projects", "Educational Projects 1", 1, "<b>Educational Projects</b>\n   *  Created a chess game using Microsoft C#\n   *  Designed a life simulation game using Unity and C#", EP),
            new ResumeItem("Educational Projects", "Educational Projects 2", 2, "<b>Educational Projects</b>\n   *  Created an online chess game using C# that allowed participants to play over a peer-to-peer network.\n   *  Designed a life simulation game implementing Unity and C# which was intended to assist Veterans in acquiring\n      a job while transitioning to civilian life.", EP),
            new ResumeItem("Educational Projects", "Educational Projects 3", 3, "<b>Educational Projects</b>\n   *  Designed and implemented a peer-to-peer chess game using C# and TCP-IP which allowed participants to play online.\n      This endeavor required understanding the TCP/IP protocol, C# language and software engineering.\n   *  Worked with a team of developers to design a life simulation game implementing Unity and C# which was intended to\n      assist Veterans in acquiring a job while getting accustomed to civilian life.  The effort included honing project and\n      time management skills along with emphasizing teamwork.", EP),
            
            new ResumeItem("Volunteer Experience", "Volunteer Experience 1", 1, "<b>Volunteer Experience</b>\n   Meals on Wheels volunteer", VE),
            new ResumeItem("Volunteer Experience", "Volunteer Experience 2", 2, "<b>Volunteer Experience</b>\n   Meals on Wheels, Detroit Chapter, 7/5/20-11/30/20\n   Delivered food parcels", VE),
            new ResumeItem("Volunteer Experience", "Volunteer Experience 3", 3, "<b>Volunteer Experience</b>\n   Meals on Wheels, Detroit Chapter, 7/5/20-11/30/20\n   Organized food delivery route, scheduled other drivers, coordinated with members of the community", VE)
        }
    );
 }
