using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Scene_Manager : MonoBehaviour
{
    //Author: Austin Greear
    //Purpose: The purpose of this script is to manage the loading and initialization scenes.
    //By initialization of scenes i mean thing like the fact that it invokes the Scene_Start_Event unity event at the start of the scripts initialization.

    public static Scene_Manager Instance;

    public UnityEvent Scene_Start_Event;

    //Pre: None
    //Post: Initializes the instance, Invokes the Scene start event.
    public void Start()
    {
        Instance = this;

        Scene_Start_Event.Invoke();

        Debug.Log("Start");
    }

    //Pre: Takes a string as the name of the scene to load
    //Post: Loads the scene by the string given
    public void Load_Scene_By_Name(string name) 
    {
        SceneChange.PreSceneChange(); // Save inventory and currency before changing scenes
        SceneManager.LoadScene(name);
    }

    public void SetTimeScale(float scale) 
    {
        Time.timeScale = scale;
    }

    public void SceneStart()
    {
        if (Resume.current_job != null) 
        {

        }
    }
}
