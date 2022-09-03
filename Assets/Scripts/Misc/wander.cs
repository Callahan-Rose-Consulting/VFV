using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class wander : MonoBehaviour
{
    public Rigidbody2D _body;
    private bool stopwandering=false;
    [SerializeField] private GameObject attention;
    public bool noticeMe = true;
    private Animator _animator;
    public Vector3 goHere;
    public Vector3 canvasLoc;
    public double zoom = 1;
    public GameObject PlayersCam;
    //Small change by Austin Greear 5/11/2020
    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        attention.GetComponent<SpriteRenderer>().enabled = false;
        _body.isKinematic = false;

        StartCoroutine("randomlocation");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")//@CS
        {
            Debug.Log("Player is Approcahing");
            //PlayersCam=other.gameObject.transform.GetChild(0).gameObject; this doesnt work since when the AI hits another collider,
            //then it will look for a child on that object.

            PlayersCam = GameObject.Find("Main Camera");//@CS

            //  show = true;
            stopwandering = true;
            _body.isKinematic = true;
            if (noticeMe)
            {
                Debug.Log("Hey");
                attention.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }



    private void OnTriggerExit2D(Collider2D other)
    {
        _body.isKinematic = false;
        stopwandering = false;
        if (noticeMe)
        {
            attention.GetComponent<SpriteRenderer>().enabled = false;

        }
    }

    IEnumerator randomlocation()
    {
        while (true)
        {
            if (!stopwandering)
            {
                var x = 2f * Random.insideUnitCircle;
                if (x.x > x.y)
                    _body.velocity = new Vector2(x.x, 0);
                else
                {
                    _body.velocity = new Vector2(0, x.y);
                }
            }
            else
            {
                _body.velocity = Vector2.zero;

            }

            yield return new WaitForSeconds(1);
                _body.velocity = Vector2.zero;
                yield return new WaitForSeconds(5);
            }





    }

    double customLerp(double x)
    {
        return (1 / (1 + Math.Pow(Math.E, (-10 * x + 5))));
    }
    IEnumerator letmeshowyouwheretogo()
    {
        for (double i = .15; i < 1; i += .05)
        {
            var x=customLerp(i);
            Debug.Log(x);
            yield return new WaitForSeconds(.02f);
            PlayersCam.transform.localPosition = goHere * (float) x;
        }
        yield return new WaitForSeconds(2);
        PlayersCam.transform.localPosition = new Vector3(0,0,-1);

        yield return null;
    }
    IEnumerator canvasZoom()
    {
        yield return new WaitForSeconds(1);
        for (double i = .15; i < 1; i += .05)
        {
            var x=customLerp(i);
            Debug.Log(x);
            yield return new WaitForSeconds(.02f);
            PlayersCam.transform.localPosition = canvasLoc * (float) x;
            PlayersCam.GetComponent<Camera>().orthographicSize = 7 - ((6 * (float) x));
        }

        yield return null;
    }

    public void gotothislocation()
    {
        StartCoroutine("letmeshowyouwheretogo");
    }

    public void zoomonCanvas()
    {
        StartCoroutine("canvasZoom");
    }

    public void resetCamera()
    {
        PlayersCam.transform.localPosition = new Vector3(0,0,-1);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
