using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != null)
        {
            //add damage to enemy
            Debug.Log("Player attacked gameobject:" + collision.gameObject.name + "(tag:" + collision.gameObject.tag + ")");
        }
        else
        {
            Debug.Log("Player attacked gameobject:" + collision.gameObject.name + "(tag:" + collision.gameObject.tag + ")");
        }
    }
}
