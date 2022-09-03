﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class video_manager : MonoBehaviour
{
    //Author: Austin Greear
    //Puprpose: Script used to handle the playing and skipping of video clips.

    public static video_manager instance;

    public VideoPlayer player;

    public List<VideoClip> viewed_videos = new List<VideoClip>();

    public Fade stored_fade;
    //public bool play_first = true;

    public void play_video_clip(Video_manager_clip vmc) 
    {
        player.playbackSpeed = 1.0f;

        start_event.RemoveAllListeners();

        start_event = vmc.start_event;

        start_event.AddListener(invoke_basic_start_event);



        end_event = vmc.end_event;



        stored_event.RemoveAllListeners();

        stored_event = vmc.stored_event;

        stored_event.AddListener(invoke_basic_stored_event);

        //player.source = VideoSource.VideoClip;
        //player.clip = vmc.clip;

        this.gameObject.SetActive(true);

        load_clip(vmc);

        player.Play();
        
    }

    public void load_clip(Video_manager_clip vmc)
    {
        if (vmc.play_first)
        {
#if (UNITY_EDITOR || UNITY_STANDALONE)
                            player.url = vmc.clip_url;
                            player.source = VideoSource.Url;
                            
#elif (UNITY_WEBGL)
                            player.url = vmc.clip_url;
                            player.source = VideoSource.Url;    
#endif
            vmc.play_first = false;
        }
        else
        {

#if (UNITY_EDITOR || UNITY_STANDALONE)
                            player.url = vmc.clip2_url;
                            player.source = VideoSource.Url;
                            
#elif (UNITY_WEBGL)
                            player.url = vmc.clip2_url;
                            player.source = VideoSource.Url;    
#endif
        }
    }

    public void Update() 
    {
        //Skiping Video
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E)) && player.playbackSpeed != 0.0f && (player.time > 1.0f) && (player.time < player.length)) 
        {
            player.playbackSpeed = 0.0f;

            fade_in_camera(stored_fade);
        }
    }

    public void add_to_video_list() 
    {
        VideoClip clip = player.clip;

        if (!viewed_videos.Find((x) => x.name == clip.name)) 
        {
            viewed_videos.Add(clip);
        }
    }

    public string default_clip;

    public void Awake()
    {
        instance = this;

        player.started += video_start_event;

        player.loopPointReached += video_end_event;

        this.gameObject.SetActive(false);

        player.source = VideoSource.Url;
        //.mp4
        //player.url = System.IO.Path.Combine(Application.streamingAssetsPath, default_clip + ".mov");
        player.url = System.IO.Path.Combine(Application.streamingAssetsPath, default_clip + ".mp4");
    }

    public UnityEvent start_event;

    public UnityEvent end_event;

    public UnityEvent stored_event;

    public UnityEvent basic_start_event;

    public void invoke_basic_start_event() 
    {
        basic_start_event.Invoke();
    }

    public UnityEvent basic_stored_event;

    public void invoke_basic_stored_event()
    {
        basic_stored_event.Invoke();
    }

    public void toggle_player(VideoPlayer vp) 
    {
        Debug.Log("End???");

        if (vp.isPaused)
        {
            vp.Play();
        }
        else 
        {
            vp.Pause();
        }
    }

    public void video_start_event(VideoPlayer vp)
    {
        start_event.Invoke();
    }

    public void video_end_event(VideoPlayer vp)
    {
        end_event.Invoke();
    }

    public void fade_in_camera(Fade fade)
    {
        fade.complete_fade_event = stored_event;

        fade.FadeOut();
    }
}
