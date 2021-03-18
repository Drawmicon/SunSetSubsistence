using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundCollider : MonoBehaviour//THIS REPRESENTS THE SOUND PRODUCED BY THE GAMEOBJECT    
{
    public bool loud;
    public float colliderSize, standardColliderSize, maxColliderSize;
    private SphereCollider collin;
    public float sizeRate;
    // Start is called before the first frame update
    void Start()
    {
        loud = false;
        
       collin = GetComponent<SphereCollider>();

        standardColliderSize = collin.radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (loud && collin.radius <= maxColliderSize)
        {
            collin.radius+=Time.deltaTime*sizeRate;
        }
        else
        {
            if (collin.radius >= standardColliderSize)
            {
                collin.radius -= Time.deltaTime * sizeRate;
            }
        }
    }
    
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "SoundCollider" || collision.gameObject.tag == "EnemyBodyCollider")
        {

        }
    }*/
}
