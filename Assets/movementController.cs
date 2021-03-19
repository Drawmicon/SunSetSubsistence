using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementController : MonoBehaviour
{
    public dayNightCycle_Script dnc;
    public CharacterController controller;
    public float speed, runSpeed;
    private float defaultSpeed;

    public Vector3 velocity;
    public float gravity, normalGravity, glideGravity;
    public LayerMask ground;

    public float jumpForce;

    private bool glideMode;

    public float wingTime, maxWingTime;

    public player_Script ps;

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {

        if(glideMode)
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

        if (!dnc.dayTime && ps.healthScore > 0f)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


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

        if (Input.GetKey("space") && ps.healthScore > 0f)
        {
            glideMode = true;
        }
        else
        {
            glideMode = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && controller.isGrounded && ps.healthScore > 0f)
        {
            speed = runSpeed;
        }
        else
        {
            speed = defaultSpeed;
        }
    }
    //https://www.youtube.com/watch?v=_QajrabyTJc&ab_channel=Brackeys
}
