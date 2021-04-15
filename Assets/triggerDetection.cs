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
    public float soundLimit;
    public float maxSoundLimit, defaultSoundLimit, touchLimit, attackDistanceLimit, attackAngleLimit;
    public bool isInAttackRange, attackAngle, attackDistance;
    public bool touchDetected, soundDetected;
    // Start is called before the first frame update
    void Start()
    {
        eai = GetComponentInParent<EnemyAI>();
        touchDetected = false;
        soundDetected = false;
        isInAttackRange = false;//if true, then enemy is in attacking range
        soundLimit = defaultSoundLimit;
        ebc = GetComponentInParent<enemyBodyCollider>();
        alertTimer = 0;
        susTimer = 0;
        //enemyRen = enemyBody.GetComponent<Renderer>();
        enemyRen = GetComponentInParent<Renderer>();
        original = enemyRen.material.color;
        etc = GetComponentInChildren<enemyTextController>();

        player = GameObject.FindGameObjectWithTag("Player");

        ps = player.GetComponent<player_Script>();
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
            soundLimit = defaultSoundLimit;//also touch limit
        }
        soundDetection = (player.transform.position - this.transform.position);
        Debug.DrawRay(this.transform.position, soundDetection, Color.black);//ray from enemy to players
        Debug.DrawRay(player.transform.position, ps.playerForward, Color.white);//ray from enemy to players
        if (Vector3.Angle(ps.playerForward, soundDetection) <= attackAngleLimit)
        {
            attackAngle = true;
        }
        else
        {
            attackAngle = false;
        }
        if (soundDetection.magnitude <= attackDistanceLimit)
        {
            //Debug.Log("distance from player to enemy: " + soundDetection.magnitude);
            attackDistance = true;
        }
        else
        {
            attackDistance = false;
        }
        if (attackAngle && attackDistance)//if angle of enemy to player and forward player vector is close, and magnitude is close, then in attacking range
        {
            isInAttackRange = true;
        }
        else
        {          
            isInAttackRange = false;
        }

        
        if (soundDetection.magnitude <= touchLimit)//if player is within touch limit
        {
            touchDetected = true;
            alertMode = true;
            alertTimer = maxAlertTimer;
            susTimer = maxSusTimer;
            Debug.DrawRay(this.transform.position, soundDetection * touchLimit, Color.red);
        }
        else
        {
            touchDetected = false;
        }

        if (soundDetection.magnitude <= soundLimit && ps.isMoving && ps.isGrounded && !ps.isQuiet)//if player is within sound detection limit and player is moving and is grounded
        {
            soundDetected = true;
            if (soundDetection.magnitude <= touchLimit)//if player is within touch limit
            { //playerAlertDetected = true;
                touchDetected = true;
                alertMode = true;
                alertTimer = maxAlertTimer;
                susTimer = maxSusTimer;
                Debug.DrawRay(this.transform.position, soundDetection * touchLimit, Color.red);
            }
            else
            {
                //playerSusDetected = true;
                touchDetected = false;
                susMode = true;
                susTimer = maxSusTimer;
                Debug.DrawRay(this.transform.position, soundDetection * soundLimit, Color.red);
            }
        }
        else
        {
            soundDetected = false;
        }

        //********************************************************

        //enemy health
        float enemyHealth=ebc.enemyHealth;
        float maxEnemyHealth=ebc.maxEnemyHealth;
        //check if player is in collider
        inInnerCollider = inner.inCollider;
        inOuterCollider = outer.inCollider;

        //change number of exclaimation marks based on amount of time left on alert mode timer
        if (playerAlertDetected)
        {
            if (eai.isAttacking)
            {
                etc.textOutput = "Attack!\n" + etc.names[etc.nameChoice] + "(" + enemyHealth + "/" + maxEnemyHealth + ")";
            }
            else
            {
                etc.textOutput = "!!!!!\n" + etc.names[etc.nameChoice] + "(" + enemyHealth + "/" + maxEnemyHealth + ")";
            }
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
        }//if detected, change color of enemy, if applicable
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
        
        //countdown timers
        if(alertTimer > 0f)
        {
            alertMode = true;
            alertTimer -= Time.deltaTime;
        }
        else
        {
            alertMode = false;
            if(susTimer > 0f)
            {
                if (Vector3.Angle(this.forwardParentVector, soundDetection) <= detectionRadius)
                {
                    this.transform.LookAt(ps.lastDetectedDirection);
                }
                susMode = true;
                susTimer -= Time.deltaTime;
            }
            else
            {
                susMode = false;
            }
        }

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
                        this.transform.LookAt(player.transform);
                        alertTimer = maxAlertTimer;
                        susTimer = maxSusTimer;
                    }
                    else//if player is only detected in outer collider
                    {
                        playerSusDetected = true;
                        susTimer = maxSusTimer;
                    }
                }
            }
            else//if not detected, normal mode
            {
                playerAlertDetected = false;
                playerSusDetected = false;
                this.transform.rotation = this.transform.parent.rotation; 
            }
        }
        else
        {
           // Debug.Log("Enemy Raycast for enemy object " + this.name + " not working!");
        }
        
    }
}
