using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDestinationScript : MonoBehaviour
{
    public bool occupied;
    public int type;
    public GameObject follow;
    public float followDistance;
    public List<GameObject> TouchingObjects;
    private SphereCollider sCollider;
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        type = 0;//0:default
        occupied = false;
        TouchingObjects = new List<GameObject>();
        sCollider = GetComponent<SphereCollider>();
        sCollider.radius = radius;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (!TouchingObjects.Contains(collision.gameObject))
            TouchingObjects.Add(collision.gameObject);
    }

    void OnTriggerExit(Collider collision)
    {
        if (TouchingObjects.Contains(collision.gameObject))
            TouchingObjects.Remove(collision.gameObject);
    }
    void findType()
    {
        int foodCount = 0;
        int npcCount = 0;
        int farmAnimal = 0;
        for(int i = 0; i < TouchingObjects.Count; i++)
        {
            if( TouchingObjects[i].tag == "Item")
            {
                foodCount++;
            }
            if(TouchingObjects[i].tag == "Enemy" )
            {
                npcCount++;
            }

            if (TouchingObjects[i].tag == "chicken")
            {
                farmAnimal++;
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(follow != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, follow.transform.position, followDistance);
        }
        else
        {
            //findType();
        }
    }
}
