using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerDetection : MonoBehaviour
{
    public triggerDetectionSubScript outer, inner;
    public GameObject player;
    public GameObject enemyBody;
    public Renderer enemyRen;
    public Color original;

    public Vector3 enemy2Player, forwardParentVector;

    public bool inInnerCollider, inOuterCollider;

    public LayerMask playerLayers;

    public bool playerSusDetected, playerAlertDetected;
    public float detectionRadius;
    //********************************************
    public bool susMode, alertMode;
    public float susTimer, maxSusTimer, alertTimer, maxAlertTimer, susLookAtTimer, maxSusLookAtTimer;//current times on modes, and max times

    public EnemyAI eai;

    // Start is called before the first frame update
    void Start()
    {

        alertTimer = 0;
        susTimer = 0;
        if (outer != null && outer.width > 0)
        {
            detectionRadius = outer.width;
        }
        //enemyRen = enemyBody.GetComponent<Renderer>();
        enemyRen = GetComponentInParent<Renderer>();
        original = enemyRen.material.color;
    }

    //Check around area if player is not detected, but still in sus/alert mode

    // Update is called once per frame
    void Update()
    {

        if (playerAlertDetected)
        {
            enemyRen.material.SetColor("_Color", Color.red);
            player = eai.lookAtTarget;
            eai.noticingSomething = true;
        }
        else
        {
            if (playerSusDetected)
            {
                player = eai.lookAtTarget;
                eai.noticingSomething = true;

                enemyRen.material.SetColor("_Color", Color.yellow);
                if (susLookAtTimer <= 0)//look at player if they stay in sus area for half of max sus time
                {
                    this.transform.LookAt(player.transform);
                }
            }
            else
            {
                enemyRen.material.SetColor("_Color", original);
            }
        }
        

        //check if player is in collider
        inInnerCollider = inner.inCollider;
        inOuterCollider = outer.inCollider;

        enemy2Player = (player.transform.position - this.transform.position).normalized * inner.distance;
        forwardParentVector = this.transform.parent.transform.forward.normalized * 3;
        //if raycast to player is true
        if (Vector3.Angle(enemy2Player, forwardParentVector) <= detectionRadius)
        {
            Debug.DrawRay(this.transform.position, enemy2Player, Color.yellow);//ray from enemy to players
        }
        Debug.DrawRay(this.transform.position, forwardParentVector, Color.blue);//ray from enemy to players

        Ray lightHit = new Ray(this.transform.position, enemy2Player);
        RaycastHit hit;
        if (Physics.Raycast(lightHit, out hit, inner.distance, playerLayers))
        {
            if ((hit.collider.tag == "Player") && inOuterCollider && Vector3.Angle(enemy2Player, forwardParentVector) <= detectionRadius)//if player detected in outer collider
            {
                if (inInnerCollider)//if player detected in inner collider and outer collider
                {
                    playerAlertDetected = true;
                    
                    this.transform.LookAt(player.transform);
                    alertTimer = maxAlertTimer;
                    susTimer = maxSusTimer;
                }
                else//if player is only detected in outer collider
                {
                    playerSusDetected = true;
                    susTimer = maxSusTimer;
                    if (susLookAtTimer <= 0)
                    {                      
                        susLookAtTimer = maxSusLookAtTimer;
                    }
                }
            }
            else//if not detected, normal mode
            {
                //if alert or suspicious mode timers are done, return to normal
                if (alertTimer <= 0)// if alert timer is done, turn off alert bool
                {
                    playerAlertDetected = false;
                    if (susTimer <= 0)// if sus timer is done, turn off sus bool and look forward
                    {
                        playerSusDetected = false;
                        this.transform.rotation = this.transform.parent.rotation;
                    }
                    else//if sus timer not done, countdown
                    {
                        susTimer -= Time.deltaTime;
                    }
                }
                else//if alert timer not done, countdown
                {
                    alertTimer -= Time.deltaTime;
                }           
            }
        }
        else
        {
            Debug.Log("Enemy Raycast for enemy object " + this.name + " not working!");
        }
        
    }
}
