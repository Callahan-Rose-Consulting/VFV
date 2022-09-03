/*
 This script alters a sprite's Order in Layer attribute in its Sprite Renderer.
 This is done to allow the player to be able to walk in front of and behind objects,
  layering objects on top of each other properly.

Note: This script seems to act differently in the unity editor if not using Standalone (1920*1080). 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSpriteSorter : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public int height = 150;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void LateUpdate()
    {
        //could change this so it changes the sorting layer instead of the order in the layer
        //spriteRenderer.sortingOrder = (int)Camera.main.WorldToScreenPoint(spriteRenderer.bounds.min).y * -1 + 150;
        spriteRenderer.sortingOrder = (int)Camera.main.WorldToScreenPoint(spriteRenderer.bounds.min).y * -1 + height;
        
    }
}
