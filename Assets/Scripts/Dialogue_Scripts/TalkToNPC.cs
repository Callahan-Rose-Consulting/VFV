/*#TOPIC
TalkToNPC.cs is a script placed on each NPC that can be talked to.
This handles being able to press E to talk to an NPC, making them talk-to-able.
The messages that each NPC has is stored in here as well.
Each message can use certain keywords like #YES_NO# to prompt a yes or no question.
These keywords are normally wrapped by hashtags and are read and replaced so they do not get printed during runtime.
This script also holds the functionality to print out each message, one character at a time.
This script also opens and closes each textbox
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO; //input and output
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class TalkToNPC : MonoBehaviour
{
    public static bool firstRun = false;
    public static string playerFileName;

    public InputSmartGoal inputSmartGoal;
    public static bool displayInputBox = false;

    public static string fileName;

    public static string[] valueProperties = {"VISION", "ALIGNMENT", "UNDERSTAND", "ENACT"};
    public static string[] starProperties = {"SITUATION", "TASK", "ACTION", "RESULT"};

    public static int progressBarLine = 0;

    private float delay;
    private string currentMessage = "";
    [TextArea(5, 10)]
    public string[] messages;
    public string NPCName;
    private string UnknownName = "Unknown";
    private string playerName = "You";
    public bool Known = true;
    public RawImage NameBox;
    public int messageCount = 0;
    public Image ImageToShow;
    Canvas UICanvas;
    AudioSource TalkSFX;
    RawImage TextBox;
    private TextMeshProUGUI NameBoxText;
    private TextMeshProUGUI text;

    private RawImage YesNoBox;

    public Button YesButton;

    public Button NoButton;

    public bool messageDone = false;
    private bool messageIsTyping = false;
    private bool firstMessage = false;
    private bool multipleMessages = false;
    private bool isTalking = false;
    private bool InnerDialogue = false;
    public bool isItem = false;
    private bool textboxIsClosing = false;
    private Player_Movement player;
    private GameManager gameManager;
    public int EnemyCommunications;
    public static bool interviewTaken = false;
    public static bool dialogueActive = false;
    public static bool endGame = false;
    public static bool yesNoInstructions = true;

    public static int numPerfectAnswers = 0;
    public static int numQuestionsAsked = 0;
    public static string playerResultsFile = "";


    //This function makes a Directory in the root folder of the game if /Player Results dir does not exist
    //Inside the /Player Results dir a file is created when when the first frame of the world map is called
    //Example: starting the game with player named "Don" will make the file Don 1-01-2022 H13M30S32.txt"
    //Note: the file name is created by using the players name + the time the file is created as in: 1-01-2022 is Jan 1st 2022 | H13 is 1pm| M30 is 30min| S32 is 32sec
    //The reason this is done is to generate a unique file for each time the game is played, even if the same Don tries to play the game 5 times within the same min.

    //The Players name, and initial stats are written to the file. Later functions are called in the game that update the created file.
    //ref is used to pass playerFileName by reference as to update the string based on. In C# pointers aren't really used, instead use ref and it does the job of pass by reference
    //Created by Don Murphy
    void CreateFile(ref string playerFileName)
    {
        //create a Directory for the player results
        string Dir = "Player Results";
        if (!Directory.Exists(Dir))
        {
            Directory.CreateDirectory(Dir);
        }

        //Cleanstring removes special characters. This is needed so players can't pick a name that would stop a text file from being generated.
        string playerName = Questionnaire.CleanString(PlayerPrefs.GetString("PlayerName"));

        //The DateTime is added at the end of players name. This makes each playthrough have a unique file name
        //Example: Mike 8-19-2022 H-12M-9.txt
        string fileName = playerName + " " +
            DateTime.Now.Month.ToString() + "-" +
            DateTime.Now.Day.ToString() + "-" +
            DateTime.Now.Year.ToString() +

            " H" + DateTime.Now.Hour.ToString() +
            "M" + DateTime.Now.Minute.ToString() +
            "S" + DateTime.Now.Second.ToString() +
            ".txt";

        //creating string for writing info in player results.
        string path = @"Player Results" + "/" + fileName;

        //creating string for reading and writing the head of the player results file.
        string head = @"Assets\\Player Inventory\\Player Result Information\\header.txt";
        string headInfo = File.ReadAllText(head);

        //creating string for reading and writing the ending of the player results file.
        string end = @"Assets\\Player Inventory\\Player Result Information\\ending.txt";
        string endInfo = File.ReadAllText(end);

        //create blank text doc and name it according to fileName in the Player Results Directory
        using (FileStream fs = File.Create(path)) { };
        string VFV = "";
        playerFileName = path;

        File.AppendAllText(path, headInfo);
        File.AppendAllText(path, "\n");
        File.AppendAllText(path, "---------------Performace Report----------------\n");
        File.AppendAllText(path, "Player name: " + playerName + '\n');
        File.AppendAllText(path, "Branch of Service: " + Resume.TourBranch + '\n');
        File.AppendAllText(path, "*Initial Self Assessment:\n");
        File.AppendAllText(path, "\t-Initial Leadership________________________________:" + Questionnaire.PrintStartingStat("leadership") + "\n");
        File.AppendAllText(path, "\t-Initial Teamwork__________________________________:" + Questionnaire.PrintStartingStat("teamwork") + "\n");
        File.AppendAllText(path, "\t-Initial Technology________________________________:" + Questionnaire.PrintStartingStat("technology") + "\n");
        File.AppendAllText(path, "\t-Initial Professionalism___________________________:" + Questionnaire.PrintStartingStat("professionalism") + "\n");
        File.AppendAllText(path, "\t-Initial Communication_____________________________:" + Questionnaire.PrintStartingStat("communication") + "\n");
        File.AppendAllText(path, "\t-Initial Critical Thinking_________________________:" + Questionnaire.PrintStartingStat("critical thinking") + "\n");
        File.AppendAllText(path, "\n");
        File.AppendAllText(path, "*Final Self Assesment:\n");
        File.AppendAllText(path, "\t-Final Leadership__________________________________:" + Questionnaire.PrintStartingStat("leadership") + "\n");
        File.AppendAllText(path, "\t-Final Teamwork____________________________________:" + Questionnaire.PrintStartingStat("teamwork") + "\n");
        File.AppendAllText(path, "\t-Final Technology__________________________________:" + Questionnaire.PrintStartingStat("technology") + "\n");
        File.AppendAllText(path, "\t-Final Professionalism_____________________________:" + Questionnaire.PrintStartingStat("professionalism") + "\n");
        File.AppendAllText(path, "\t-Final Communication_______________________________:" + Questionnaire.PrintStartingStat("communication") + "\n");
        File.AppendAllText(path, "\t-Final Critical Thinking___________________________:" + Questionnaire.PrintStartingStat("critical thinking") + "\n");
        File.AppendAllText(path, "\n");
        File.AppendAllText(path, endInfo);

    }
    //This function takes in the name of the result to be updated and the name of the file to update.
    //This function is designed to be called when an event happens in game that would inicate some form of progression.
    //Examples:
    //      The players Technology skill increases -> UpdatePlayerResults ("Final Technology", playerFileName)
    //      or UpdatePlayerResults ("Final Technology", "mytext.txt")
    //NOTE: playerFileName should be used as the second parameter because that is the variable that stores the file name of the current session.
    //created by Don Murphy
    //edited by Kareem Ibrahim
    public static void UpdatePlayerResults(string resultName, string FileToUpdate, string companyName = "", string jobTitle = "")
    {
        string playerFileName = FileToUpdate;
        fileName = FileToUpdate;

        var allLines = File.ReadAllLines(playerFileName); //read file into lines var
        int i = -1;

        foreach (string line in allLines) //iterate though all strings in file
        {
            i++;

            //Final Self Assesment
            if (line.Contains("Final"))
            {
                string finalStatRegex = @"^\s+-" + resultName + @"_*:\s*\d*\s?$"; //Match for Final Stats section

                Match matchFinalStat = Regex.Match(line, finalStatRegex); //UPDATE for multiple matches
                Match matchNum = Regex.Match(allLines[i], @"\d+"); //Match num in string

                if (matchFinalStat.Success && matchNum.Success) //string match from file
                {
                    if (Int32.TryParse(matchNum.Value, out int val))
                    {
                        val++; //increase leadership value by 1

                        allLines[i] = Regex.Replace(matchFinalStat.ToString(), matchNum.ToString(), val.ToString()); //(input, pattern, replacement)  (original string, found num, updated num)
                    }
                }
            }

            //Videos Watched
            else if (line.Contains("Video"))
            {
                //string InterviewRegex = @"\s*-[a-zA-Z]+:" + resultName + "+_+:\\s*NO";
                string InterviewRegex = @"\s*-Video:" + resultName + "+_+:\\s*NO";
                Match matchVideo = Regex.Match(line, InterviewRegex);
                Match matchAnswerNO = Regex.Match(line, "NO");
                if (matchVideo.Success && matchAnswerNO.Success)
                {
                    allLines[i] = Regex.Replace(matchVideo.ToString(), matchAnswerNO.ToString(), "YES");
                }
            }
            //Informational Books Read
            else if (line.Contains("Book"))
            {
                //previously tried regex kept for making/editing template
                // s*-Book:([A-Za-z0-9 !',+.]+)++.+
                //s*-Book:Team Synergy+_+:\s*NO
                //s*-Book:([CompTia A+ Certification Prep Questions])+:\s*NO
                string BookRegex = "\\s*" + @"-Book:" + resultName + "+.+";

                Match matchBook = Regex.Match(line, BookRegex);
                Match matchAnswerNO = Regex.Match(line, "NO");

                if (matchBook.Success && matchAnswerNO.Success)
                {
                    allLines[i] = Regex.Replace(matchBook.ToString(), matchAnswerNO.ToString(), "YES");
                }

            }

            else if (line.Contains("END OF INTERVIEW PERFORMANCE:")) {
                allLines[i] = "\nInterview Results for the " + jobTitle + " role at " + companyName + ":\n\n" + allLines[i];
                progressBarLine = i + 2;
            }

        }

        File.WriteAllLines(playerFileName, allLines); //rewerite file with update
    }

    public static void UpdateInterviewResults(string updateType, string FileToUpdate, string question, string[] userWords) {
        string playerFileName = FileToUpdate;
        playerResultsFile = FileToUpdate;

        var allLines = File.ReadAllLines(playerFileName); //read file into lines var
        int lineNumber = -1;

        numQuestionsAsked++;

        foreach (string line in allLines) {
            lineNumber++;

            if (!line.Contains("END OF INTERVIEW PERFORMANCE:")) {
                continue;
            }

            string original = allLines[lineNumber];

            allLines[lineNumber] = "\nQuestion: " + question + "\n";
            allLines[lineNumber] += "Feedback:\n\nYou got the " + updateType + " properties of:";

            for (int k = 0; k < userWords.Length; k++) {
                allLines[lineNumber] += " " + userWords[k];
            }


            if (updateType == "VALUE" && userWords.Length != valueProperties.Length) {
                allLines[lineNumber] += "\nYou missed out on the VALUE PROPERTY OF:";

                for (int j = 0; j < valueProperties.Length; j++) {
                        if (!userWords.Contains(valueProperties[j])) {
                            allLines[lineNumber] += " " + valueProperties[j];
                        }
                }

                allLines[lineNumber] += "\n";
            }

            // must be STAR
            else if (updateType == "STAR" && userWords.Length != starProperties.Length) {
                allLines[lineNumber] += "\nYou missed out on the START PROPERTY OF:";

                for (int j = 0; j < starProperties.Length; j++) {
                        if (!userWords.Contains(starProperties[j])) {
                            allLines[lineNumber] += " " + starProperties[j];
                        }
                }

                allLines[lineNumber] += "\n";
            }

            else {
                numPerfectAnswers++;
            }

            allLines[lineNumber] += original;

            break;
        }



        File.WriteAllLines(playerFileName, allLines); //rewerite file with update
    }


    public bool getIsTalking()
    {
        return isTalking;
    }

    void Awake()
    {
        //Make If statements here for videos

        initialize_components();

        target_text = UICanvas.GetComponentInChildren<TextMeshProUGUI>();

        if (messages[messageCount].Contains("#MULTI_START#"))
        {
            multipleMessages = true;
        }

        // FindObjectOfType<AudioManager>().Play("interview");
        if (firstRun == false)
        {

            CreateFile(ref playerFileName);

        }
        firstRun = true;
    }


    public void initialize_components()
    {
        delay = 0.0f; // PlayerPrefs.GetFloat("TextSpeed");
        player = GameObject.Find("Player").GetComponent<Player_Movement>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        UICanvas = GameObject.Find("DialogueUI").GetComponent<Canvas>();
        TextBox = UICanvas.transform.GetChild(0).GetComponent<RawImage>();
        text = TextBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TalkSFX = gameObject.GetComponent<AudioSource>();
        YesNoBox = GameObject.Find("YesNoBox").GetComponent<RawImage>();

        GameObject b = GameObject.Find("YesBTN");

        if (b != null)
        {
            YesButton = b.GetComponent<Button>();
        }

        b = GameObject.Find("NoBTN");

        if (b != null)
        {
            NoButton = b.GetComponent<Button>();
        }

        NameBox = GameObject.Find("NameBox").GetComponent<RawImage>();
        NameBoxText = GameObject.Find("NameBox").transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        NameBoxText.text = NPCName;


    }


    /*
     * ReplaceKeywords is our function to read and replace each keyword.
     * After replacing the keyword it returns the proper message to be printed.
     * This function is called AFTER reading and performing the proper keyword operation, such as making a message a #YES_NO# question
     */
    public string ReplaceKeywords(string message)
    {
        string newMessage = message;
        if (newMessage != "")
        {
            newMessage = newMessage.Replace("@", PlayerPrefs.GetString("PlayerName"));
            if (Resume.dream_job != null)
            {
                newMessage = newMessage.Replace("@DC", Resume.dream_job.Company);
            }
            else
            {
                newMessage = newMessage.Replace("@DC", "Dream Company here...");
            }
            newMessage = newMessage.Replace("COMMUNICATION_SKILL", PlayerPrefs.GetInt("CommunicationLevel").ToString());
            newMessage = newMessage.Replace("COMMUNICATION_PLUS_ONE", (PlayerPrefs.GetInt("CommunicationLevel") + 1).ToString());

            newMessage = newMessage.Replace("#SKILL_INCREASE_COMMUNICATION#", "");
            newMessage = newMessage.Replace("#SHOW_IMAGE#", "");
            newMessage = newMessage.Replace("#HIDE_IMAGE#", "");
            newMessage = newMessage.Replace("#REVEAL_NAME#", "");
            newMessage = newMessage.Replace("#INNER_DIALOGUE_BEGIN#", "");
            newMessage = newMessage.Replace("#INNER_DIALOGUE_END#", "");
            newMessage = newMessage.Replace("#PLAYER_TALKING_BEGIN#", "");
            newMessage = newMessage.Replace("#PLAYER_TALKING_END#", "");
            newMessage = newMessage.Replace("#SKILL_INCREASE_INTELLIGENCE#", "");
            newMessage = newMessage.Replace("#SKILL_CHECK#", "");
            newMessage = newMessage.Replace("#LOAD_INTERVIEW#", "");
            newMessage = newMessage.Replace("#YES#", "");
            newMessage = newMessage.Replace("#NO#", "");
            newMessage = newMessage.Replace("#YES_NO#", "");
            newMessage = newMessage.Replace("#YES_NO_COMPLETE#", "");
            newMessage = newMessage.Replace("#MULTI_START#", "");
            newMessage = newMessage.Replace("#MULTI_END#", "");
            newMessage = newMessage.Replace("#SA1#", "");
            newMessage = newMessage.Replace("#SA2#", "");
            newMessage = newMessage.Replace("#FADE_OUT#", "");
            newMessage = newMessage.Replace("#FADE_IN#", "");
            newMessage = newMessage.Replace("#loc#", "");
            newMessage = newMessage.Replace("#ADD_EXPERIENCE#", "");
            newMessage = newMessage.Replace("#Post#", "");
            newMessage = newMessage.Replace("#ADD_SUMMARY#", "");
            newMessage = newMessage.Replace("*Lead*", "");
            newMessage = newMessage.Replace("*Team*", "");
            newMessage = newMessage.Replace("*Tech*", "");
            newMessage = newMessage.Replace("*Prof*", "");
            newMessage = newMessage.Replace("*Com*", "");
            newMessage = newMessage.Replace("*Crit*", "");
            newMessage = newMessage.Replace("#showcanvas#", "");
            newMessage = newMessage.Replace("#hidecanvas#", "");
            newMessage = newMessage.Replace("#topic#", "");
            newMessage = newMessage.Replace("#SKIP_START#", "");
            newMessage = newMessage.Replace("#SKIP_END#", "");
            newMessage = newMessage.Replace("#BRANDING#", "");
            newMessage = newMessage.Replace("#triggerEndGame", "");

            //career fair RepaceKeyWords
            //added by Don Murphy
            newMessage = newMessage.Replace("#CF_Engineering#", "");
            newMessage = newMessage.Replace("#CF_Business#", "");
            newMessage = newMessage.Replace("#CF_Arts#", "");
            newMessage = newMessage.Replace("#CF_BlueCollar#", "");
            newMessage = newMessage.Replace("#CF_Education#", "");


            //Changes by Austin Greear 4/26/2020
            newMessage = newMessage.Replace("#INVOKE_EVENT#", "");

            Regex getMessage = new Regex(@"\*.[^_]*\*");

            Match x = getMessage.Match(newMessage);

            if (x.Success)
            {
                int index = 0;

                newMessage = newMessage.Replace(x.Value, "");
            }

            newMessage = newMessage.Replace("#FADE_EVENT#", "");
            newMessage = newMessage.Replace("#Disable#", "");
            newMessage = newMessage.Replace("#NPC_NAME#", NPCName);
            newMessage = newMessage.Replace("#FEEDBACK#", "");
            newMessage = newMessage.Replace("#INCREASE#", "");
            newMessage = newMessage.Replace("#CHECK#", "");
            newMessage = newMessage.Replace("#END_WORK#", "");

            if (Resume.current_job != null)
            {
                newMessage = newMessage.Replace("#INCOME#", "" + Resume.current_job.Income);
            }
        }
        return newMessage;
    }

    public bool change_state = true;

    public bool repeating_dialogue = false;

    //Change by Austin Greear 6/11/2020
    public void end_dialogue()
    {
        dialogueActive = false;
        textboxIsClosing = true;
        StartCoroutine(ChangeSizeCoroutine(0.5f, 0f, TextBox));
        if (YesNoBox != null) StartCoroutine(ChangeSizeCoroutine(0.5f, 0f, YesNoBox));
        if (NameBox != null) StartCoroutine(ChangeSizeCoroutine(0.5f, 0f, NameBox));

        StartCoroutine(DialogueToggle(0.51f, false, true, false, false));

        if (change_state)
        {
            GameManager.instance.change_game_state("Normal");
        }

        if (repeating_dialogue)
        {
            messageCount = 0;
            end_dialogue_event.Invoke();
        }
    }

    // Update is called once per frame
    //Update is only meant to check for the user hitting enter or space -- which is when the user is advancing dialogue that is already open.
    void Update()
    {
        if ((Input.GetKeyDown("return") || Input.GetKeyDown("space")) && messageDone && !multipleMessages && !textboxIsClosing)//To close a message
        {
            end_dialogue();
        }
        else if ((Input.GetKeyDown("return") || Input.GetKeyDown("space")) && messageDone && multipleMessages)
        {
            DecideWhichDialogueToShow();//Will show the next dialogue in the multimessage chain
        }

        else if (GameManager.instance.game_state == "Normal" && displayInputBox == true)
        {
            displayInputBox = false;
            change_state = true;
        }
    }

    IEnumerator DialogueToggle(float time, bool messageDoneState, bool playerCanMoveState, bool isTalkingState, bool textboxIsClosingState)
    {
        WaitForSeconds waitTime = new WaitForSeconds(time);

        yield return waitTime;
        messageDone = messageDoneState;

        if (change_state)
        {
            player.canMove = playerCanMoveState;
        }

        isTalking = isTalkingState;
        dialogueActive = isTalkingState;
        textboxIsClosing = textboxIsClosingState;

        NameBox.gameObject.SetActive(true);
        InnerDialogue = false;
    }

    //This Coroutine is called when inflating or deflating a textbox over a given amount of time.
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

    //OnTriggerStay2D checks to see if the player is colliding with the NPC, then sees if the player presses our interaction button - currently E
    //The function then calls DecideWhichDialogueToShow() - which basically looks for any keywords in the message and processes them.
    void OnTriggerStay2D(Collider2D other)
    {
        if (!DayChanger.endOfDay)
        {
            if (Input.GetButton("Interact") && other.CompareTag("Player") && !messageDone && !messageIsTyping && !displayInputBox)
            {
                //@CS Disable mouse click so that the user cannot unselect the dialogue choices
                //implementation - waiting until we can fully get rid of the mouse entirely
                //Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;

                dialogueActive = true;
                isTalking = true;
                /*if (Known)*/
                NameBoxText.text = NPCName;
                //else NameBoxText.text = UnknownName;
                DecideWhichDialogueToShow();

                if (change_state)
                {
                    GameManager.instance.change_game_state("Dialogue");
                }
            }
        }
    }

    public void trigger_dialogue()
    {
        if (!messageDone && !messageIsTyping)
        {
            dialogueActive = true;
            isTalking = true;
            /*if (Known) */
            NameBoxText.text = NPCName;
            //else NameBoxText.text = UnknownName;
            DecideWhichDialogueToShow();
        }
    }

    /*
     *DecideWhichDialogueToShow looks to see if each message contains any of the keywords and then processes the message accordingly.
     *Messages can have multiple keywords, such as "#REVEAL_NAME##SA1#Hello! My name is Steve Harvey!"
     *      This message would set the NPC's name to be known, as well as take away one significant action point from the player.
     *
     *One of the most used keywords is #MULTI_START# and #MULTI_END#
     * #MULTI_START# begins treating the messages as a chain, and will keep displaying messages sequentially until #MULTI_END# is read in a following message.
     * Do NOT use #MULTI_START# without #MULTI_END#
     */
    public void DecideWhichDialogueToShow()
    {
        StartCoroutine(ChangeSizeCoroutine(0.5f, 1f, TextBox));
        if (!InnerDialogue) // If the inner dialog is off, show the name box
            StartCoroutine(ChangeSizeCoroutine(0.5f, 1f, NameBox));

        if (messageCount < 0 || messageCount >= messages.Length)
        {
            Debug.Log(messages.Length + " " + name);
            return;
        }

        if (messages[messageCount].Contains("#MULTI_START#"))
        {
            multipleMessages = true;
            //Debug.Log("Multi messages ON");
        }
        else if (messages[messageCount].Contains("#MULTI_END#"))
        {
            multipleMessages = false;
            //Debug.Log("Multi messages OFF");
        }

        if (messages[messageCount].Contains("#SHOW_IMAGE#"))
        {
            ImageToShow.enabled = true;
        }

        if (messages[messageCount].Contains("#HIDE_IMAGE#"))
        {
            ImageToShow.enabled = false;
        }
        if (messages[messageCount].Contains("#NetworkingText#"))
        {
            if (Calendar.CurrentDay == Calendar.DayOfEvent)
            {
                messages[messageCount] = messages[messageCount].Replace("#NetworkingText#", " The networking event is today. Head to the Wellness Center when you have the time!");
            }
            else
            {
                messages[messageCount] = messages[messageCount].Replace("#NetworkingText#", "");
            }
        }
        if (messages[messageCount].Contains("#REVEAL_NAME#"))
        {
            Known = true;
            NameBoxText.text = NPCName;
        }
        if (messages[messageCount].Contains("#showcanvas"))
        {
            transform.GetChild(1).gameObject.SetActive(true);
            gameObject.GetComponent<wander>().zoomonCanvas();
        }
        if (messages[messageCount].Contains("#hidecanvas"))
        {
            gameObject.GetComponent<wander>().resetCamera();

            transform.GetChild(1).gameObject.SetActive(false);
        }
        //added by Don Murphy
        //TODO: add this functionality to UpdatePlayerResults function instead of hardcoding
        if (messages[messageCount].Contains("#CF_Engineering#"))
        {
            string myFile = File.ReadAllText(playerFileName);
            string updateCareer = myFile.Replace("[No Career Fair Choice]", "Engineering");
            File.WriteAllText(playerFileName, updateCareer);


        }
        //added by Don Murphy
        //TODO: add this functionality to UpdatePlayerResults function instead of hardcoding
        if (messages[messageCount].Contains("#CF_Business#"))
        {
            string myFile = File.ReadAllText(playerFileName);
            string updateCareer = myFile.Replace("[No Career Fair Choice]", "Business");
            File.WriteAllText(playerFileName, updateCareer);
        }
        //added by Don Murphy
        //TODO: add this functionality to UpdatePlayerResults function instead of hardcoding
        if (messages[messageCount].Contains("#CF_Arts#"))
        {
            string myFile = File.ReadAllText(playerFileName);
            string updateCareer = myFile.Replace("[No Career Fair Choice]", "Arts");
            File.WriteAllText(playerFileName, updateCareer);
        }
        //added by Don Murphy
        //TODO: add this functionality to UpdatePlayerResults function instead of hardcoding
        if (messages[messageCount].Contains("#CF_BlueCollar#"))
        {
            string myFile = File.ReadAllText(playerFileName);
            string updateCareer = myFile.Replace("[No Career Fair Choice]", "Blue Collar");
            File.WriteAllText(playerFileName, updateCareer);
        }
        //added by Don Murphy
        //TODO: add this functionality to UpdatePlayerResults function instead of hardcoding
        if (messages[messageCount].Contains("#CF_Education#"))
        {
            string myFile = File.ReadAllText(playerFileName);
            string updateCareer = myFile.Replace("[No Career Fair Choice]", "Education");
            File.WriteAllText(playerFileName, updateCareer);
        }


        //Change by Austin Greear 4/26/2020
        if (messages[messageCount].Contains("#INVOKE_EVENT#"))
        {
            Regex getMessage = new Regex(@"\*.[^_]*\*");

            Match x = getMessage.Match(messages[messageCount]);

            if (x.Success)
            {
                int index = 0;

                index = int.Parse(x.Value.Replace("*", String.Empty));

                if (index >= 0 && index < Events.Count)
                {
                    Events[index].Invoke();
                }
            }
            else
            {
                Dialogue_Event.Invoke();
            }
        }

        //Change by Austin Greear 6/10/2020
        if (messages[messageCount].Contains("#FADE_EVENT#"))
        {
            Regex getMessage = new Regex(@"\*.[^_]*\*");

            Match x = getMessage.Match(messages[messageCount]);

            if (x.Success)
            {
                int index = 0;

                //messages[messageCount] = messages[messageCount].Replace(x.Value, String.Empty);

                index = int.Parse(x.Value.Replace("*", String.Empty));

                if (index >= 0 && index < Events.Count)
                {
                    Fade.instance.complete_fade_event = Events[index];

                    Fade.instance.FadeOut();
                }
            }
            else
            {
                Fade.instance.complete_fade_event = Dialogue_Event;

                Fade.instance.FadeOut();
            }
        }

        //Change by Austin Greear 5/7/2020
        if (messages[messageCount].Contains("#Disable#"))
        {
            this.enabled = false;
        }

        //Change by Austin Greear 5/7/2020
        if (messages[messageCount].Contains("#Enable#"))
        {
            this.enabled = true;
        }

        //Change by Austin Greear 5/7/2020
        if (messages[messageCount].Contains("#END_WORK#"))
        {
            end_dialogue();
            Work_Event_Manager.instance.end_work_day();
        }

        //Change by Austin Greear 5/7/2020
        if (messages[messageCount].Contains("#QUESTION# "))
        {
            if (Interview_Questions.instance != null)
            {
                Interview_Questions.instance.talkToNPC = this;

                string index = messages[messageCount].Replace("#QUESTION# ", "");

                string[] splitStr = index.Split(' ');

                for (int i = 0; i < splitStr.Length; i++)
                {
                    if (i == 0)
                    {
                        Interview_Questions.instance.load_question(Int16.Parse(splitStr[i]));
                    }
                }
            }
        }

        //Created by mohsen
        if (messages[messageCount].Contains("#INPUT_SMARTGOAL#"))
        {
            inputSmartGoal.handleDisplay(ref displayInputBox, ref change_state, playerFileName);
        }

        //Change by Austin Greear 5/7/2020
        if (messages[messageCount].Contains("#EXPERIENCE# "))
        {
            if (Experience_Reactions.instance != null)
            {
                string index = messages[messageCount].Replace("#EXPERIENCE# ", "");

                Experience_Reactions.instance.load_dialogue(Int16.Parse(index));
            }
        }

        //Change by Austin Greear 5/7/2020
        if (messages[messageCount].Contains("#FEEDBACK#"))
        {
            // END OF INTERVIEW AND FEEDBACK GIVEN

            UpdateProgressBar();

            messages[messageCount] = messages[messageCount].Replace("#FEEDBACK#", "");
            if (Experience_Reactions.instance != null)
            {
                Experience_Reactions.instance.load_feed_back();
            }
        }

        //Change by Austin Greear 7/17/2020
        if (Resume.current_job != null)
        {
            if (messages[messageCount].Contains("#INCOME#"))
            {
                Player_Inventory.instance.increment_currency((float)Resume.current_job.Income);
                Player_Inventory.instance.totalEarnings += (float)Resume.current_job.Income;
                Debug.Log("totalEarnings:" + Player_Inventory.instance.totalEarnings);
            }
        }

        if (messages[messageCount].Contains("#INNER_DIALOGUE_BEGIN#"))
        {
            NameBox.gameObject.SetActive(false);
            InnerDialogue = true;
            // The name box is made visible before the INNER_DIALOG_BEGIN is checked
            //
            if (null != NameBox) StartCoroutine(ChangeSizeCoroutine(0.5f, 0f, NameBox));  // Added 8/1/2021 to remove empty name box after work event.
        }
        if (messages[messageCount].Contains("#INNER_DIALOGUE_END#"))
        {
            NameBox.gameObject.SetActive(true);
            InnerDialogue = false;
            if (null != NameBox) StartCoroutine(ChangeSizeCoroutine(0.5f, 1, NameBox));  // Added 8/1/2021 to remove empty name box after work event.
        }

        if (messages[messageCount].Contains("#PLAYER_TALKING_BEGIN#"))
        {
            //NameBox.gameObject.SetActive(false);
            NameBoxText.text = playerName;
            //InnerDialogue = true;
            // The name box is made visible before the INNER_DIALOG_BEGIN is checked
            //
            //if (null != NameBox) StartCoroutine(ChangeSizeCoroutine(0.5f, 0f, NameBox));  // Added 8/1/2021 to remove empty name box after work event.
        }
        if (messages[messageCount].Contains("#PLAYER_TALKING_END#"))
        {
            NameBoxText.text = NPCName;
            //NameBox.gameObject.SetActive(true);
            //InnerDialogue = false;
            //if (null != NameBox) StartCoroutine(ChangeSizeCoroutine(0.5f, 1, NameBox));  // Added 8/1/2021 to remove empty name box after work event.
        }

        //This keyword pulls from the NPC's AddExperience component, which holds an experience that the player receives
        if (messages[messageCount].Contains("#ADD_EXPERIENCE#"))
        {
            AddExperience addexp = gameObject.GetComponent<AddExperience>();

            if (addexp != null)
            {
                addexp.add_exp();

                if (Selection_Menu.instance != null)
                {
                    //Debug.Log("Added and Updated");
                    Selection_Menu.instance.populate_content_folders();
                }
            }
        }

        //These can be used in conjunction to fade the screen to black and back to normal
        if (messages[messageCount].Contains("#FADE_OUT#"))
        {
            GameObject.Find("FadeToBlack").GetComponent<Fade>().FadeOut();
        }
        else if (messages[messageCount].Contains("#FADE_IN#"))
        {
            GameObject.Find("FadeToBlack").GetComponent<Fade>().FadeIn();
        }

        if (messages[messageCount].Contains("#LOAD_INTERVIEW#"))
        {
            SceneChange.PreSceneChange();
            Debug.Log("Changing to interview");
            interviewTaken = true;
            SceneManager.LoadScene("DialogueTest");
        }
        if (messages[messageCount].Contains("#loc#"))
        {
            // wander.show = true;
        }

        if (messages[messageCount].Contains("#Post#"))
        {
            Regex getMessage = new Regex(@"\*.[^_]*\*");
            Match x = getMessage.Match(messages[messageCount]);
            if (x.Success)
            {
                messages[messageCount] = messages[messageCount].Replace(x.Value, String.Empty);
                bulletinBoard.messages.Add(x.Value.Replace("_", String.Empty));
            }
        }
        //adds the desired description/summary to the day summary
        if (messages[messageCount].Contains("#ADD_SUMMARY#"))
        {
            Regex getMessage = new Regex(@"\*.[^_]*\*");
            Match x = getMessage.Match(messages[messageCount]);
            if (x.Success)
            {
                //Debug.Log("HEY X WAS A SUCCESS");
                //messages[messageCount] = messages[messageCount].Replace(x.Value, String.Empty);
                if (DayChanger.summary_messages[0].Equals(""))
                {
                    //Debug.Log("WE GOT HERE");
                    DayChanger.summary_messages[0] = x.Value.Replace("_", String.Empty);
                }
                else if (DayChanger.summary_messages[1].Equals(""))
                {
                    //Debug.Log("WE GOT THERE");
                    DayChanger.summary_messages[1] = x.Value.Replace("_", String.Empty);
                }
                else
                {
                    DayChanger.summary_messages[2] = x.Value.Replace("_", String.Empty);
                }
            }
        }

        //to recognize if the branding video has been watched
        if (messages[messageCount].Contains("#BRANDING#"))
        {
            GameManager.watchedBranding = true;
        }


        //Each keyword here can be used together to increase certain player skills
        //EX: "#INCREASE#*Tech*I feel like my technology skills have improved!" would be a suitable message to increase the players tech skills by 1.
        //Change by Austin Greear 5/18/2020

        //UpdatePlayerResults does a regex search and updates the player results file accordingly
        //this function was designed with future functionality inimind. By adding a new regex search and a new update for the match, this function can update any progress that happens in-game
        //added by Don Murphy
        if (messages[messageCount].Contains("#INCREASE#"))
        {
            Regex getMessage = new Regex(@"\*.[^_]*\*");
            Match x = getMessage.Match(messages[messageCount]);
            if (x.Success)
            {
                string final_val = x.Value;

                messages[messageCount].Replace(final_val, "");

                switch (final_val)
                {
                    case "*Lead*":
                        SkillIncreaseGenericDialogue(gameManager.Leadership);
                        //change by Don Murphy
                        UpdatePlayerResults("Final Leadership", playerFileName);
                        break;

                    case "*Team*":
                        SkillIncreaseGenericDialogue(gameManager.Teamwork);
                        //change by Don Murphy
                        UpdatePlayerResults("Final Teamwork", playerFileName);
                        break;

                    case "*Tech*":
                        SkillIncreaseGenericDialogue(gameManager.Technology);
                        //change by Don Murphy
                        UpdatePlayerResults("Final Technology", playerFileName);
                        break;

                    case "*Prof*":
                        SkillIncreaseGenericDialogue(gameManager.Professionalism);
                        //change by Don Murphy
                        UpdatePlayerResults("Final Professionalism", playerFileName);
                        break;

                    case "*Com*":
                        SkillIncreaseGenericDialogue(gameManager.Communication);
                        //change by Don Murphy
                        UpdatePlayerResults("Final Communication", playerFileName);
                        break;

                    case "*Crit*":
                        SkillIncreaseGenericDialogue(gameManager.CritThinking);
                        //change by Don Murphy
                        UpdatePlayerResults("Final Critical Thinking", playerFileName);
                        break;

                    case "*Hr*":
                        SkillIncreaseGenericDialogue(gameManager.HumanResources);
                        break;

                    case "*IT*":
                        SkillIncreaseGenericDialogue(gameManager.IT);
                        break;

                    case "*SDev*":
                        SkillIncreaseGenericDialogue(gameManager.SoftwareDevelopment);
                        break;

                    default:
                        SkillIncreaseGenericDialogue(gameManager.CritThinking);
                        UpdatePlayerResults("Final Critical Thinking", playerFileName);
                        break;
                }
            }
        }


        //These keywords are used to evaluate if the player has a high enough skill. One example we have is where the player cannot talk to an NPC without a high enough Communications skill.
        //Change by Austin Greear 5/18/2020
        if (messages[messageCount].Contains("#CHECK#"))
        {
            Regex getMessage = new Regex(@"\*.[^_]*\*");
            Match x = getMessage.Match(messages[messageCount]);
            if (x.Success)
            {
                string final_val = x.Value;

                messages[messageCount].Replace(final_val, "");

                switch (final_val)
                {
                    case "*Lead*":
                        SkillCheckGenericDialogue(gameManager.Leadership);


                        break;

                    case "*Team*":
                        SkillCheckGenericDialogue(gameManager.Teamwork);
                        break;

                    case "*Tech*":
                        SkillCheckGenericDialogue(gameManager.Technology);
                        break;

                    case "*Prof*":
                        SkillCheckGenericDialogue(gameManager.Professionalism);
                        break;

                    case "*Com*":
                        SkillCheckGenericDialogue(gameManager.Communication);
                        break;

                    case "*Crit*":
                        SkillCheckGenericDialogue(gameManager.CritThinking);
                        break;

                    case "*Hr*":
                        SkillCheckGenericDialogue(gameManager.HumanResources);
                        break;

                    case "*IT*":
                        SkillCheckGenericDialogue(gameManager.IT);
                        break;

                    case "*SDev*":
                        SkillCheckGenericDialogue(gameManager.SoftwareDevelopment);
                        break;


                    default:
                        SkillCheckGenericDialogue(gameManager.CritThinking);
                        break;
                }
            }
        }

        if (messages[messageCount].Contains("#triggerEndGame#"))
        {
            endGame = true;
        }

        //Change by Austin Greear 6/2/2020

        if (messages[messageCount].Contains("#SKILL_CHECK#"))
        {
            SkillCheckDialogue();
        }
        else if (messages[messageCount].Contains("#SKILL_INCREASE_INTELLIGENCE#"))
        {
            SkillIncreaseDialogue(gameManager.Leadership);
        }
        else if (messages[messageCount].Contains("#SKILL_INCREASE_COMMUNICATION#"))
        {
            SkillIncreaseDialogue(gameManager.Professionalism);
        }
        else if (messages[messageCount].Contains("#YES_NO#"))
        {
            //The following section of code is a temporary announcement to the player because clicking doesn't work for yes/no but arrow keys dont work well for selecting interview answers
            //yesnoInstructions boolean added to have this message only appear once.
            if (!messages[messageCount].Contains("(Please use arrow keys + space/enter") && yesNoInstructions)
            {
                messages[messageCount] = messages[messageCount] + " (Please use arrow keys + space/enter to select answer for yes/no questions. All other features: Use mouse click) ";
                yesNoInstructions = false;
            }
            //temporary code ends
            if (YesButton != null && NoButton != null)
            {
                YesButton.onClick.RemoveAllListeners();

                NoButton.onClick.RemoveAllListeners();

                YesButton.onClick.AddListener(ClickYesOption);

                NoButton.onClick.AddListener(ClickNoOption);
            }

            YesNoDialogue();
        }
        else if (messages[messageCount].Contains("#SKIP_START#"))
        {
            Debug.Log("Found Skip_start");
            Debug.Log("This is interviewTaken:" + interviewTaken);
            Debug.Log("Message Count at start: " + messageCount);
            if (!interviewTaken)
            {
                if (messages[messageCount + 1] != null)
                {
                    Debug.Log("Started Skipping");
                    Debug.Log("Message Count inside null: " + messageCount);
                    while (!messages[messageCount].Contains("#SKIP_END#"))
                    {
                        Debug.Log("Skipped one");
                        messageCount = messageCount + 1;
                        Debug.Log("Message Count at change: " + messageCount);


                    }
                    if (messages[messageCount + 1].Contains("#SKIP_END#"))
                    {
                        messageCount = messageCount + 1;
                    }
                    Debug.Log("Message Count at end: " + messageCount);
                    Debug.Log("Stopped Skipping");

                }
            }
            else
            {
                messageCount++;
            }

        }
        else if (messages[messageCount].Contains("#SKIP_END#"))
        {

            interviewTaken = false;
            messageCount++;
            Debug.Log("This is interviewTaken at SKIP_END:" + interviewTaken);
        }
        //These two keywords will spend one or two significant action points
        else if (messages[messageCount].Contains("#SA1#"))
        {
            SignificantActionDialogue(1);
        }
        else if (messages[messageCount].Contains("#SA2#"))
        {
            SignificantActionDialogue(2);
        }
        else
        {
            BasicDialogue();
            if (!InnerDialogue) NameBox.gameObject.SetActive(true);
        }
    }


    // update progress bar with results from interview
    public static void UpdateProgressBar() {
        if (numQuestionsAsked < 4) {
            return;
        }

        var allLines = File.ReadAllLines(playerResultsFile); //read file into lines var

        for (int i = 0; i < numPerfectAnswers; i++) {
            allLines[progressBarLine] += "⬛";
        }

        for (int j = numPerfectAnswers; j < numQuestionsAsked; j++) {
            allLines[progressBarLine] += "⬜";
        }

        allLines[progressBarLine] += " - " + Convert.ToInt32((numPerfectAnswers * 100) / numQuestionsAsked) + "%";

        File.WriteAllLines(playerFileName, allLines); //rewerite file with update

    }

    public UnityEvent Dialogue_Event;

    public UnityEvent end_dialogue_event;

    public List<UnityEvent> Events = new List<UnityEvent>();

    public void Change_Dialogue_Event(int index)
    {
        if (index >= 0 && index < Events.Count)
        {
            Dialogue_Event = Events[index];
        }
    }

    public void Change_Name(string Name)
    {
        NPCName = Name;

        NameBoxText.text = Name;

        NameBox.gameObject.SetActive(true);
        InnerDialogue = false;
    }


    //Change by Austin Greear 5/7/2020
    public void insert_messages(string[] in_messages)
    {
        var message_array = new List<string>();

        string[] message_end = messages.ToList().GetRange(messageCount, messages.Length - messageCount).ToArray();

        message_array.AddRange(in_messages);

        message_array.AddRange(message_end);

        messages = message_array.ToArray();

        messageCount = 0;
    }

    //Change by Austin Greear 5/7/2020
    public void insert_message_at_index(string[] in_messages, int index)
    {
        if ((index + 1 >= 0 && index + 1 < messages.Length) && (index - 1 >= 0 && index - 1 < messages.Length))
        {
            var message_array = new List<string>();

            string[] message_end = messages.ToList().GetRange(index + 1, messages.Length - index - 1).ToArray();

            message_array.Add(messages[messageCount]);

            message_array.AddRange(in_messages);

            message_array.AddRange(message_end);

            messages = message_array.ToArray();

            messageCount = 0;
        }
    }

    //Change by Austin Greear 5/7/2020
    public void add_messages_to_end(string[] in_messages)
    {
        var message_array = new List<string>();

        message_array.AddRange(messages);

        message_array.AddRange(in_messages);

        messages = message_array.ToArray();
    }

    //Change by Austin Greear 5/11/2020
    public void reset_message_count()
    {
        messageCount = 0;
    }



    //Change by Austin Greear 5/11/2020
    public void set_message_count(int i)
    {
        messageCount = i;
    }

    //BasicDialogue() is used for printing messages that are simple, unlike YES_NOs
    public void BasicDialogue()
    {
        StartCoroutine(ShowText(ReplaceKeywords(messages[messageCount])));

        //Will only increment the message count if not at the end of the list of messages
        if (messageCount < messages.Length - 1) messageCount++;

        player.canMove = false;
    }

    //This prompts the user with the YES and NO buttons while also waiting for the user to select an option
    void YesNoDialogue()
    {
        StartCoroutine(ShowTextYesNoQuestion(ReplaceKeywords(messages[messageCount])));

        //Will only increment the message count if not at the end of the list of messages
        //if (messageCount < messages.Length - 1) messageCount++;

        player.canMove = false;
    }

    void SkillIncreaseDialogue(Skill skill)
    {
        StartCoroutine(ShowText(ReplaceKeywords(messages[messageCount])));

        skill.LevelUp();

        //Will only increment the message count if not at the end of the list of messages
        if (messageCount < messages.Length - 1) messageCount++;

        player.canMove = false;
    }

    //Added by Austin Greear 5/18/2020
    void SkillIncreaseGenericDialogue(Skill skill)
    {
        skill.LevelUp();
    }

    void SkillCheckDialogue()
    {
        if (!gameManager.Communication.SkillCheck(EnemyCommunications)) //fails CHARISMA ONLY check
        {
            StartCoroutine(ShowText(ReplaceKeywords(messages[messageCount])));
        }
        else//passes skill check
        {
            messageCount++; //Increments message,
            DecideWhichDialogueToShow();//then decides which kind of message to show
        }
        player.canMove = false;
    }

    void SkillCheckGenericDialogue(Skill skill)
    {
        if (!skill.SkillCheck(EnemyCommunications)) //fails CHARISMA ONLY check
        {
            StartCoroutine(ShowText(ReplaceKeywords(messages[messageCount])));
        }
        else//passes skill check
        {
            messageCount++; //Increments message,
            DecideWhichDialogueToShow();//then decides which kind of message to show
        }
        player.canMove = false;
    }

    void SignificantActionDialogue(int actionAmount)
    {
        Calendar.IncreaseSignificantActions(actionAmount);

        StartCoroutine(ShowText(ReplaceKeywords(messages[messageCount])));

        //Will only increment the message count if not at the end of the list of messages
        if (messageCount < messages.Length - 1) messageCount++;

        player.canMove = false;
    }

    public TextMeshProUGUI target_text;

    public void Set_Target_Text(TextMeshProUGUI text)
    {
        target_text = text;
    }

    //This handles printing each character out in a message, with a very short delay in between printing each character
    IEnumerator ShowText(string message)
    {
        messageDone = false;
        messageIsTyping = true;
        WaitForSeconds waitTime = new WaitForSeconds(delay);
        for (int i = 0; i < message.Length; i++)
        {
            currentMessage = message.Substring(0, i);
            target_text.text = currentMessage;
            if (!isItem)
            {
                if (message[i] != ' ' || message[i] != '.' || message[i] != '?' || message[i] != ',' || message[i] != '!')
                {
                    TalkSFX.pitch = Random.Range(0.4f, 0.75f);
                    TalkSFX.Play();
                }
            }

            yield return waitTime;
        }
        target_text.text = message;

        yield return new WaitForSeconds(0.2f); //@CS adding some padding so people can't blow through the shorter messages as fast

        messageDone = true;
        messageIsTyping = false;
    }

    //Only for yes or no question messages
    //Calls ChangeSizeCoroutine to inflate the YesNoBox
    IEnumerator ShowTextYesNoQuestion(string message)
    {
        messageDone = false;
        messageIsTyping = true;
        WaitForSeconds waitTime = new WaitForSeconds(delay);
        for (int i = 0; i < message.Length; i++)
        {
            currentMessage = message.Substring(0, i);
            UICanvas.GetComponentInChildren<TextMeshProUGUI>().text = currentMessage;
            if (message[i] != ' ')
            {
                TalkSFX.pitch = Random.Range(0.4f, 0.75f);
                TalkSFX.Play();
            }
            yield return waitTime;
        }


        UICanvas.GetComponentInChildren<TextMeshProUGUI>().text = message;

        StartCoroutine(ChangeSizeCoroutine(0.5f, 1f, YesNoBox)); //Inflates yes no box

        YesNoBox.enabled = true;
        for (int i = 0; i < YesNoBox.transform.childCount; i++)
        {
            if (YesNoBox.transform.GetChild(i).gameObject.GetComponent<Button>() != null) YesNoBox.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
            YesNoBox.transform.GetChild(i).gameObject.SetActive(true);
        }

        //Waiting until the YesNoBox is inflated, then sets the buttons to be interactable
        yield return new WaitForSeconds(0.6f);

        for (int i = 0; i < YesNoBox.transform.childCount; i++)
        {
            if (YesNoBox.transform.GetChild(i).gameObject.GetComponent<Button>() != null) YesNoBox.transform.GetChild(i).gameObject.GetComponent<Button>().interactable = true;
        }

        //This puts the focus on the yes button so that the player can use arrow keys to select @CS
        YesNoBox.transform.GetChild(1).GetComponent<Button>().Select();

        messageDone = true;
        messageIsTyping = false;
    }

    //Called after clicking YES. Increments the messageCount up by 2, then shows that message. Currently the Yes message is 2 after the question message, while the NO message is 1 message after the question
    public void ClickYesOption()
    {
        if (isTalking)
        {
            YesNoBox.enabled = false;
            for (int i = 0; i < YesNoBox.transform.childCount; i++)
            {
                YesNoBox.transform.GetChild(i).gameObject.SetActive(false);
            }

            YesNoBox.transform.localScale = new Vector3(0, 0, 0);

            messageCount = messageCount + 2; //Will always be the message following the YES/NO messages
            DecideWhichDialogueToShow();
        }
    }

    //Called after clicking NO. Simply shows the message after the question, but does NOT increment the messageCount variable.
    //This is done to keep the NPC stuck on the question until the player answers yes.
    //Feel free to change this up.
    //Future implementation could have the user click yes or no, it would show the yes or no message, then it would read in what to do from there.
    //AKA the YES or NO message could include a keyword to set the messageCount back to the question message or to continue it.
    public void ClickNoOption()
    {
        if (isTalking)
        {
            YesNoBox.enabled = false;
            for (int i = 0; i < YesNoBox.transform.childCount; i++)
            {
                YesNoBox.transform.GetChild(i).gameObject.SetActive(false);
            }

            YesNoBox.transform.localScale = new Vector3(0, 0, 0);

            //TODO, change this so that it uses DecideWhichDialogueToShow(); since it does not allow for tags
            StartCoroutine(ShowText(ReplaceKeywords(messages[messageCount + 1])));//The no message in the list

            // Temporary code to allow the job to day to end if the player says no to a Manager request.
            if (messages[messageCount + 1].Contains("#END_WORK#"))
            {
                end_dialogue();
                Work_Event_Manager.instance.end_work_day();
            }
        }
    }

    public Vector3 prev_position;

    public void set_position(string vector3)
    {
        Vector3 vector = transform.localPosition;

        prev_position = transform.localPosition;

        string[] splitStr = vector3.Split(' ');

        for (int i = 0; i < splitStr.Length; i++)
        {
            if (i == 0)
            {
                vector.x = float.Parse(splitStr[i]);
            }
            else if (i == 1)
            {
                vector.y = float.Parse(splitStr[i]);
            }
            else
            {
                vector.z = float.Parse(splitStr[i]);
            }
        }

        transform.localPosition = vector;
    }

    public void return_to_prev_position()
    {
        transform.localPosition = prev_position;
    }
}
