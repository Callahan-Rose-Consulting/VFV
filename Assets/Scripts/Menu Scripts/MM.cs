using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MM : MonoBehaviour
{
    [SerializeField] public GameObject x;
    void Start()
    {
        
    }

    public void loadGame() //used to instantiate theeloaded save data from a previous gameplay session
    {
       GameObject menu=(GameObject) Instantiate(Resources.Load("NewGameOrLoad"));
       menu.transform.parent = x.transform.parent;
    }

    public void quit() //advanced quit function thats take into account what type of game build is running and quits accordingly
    {
        #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE)
            Application.Quit();
        #elif (UNITY_WEBGL)
            Application.OpenURL("https://tinyurl.com/VetsandRC");
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
