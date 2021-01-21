using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dayNightCycle_Script : MonoBehaviour
{
    public GameObject ground;
    public GameObject sun;
    public GameObject moon;
    
    public float sunAngle;//current angle of sun
    public float sunRate;
    public float sunQuickRate;
    public bool normalSpeed = true;

    public int dayCounter = 0;

    RaycastHit hit;
    bool cycleOn;
    Vector3 parallel;

    float lengthOfDay;

    public float nightStart, nightEnd;
    public bool dayTime;

    // Start is called before the first frame update
    void Start()
    {
        cycleOn = true;

        //find light, terrain, etc objects 
        if (ground == null)
        { ground = GameObject.FindGameObjectWithTag("Ground"); }
        if (sun == null)
        { sun = GameObject.FindGameObjectWithTag("Sun"); }
        if (moon == null)
        { moon = GameObject.FindGameObjectWithTag("Moon"); }

        //position pivot to center of terrain
        this.transform.position = ground.transform.position;
        //set pivot to flat x axis rotation
        this.transform.localEulerAngles = new Vector3(0,transform.rotation.y, transform.rotation.z);
        //set sun and moon to same position and correct rotation
        sun.transform.position = transform.position;
        moon.transform.position = transform.position;
        sun.transform.rotation = transform.rotation;
        moon.transform.localEulerAngles = new Vector3(sun.transform.rotation.x + 180, sun.transform.rotation.y, sun.transform.rotation.z);
        /*
        //set rotation of sun and moon (moon = sun+180)
        sun.transform.eulerAngles = this.transform.eulerAngles;
        moon.transform.eulerAngles = this.transform.eulerAngles;
        */
        //set sun and moon as child of pivot
        sun.transform.parent = this.transform;
        moon.transform.parent = this.transform;

        lengthOfDay = Time.time;

        //************************************
    }


    void FixedUpdate()
    {
        //if normal speed, sun rotation rate is normal, else, use quick rate
        if (normalSpeed)
        {
            sunAngle += Time.deltaTime * sunRate;
        }
        else { sunAngle += Time.deltaTime * sunQuickRate; }

        //if sun set, daytime is on, and day counter is incremented
        if (sunAngle >= 360)
        {
            sunAngle %= 360;
            dayCounter++;
        }

        if (sunAngle > nightStart && sunAngle < nightEnd)
        {
            dayTime = true;
        }
        else
        {
            dayTime = false;
        }

        transform.localRotation = Quaternion.Euler(sunAngle, 0, 0);
    }

    /*
    private void setSun(float angle)
    {
        //set sun to local angle instantly
    }

    public static float Clamp0360(float eulerAngles)
    {
        float result = eulerAngles - Mathf.CeilToInt(eulerAngles / 360f) * 360f;
        if (result < 0)
        {
            result += 360f;
        }
        return result;
    }

    // Applies a rotation of 90 degrees per second around the Y axis
    void Update()
    {
        if (normalSpeed) {
            transform.rotation *= Quaternion.AngleAxis(sunRate*Time.deltaTime, Vector3.right);
            //sunAngle = sunRate * Time.deltaTime;
        }
        else
        {
            transform.rotation *= Quaternion.AngleAxis(sunQuickRate*Time.deltaTime, Vector3.right);
            //sunAngle = sunQuickRate * Time.deltaTime;
        }
        sunAngle = Clamp0360(transform.localEulerAngles.x);
       
    }
    */

    //sunset = ~80 degrees
    //sunrise = ~290 degrees

    /*
    // Update is called once per frame                                               
    void Update()
    {
    
        sunAngle = this.transform.localEulerAngles.x;

        if (sunAngle == 359)
        {
            lengthOfDay -= Time.time;
            dayCounter++;
            Debug.Log("Day: " + dayCounter + " , (" + lengthOfDay + ")");
            lengthOfDay = Time.time;
            //sunAngle %= 360;
        }

        if (cycleOn)
        {
            if (normalSpeed)
            {
                //sunAngle += sunRate* Time.deltaTime;
                //rotate pivot on local x axis
                //this.transform.RotateAround(this.transform.position, transform.right, (sunRate / (60 * 60 * 24)) * Time.deltaTime);
                this.transform.Rotate((sunRate / (60 * 60 * 24)), 0.0f, 0.0f, Space.Self);//86,400

                //this.transform.eulerAngles.x = Mathf.Atan2(transform.forward.z, transform.forward.x) * Mathf.Rad2Deg;

                //Vector3 eulers = this.transform.rotation.eulerAngles;
                //this.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, eulers.y, eulers.z));
            }
            else 
            {
                //use sunQuickRate 
                //sunAngle += sunQuickRate * Time.deltaTime;
                //Vector3 eulers = this.transform.rotation.eulerAngles;
                //this.transform.rotation = Quaternion.Euler(new Vector3(sunAngle, eulers.y, eulers.z));
                this.transform.Rotate((sunQuickRate / (60 * 60 * 24)), 0.0f, 0.0f, Space.Self);//86,400
            }

        }
        else
        {
            Debug.Log("Sun and Moon rotation is off");           
        }
        Debug.DrawRay(this.transform.position, this.transform.eulerAngles*100, Color.green);
        //sunAngle = ((Vector3.Angle(sun.transform.eulerAngles, transform.forward))/360)/360;
        //sunAngle = ((Vector3.Angle(sun.transform.eulerAngles, transform.forward)) * ((Vector3.Angle(sun.transform.eulerAngles, transform.forward))/360));
        

       // Vector3 sun2d = Vector3.Project(sun.transform.position, Vector3.forward);
       // Vector3 center2d = Vector3.Project(transform.position, Vector3.forward);
        //
       // sunAngle = ((Vector3.Angle(sun2d, center2d)));

        //sun.transform.LookAt(transform.position);//light source is just vector of light direction
        //RotateAround(Vector3 point, Vector3 axis, float angle);
    }*/
}
