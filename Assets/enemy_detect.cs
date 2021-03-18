using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_detect : MonoBehaviour
{
    private float coneDetectionRadius;
    private Vector3 enemyForwardDirection;
    private Vector3 enemyLookingHere;//currently where the enemy view is
    public Vector3 enemyViewPoint;//target position enemy viewing will move to 

    public GameObject player;
    private player_Script ps;
    public bool playerInLight;
    public float distance;//detection distance
    private Vector3 enemy2playerVec;

    private Renderer enemyRenderer;
    private Color originalEnemyColor;
    private dayNightCycle_Script dn;//check if daytime

    public float stareTimer;
    private float maxStareTimer;
    public GameObject enemyLookAtTargetObject;

    public float lookSpeed, minLookSpeed, maxLookSpeed;
    public int enemyState;
    private enemyViewTarget evt;
    private Vector3 left;

    public float colliderRadius;

    private Vector3 leftDetector, rightDetector;
    public float detectionAngle;

    public float turnTimer, maxTurnTimer;
    private bool leftTurn;
    public float anglePerSecondTurn;

    public bool loitering; //if enemy is not moving towards new target destination

    public float enemyStateTimer, maxEnemyStateTimer;
    private Quaternion _lookRotation;
    private Vector3 _direction;

    public EnemyAI eai;
    // Start is called before the first frame update
    void Start()
    {
        loitering = true;
        enemyStateTimer = maxEnemyStateTimer;
        //enemyLookAtTargetObject = GameObject.FindGameObjectWithTag("Enemy_LookAt_Target");
        enemyForwardDirection = transform.rotation * Vector3.forward * distance;// forward vector of enemy
        enemyViewPoint = enemyForwardDirection * distance;
        leftTurn = false;
        enemyState = 0;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        ps = player.GetComponent<player_Script>();
        // Get the Renderer component from the new cube
        enemyRenderer = this.GetComponentInChildren<Renderer>();
        originalEnemyColor = enemyRenderer.material.color;
        maxStareTimer = stareTimer;
        //evt = enemyLookAtTargetObject.GetComponent<enemyViewTarget>();
        //enemyLookingHere = enemyLookAtTargetObject.transform.position - this.transform.position;
        //enemyForwardDirection = (this.transform.position - this.transform.forward).normalized * distance;
        //eai = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyAI>();
        eai = this.GetComponent<EnemyAI>();
    }


    private Vector3 generateViewPoint(float maxDistance, Vector3 forward)
    {
        Vector3 point = new Vector3();
        float lrMagnitude = Random.Range(0, maxDistance);
        
        if (Random.Range(0, 1) == 0)
        {
            lrMagnitude *= -1;
        }
        Debug.Log("lrMag: " + lrMagnitude);

        //left = Vector3.Cross(forward , Vector3.left).normalized;
        Vector3 dir =  transform.position- enemyForwardDirection;
        left = Vector3.Cross(dir, Vector3.forward).normalized;
        point = ((left*distance - enemyForwardDirection).normalized)*distance;
        Debug.Log("ViewPoint: "+point);
        /*Debug.DrawRay(forward, left, Color.yellow);//left
        Debug.DrawRay(this.transform.position, point, Color.green);//final result
        Debug.DrawRay(this.transform.position, forward, Color.blue);*/
        return point;
    }

    // Update is called once per frame
    void Update()
    {
        //timer to alternate between turning left or right
        if (turnTimer <= 0f)//flip turn bool every time period
        {
            leftTurn = !leftTurn;
            turnTimer = maxTurnTimer;
        }
        else
        {
            turnTimer -= Time.deltaTime;
        }

        if (enemyStateTimer <= 0)
        {
            switch (enemyState)//0 = calm, 1 = suspicious, 2 = alert, 3 = sleep, 4 = stop and glance at point of interest
            {
                case 0:
                    enemyStateTimer = maxEnemyStateTimer;
                    break;
                case 1:
                    enemyStateTimer = maxEnemyStateTimer;
                    enemyState = 0;
                    break;
                case 2:
                    enemyStateTimer = maxEnemyStateTimer;
                    enemyState = 1;
                    break;
                default:
                    break;
            }
        }
        else
        {
            enemyStateTimer -= Time.deltaTime;
        }

        //enemyLookingHere = enemyLookAtTargetObject.transform.position - this.transform.position;//white draw ray
        enemy2playerVec = (player.transform.position - this.transform.position);

        //how long to stare at a point
        if (stareTimer < 0f)
        {
            enemyViewPoint = generateViewPoint(5, enemyForwardDirection * coneDetectionRadius);
            stareTimer = maxStareTimer;
        }
        else
        {
            stareTimer -= Time.deltaTime;
        }


        enemy2playerVec = (player.transform.position - this.transform.position);//enemy to player vector
        enemyForwardDirection = transform.rotation * Vector3.forward * distance;// forward vector of enemy
        leftDetector = Quaternion.AngleAxis(detectionAngle * -1, Vector3.up) * this.transform.forward * distance;
        rightDetector = Quaternion.AngleAxis(detectionAngle, Vector3.up) * this.transform.forward * distance;

        Debug.DrawRay(this.transform.position, enemy2playerVec, Color.green);//from enemy to player
        Debug.DrawRay(this.transform.position, enemyForwardDirection, Color.blue);//front of enemy
        Debug.DrawRay(this.transform.position, leftDetector, Color.magenta);//left
        Debug.DrawRay(this.transform.position, rightDetector, Color.magenta);//right

        //Debug.DrawRay(this.transform.position, enemyViewPoint, Color.red);//enemy viewing destination, will lock on to enemy
        Debug.DrawRay(this.transform.position, enemyLookingHere, Color.white);//where enemy is looking currently

        /*
        Ray lightHit4 = new Ray(this.transform.position, enemy2playerVec);
        RaycastHit hit4;
        Physics.Raycast(lightHit4, out hit4, distance);

        Ray lightHit3 = new Ray(this.transform.position, enemyForwardDirection);
        RaycastHit hit3;
        Physics.Raycast(lightHit3, out hit3, distance);

        Ray lightHit = new Ray(this.transform.position, leftDetector);
        RaycastHit hit;
        Ray lightHit2 = new Ray(this.transform.position, rightDetector);
        RaycastHit hit2;
        */
        /*Physics.Raycast(lightHit3, out hit3, distance);
        Physics.Raycast(lightHit, out hit, distance);
        Physics.Raycast(lightHit2, out hit2, distance);*/
        Ray lightHit = new Ray(this.transform.position, enemy2playerVec);//Check if distance from enemy to player if clear
        RaycastHit hit;
        Physics.Raycast(lightHit, out hit, distance);
        //if player is in viewable position in front of enemy; angle and distance, and view to player is not blocked
        if ((Vector3.Angle(enemyForwardDirection, enemy2playerVec) <= coneDetectionRadius && enemy2playerVec.magnitude <= distance) && (hit.collider.tag == "Player" || hit.collider.tag == "Player_Controller"))
        
            //if enemy is in sight range, aka if view to player is not blocked and within viewing angle
            //if (( hit4.collider.tag == "Player" || hit4.collider.tag == "Player_Controller") && Vector3.Angle(enemy2playerVec, enemyLookingHere) < coneDetectionRadius && enemy2playerVec.magnitude <= distance)
            /* if (hit4.collider != null)
             {
                 if ((hit4.collider.tag == "Player" || hit4.collider.tag == "Player_Controller"))//check if raycast hits player
                 {
                     if (Vector3.Angle(enemy2playerVec, enemyForwardDirection) < coneDetectionRadius)//check if within viewing range
                     */
            {

                    if (enemy2playerVec.magnitude <= distance)//check if within distance
                    {

                        if (ps.playerLit || dn.dayTime || enemyState == 1 || enemyState == 2)// if player is in lamp light, or its day time, or enemy is alert
                        {
                            enemyState = 1;

                            this.transform.LookAt(player.transform);

                            if (enemyForwardDirection.normalized == enemy2playerVec.normalized)
                            {
                                enemyState = 2;
                                lookSpeed = maxLookSpeed;
                            }
                        }
                    }
                }

        else//else if player not detected, if waiting, look around, else look at target destination
        {

            /*if (loitering)//if waiting around, look around
            {
                if (enemyLookingHere.normalized == (enemyViewPoint-transform.position).normalized)
                {
                    enemyViewPoint = generateViewPoint(distance, enemyForwardDirection);

                    //find the vector pointing from our position to the target
                    _direction = (enemyViewPoint - transform.position).normalized;

                    //create the rotation we need to be in to look at the target
                    _lookRotation = Quaternion.LookRotation(_direction);
                }

                //rotate us over time according to speed until we are in the required rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * lookSpeed);
            }
            else//else look towards the destination, unless view blocked
            {
                Physics.Raycast(lightHit3, out hit3, distance);
                //if collider blocking view of object, look around by turning away from that direction
                if (enemyLookingHere.normalized != (enemyViewPoint - transform.position).normalized)
                {
                    if (Physics.Raycast(lightHit, out hit, distance) && Physics.Raycast(lightHit2, out hit2, distance))
                    {
                        if (hit.distance < hit2.distance)
                        {
                            //turn left
                            transform.RotateAround(transform.position, transform.up, Time.deltaTime * anglePerSecondTurn);
                        }
                        else
                        {
                            //turn right
                            transform.RotateAround(-transform.position, transform.up, Time.deltaTime * anglePerSecondTurn);
                        }
                    }
                    else
                    {
                        if (Physics.Raycast(lightHit, out hit, distance))
                        {
                            //turn left
                            transform.RotateAround(transform.position, transform.up, Time.deltaTime * anglePerSecondTurn);
                        }
                        else
                        {
                            if (Physics.Raycast(lightHit2, out hit2, distance))
                            {
                                //turn right
                                transform.RotateAround(transform.position, transform.up * -1, Time.deltaTime * anglePerSecondTurn);
                            }
                        }
                    }
                }
                else
                {
                    _direction = (enemyViewPoint - transform.position).normalized;

                    //create the rotation we need to be in to look at the target
                    _lookRotation = Quaternion.LookRotation(_direction);

                    //rotate us over time according to speed until we are in the required rotation
                    transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * lookSpeed);
                }
            }
            */

            if (leftTurn)
            {
                transform.RotateAround(transform.position, transform.up, Time.deltaTime * lookSpeed);
            }
            else
            {
                transform.RotateAround(transform.position, -transform.up, Time.deltaTime * lookSpeed);
            }
        }

        /* Debug.DrawRay(enemyForwardDirection, left, Color.yellow);//left
         Debug.DrawRay(this.transform.position, enemyViewPoint, Color.green);//final result
         Debug.DrawRay(this.transform.position, enemyForwardDirection, Color.blue);*/

            //light2playerVec = targetPoint - initialPoint;

            //Debug.DrawRay(this.transform.position, this.transform.parent.forward * distance, Color.blue);//front of enemy

            //Debug.DrawRay(this.transform.position, enemyViewPoint, Color.red);//enemy focus direction
            //Debug.DrawRay(this.transform.position, enemyLookAtTargetObject.transform.position - this.transform.position, Color.white);//target view object is located
    }

    //add objects to list of objects to point to with raycasts
    private void OnCollisionEnter(Collision collision)
    {
        
    }

    //remove objects from list
    private void OnCollisionExit(Collision collision)
    {
        
    }


}


/*
 * use random points in a circle facing the enemy as random points the enemy can face, giving them more random looking motion 
 */