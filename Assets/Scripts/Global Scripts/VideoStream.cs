using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoStream : MonoBehaviour
{
    public RawImage Image;

    public VideoPlayer video;

    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        video = GetComponent<VideoPlayer>();

        StartCoroutine("PlayVideo");

    }

    IEnumerator PlayVideo()
    {
        video.Prepare();
        WaitForSeconds waitForSeconds=new WaitForSeconds(.25f);
        while (!video.isPrepared)
        {
            yield return waitForSeconds;
        }

        Image.texture = video.texture;
        video.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
