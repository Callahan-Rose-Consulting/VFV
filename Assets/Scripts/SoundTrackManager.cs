using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrackManager : MonoBehaviour
{
    private AudioSource[] audioPlayers;
    private bool faded = false;

    public AudioClip[] soundtracks;
    public float soundtrackVol = 0.5f;

    public AudioClip[] ambience;
    public float ambientVol = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayers = GetComponents<AudioSource>();

        audioPlayers[0].volume = 0.5f;

        PlaySoundtrack();
        //PlayAmbience();
    }

    private void PlaySoundtrack()
    {
        AudioClip clip = soundtracks[Random.Range(0, soundtracks.Length)];
        audioPlayers[0].clip = clip;
        audioPlayers[0].volume = 0;
        audioPlayers[0].Play();

        Invoke("PlaySoundtrack", clip.length + 1f);
    }

    private void PlayAmbience()
    {
        AudioClip clip = ambience[Random.Range(0, ambience.Length)];
        audioPlayers[1].clip = clip;
        audioPlayers[1].loop = true;
        audioPlayers[1].volume = 0;
        audioPlayers[1].Play();
    }

    private void AudioFadeIn(AudioSource source)
    {
        float targetVol = soundtrackVol;
        if (source.Equals(audioPlayers[1]))
            targetVol = ambientVol;

        if (source.volume == targetVol)
            return;

        if (source.volume < targetVol - 0.01f)
        {
            float cubic = Mathf.Clamp(1 - ((targetVol - source.volume) / targetVol), 0.01f, 1f);
            source.volume = Mathf.Lerp(source.volume, Mathf.Lerp(source.volume, targetVol, Time.deltaTime), cubic);
        }
        else
        {
            source.volume = targetVol;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AudioFadeIn(audioPlayers[0]);
        AudioFadeIn(audioPlayers[1]);
    }
}
