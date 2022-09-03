using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class FastTravelScript : MonoBehaviour
{
    public UnityEvent interactEvent;

    // Update is called once per frame
    void Update()
    {
    }

    public void instant_teleport(GameObject location)
    {
        GameManager.instance.player_character.gameObject.transform.position = location.transform.position;
    }
}
