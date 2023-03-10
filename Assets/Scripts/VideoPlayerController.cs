using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;


public class VideoPlayerController : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public RenderTexture rt;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += EndReached;
    }

    void Start()
    {
        videoPlayer.Play();
    }

    void EndReached(VideoPlayer vp)
    {
       SceneManager.LoadScene("NewMainMenu");
     }
}


