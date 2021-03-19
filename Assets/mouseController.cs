using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public GameObject playerBody;
    private float xRotation = 0f;

    public dayNightCycle_Script dnc;

    public cinemaCameraController ccc;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        if(GameObject.FindGameObjectWithTag("Player")!=null)
        {
            playerBody = GameObject.FindGameObjectWithTag("Player");
        }
        if (dnc == null)
        {
            dnc = GameObject.FindGameObjectWithTag("SunMoonController").GetComponent<dayNightCycle_Script>();
        }

        ccc = playerBody.GetComponentInChildren<cinemaCameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        //if night time and using gameplay cam
        if (!dnc.dayTime && ccc.switchCamMode) //switchCamMod(true) == gameplay cam
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            playerBody.transform.Rotate(Vector3.up * mouseX);

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            this.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }
}
