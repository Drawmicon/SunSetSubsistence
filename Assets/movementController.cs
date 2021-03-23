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
    public LayerMask [] ground;

    public float jumpForce, bumpForce;

    private bool glideMode;

    public float wingTime, maxWingTime;//timer for wings to rest action

    public player_Script ps;

    public float fallVelocity;

    public float punchDamage;
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

    public void punchForce(Vector3 punch)
    {
        ps.healthScore -= punchDamage;
        velocity += punch;       
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

        //fall damage: velocity of gravity aceleration > x value
        if(Mathf.Abs(velocity.y) > fallVelocity && ps.isGrounded)//if player fall velocity is high and is grounded, add damage and bounce force
        {
            Debug.Log("Player recieved fall damage " + Mathf.Abs(velocity.y * .1f));
            ps.healthScore -= fallVelocity * 0.1f;
            velocity.y = Mathf.Sqrt(bumpForce * -2f * gravity);
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
