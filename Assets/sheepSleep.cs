using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sheepSleep : MonoBehaviour
{

    public bool alert, asleep;//alert of danger, sleeping
    public Collider soundCollider, collider;
    public float speed, minSpeed, maxSpeed, rotSpeed;
    private float health, maxHealth, healthRate;//current health, max health, rate health is increased/decreased
    private Vector3 defaultColliderSize;

    private Vector3 forwardDetector, leftDetector, leftDetector2, rightDetector, rightDetector2, move;
    public float detectionDistance, minDetectionDistance, maxDetectionDistance, raisedUpDistance;

    public CharacterController cc;

    // Start is called before the first frame update
    void Start()
    {
        /*collider = this.GetComponent<BoxCollider>();
        defaultColliderSize = collider.bounds.size;

        cc = this.GetComponent<CharacterController>();*/
    }


    void drawRay()//cross (X) pattern for rays
    {
        Debug.DrawRay(this.transform.position, leftDetector * detectionDistance, Color.magenta);//from enemy to player
        Debug.DrawRay(this.transform.position, leftDetector2 * detectionDistance, Color.magenta);//from enemy to player
        Debug.DrawRay(this.transform.position, rightDetector * detectionDistance, Color.magenta);//front of enemy
        Debug.DrawRay(this.transform.position, rightDetector2 * detectionDistance, Color.magenta);//front of enemy
        Debug.DrawRay(this.transform.position, forwardDetector * detectionDistance, Color.magenta);//front of enemy
        Debug.DrawRay(this.transform.position, move, Color.white);//Direction TO MOVE
    }

    void diffuse()
    {
        Ray lightHit = new Ray(this.transform.position, leftDetector);
        RaycastHit hit;
        Ray lightHit2 = new Ray(this.transform.position, rightDetector);
        RaycastHit hit2;
        Ray lightHit3 = new Ray(this.transform.position, leftDetector);
        RaycastHit hit3;
        Ray lightHit4 = new Ray(this.transform.position, rightDetector);
        RaycastHit hit4;
        Physics.Raycast(lightHit, out hit, detectionDistance);
        Physics.Raycast(lightHit2, out hit2, detectionDistance);
        Physics.Raycast(lightHit3, out hit3, detectionDistance);
        Physics.Raycast(lightHit4, out hit4, detectionDistance);

        //add all the vectors to get the vector direction that is most free
        move = this.transform.position + hit.point + hit2.point + hit3.point + hit4.point;
        move.y = raisedUpDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if(asleep)
        {
            detectionDistance = minDetectionDistance;
        }
        if(alert)
        {
            detectionDistance = maxDetectionDistance;
        }

        drawRay();
        forwardDetector = this.transform.forward;
        //forwardDetector.y = raisedUpDistance;
        leftDetector = Quaternion.AngleAxis(45 * -1, Vector3.up) * forwardDetector ;
        leftDetector2 = Quaternion.AngleAxis(90 * -1, Vector3.up) * leftDetector;
        rightDetector = Quaternion.AngleAxis(45, Vector3.up) * forwardDetector;
        rightDetector2 = Quaternion.AngleAxis(90, Vector3.up) * rightDetector;

        //***************************************************************************************
        diffuse();
        
        if(move != Vector3.zero)
        {
            //cc.enabled = false;
            transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
            //cc.enabled = true;
            //cc.Move(move);
        }
    }
}
