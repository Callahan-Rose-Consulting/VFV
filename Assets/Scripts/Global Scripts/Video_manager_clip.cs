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

    void Start() 
    {

        if (clip != null)
        {
            clip_url = System.IO.Path.Combine(Application.streamingAssetsPath, clip.name + ".mp4");

            /*
           Creator: Chase Best
           Purpose: These if/ else if statements start when a video is played. The conditions match for the specific videos in the game files.
                    When a match is found, the variable is set and is passed to the UpdatePlayerResults function in TalkToNPC.cs
           */
            if (clip.name == "Interview Prep")
            {
                string videoWatched = "Interview Prep";
            }
            else if (clip.name == "Networking with Voice Over")
            {
                string videoWatched = "Networking"
            }
            else if (clip.name == "Personal Branding")
            {
                string videoWatched = "Personal Branding"
            }
            else if (clip.name == "Star")
            {
                string videoWatched = "Star"
            }
            else if (clip.name == "Survive Adapt and Flourish")
            {
                string videoWatched = "Survive Adapt and Flourish"
            }
            else if (clip.name == "Sweet Spot")
            {
                string videoWatched = "Sweet Spot"
            }
            else if (clip.name == "Sweet Spot with Voice Over 2")
            {
                string videoWatched = "Sweet Spot"
            }
            else if (clip.name == "Value")
            {
                string videoWatched = "Value"
            }
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
