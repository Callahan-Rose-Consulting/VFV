using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBulletinBoard : MonoBehaviour
{
    public GameObject bulletinImage;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            HideBulletin();
        }
    }

    void DisplayBulletin()
    {
        bulletinImage.SetActive(true);
    }

    void HideBulletin()
    {
        bulletinImage.SetActive(false);
    }

    public void ShowBulletin()
    {
        DisplayBulletin();
    }
}
