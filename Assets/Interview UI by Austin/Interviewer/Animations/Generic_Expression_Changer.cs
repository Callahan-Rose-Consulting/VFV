using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generic_Expression_Changer : MonoBehaviour
{
    //Author: Austin Greear
    //Puprpose: Script used to manage the expressions of an interviewer during dialogue.
    //It can change them through setting up a sprite index to change the given images sprite.

    public Image image;

    public int Expression_Index = 0;

    public List<Sprite> Expressions = new List<Sprite>();

    public void Start() 
    {
        image = GetComponent<Image>();
    }

    public void generic_set_expression() 
    {
        if (Expression_Index >= 0 && Expression_Index < Expressions.Count) 
        {
            if (image != null) 
            {
                image.sprite = stored_sprite;
            }
        }
    }

    public Sprite stored_sprite;

    public void change_expression_int(int index) 
    {
        Expression_Index = index;

        stored_sprite = Expressions[Expression_Index];
    }

    public void change_expression_sprite(Sprite sprite)
    {
        stored_sprite = sprite;
    }
}
