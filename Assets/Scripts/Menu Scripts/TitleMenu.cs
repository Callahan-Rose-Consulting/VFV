using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    public GameObject fullscreenToggle;

    private void Start() {
        // Minimizing the Options Panel
        GameObject.Find("OptionsPanel").transform.localScale = new Vector3(0, 0, 0);

        //initializes fullscreen toggle
        fullscreenToggle = GameObject.Find("FSToggle");
        if (Screen.fullScreen) {
            fullscreenToggle.GetComponent<Toggle>().isOn = true;
        }
        else {
            fullscreenToggle.GetComponent<Toggle>().isOn = false;
        }
        /**/
    }
    
    public void PlayGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void QuitGame() {
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
    public GameObject optionsPanel;
    public void ShowOptions() {
        GameObject.Find("OptionsPanel").transform.localScale = new Vector3(2, 2, 1);
    }
    public void HideOptions() {
        GameObject.Find("OptionsPanel").transform.localScale = new Vector3(0, 0, 0);
    }
    public void HideNetworkErr() {
        GameObject.Find("NetworkErrPanel").transform.localScale = new Vector3(0, 0, 0);
    }
    public void ToggleFullScreen() {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void ShowCredits() {
        GameObject.Find("CreditsPanel").transform.localScale = new Vector3(3, 3, 1);
    }
    public void HideCredits() {
        GameObject.Find("CreditsPanel").transform.localScale = new Vector3(0, 0, 0);
    }
   
}

