using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class simpleNav : MonoBehaviour
{
    public Transform destination;
    public NavMeshAgent agent;
    public float nearbyLength;
    private bool reachedDestination;

    // Start is called before the first frame update
    void Start()
    {
        reachedDestination = false;
        destination = GameObject.FindGameObjectWithTag("TargetDestination").transform;
        if (agent == null)
        {
            Debug.Log("Error: No object with \"TargetDestination\" tag ");
        }
        agent = GetComponent<NavMeshAgent>();
        if(agent == null)
        {
            Debug.Log("Error: no nav mesh attached to " + gameObject.name);
        }
        else
        {
            SetDestination();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((destination.position - transform.position).magnitude > nearbyLength && reachedDestination)//if far away from destination and not reached it already
        {
            SetDestination();//set destintion
        }
        else
        {
            if ((destination.position - transform.position).magnitude < nearbyLength)//otherwise, if close to destination and reached it previously, 
            {
                reachedDestination = true;
            }
        }

    }

    private void SetDestination()
    {
        if(destination != null)
        {
            Vector3 targetVector = destination.transform.position;
            agent.SetDestination(targetVector);
        }
    }
}
