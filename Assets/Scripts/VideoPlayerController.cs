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

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.E))
        {
            videoPlayer.Stop();
            SceneManager.LoadScene("NewMainMenu");
        }
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


