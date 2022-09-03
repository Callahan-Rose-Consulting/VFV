/*
 This script handles the coroutines for fading the screen in and out.
 Simply, what it does is justs adjusts a black panel's alpha up and down
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Fade : MonoBehaviour
{
    public static Fade instance;

    public Image blackFade;

    public UnityEvent complete_fade_event;

    public GameObject taxiButton;

    void Awake() 
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FadeIn();
    }
    
    public void FadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInCoroutine());
    }

    IEnumerator FadeInCoroutine()
    {
        taxiButton.SetActive(true);
        clear_events();
        blackFade.enabled = true;
        blackFade.canvasRenderer.SetAlpha(1f);
        WaitForSeconds waitTime = new WaitForSeconds(1);
        blackFade.CrossFadeAlpha(0f, 1f, false);
        yield return waitTime;
        blackFade.enabled = false;
    }

    public void FadeOut()
    {
        StopAllCoroutines();
        blackFade.enabled = true;
        blackFade.canvasRenderer.SetAlpha(0f);
        blackFade.CrossFadeAlpha(1f, 1f, false);
        blackFade.enabled = true;

        StartCoroutine(FadeOutEventCoroutine(1f));
    }

    public void FadeOut_With_Event(UnityEvent u_event)
    {
        complete_fade_event = u_event;

        blackFade.enabled = true;
        blackFade.canvasRenderer.SetAlpha(0f);
        blackFade.CrossFadeAlpha(1f, 1f, false);
        blackFade.enabled = true;

        StartCoroutine(FadeOutEventCoroutine(1f));
    }

    IEnumerator FadeOutEventCoroutine(float time)
    {
        taxiButton.SetActive(false);
        WaitForSeconds waitTime = new WaitForSeconds(time);

        yield return waitTime;

        if (complete_fade_event != null)
        {
            complete_fade_event.Invoke();
        }
    }

    UnityEvent empty_event;

    public void clear_events() 
    {
        complete_fade_event = empty_event;
    }
}
