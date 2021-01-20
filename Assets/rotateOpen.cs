using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateOpen : MonoBehaviour
{

    public GameObject player;
    public float distance;
    public float opened;
    public float closed;
    public float rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(this.transform.position, (player.transform.position- this.transform.position), Color.green);
        Debug.Log("Magnitude: " + (player.transform.position - this.transform.position).magnitude + " , RotationX: "+ this.transform.localEulerAngles.x);
        if ((player.transform.position - this.transform.position).magnitude >= distance)
        {
            if(this.transform.localEulerAngles.x >= opened)
            {
                //rotate box lid to open
                this.transform.Rotate(new Vector3(-rotationSpeed, 0, 0), Space.World);
            }
        }
        else
        {
            if (this.transform.localEulerAngles.x >= closed)
            {
                this.transform.Rotate(new Vector3(rotationSpeed, 0, 0), Space.World) ;
            }
        }
    }
}
