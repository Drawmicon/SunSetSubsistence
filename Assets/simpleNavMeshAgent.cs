using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.AI;

public class simpleNavMeshAgent : MonoBehaviour
{
    public GameObject targetObject;
    public Transform nextDestination;
    public float closeDistance;
    public NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("No Navmesh attached to " + gameObject.name);
        }
        else
        {
            nextDestination = targetObject.transform;
            agent.Warp(new Vector3 (1, .6f, 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //nextDestination = targetObject.transform.position;
        //if ((nextDestination - transform.position).magnitude > closeDistance)
        //{
        if (agent.isActiveAndEnabled)
        {
            if (nextDestination.position != null)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(nextDestination.position, out hit, 5.0f, NavMesh.AllAreas))
                {
                    nextDestination.position = hit.position;
                    Debug.Log("Navmesh sample position: " +nextDestination.position);
                    agent.SetDestination(nextDestination.position);
                }
                else
                {
                    Debug.LogError("Navmesh sample position is false for " + gameObject.name);
                }
            }
        }
        else
        {
            Debug.LogError("Agent is not active and enabled for " + gameObject.name);
        }
        //}
    }

    //when following player, and then lose player, use vector pointing to last known direction
    //create vector by constantly geting the position of the player every second, and save the latest 2 positions
}
