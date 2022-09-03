using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class bulletinBoard : MonoBehaviour
{
    
    public static List<string> messages=new List<string>();
    
    public bool bulletinBoardOpen = false;
    public GameObject BulletinBoardUI;
    public GameObject BoardText;
    public GameObject player;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

   
    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetButtonDown("Interact") && other.CompareTag("Player") )
        {

                if (bulletinBoardOpen)
                {
                    bulletinBoardOpen = false;
                    BulletinBoardUI.GetComponent<Canvas>().enabled = false;
                    player.GetComponent<Player_Movement>().canMove = true;
                }
                else
                {
                    if (messages.Count > 1)
                    {
                        BoardText.GetComponent<Text>().text = string.Join(Environment.NewLine, messages.Skip(1).ToArray());
                    }
                    else
                    {
                        BoardText.GetComponent<Text>().text = string.Join(Environment.NewLine, messages.ToArray());
                    }
                    bulletinBoardOpen = true;
                    BulletinBoardUI.GetComponent<Canvas>().enabled = true;
                    player.GetComponent<Player_Movement>().canMove = false;
                }
            
        }
    }


 
}
