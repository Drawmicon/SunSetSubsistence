using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cinemaCameraExtra : MonoBehaviour
{
    public GameObject parentObject;
    public GameObject positionObject;
    public bool moveTowardsPosition;// activate to move cinematic cam to active position
    public float speed;
    public bool atCinemaPosition;
    public bool atParentPosition;

    public Camera main, cinema;
    // Start is called before the first frame update
    void Start()
    {
        parentObject = GameObject.FindGameObjectWithTag("CinemaCameraParent");
        positionObject = GameObject.FindGameObjectWithTag("CinemaCameraPosition");
        moveTowardsPosition = true;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(parentObject.transform);

        if(this.transform.position == positionObject.transform.position)//cinema camera viewing position
        {
            atCinemaPosition = true;
            main.enabled = false;
            cinema.enabled = true;
        }
        else
        {
            atCinemaPosition = false;
            main.enabled = true;
            cinema.enabled = false;
        }

        if (this.transform.position == parentObject.transform.position)//gameplay camera viewing position
        {
            atParentPosition = true;
            main.enabled = true;
            cinema.enabled = false;
        }
        else { atParentPosition = false;
            main.enabled = false;
            cinema.enabled = true;
        }

        if (moveTowardsPosition)
        {
            float step = speed * Time.unscaledDeltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, positionObject.transform.position, step);
        }
        else
        {
            float step = speed * Time.unscaledDeltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, parentObject.transform.position, step);
        }

    }
}
