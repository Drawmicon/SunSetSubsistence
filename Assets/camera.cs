using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{//ATTACH TO CAMERA

    public GameObject target;
    public GameObject player;

    public Vector3 cameraDistance;

    public float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cameraDistance = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //Vector2 v2 = (Input.GetAxis("MouseX"), Input.GetAxis("MouseY"));

        //make camera look at player
        transform.LookAt(target.transform.position);

        //player will face target, aka point where camera is pointing to
        player.transform.LookAt(target.transform.position);
    }
}



