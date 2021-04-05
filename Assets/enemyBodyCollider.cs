using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBodyCollider : MonoBehaviour
{
    public float enemyHealth, maxEnemyHealth;
    public player_Script ps;
    public float playerAttack;

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
        enemyHealth = maxEnemyHealth;
        contactAlert = false;
        collin = GetComponent<CapsuleCollider>();

        colliderRadius = collin.radius;
        colliderHeight = collin.height;
        //colliderDirection = collin.direction;
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<player_Script>();
    }

    // Update is called once per frame
    void Update()
    { 
        if(enemyHealth <= 0f)
        {
            Destroy(this);
        }

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
            contactAlert = false;
            if (timer <= 0f)// if timer is not set
            {
                timer = Random.Range(maxTimer, minTimer);//set timer to random value
            }
        }
    }

    
    private void OnCollisionEnter(Collision collision)
    {//if collision of enemy, misc-object or player body/sound, alert enemy
        if(collision.gameObject.tag == "playerAttack" /*|| collision.gameObject.tag == "Player" || collision.gameObject.tag == "PlayerBodyCollider" || collision.gameObject.tag == "PlayerSoundCollider"*/  ||collision.gameObject.tag == "misc")
        {
            enemyHealth -= playerAttack;
            Debug.Log("Enemy Body Collision Detected at Time:"+Time.time);
        }

        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "playerSound")
        {
            contactAlert = true;
        }
    }
}
