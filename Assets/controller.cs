using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{//ATTACH TO PLAYER
    public float speed;
    public Rigidbody rb;

   /* 
    public GameObject target;
    public Vector3 offset;
    public bool useOffsetValue;
    public float rotateSpeed;
    public GameObject pivot;*/

    public float jumpForce;


    public float accelerationRate;
    public float accelerationLimit;
    [SerializeField]
    private float currentSpeed;

    //*************************************
    public float flightForce;
    public float totalFlightFuel;
    public float flightRechargeDelay;
    public float flightRechargeRate;
    public float currentFlightFuel;
    private bool jumped = false;
    //*************************************

    public float rotateSpeed;
    Quaternion desiredRotation;
    /*float speed; //in degrees

        rigidbody.MoveRotation(Quaternion.RotateTowards(rigidbody.rotation, desiredRotation, speed * Time.deltaTime);*/

        // Start is called before the first frame update
        void Start()
    {
        currentSpeed = speed;
        //set rigidbody variable to player, script is connected to player
        rb = GetComponent<Rigidbody>();

        //set desiredRotation as starting rotation
        desiredRotation = transform.rotation;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (desiredRotation != transform.rotation)//https://answers.unity.com/questions/332001/how-to-reset-a-gamev-object-to-its-original-rotati.html
        {
            //rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, desiredRotation, speed * Time.deltaTime));
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.time * rotateSpeed);
        }

        Vector3 movement;

        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && currentSpeed < accelerationLimit)//if left shift is pressed and moving, and not at acceleration limit
        {
            currentSpeed += accelerationRate;
        }
        else
        {
            if (currentSpeed > speed)
            {
                currentSpeed -= accelerationRate;
            }
        }
        //new 3d vector
        movement = new Vector3(Input.GetAxis("Horizontal") * currentSpeed, rb.velocity.y, Input.GetAxis("Vertical") * currentSpeed);//
                                                                                                                                    //Vector3 movement = new Vector3(Input.GetAxis("Horizontal"),0f, Input.GetAxis("Vertical"));

        float DisstanceToTheGround = GetComponent<Collider>().bounds.extents.y;

        bool IsGrounded = Physics.Raycast(transform.position, Vector3.down, DisstanceToTheGround + 0.1f);

        //if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetKey("space") && IsGrounded)//jump is set default as space 
        {
            movement.y += jumpForce + (currentSpeed * 0.1f);
            jumped = true;
        }

        if (IsGrounded)
        {
            rb.freezeRotation = true;
        }
        else 
        {
            rb.freezeRotation = false;
        }

        /*
        if(IsGrounded)
        { jumped = false; }

        if (Input.GetKeyDown("space") && !IsGrounded && jumped)
        {
            movement.y += flightForce;
        }
        */




        //set velocity as direction and magnitude of movement vector
        //movement = movement.normalized * (Time.deltaTime * movement.magnitude);
        rb.velocity = movement;
    }
}
