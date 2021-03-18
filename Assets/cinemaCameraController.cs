using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cinemaCameraController : MonoBehaviour
{
    public GameObject player;
    public GameObject cinCam;
    public bool switchCamMode;

    public float stoppingAngle;
    public float rotSpeed, maxRotSpeed, defaultRotSpeed;
    public float currentAngle;

    public cinemaCameraExtra cce;
    // Start is called before the first frame update
    void Start()
    {
        defaultRotSpeed = rotSpeed;
        player = GameObject.FindGameObjectWithTag("Player");
        cinCam = GameObject.FindGameObjectWithTag("CinemaCamera");
        cce = cinCam.GetComponent<cinemaCameraExtra>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAngle >= 360)
        {
            currentAngle %= 360;
        }

        if (!switchCamMode)//if cinema camera should be rotating around at a distance
        {
            rotSpeed = defaultRotSpeed;
            if(cce.atParentPosition)
            {
                currentAngle = 360;
            }
            cce.moveTowardsPosition = true;//if true, move cinema camera to viewing position
            transform.localRotation = Quaternion.Euler(0, currentAngle, 0);//rotate parent of cinema camerea
            currentAngle += Time.unscaledDeltaTime * rotSpeed;          
        }
        else
        {
            rotSpeed = maxRotSpeed;
            if (currentAngle <= stoppingAngle-5 || currentAngle >= stoppingAngle+5)//if not current angle to stop at, keep rotating
            {
                transform.localRotation = Quaternion.Euler(0, currentAngle, 0);
                currentAngle += Time.unscaledDeltaTime * rotSpeed;
            }
            else//else, set cinema camera position to parent position
            {
                cce.moveTowardsPosition = false;
            }
        }
    }
}
