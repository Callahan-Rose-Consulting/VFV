/*
 This script handles the player movement, as you could imagine ;)
 One important variable is canMove, which can be altered from other scripts, mainly TalkToNPC.cs
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Player_Movement : MonoBehaviour
{
    Rigidbody2D body;
    private Animator animator;
    [SerializeField]
    private GameObject Stats;
    float horizontal;
    float vertical;
    float moveLimiter = 0.75f;
    public float runSpeed = 12.0f;
    public bool canMove;
    SpriteRenderer renderer;

    public GameObject bulletinBoard;
    GameObject oDebug;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();

        canMove = true;
        this.gameObject.transform.position = SaveLoad.lastPlayerLoc;

        oDebug = GameObject.Find("Debug Options");
        oDebug.SetActive(false);

    }

    void Update()
    {
        transform.Rotate(0, 0, 0);
        // Gives a value between -1 and 1

        if (bulletinBoard.GetComponent<bulletinBoard>().bulletinBoardOpen == false)
        {
            /*
            if (Input.GetKeyDown(KeyCode.Tab) && !Stats.active)
            {
                Stats.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                Stats.SetActive(false);
            }
            */
        }
        if (canMove)
        {


            horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
            vertical = Input.GetAxisRaw("Vertical"); // -1 is down

            bool up_input = (Input.GetKey("up") || Input.GetKey("w"));

            bool down_input = (Input.GetKey("down") || Input.GetKey("s"));

            bool left_input = (Input.GetKey("left") || Input.GetKey("a"));

            bool right_input = (Input.GetKey("right") || Input.GetKey("d"));

            bool resume = Input.GetKey("r");

            bool cheat = Input.GetKeyDown(KeyCode.C);

            bool EndSummary = Input.GetKey(";");

            if (down_input && !up_input) {
                if (body.velocity.y < 0f) {
                    animator.SetTrigger("WalkingDown");
                }
            }
            else if (up_input && !down_input) {
                if (body.velocity.y > 0f) {
                    animator.SetTrigger("WalkingUp");
                }
            }
            else if (left_input && !right_input) {
                if (body.velocity.x < 0f) {
                    animator.SetTrigger("WalkingLeft");
                }
            }
            else if (right_input && !left_input) {
                if (body.velocity.x > 0f) {
                    animator.SetTrigger("WalkingRight");
                }
            }
            else if (cheat)
            {
                if (oDebug.activeSelf)
                {
                    oDebug.SetActive(false);
                }
                else
                {
                    oDebug.SetActive(true);
                }
            }
                //bool bCheat = GameObject.Find("Debug Options").activeSelf;
                ////GameObject oCheat = GameObject.Find("Debug Options").activeSelf ;
                //if (GameObject.Find("Debug Options").activeSelf)
                //{
                //    GameObject.Find("Debug Options").SetActive(false);
                //}
                //else
                //{
                //    GameObject.Find("Debug Options").SetActive(true);
                //}
            else if (resume)
            {
                SceneChange.PreSceneChange();
                SceneManager.LoadScene("Resume");
            }
            else if (EndSummary)
            {
                SceneChange.PreSceneChange();
                SceneManager.LoadScene("End Summary");
            }
            else {
                animator.SetTrigger("Idle");
            }
        }
        else
        {
            animator.SetTrigger("Idle");
            horizontal = 0;
            vertical = 0;
        }
        
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
            //animator.SetTrigger("Idle");
        }

        body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    public Vector3 prev_position;

    public void set_position(string vector3)
    {
        Vector3 vector = transform.localPosition;

        prev_position = transform.localPosition;

        string[] splitStr = vector3.Split(' ');

        for (int i = 0; i < splitStr.Length; i++)
        {
            if (i == 0)
            {
                vector.x = float.Parse(splitStr[i]);
            }
            else if (i == 1)
            {
                vector.y = float.Parse(splitStr[i]);
            }
            else
            {
                vector.z = float.Parse(splitStr[i]);
            }
        }

        transform.localPosition = vector;
    }

    public void return_to_prev_position() 
    {
        transform.localPosition = prev_position;
    }

    public void set_can_move(bool b) 
    {
        canMove = b;
    }
}
