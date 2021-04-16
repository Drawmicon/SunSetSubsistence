using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementController : MonoBehaviour
{
    public dayNightCycle_Script dnc;
    public CharacterController controller;
    public float speed, runSpeed, slowSpeed, slowRunSpeed;
    private float defaultSpeed;

    public Vector3 velocity;
    public float gravity, normalGravity, glideGravity;
    public LayerMask [] ground;

    public float jumpForce, bumpForce;

    private bool glideMode;

    public float wingTime, maxWingTime;//timer for wings to rest action

    public player_Script ps;

    public float fallVelocity;

    public float punchDamage;

    public soundCollider sc;//controls sound collider size
    private Vector3 curPos, lastPos;//
    public bool playerMoving;//checks if their is movement for the player

    public armController ac;

    public cinemaCameraExtra cce;

    public bool isLoud, isAttacking;
    // Start is called before the first frame update
    void Start()
    {
        cce = GetComponentInChildren<cinemaCameraExtra>();

        if (sc == null && GameObject.FindGameObjectWithTag("playerSound") != null)
        {
            sc = GameObject.FindGameObjectWithTag("playerSound").GetComponent<soundCollider>();
        }

        ac = GetComponentInChildren<armController>();

        glideMode = false;
        normalGravity = gravity;
        defaultSpeed = speed;
        if(dnc == null)
        {
            dnc = GameObject.FindGameObjectWithTag("SunMoonController").GetComponent<dayNightCycle_Script>();
        }
        if (GetComponent<CharacterController>() != null)
        {
            controller = GetComponent<CharacterController>();
        }
        else
        {
            Debug.LogError("moveController script is unable to get character controller component from player");
        }
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<player_Script>();
    }
    public void isMoving()//check if player is moving and grounded
    {
        curPos = transform.position;
        if (curPos == lastPos && ps.isGrounded)
        {
            playerMoving = false;
        }
        else { playerMoving = true; }
        lastPos = curPos;
    }

    public void punchForce(Vector3 punch)
    {
        ps.healthScore -= punchDamage;
        velocity += punch;       
    }

    // Update is called once per frame
    void Update()
    {
        isAttacking = ac.swing;//check if player is attacking
        isMoving();// check if player is moving on ground

        if (glideMode)
        {
            gravity = glideGravity;
        }
        else
        {
            gravity = normalGravity;
        }

        if (controller.isGrounded && velocity.y<0)//if not grounded and downward force is less than zero
        {
            velocity.y = -2;
        }

        if (!dnc.dayTime && ps.healthScore > 0f && cce.atCinemaPosition == false)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        /*
        if (Input.GetKeyDown("space") && ps.healthScore > 0f && !dnc.dayTime)//check mode of menu script for maximum security
        {
            if (wingTime <= 0f)
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                wingTime = maxWingTime;
            }
        }
        else
        {
            if (wingTime > 0f)
            {
                wingTime -= Time.deltaTime;
            }
        }
        */
        if (Input.GetKeyDown("space") && !dnc.dayTime && cce.atCinemaPosition == false)//check mode of menu script for maximum security
        {
            if (wingTime <= 0f && ps.isGrounded)// if wing rest is done and is grounded
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                wingTime = maxWingTime;
                glideMode = false;
            }
            else
            {
                if (wingTime <= 0f && ps.healthScore > ps.MaxHealthScore * .75)//if wing rest is done and health is greater than half of max
                {
                    velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                    wingTime = maxWingTime;
                    glideMode = true;
                }
            }

        }
        else
        {
            glideMode = false;
            if (wingTime > 0f)
            {
                wingTime -= Time.deltaTime;
            }
        }
        //***********************************************

        if ((Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.C))) && controller.isGrounded && ps.healthScore > 0f && cce.atCinemaPosition == false)
        {
            if ((Input.GetKey(KeyCode.LeftShift)))
            {
                if (playerMoving)
                {
                    if (sc != null)
                    {
                        sc.loud = true;
                        isLoud = true;
                    }
                }
                else
                {
                    if (sc != null)
                    {
                        sc.loud = false;
                        isLoud = false;
                    }
                }
                if (ps.isFound)
                {
                    speed = slowRunSpeed;
                }
                else { 
                    speed = runSpeed;
                }
            }
            else
            {
                if ((Input.GetKey(KeyCode.C)))
                {
                    if (playerMoving)
                    {
                        ps.isQuiet = true;
                    }
                    else
                    {
                        ps.isQuiet = false;
                    }
                    speed = slowSpeed;
                }
            }
        }
        else
        {
            if (sc != null)
            {
                sc.loud = false;
                isLoud = false;
                //sc.quiet = false;
                ps.isQuiet = false;
            }
            if (ps.isFound)
            {
                speed = slowSpeed;
            }
            else
            {
                speed = defaultSpeed;
            }
        }

        //fall damage: velocity of gravity aceleration > x value
        if(Mathf.Abs(velocity.y) > fallVelocity && ps.isGrounded && cce.atCinemaPosition == false)//if player fall velocity is high and is grounded, add damage and bounce force
        {
            Debug.Log("Player recieved fall damage " + Mathf.Abs(velocity.y * .1f));
            ps.healthScore -= fallVelocity * 0.1f;
            velocity.y = Mathf.Sqrt(bumpForce * -2f * gravity);
            if (sc != null)
            {
                sc.collin.radius = sc.maxColliderSize;
            }
        }

        //********Attack button********************** E == attack
        if (Input.GetKeyDown(KeyCode.E) && !dnc.dayTime && cce.atCinemaPosition == false)//check if cinema camera is deactive
        {
            ac.swing = true;
        }
    }
    //https://www.youtube.com/watch?v=_QajrabyTJc&ab_channel=Brackeys


    private void OnCollisionEnter(Collision collision)
    {
        //enemy punch player
        if(collision.gameObject.tag == "enemyAttack" || collision.gameObject.tag == "wall")
        {
            Vector3 dir = collision.contacts[0].point - transform.position;
            // We then get the opposite (-Vector3) and normalize it
            dir = -dir.normalized;
            // And finally we add force in the direction of dir and multiply it by force. 
            // This will push back the player
            punchForce(dir*punchDamage);
        }
    }
}
