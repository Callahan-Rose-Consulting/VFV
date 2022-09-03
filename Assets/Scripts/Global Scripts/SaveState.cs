using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveState : MonoBehaviour
{
    public static SaveState Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public int leadership;
    public int teamwork;
    public int technology;
    public int professionalism;
    public int communication;
    public int critThinking;
    public float textSpeed;
    public string playerName;
    public Vector3 playerLoc;

    public void SavePlayer()
    {
        SaveState.Instance.leadership = leadership;
        SaveState.Instance.teamwork = teamwork;
        SaveState.Instance.technology = technology;
        SaveState.Instance.professionalism = professionalism;
        SaveState.Instance.communication = communication;
        SaveState.Instance.critThinking = critThinking;
        SaveState.Instance.textSpeed = textSpeed;
        SaveState.Instance.playerName = playerName;
        SaveState.Instance.playerLoc = playerLoc;
    }



}
