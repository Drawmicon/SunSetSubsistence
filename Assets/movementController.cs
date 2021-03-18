using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementController : MonoBehaviour
{
    public dayNightCycle_Script dnc;
    public CharacterController controller;
    public float speed, runSpeed;
    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!dnc.dayTime)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * speed * Time.deltaTime);
        }
    }
}
