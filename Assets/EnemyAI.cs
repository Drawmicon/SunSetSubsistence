using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //public enemyBodyCollider ebc;
    public dayNightCycle_Script dnc;
    public float enemyHealth, maxEnemyHealth;

    public int enemyState;//0 = calm, 1 = suspicious, 2 = alert, 3 = sleep, 4 = stop and glance at point of interest, 5 = loiter
    public float randomLoiterTime, maxLoiterTime, minLoiterTime, fullLoiterTime;//generate random time, set that as max, decrement as timer runs
    public float speedWalk;
    private float speedRun;
    private GameObject[] positions2Move2;
    private int position2Move2Backup;
    private GameObject player;
    public Vector3 targetPosition;//location enemy will move to
    public float nearEnoughToTarget;//proximity to tell enemy to stop moving to position
    public bool noticingSomething;//if enemy notices something aka true, stop
    public GameObject lookAtTarget;

    //***************************https://www.youtube.com/watch?v=UjkSFoLxesw&ab_channel=UnityUnityVerified

    public NavMeshAgent agent;
    public LayerMask ground, playerLayer;

    public float speed, minSpeed, maxSpeed;
    //public enemy_detection ed;
    //public GameObject enemyDetectionObject;
    protected Vector3 leftDetector, leftDetector2;
    [SerializeField]
    private bool detectionLeftRight;//will turn on if the enemy is detecting something forward
    protected Vector3 rightDetector, rightDetector2;
    public Vector3 forwardDetector;
    public float detectionAngle, detectionAngle2;
    public float detectionDistance, detectionDistance2;
    public float detectionHeight;
    public float anglePerSecond;//rotation speed
    public float maxTurnTimer;//max time when setting left or right default turn
    private bool leftTurn;//if true, left == default turn
    private float turnTimer;//current time for current default turn

    public bool here;//on when enemy is at destination, off when traversing

    //public enemy_detection ed;
    public triggerDetection td;
    public player_Script ps;
    public GameObject grave;
    public enemyBodyCollider ebc;

    public LayerMask ignorePlayer;

    public NavMeshAgent enemyNavMeshAgent ;

    private void Start()
    {
        enemyNavMeshAgent = this.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        ebc = GetComponentInChildren<enemyBodyCollider>();
        maxEnemyHealth = ebc.maxEnemyHealth;
        ps = GameObject.FindGameObjectWithTag("Player").GetComponent<player_Script>();
        if (td == null)
        {
            td = GetComponentInChildren<triggerDetection>();
        }
        enemyHealth = maxEnemyHealth;
        noticingSomething = false;
        detectionLeftRight = false;
        here = true;
        turnTimer = maxTurnTimer;
        leftTurn= true;
        /*if(enemyDetectionObject = null)
        {
            enemyDetectionObject = GameObject.FindGameObjectWithTag("Enemy_Detection");
        }
        if (ed == null)
        {
            ed = GetComponentInChildren<enemy_detection>();
        }*/
        speed = Random.Range(minSpeed, maxSpeed);
        randomLoiterTime = Random.Range(minLoiterTime, maxLoiterTime);
        //fullLoiterTime = randomLoiterTime;
        agent = GetComponent<NavMeshAgent>();

        positions2Move2 = GameObject.FindGameObjectsWithTag("TargetDestination");
        dnc = GameObject.FindGameObjectWithTag("SunMoonController").GetComponent<dayNightCycle_Script>();

        //NavMeshHit hit;
        //NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas);
        enemyNavMeshAgent.speed = speed;
    }

    void drawLines()
    {
        Debug.DrawRay(this.transform.position, leftDetector * detectionDistance, Color.magenta);//from enemy to player
        Debug.DrawRay(this.transform.position, leftDetector2 * detectionDistance2, Color.magenta);//from enemy to player
        Debug.DrawRay(this.transform.position, rightDetector * detectionDistance, Color.magenta);//front of enemy
        Debug.DrawRay(this.transform.position, rightDetector2 * detectionDistance2, Color.magenta);//front of enemy
        Debug.DrawRay(this.transform.position, forwardDetector * detectionDistance, Color.magenta);//front of enemy

        //forwardDetector = (this.transform.position - this.transform.forward * detectionDistance).normalized;

        forwardDetector = this.transform.forward;
        leftDetector = Quaternion.AngleAxis(detectionAngle * -1, Vector3.up) * this.transform.forward;
        leftDetector2 = Quaternion.AngleAxis(detectionAngle2 * -1, Vector3.up) * this.transform.forward;
        rightDetector = Quaternion.AngleAxis(detectionAngle, Vector3.up) * this.transform.forward;
        rightDetector2 = Quaternion.AngleAxis(detectionAngle2, Vector3.up) * this.transform.forward;
    }

    void objectAvoidance()
    {
        drawLines();
        Ray lightHit = new Ray(this.transform.position, leftDetector);
        RaycastHit hit;
        Ray lightHit2 = new Ray(this.transform.position, rightDetector);
        RaycastHit hit2;

        //turn away if there is an object in front
        if (Physics.Raycast(lightHit, out hit, detectionDistance, ~ignorePlayer) && Physics.Raycast(lightHit2, out hit2, detectionDistance, ~ignorePlayer))//ERROR: enemy might not be able to see player if its own body is blocking
        {           
            if (hit.collider.tag != "Player" || hit.collider.tag != "Player_Controller")//if left detected
            {
                detectionLeftRight = true;
                if (hit2.collider.tag != "Player" || hit2.collider.tag != "Player_Controller")//if right also detected
                {
                    detectionLeftRight = true;
                    speed *= .6f;//reduce speed
                    if (leftTurn)
                    {
                        transform.RotateAround(transform.position, transform.up, Time.deltaTime * -anglePerSecond);//turn away in direction
                    }
                    else
                    {
                        transform.RotateAround(transform.position, transform.up, Time.deltaTime * anglePerSecond);//turn away in opposite direction
                    }
                }
                else//if right not also detected, then only turn left
                {
                    detectionLeftRight = true;
                    speed *= .6f;//reduce speed
                    transform.RotateAround(transform.position, transform.up, Time.deltaTime * anglePerSecond);
                }
            }
            else//if left not also detected, turn only right
            {
                detectionLeftRight = true;
                speed *= .6f;//reduce speed
                //if left detects player, then move left
                transform.RotateAround(transform.position, transform.up, Time.deltaTime * anglePerSecond);//turn away in opposite direction
            }
        }
        else
        {
            //if not blocking view from both raycasts, check individually
            if (Physics.Raycast(lightHit, out hit, detectionDistance, ~ignorePlayer))//ERROR: enemy might not be able to see player if its own body is blocking
            {
                detectionLeftRight = true;
                if (hit.collider.tag != "Player" || hit.collider.tag != "Player_Controller")
                {
                    speed *= .6f;//reduce speed
                    transform.RotateAround(transform.position, transform.up, Time.deltaTime * anglePerSecond);
                }
            }
            else
            {                
                if (Physics.Raycast(lightHit2, out hit2, detectionDistance, ~ignorePlayer))
                {
                    detectionLeftRight = true;
                    if (hit2.collider.tag != "Player" || hit2.collider.tag != "Player_Controller")
                    {
                        speed *= .6f;//reduce speed
                        transform.RotateAround(transform.position, transform.up, Time.deltaTime * -anglePerSecond);

                    }
                }
                else
                {
                    detectionLeftRight = false;
                }
            }

        }
    }

    void back2Wall()
    {
        Ray lightHit3 = new Ray(this.transform.position, leftDetector2);
        RaycastHit hit3;
        Ray lightHit4 = new Ray(this.transform.position, rightDetector2);
        RaycastHit hit4;

        if (detectionLeftRight  == false)
        {
            if (Physics.Raycast(lightHit3, out hit3, detectionDistance2, ~ignorePlayer) && Physics.Raycast(lightHit4, out hit4, detectionDistance2, ~ignorePlayer))//ERROR: enemy might not be able to see player if its own body is blocking
            {
                if (hit3.collider.tag != "Player" || hit3.collider.tag != "Player_Controller")//if left detected
                {
                    if (hit4.collider.tag != "Player" || hit4.collider.tag != "Player_Controller")//if right also detected
                    {
                        speed *= .6f;//reduce speed
                        if (leftTurn)
                        {
                            transform.RotateAround(transform.position, transform.up, Time.deltaTime * -anglePerSecond);//turn away in direction
                        }
                        else
                        {
                            transform.RotateAround(transform.position, transform.up, Time.deltaTime * anglePerSecond);//turn away in opposite direction
                        }
                    }
                    else//if right not also detected, then only turn left
                    {
                        speed *= .6f;//reduce speed
                        transform.RotateAround(transform.position, transform.up, Time.deltaTime * anglePerSecond);
                    }
                }
                else//if left not also detected, turn only right
                {
                    speed *= .6f;//reduce speed
                                 //if left detects player, then move left
                    transform.RotateAround(transform.position, transform.up, Time.deltaTime * anglePerSecond);//turn away in opposite direction
                }
            }
            else
            {
                //if not blocking view from both raycasts, check individually
                if (Physics.Raycast(lightHit3, out hit3, detectionDistance2, ~ignorePlayer))//ERROR: enemy might not be able to see player if its own body is blocking
                {
                    if (hit3.collider.tag != "Player" || hit3.collider.tag != "Player_Controller")
                    {
                        speed *= .6f;//reduce speed
                        transform.RotateAround(transform.position, transform.up, Time.deltaTime * anglePerSecond);
                    }
                }
                else
                {
                    if (Physics.Raycast(lightHit4, out hit4, detectionDistance2, ~ignorePlayer))
                    {
                        if (hit4.collider.tag != "Player" || hit4.collider.tag != "Player_Controller")
                        {
                            speed *= .6f;//reduce speed
                            transform.RotateAround(transform.position, transform.up, Time.deltaTime * -anglePerSecond);

                        }
                    }
                }

            }
        }
    }

    void facing()//rotate body towards tagged object of interest
    {
        this.transform.LookAt(lookAtTarget.transform);
        /*if(lookAtTarget.tag == "Player")
        {
            enemyState = 1;//
        }*/
    }

    public Vector3 randomRotate()
    {
        Vector3 randomRotate = new Vector3(Random.insideUnitCircle.x*5 + this.transform.position.x, transform.position.y, Random.insideUnitCircle.y*5 + this.transform.position.z);
        //Instantiate(grave, randomRotate, Quaternion.identity);
        return randomRotate;
    }

    void search()
    {
        //choose random direction + last detected direction vector
        agent.SetDestination(this.transform.position + ps.lastDetectedDirection.normalized * Random.Range(1,10));
        //move and rotate to that spot
    }

    private Vector3 RandomDirection(float min, float max)
    {
        var x = Random.Range(min, max);
        var y = Random.Range(min, max);
        var z = this.transform.position.z;
        return new Vector3(x, y, z);
    }

    private void Update()
    {
        if(td.alertMode || td.playerAlertDetected)
        {
            enemyNavMeshAgent.speed = speed*1.5f;
        }
        else
        {
            enemyNavMeshAgent.speed = speed;
        }

        enemyHealth = ebc.enemyHealth;

        //********************************************************************************
        //*************************object avoidance************************************** arduino cars
        //********************************************************************************
        if (turnTimer <= 0f)//flip turn bool every time period
        {
            leftTurn = !leftTurn;
            turnTimer = maxTurnTimer;
        }
        else
        {
            turnTimer -= Time.deltaTime;
        }
        //objectAvoidance();
        drawLines();
        //destroy enemy if enemy health is 0
        if (enemyHealth <= 0f)
        {          
            //stop all movement
            agent.isStopped = true;
            Vector3 pos = this.transform.position;
            Quaternion rot = this.transform.rotation;
            pos.y -= 2;
            //if (dnc.dayTime)//if enemy health is 0 and its daytime, destroy enemy
            //{             
                //destroy enemy, spawn grave
                Destroy(this);
                if(grave !=  null)
                {
                    Instantiate(grave, pos, rot);
                }
            //}
        }

        //********************************************************************************
        //*************************object avoidance**************************************
        //********************************************************************************


        //--------------------------------if alertmode is true and looking at player, player is lit


        //NORMAL BEHAVIOR>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        if (td.playerAlertDetected)
        {
            if (agent.destination != player.transform.position)
            {
                agent.SetDestination(player.transform.position);
            }
            this.transform.LookAt(player.transform);
        }
        else
        {
            if (td.playerSusDetected || td.susMode)//if player is suspicious, only look in direction of player, dont follow to player
            {
                this.transform.LookAt(player.transform);
            }
            else
            {

                if ((targetPosition - transform.position).magnitude <= nearEnoughToTarget || here)//if enemy is at destination
                {
                    if (randomLoiterTime > 0f)//if not done loitering, just stay at position and count down
                    {
                        randomLoiterTime -= Time.deltaTime * 1f;//countdown while waiting
                        here = true;

                        //objectAvoidance();
                        if (agent.isStopped == false)
                        {
                            agent.isStopped = true;
                        }
                        if (noticingSomething)//if enemy notices something, face that object
                        {
                            facing();
                        }
                        else//else avoid facing non-tagged objects
                        {
                            objectAvoidance();
                            back2Wall();
                        }
                    }
                    else//if done loitering, moving to another spot
                    {
                        agent.isStopped = false;
                        //move to spot, if not detecting anything interesting
                        if (positions2Move2.Length > 0)//if positions to move to exist
                        {
                            position2Move2Backup = Random.Range(0, positions2Move2.Length);
                            if (td.alertMode && !td.playerAlertDetected)
                            {
                                targetPosition = randomRotate();
                            }
                            else
                            {
                                targetPosition = positions2Move2[position2Move2Backup].transform.position;//pick position
                            }
                            randomLoiterTime = Random.Range(minLoiterTime, maxLoiterTime);//set random loiter time
                            here = false;//no longer here

                        }
                    }
                }
                else
                {
                    agent.SetDestination(targetPosition);
                }
            }
        }

        //if timer not done, run timer, continue loitering
        //else if at target, choose new target position
        //      else move to target
        //if not loitering
        //if enemy is greater than x distance from target position, move closer, else dont
    }

    private void OnDestroy()//make alert collider bigger to alert nearby enemies
    {
        //spawn grave at position? or random position?

        
    }

}
