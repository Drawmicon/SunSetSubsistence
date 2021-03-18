using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdPersonController : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;
    public float speed, maxSpeed, accelerations, originalSpeed;
    public float turnSpeed = 5f;

    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity = 1f;

    public dayNightCycle_Script dn;

    public float jumpPower = 5f;

    private Renderer playerRenderer;
    private Color originalColor;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        speed = originalSpeed;
        controller = GetComponent<CharacterController>();
        dn = (GameObject.FindGameObjectWithTag("SunMoonController")).GetComponent<dayNightCycle_Script>();

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        playerRenderer = player.GetComponent<Renderer>();
        originalColor = playerRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {

        if (!dn.dayTime)
        {

            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
            var movement = new Vector3(horizontal, 0f, vertical).normalized;

            transform.Rotate(Vector3.up, horizontal * turnSpeed * Time.deltaTime);

            if (vertical != 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift) && speed <= maxSpeed)
                {
                    speed += accelerations;
                }
                else
                {
                    if(speed > originalSpeed)
                    {
                        speed -= accelerations;
                    }
                }
                controller.SimpleMove(transform.forward * speed * vertical);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                /*if (speed < maxSpeed)
                {
                    speed+= accelerations;
                }*/
                speed = maxSpeed;
            }
            else
            {
                /*if(speed > originalSpeed)
                {
                    speed -= accelerations;
                }*/
                speed = originalSpeed;
            }
        }
        else {

            playerRenderer.material.SetColor("_Color", Color.white);
        }
        /*if(movement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, direction.y)*Mathf.Rad2Deg+ cam.eulerAngles.y;         
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }*/


    }
}
