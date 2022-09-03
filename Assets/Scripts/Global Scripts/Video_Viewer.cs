using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video_Viewer : MonoBehaviour
{
    public List<TalkToNPC> video_objects = new List<TalkToNPC>();

    public void load_video_players() 
    {
        for (int i = 0; i < video_manager.instance.viewed_videos.Count; i++) 
        {
            if (i >= 0 && i < video_objects.Count) 
            {

            }
        }
    }

    public void play_video(VideoClip video) 
    {
        video_manager.instance.player.clip = video;

        video_manager.instance.player.gameObject.SetActive(true);
    }
}
