using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLButtons : MonoBehaviour
{
    bool isNetworkAvailable = true; //set to false if using CheckConnection and Start;
    
    /*  Will not work for WebGL */
    IEnumerator CheckConnection() {

        /*const float timeout = 2f;
        float startTime = Time.timeSinceLevelLoad;
        var ping = new Ping("8.8.8.8");
        Debug.Log("Checking network...");
        while (true) {
            if (ping.isDone) {
                Debug.Log("Network available.");
                isNetworkAvailable = true;
                yield break;
            }
            if (Time.timeSinceLevelLoad - startTime > timeout) {
                Debug.Log("No network.");
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }*/

        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {
            isNetworkAvailable = false;
        }
        else
        {
            isNetworkAvailable = true;
        }


    }
    void Start() {
    #if (UNITY_EDITOR || UNITY_STANDALONE || DEVELOPMENT_BUILD)
        //[StartCoroutine(CheckConnection());
    #endif
    }

    public void FordFund() {
        LaunchURL("https://www.fordfund.org/");
    }
    
    public void AddHelp() {
        //LaunchURL("https://tinyurl.com/VetsandRC");
        LaunchURL("https://sites.google.com/a/umich.edu/mike-callahan/home?authuser=0");


    }

    public void LinkedinPage()
    {
        LaunchURL("https://www.linkedin.com/groups/14140078/");
    }

    public void GitHubPage()
    {
        LaunchURL("https://github.com/Callahan-Rose-Consulting/VFV");
    }
    
    private void LaunchURL(string URL) {
        if (!isNetworkAvailable) {
            GameObject.Find("NetworkErrPanel").transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            Application.OpenURL(URL);
        }
        
    }
    
}
