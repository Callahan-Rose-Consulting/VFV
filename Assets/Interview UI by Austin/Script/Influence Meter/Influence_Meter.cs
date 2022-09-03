using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Influence_Meter : MonoBehaviour
{
    //Author: Austin Greear
    //Puprpose: Script used to manage the animations of the influence meter.

    public float speed = 0.5f;

    public Transform normal_transform;

    public Transform follow_transform;

    public bool reduce = false;

    public float Max_Xscale = 2.2f;

    public Animator anim;

    public TextMeshProUGUI point_text;

    public TextMeshProUGUI background_point_text;

    public void Start() 
    {
        anim = GetComponent<Animator>();
    }

    //Pre: None
    //Post: interpolates the scale of the transforms to each other based off of the reduce variable
    void Update()
    {
        if (!reduce)
        {
            //Interpolates the normal transform to the follow transform for going up
            normal_transform.localScale = Vector3.Lerp(normal_transform.localScale, new Vector3(follow_transform.localScale.x, normal_transform.localScale.y, normal_transform.localScale.z), speed * Time.deltaTime);
        }
        else
        {
            //Interpolates the follow transform to the normal transform for going down
            follow_transform.localScale = Vector3.Lerp(follow_transform.localScale, new Vector3(normal_transform.localScale.x, follow_transform.localScale.y, follow_transform.localScale.z), speed * Time.deltaTime);
        }
    }

    //Pre: a float for the amount of meter gain
    //Post: modifies the scale of the normal or follow transform dependant on the value. i.e if the amount is negative reduce is set to true and the normal transform is modified. 
    public bool increment_meter(float amount)
    {
        anim.SetTrigger("Toggle");

        if (amount < 0)
        {
            point_text.text = "-" + (Mathf.Abs(amount) * 100) + "%";

            reduce = true;

            normal_transform.localScale = new Vector3(normal_transform.localScale.x + (amount * Max_Xscale), normal_transform.localScale.y, normal_transform.localScale.z);

            if (normal_transform.localScale.x < 0.0f)
            {
                normal_transform.localScale = new Vector3(0.0f, normal_transform.localScale.y, normal_transform.localScale.z);
            }

        }
        else 
        {
            point_text.text = "+" + (amount * 100) + "%";

            reduce = false;

            follow_transform.localScale = new Vector3(follow_transform.localScale.x + (amount * Max_Xscale), follow_transform.localScale.y, follow_transform.localScale.z);
        }

        background_point_text.text = point_text.text;

        if (follow_transform.localScale.x >= Max_Xscale)
        {
            follow_transform.localScale = new Vector3(Max_Xscale, follow_transform.localScale.y, follow_transform.localScale.z);

            return true;
        }
        else
        {
            return false;
        }
    }

    public void test_increment_meter(float amount)
    {
        anim.SetTrigger("Toggle");

        if (amount < 0)
        {
            point_text.text = "-" + (Mathf.Abs(amount) * 100) + "%";

            reduce = true;

            normal_transform.localScale = new Vector3(normal_transform.localScale.x + (amount * Max_Xscale), normal_transform.localScale.y, normal_transform.localScale.z);

            if (normal_transform.localScale.x < 0.0f)
            {
                normal_transform.localScale = new Vector3(0.0f, normal_transform.localScale.y, normal_transform.localScale.z);
            }

        }
        else
        {
            point_text.text = "+" + (amount * 100) + "%";

            reduce = false;

            follow_transform.localScale = new Vector3(follow_transform.localScale.x + (amount * Max_Xscale), follow_transform.localScale.y, follow_transform.localScale.z);
        }

        background_point_text.text = point_text.text;

        if (follow_transform.localScale.x >= Max_Xscale)
        {
            follow_transform.localScale = new Vector3(Max_Xscale, follow_transform.localScale.y, follow_transform.localScale.z);

            return;
        }
        else
        {
            return;
        }
    }

    public void reset_meter()
    {
        reduce = true;

        normal_transform.localScale = new Vector3(0.0f, normal_transform.localScale.y, normal_transform.localScale.z);
    }

    //Pre: None
    //Post: Returns the percentage of the meter's scale
    public float meter_percentage() 
    {
        if (reduce)
        {
            return normal_transform.localScale.x / Max_Xscale;
        }
        else 
        {
            return follow_transform.localScale.x / Max_Xscale;
        }
    } 
}
