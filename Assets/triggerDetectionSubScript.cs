using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CHECK IF PLAYER IS IN TRIGGER COLLIDER AND RAYCAST TO PLAYER IS UNBLOCKED
public class triggerDetectionSubScript : MonoBehaviour
{
    public GameObject player;
    public Vector3 enemy2Player;
    public float distance, width;
    public bool inCollider;

    // Start is called before the first frame update
    void Start()
    {
        inCollider = false;
        player = GameObject.FindGameObjectWithTag("Player");
        distance = GetComponent<CapsuleCollider>().height;
        width = GetComponent<CapsuleCollider>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            inCollider = true;
        }
        //Debug.Log("Object has entered trigger collider: " + other.name+ " (tag:"+other.tag+")");
        /*
        if(other.tag == "Enemy") //if enemy notices other enemy
        {

        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inCollider = false;
        }
        //Debug.Log("Object has exited trigger collider: " + other.name + " (tag:" + other.tag + ")");

        /*
        if(other.tag == "Enemy") //if enemy notices other enemy
        {

        }*/
    }
}
