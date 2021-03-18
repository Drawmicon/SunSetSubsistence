using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBodyCollider : MonoBehaviour
{
    public bool contactAlert;
    public GameObject alertTarget;
    public Vector3 alertPosition;//point of collision source
    public float colliderRadius, colliderHeight/*, colliderDirection*/;
    private CapsuleCollider collin;
    public float sizeRate;

    public float timer, maxTimer, minTimer;
    // Start is called before the first frame update
    void Start()
    {
        contactAlert = false;
        collin = GetComponent<CapsuleCollider>();

        colliderRadius = collin.radius;
        colliderHeight = collin.height;
        //colliderDirection = collin.direction;
    }

    // Update is called once per frame
    void Update()
    {
        if(contactAlert)// if alerted 
        {
            if (timer > 0f)
            {
                if (timer >= 0f)//if timer is not done, count down
                {
                    timer -= Time.deltaTime;
                }
                else//if timer is done, set contactAlert to false
                {
                    contactAlert = false;
                }
            }
        }
        else// if not alerted
        {
            if (timer <= 0f)// if timer is not set
            {
                timer = Random.Range(maxTimer, minTimer);//set timer to random value
            }
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {//if collision of enemy, misc-object or player body/sound, alert enemy
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Player" /*|| collision.gameObject.tag == "PlayerBodyCollider" || collision.gameObject.tag == "PlayerSoundCollider"*/  ||collision.gameObject.tag == "misc")
        {
            contactAlert = true;
            alertPosition = collision.gameObject.transform.position;
            alertTarget = collision.gameObject;
            Debug.Log("Enemy Body Collision Detected at Time:"+Time.time);
        }
    }
}
