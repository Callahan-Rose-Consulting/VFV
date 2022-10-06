using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class Video_manager_clip : MonoBehaviour
{
    //Author: Austin Greear
    //Puprpose: Script used to store clip information to be used by the video manager

    public UnityEvent start_event;

    public UnityEvent end_event;

    public UnityEvent stored_event;

    //public Video_Clip clip;

    public VideoClip clip;

    public string clip_url;

    public VideoClip clip2;

    public bool play_first = true;

    public string clip2_url;

    public string resultsVideo;
   
    public string resultsVideo2;  //Don Murphy- This was created to handle the fact that Syed and Dujon both play 2 videos

    void Start() 
    {

        if (clip != null)
        {
            clip_url = System.IO.Path.Combine(Application.streamingAssetsPath, clip.name + ".mp4");

            
        }
        else 
        {
            clip_url = System.IO.Path.Combine(Application.streamingAssetsPath, clip_url + ".mp4");
        }

        if (clip2 != null)
        {
            clip2_url = System.IO.Path.Combine(Application.streamingAssetsPath, clip2.name + ".mp4");
        }
        else
        {
            clip2_url = System.IO.Path.Combine(Application.streamingAssetsPath, clip2_url + ".mp4");
        }
    }

    public void play_clip() 
    {
        video_manager.instance.play_video_clip(this);
    }
}
