/*
 This class handles getting player stats and setting them within the players stats menu upon pressing TAB
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GetPlayerStats : MonoBehaviour
{
    private TMP_Text statNames, statValues;
    [SerializeField]
    private GameObject managerObj;
    private GameManager manager;
    private Skill leadership, teamwork, technology, professionalism, communication, critThinking;
    // Start is called before the first frame update
    void Awake() 
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        manager = managerObj.GetComponent<GameManager>();
        statNames = this.transform.Find("Stat Names").GetComponentInChildren<TMP_Text>();
        statValues = this.transform.Find("Stat Values").GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        leadership = manager.Leadership;
        teamwork = manager.Teamwork;
        technology = manager.Technology;
        professionalism = manager.Professionalism;
        communication = manager.Communication;
        critThinking = manager.CritThinking;
        statNames.SetText(leadership.Name + "\n" + teamwork.Name + "\n" + technology.Name + "\n" + professionalism.Name + "\n" + communication.Name + "\n" + critThinking.Name);
        statValues.SetText(leadership.Level + "\n" + teamwork.Level + "\n" + technology.Level + "\n" + professionalism.Level + "\n" + communication.Level + "\n" + critThinking.Level);
    }

    public Animator anim;

    public void OnEnable() 
    {

    }

    public void LoadInterview()
    {
        SceneManager.LoadScene("DialogueTest");
    }

    public void LoadResume()
    {
        SceneManager.LoadScene("Resume");
    }

    // World Map

    public void Set_x_Scale(float scale)
    {
        this.gameObject.transform.localScale = new Vector3(scale, this.gameObject.transform.localScale.y, this.gameObject.transform.localScale.z);
    }
}
