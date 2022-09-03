using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    //Author: Austin Greear
    //Purpose: The purpose of this script is to handle smooth zooming in and out of the camera used in some parts of the overworld

    public Camera cam;

    public float speed;

    void Start() 
    {
        cam = Camera.main;
    }

    public void modify_cam_distance(float target_distance) 
    {
        StopAllCoroutines();

        StartCoroutine(Lerp_Distance(target_distance));
    }

    IEnumerator Lerp_Distance(float target_distance)
    {
        float counter = 0.0f;

        float margin_of_error = .01f;

        while (cam.orthographicSize < (target_distance - margin_of_error) || cam.orthographicSize > (target_distance + margin_of_error))
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, target_distance, speed * Time.deltaTime);

            if (counter > 10000)
            {
                Debug.Log("INFINITE!!!");
                break;
            }

            counter += Time.deltaTime;

            yield return null;
        }
    }
}
