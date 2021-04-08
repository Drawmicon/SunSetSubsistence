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
    public enemyTextController etc;
    public enemyBodyCollider ebc;

    public player_Script ps;

    public Vector3 soundDetection;
    private float soundLimit;
    public float maxSoundLimit, defaultSoundLimit;

    // Start is called before the first frame update
    void Start()
    {
        soundLimit = defaultSoundLimit;
        ebc = GetComponentInParent<enemyBodyCollider>();
        alertTimer = 0;
        susTimer = 0;
        if (outer != null && outer.width > 0)
        {
            detectionRadius = outer.width;
        }
        //enemyRen = enemyBody.GetComponent<Renderer>();
        enemyRen = GetComponentInParent<Renderer>();
        original = enemyRen.material.color;
        etc = GetComponentInChildren<enemyTextController>();

        ps = player.GetComponent<player_Script>();
    }

    //Check around area if player is not detected, but still in sus/alert mode
    public void randomLookAround()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //********************CHECK DISTANCE FROM PLAYER TO ENEMY, IF CLOSE ENOUGH, THEN ENEMY CAN HEAR/DETECT PLAYER************************************
        if(player.GetComponent<player_Script>().isLoud)
        {
            soundLimit = maxSoundLimit;
        }
        else
        {
            soundLimit = defaultSoundLimit;
        }
        soundDetection = (player.transform.position - this.transform.position).normalized * soundLimit;
        if (soundDetection.magnitude <= soundLimit && ps.isMoving)//if player is within sound detection limit and player is moving
        {
            Debug.DrawRay(this.transform.position, soundDetection, Color.red);
            playerAlertDetected = true;
        }
        
        //********************************************************


        float enemyHealth=ebc.enemyHealth;
        float maxEnemyHealth=ebc.maxEnemyHealth;
        /*float enemyHealth, maxEnemyHealth;
        if(eai != null)
        {
            enemyHealth= eai.enemyHealth;
            maxEnemyHealth=eai.maxEnemyHealth;
        }
        else
        {
            enemyHealth = 100;
            maxEnemyHealth = 100;
        }*/

        //change number of exclaimation marks based on amount of time left on alert mode timer
        if (playerAlertDetected)
        {
            etc.textOutput = "!!!!!\n" + etc.names[etc.nameChoice] + "(" + enemyHealth + "/" + maxEnemyHealth + ")";
        }
        else
        {
            if (alertMode)
            {
                if(alertTimer >= maxAlertTimer*.8f)
                { 
                    etc.textOutput = "!!!!\n" + etc.names[etc.nameChoice] + "(" + enemyHealth + "/" + maxEnemyHealth + ")";
                }
                else
                {
                    if (alertTimer >= maxAlertTimer * .6f)//if alert timer is greater than 60% of max timer
                    {
                        etc.textOutput = "!!!\n" + etc.names[etc.nameChoice] + "(" + enemyHealth + "/" + maxEnemyHealth + ")";
                    }
                    else
                    {
                        if (alertTimer >= maxAlertTimer * .4f)//if alert timer is greater than 40% of max timer
                        {
                            etc.textOutput = "!!\n" + etc.names[etc.nameChoice] + "(" + enemyHealth + "/" + maxEnemyHealth + ")";
                        }
                        else
                        {
                            if (alertTimer >= maxAlertTimer * .6f)//if alert timer is greater than 20% of max timer
                            {
                                etc.textOutput = "!\n" + etc.names[etc.nameChoice] + "(" + enemyHealth + "/" + maxEnemyHealth + ")";
                            }
                        }
                    }
                }
            }
            else
            {
                if (playerSusDetected)
                {
                    etc.textOutput = "???\n" + etc.names[etc.nameChoice] + "(" + enemyHealth + "/" + maxEnemyHealth + ")";
                }
                else
                {
                    if (susMode)
                    {
                        etc.textOutput = "?\n" + etc.names[etc.nameChoice] + "(" + enemyHealth + "/" + maxEnemyHealth + ")";
                    }
                    else
                    {
                        etc.textOutput = "\n" + etc.names[etc.nameChoice] + "(" + enemyHealth + "/" + maxEnemyHealth + ")";
                    }
                }
            }
        }

        if (playerAlertDetected)
        {
            enemyRen.material.SetColor("_Color", Color.red);
            if (eai != null){player = eai.lookAtTarget;
            eai.noticingSomething = true; }
        }
        else
        {
            if (playerSusDetected)
            {
                if (eai != null)
                {
                    player = eai.lookAtTarget;
                    eai.noticingSomething = true;
                }

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
                if (ps.playerLit || alertMode)//check if player is lit up or alert mode is on, aka enemy is using light source
                {
                    if (inInnerCollider)//if player detected in inner collider and outer collider
                    {
                        playerAlertDetected = true;//player alert is on, enemy looks at player, start timers
                        alertMode = true;

                        this.transform.LookAt(player.transform);
                        alertTimer = maxAlertTimer;
                        susTimer = maxSusTimer;
                    }
                    else//if player is only detected in outer collider
                    {
                        playerSusDetected = true;
                        susMode = true;
                        susTimer = maxSusTimer;
                    }
                }
            }
            else//if not detected, normal mode
            {
                playerAlertDetected = false;
                playerSusDetected = false;

                //if alert or suspicious mode timers are done, return to normal
                if (alertTimer <= 0)// if alert timer is done, turn off alert bool
                {
                    alertMode = false;

                    randomLookAround();
                    
                    if (susTimer <= 0)// if sus timer is done, turn off sus bool and look forward
                    {
                        susMode = false;
                        
                        this.transform.rotation = this.transform.parent.rotation;
                    }
                    else//if sus timer not done, countdown
                    {
                        susMode = true;
                        susTimer -= Time.deltaTime;
                    }
                }
                else//if alert timer not done, countdown
                {
                    alertMode = true;
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
