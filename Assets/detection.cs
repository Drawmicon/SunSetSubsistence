using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detection : MonoBehaviour
{
    public float coneDetectionRadius;
    public float distance; 
    private Vector3 enemyFacingDirection;

    //if player is in angle range, then move facing vector to player
    //once facing vector is on player, alert, else if player goes away, suspicious
    //*****************************
    public GameObject player;
    public bool playerInLight;
    private Vector3 enemy2playerVec;
    private Renderer playerRenderer;
    public Renderer enemyRenderer;
    private Color originalEnemyColor;
    private dayNightCycle_Script dn;

    //********************
    public bool doneLooking;
    public float stareTimer;
    private float maxStareTimer;
    public float lookAtSpeed;

    public float maxLookSpeed;

    public int enemyState;

    //*******************************

    public Light spotlight;//light that shines when found player
    public EnemyAI eai;//script to motor control of enemy
    public player_Script ps;//script to player

    public float maxEnemyStateTimer;//max time for timer that switches to another enemy state 
    private float enemyStateTimer;//current time for enemy state
    public Vector3 enemyForwardDirection;//vector pointing to front of enemy

    public float turnTimer, maxTurnTimer;
    private bool leftTurn;

    public GameObject enemyTargetLookAtObject;

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

        eai = this.GetComponentInParent<EnemyAI>();

        spotlight = this.GetComponentInChildren<Light>();
        ps = player.GetComponent<player_Script>();
    }

    //******************************************************************************************************
    // Maximum turn rate in degrees per second.
    public float turningRate = 30f;
    // Rotation we should blend towards.
    private Quaternion _targetRotation = Quaternion.identity;
    // Call this when you want to turn the object smoothly.
    public void SetBlendedEulerAngles(Vector3 angles)
    {
        _targetRotation = Quaternion.Euler(angles);
    }
    // Turn towards our target rotation.
    // transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, turningRate * Time.deltaTime);
    //******************************************************************************************************

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

        rotatedVector = Quaternion.Euler(0, 180, 90) * originalVector;

        return rotatedVector;
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

        switch (enemyState)//0 = calm, 1 = suspicious, 2 = alert, 3 = sleep, 4 = stop and glance at point of interest
        {
            case 0:
                spotlight.enabled = false;
                break;
            case 1:
                enemyRenderer.material.SetColor("_Color", Color.gray);
                spotlight.enabled = true;
                break;
            case 2:
                enemyRenderer.material.SetColor("_Color", Color.black);
                spotlight.enabled = true;
                break;
            case 3:
                break;
            case 4:
                break;
            default:
                spotlight.enabled = false;
                break;
        }

        enemy2playerVec = (player.transform.position - this.transform.position);

        //light2playerVec = targetPoint - initialPoint;
        Debug.DrawRay(this.transform.position, enemy2playerVec * distance, Color.green);//from enemy to player
        //Debug.DrawRay(this.transform.position, this.transform.parent.forward * distance, Color.blue);//front of enemy
        Debug.DrawRay(this.transform.position, enemyFacingDirection, Color.red);//where enemy is looking

        Ray lightHit = new Ray(this.transform.position, enemy2playerVec);
        RaycastHit hit;
        Physics.Raycast(lightHit, out hit, distance);
        //if player is in viewable position in front of enemy

        if (ps.playerLit || (enemyState == 1 || enemyState == 2))
        {
            if (Vector3.Angle(enemyFacingDirection, enemy2playerVec) <= coneDetectionRadius && enemy2playerVec.magnitude <= distance && hit.collider.tag == "Player")
            {
                //Face player now, until out of sight
                stareTimer = maxStareTimer * 2;

                enemyState = 1;
                if (enemy2playerVec.normalized == enemyFacingDirection.normalized)
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

            }
            else
            {
                //if timer not done, count down timer
                stareTimer -= Time.deltaTime;//down 1 per 1 second

            }
        }
    }
}

//https://forum.unity.com/threads/how-do-you-rotate-a-vector.46764/
//rotatedVector = Quaternion.Euler(0,180,90) * originalVector;