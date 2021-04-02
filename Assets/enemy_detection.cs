using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_detection : MonoBehaviour
{
    public float coneDetectionRadius;
    private Vector3 enemyFacingDirection;
    public Vector3 enemyViewPoint;

    //if player is in angle range, then move facing vector to player
    //once facing vector is on player, alert, else if player goes away, suspicious
    //*****************************
    public GameObject player;
    public bool playerInLight;
    public float distance;
    private Vector3 enemy2playerVec;
    private Renderer playerRenderer;
    public Renderer enemyRenderer;
    private Color originalEnemyColor;
    private dayNightCycle_Script dn;

    //********************
    public bool doneLooking;
    public float stareTimer;
    private float maxStareTimer;
    public GameObject enemyLookAtTargetObject;
    public GameObject enemyFront;
    public float lookAtSpeed;

    public GameObject frontPlane;
    public float maxLookSpeed;

    public int enemyState;

    private enemyViewTarget evt;
    private Vector3 enemyLookingHere;

    //*******************************

    public Light spotlight;//light that shines when found player
    public EnemyAI eai;//script to motor control of enemy
    public player_Script ps;//script to player

    public float maxEnemyStateTimer;//max time for timer that switches to another enemy state 
    private float enemyStateTimer;//current time for enemy state
    public Vector3 enemyForwardDirection;//vector pointing to front of enemy

    public float turnTimer, maxTurnTimer;
    private bool leftTurn;

    //************************************




    // Start is called before the first frame update
    void Start()
    {
        enemyState = 0;
        //enemyViewPoint = frontPlane.transform.position;
        doneLooking = true;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Get the Renderer component from the new cube
        playerRenderer = player.GetComponent<Renderer>();
        enemyRenderer = this.GetComponentInParent<Renderer>();
        originalEnemyColor = enemyRenderer.material.color;

        maxStareTimer = stareTimer;

        evt = enemyLookAtTargetObject.GetComponent<enemyViewTarget>();
        lookAtSpeed = evt.speed;

        enemyLookingHere = enemyLookAtTargetObject.transform.position - this.transform.position;

        eai = this.GetComponentInParent<EnemyAI>();

        spotlight = this.GetComponentInChildren<Light>();
        ps = player.GetComponent<player_Script>();
    }

    public Vector3 findPointInCircle()
    {
        Vector3 B = Vector3.ProjectOnPlane(Random.insideUnitSphere, enemyForwardDirection).normalized * coneDetectionRadius;

        return B;

        //https://www.youtube.com/watch?v=UJxgcVaNTqY&ab_channel=KhanAcademy
        //https://answers.unity.com/questions/1618126/given-a-vector-how-do-i-generate-a-random-perpendi.html
        //https://math.stackexchange.com/questions/1396824/how-to-get-circle-points-in-3d-given-a-radius-and-a-vector-orthogonal-to-the-cir
        //https://math.stackexchange.com/questions/1172847/get-circle-around-line-in-3d-plane-where-all-points-of-the-circle-lie-on-the-lin
    }

    private Vector3 randomRadiusPoint(float distance, float maxTheta, float minTheta, Vector3 origin)
    {
        //https://stackoverflow.com/questions/30619901/calculate-3d-point-coordinates-using-horizontal-and-vertical-angles-and-slope-di
        //https://stackoverflow.com/questions/4642687/given-start-point-angles-in-each-rotational-axis-and-a-direction-calculate-end
        /*
         x = radius * sin(theta) * cos(phi);
        y = radius * sin(theta) * sin(phi);
        z = radius * cos(theta);
         */
        Vector3 point;

        /*point.x = origin.x+distance * Mathf.Sin((Random.Range(maxTheta, minTheta))) * Mathf.Cos((Random.Range(maxTheta, minTheta)));
         point.y = origin.y + distance * Mathf.Sin((Random.Range(maxTheta, minTheta))) * Mathf.Sin((Random.Range(maxTheta, minTheta)));
         point.z = origin.z + distance * Mathf.Cos((Random.Range(maxTheta, minTheta)));*/

        point.x = origin.x + distance * Mathf.Sin((Random.Range(maxTheta, minTheta))) * Mathf.Cos((Random.Range(maxTheta, minTheta)));
        point.y = origin.y + distance * Mathf.Sin((Random.Range(maxTheta, minTheta))) * Mathf.Sin((Random.Range(maxTheta, minTheta)));
        point.z = origin.z + distance * Mathf.Cos((Random.Range(maxTheta, minTheta)));


        /*point =(Random.insideUnitSphere * maxTheta);
        Vector3 myVector = Quaternion.Euler(90, 0, 0) * this.transform.parent.forward;
        point = RotatePointAroundPivot(point, this.transform.parent.forward, myVector);//rotate 90 degrees in local x axis of parent*/
        /*float distanceFloat = Random.Range(minTheta, maxTheta);
        Vector3 directionVector1 = this.transform.parent.up*distanceFloat;
        Vector3 directionVector2 = this.transform.parent.right*distanceFloat;
        point = origin + (directionVector1 * distanceFloat);*/


        return point;
    }

    //https://answers.unity.com/questions/532297/rotate-a-vector-around-a-certain-point.html
    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }

    Vector3 RandomPointInCircle(Vector3 center, float radius, float angle)
    {    //Draws circle of radius, with center center, and locates a point on that circle within angle angle     
        Vector3 position;
        position.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        position.y = center.y + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        position.z = center.z;
        return position;
    }

    //https://forum.unity.com/threads/find-a-random-point-on-a-plane-solution.243431/
    //gets random point in object mesh 
    Vector3 pointInObject(GameObject plane, float scale)
    {
        Vector3 pos;
        Vector3 min = plane.GetComponent<MeshFilter>().mesh.bounds.min;
        Vector3 max = plane.GetComponent<MeshFilter>().mesh.bounds.max;
        return pos = plane.transform.position - new Vector3((Random.Range(min.x * scale, max.x * scale)), plane.transform.localPosition.y, (Random.Range(min.z * scale, max.z * scale)));
    }


    Vector3 rotateVector(Vector3 originalVector, float xRot, float yRot, float zRot)//USE THIS TO FIND POSITION OF TARGET_VIEWING_OBJECT
    {
        Vector3 rotatedVector;

        rotatedVector = Quaternion.Euler(0, 180, 90) * (originalVector*distance);

        return rotatedVector;
    }

    //*********************************************************
    //*********************************************************
    //*********************************************************
    //*********************************************************
    //*********************************************************
    //*********************************************************

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

        switch (enemyState)//0 = calm, 1 = suspicious, 2 = alert, 3 = sleep, 4 = stop and glance at point of interest
        {
            case 0:
                evt.speed = lookAtSpeed;
                spotlight.enabled = false;
                evt.speed = lookAtSpeed;
                break;
            case 1:
                evt.speed = maxLookSpeed * .7f;
                enemyRenderer.material.SetColor("_Color", Color.gray);
                spotlight.enabled = true;
                break;
            case 2:
                evt.speed = maxLookSpeed;
                enemyRenderer.material.SetColor("_Color", Color.black);
                spotlight.enabled = true;
                break;
            case 3:
                evt.speed = lookAtSpeed * .1f;
                break;
            case 4:
                evt.speed = maxLookSpeed * .5f;
                break;
            default:
                evt.speed = lookAtSpeed;
                spotlight.enabled = false;
                break;
        }

        enemyLookingHere = enemyLookAtTargetObject.transform.position - this.transform.position;//white draw ray
        enemyFacingDirection = (enemyViewPoint - this.transform.position).normalized * distance;
        enemy2playerVec = (player.transform.position - this.transform.position);
        enemyForwardDirection = eai.forwardDetector;

        //light2playerVec = targetPoint - initialPoint;
        Debug.DrawRay(this.transform.position, enemy2playerVec * distance, Color.green);//from enemy to player
        Debug.DrawRay(this.transform.position, this.transform.parent.forward * distance, Color.blue);//front of enemy
        Debug.DrawRay(this.transform.position, enemyFacingDirection, Color.red);//where enemy is looking
        //Debug.DrawRay(this.transform.position, this.transform.forward*(distance-1), Color.yellow);//where enemy is looking at
        Debug.DrawRay(this.transform.position, enemyLookAtTargetObject.transform.position - this.transform.position, Color.white);//target view object is located

        Ray lightHit = new Ray(this.transform.position, enemy2playerVec);
        RaycastHit hit;
        Physics.Raycast(lightHit, out hit, distance);
        //if player is in viewable position in front of enemy

        if (ps.playerLit || (enemyState == 1 || enemyState == 2))
        {
            if (Vector3.Angle(enemyLookingHere, enemy2playerVec) <= coneDetectionRadius && enemy2playerVec.magnitude <= distance && hit.collider.tag == "Player" )
            {

                enemyViewPoint = player.transform.position;
                stareTimer = maxStareTimer * 2;

                enemyState = 1;
                if (enemy2playerVec.normalized == enemyLookingHere.normalized)
                {
                    enemyState = 2;
                }
                else
                {//look around

                    if (leftTurn)
                    {
                        transform.RotateAround(transform.position, transform.up, Time.deltaTime * maxLookSpeed);
                    }
                    else
                    {
                        transform.RotateAround(transform.position, -transform.up, Time.deltaTime * maxLookSpeed);
                    }
                }
            }
        }
        else
        {
            //enemyViewPoint = randomRadiusPoint(distance, coneDetectionRadius, 1, enemyFront.transform.position);
            enemyRenderer.material.SetColor("_Color", originalEnemyColor);

            //if random timer is done
            if (stareTimer <= 0f /*&& Vector3.Angle(enemyLookingHere, enemy2playerVec) > coneDetectionRadius && enemy2playerVec.magnitude > distance*/)
            {
                //if enemy view target object is at the focus point position, get new focus point position, otherwise move to focus point
                if (enemyLookAtTargetObject.transform.position == enemyViewPoint)//object moves to vector point
                {
                    stareTimer = maxStareTimer;
                    //enemyViewPoint = (this.transform.parent.forward - transform.position).normalized * distance;
                    //choose random point in unit cirle OR choose random distance and angle (within limits) to get new position in relation to forwards vector
                    //enemyViewPoint = randomRadiusPoint(distance, coneDetectionRadius, 1, enemyFront.transform.position);
                    //enemyViewPoint = rotateVector(enemyForwardDirection, Random.Range(0f, coneDetectionRadius), Random.Range(0f, coneDetectionRadius), Random.Range(0f, coneDetectionRadius));
                    //enemyViewPoint = RandomPointInCircle(this.transform.parent.transform.forward*distance, coneDetectionRadius, Random.Range(0, coneDetectionRadius));
                    //In different script, move enemy view object to location in circle in front of vector.forward of enemy
                    enemyViewPoint = findPointInCircle();
                    /*
                     
                     !!!!!!!!
                    !!!!!!!!!!
                    !!!!!!!!!!
                     !!!!!!!!

                       !!!
                       !!!
                     */
                }
                else
                {
                    this.transform.LookAt(enemyLookAtTargetObject.transform, this.transform.parent.up);

                    /*if(Vector3.Angle(enemyFacingDirection, enemy2playerVec) <= coneDetectionRadius)
                    {
                        enemyViewPoint = player.transform.position;
                        stareTimer = maxStareTimer*2;
                    }*/
                }
            }
            else
            {
                //if timer not done, count down timer
                stareTimer -= Time.deltaTime;//down 1 per 1 second
                this.transform.LookAt(enemyLookAtTargetObject.transform, this.transform.parent.up);

            }
        }

        /*
        // Declare a raycast hit to store information about what our raycast has hit
        Ray lightHit = new Ray(this.transform.position, enemy2playerVec);
        RaycastHit hit;
        if (Physics.Raycast(lightHit, out hit, distance))
        {
            if (hit.collider.tag == "Player")
            {
                if (Vector3.Angle(enemy2playerVec, enemyFacingDirection) <= coneDetectionRadius)
                {
                    Debug.Log("Player Detected By Enemy");
                    //Call SetColor using the shader property name "_Color" and setting the color to red
                    //playerRenderer.material.SetColor("_Color", Color.yellow);
                    enemyRenderer.material.SetColor("_Color", Color.yellow);

                    enemyLookAtTargetObject.transform.position = player.transform.position;
                    stareTimer = 20f;
                    //move enemy view target to player's current position
                    //if enemy view point is directly on player, player is spotted
                    //set player to red, full alert
                }
                else
                {
                    //Debug.Log("Enemy Angle: " + Vector3.Angle(enemy2playerVec, Vector3.forward));
                    //enemyRenderer.material.SetColor("_Color", originalEnemyColor);
                }
            }
            else
            {
                //if any other collider, have enemy look at object, until out of range
            }
        }
    }*/
    }

    //FIXED ERROR: set ground to different layer, thats why enemy looks at center of terrain object at the start of the game
    private void OnTriggerEnter(Collider other)//Error: may be able to detect behind walls
    {
        if (other.tag != null)//if tagged object detected, check if in view angle range, then set as focus position
        {
            Vector3 t = (other.transform.position - transform.position);
            if (Vector3.Angle(enemyLookingHere, t) <= coneDetectionRadius)
            {
                // enemyViewPoint = other.transform.position;
                //stareTimer = maxStareTimer;

                Ray lightHit2 = new Ray(this.transform.position, t);
                RaycastHit hit2;
                if (Physics.Raycast(lightHit2, out hit2, distance))//ERROR: enemy might not be able to see player if its own body is blocking, fixed with different layer
                {
                    if (hit2.collider.tag != null)// check if object is not blocked by object
                    {
                        enemyViewPoint = other.transform.position;
                        enemyRenderer.material.SetColor("_Color", Color.cyan);
                        enemyState = 3;//slightly interest, just pause at this location for a while
                        stareTimer = maxStareTimer;
                    }

                }
            }
        }
    }
}


/*
 *use random points in a circle facing the enemy as random points the enemy can face, giving them more random looking motion 
 */