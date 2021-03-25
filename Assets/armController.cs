using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armController : MonoBehaviour
{

    public float angle;
    public float speed;
    public bool visible, swing;
    public GameObject arm;

    public Transform originalPosition;
    public Vector3 originalRotation, currentRotation;
    // Start is called before the first frame update
    void Start()
    {
        visible = false;
        arm.SetActive(visible);
        originalPosition = transform;
        originalRotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        currentRotation = arm.transform.eulerAngles;
        arm.SetActive(visible);

        if(!visible)
        {
            transform.position = originalPosition.position;
            transform.eulerAngles = originalRotation;
        }

        if(swing && (angle < 360))// from 270 to 0 to 90
        {        
            visible = true;
            transform.localEulerAngles = new Vector3(  angle ,0f, 0f);
            angle = angle + speed*Time.deltaTime;
        }
        else
        {
            swing = false;
            visible = false;
            angle = 180;
        }
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //add damage to enemy
            Debug.Log("Player attacked gameobject:" + collision.gameObject.name + "(tag:" + collision.gameObject.tag + ")");
        }
    }*/
}
