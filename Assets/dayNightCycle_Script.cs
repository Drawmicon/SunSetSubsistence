using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dayNightCycle_Script : MonoBehaviour
{
    public GameObject ground;
    public GameObject sun;
    public GameObject moon;
    private Light moonLight;
    private Light sunLight;
    public GameObject player;
    
    public float sunAngle;//current angle of sun
    public float sunQuickRate;
    public bool normalSpeed = true;

    public int dayCounter = 0;

    RaycastHit hit;
    Vector3 parallel;

    float lengthOfDay;

    public float nightStart, nightEnd;
    public bool dayTime;

    public bool forceSun;
    public float forceSunAngle;

    public float sunRate;//rate at which sun angle increases
    public float secondsOfDayTime, secondsOfNightTime;//number of seconds for night mode

    //--------------------------------------
    /*public Color colorStart;
    public Color colorEnd;
    public float duration = 1.0F;*/
    public Material nightSkyMat;
    public Material daySkyMat;
    // Start is called before the first frame update
    void Start()
    {
        daySkyMat = RenderSettings.skybox;
        //*****************************************
        if (secondsOfNightTime > 165f)
        {
            //night starts at 200 degrees to 5 degrees --> 165 degrees total
            sunRate = 165f / secondsOfNightTime;
        }
        else
        {
            secondsOfNightTime = 165;
        }

        if (sunQuickRate > 0f)
        {
            secondsOfDayTime = (360 - 165) / sunQuickRate;
        }
        else
        {
            secondsOfDayTime = 6.5f;
        }
        //*******************************

        //find light, terrain, etc objects 
        if (ground == null)
        { ground = GameObject.FindGameObjectWithTag("Ground"); }
        if (sun == null)
        { sun = GameObject.FindGameObjectWithTag("Sun");
            sunLight = sun.GetComponent<Light>();
        }
        if (moon == null)
        { moon = GameObject.FindGameObjectWithTag("Moon");
            moonLight = moon.GetComponent<Light>();
        }
        if (player == null)
        { player = GameObject.FindGameObjectWithTag("Player_Controller"); }

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
        if(dayTime)
        {
            RenderSettings.skybox = daySkyMat;
            moonLight.enabled = false;
            sunLight.enabled = true;
        }
        else
        {
            RenderSettings.skybox = nightSkyMat;
            moonLight.enabled = true;
            sunLight.enabled = false;
        }

        if (forceSun)
        {
            transform.localRotation = Quaternion.Euler(forceSunAngle%360, 0, 0);
            if (forceSunAngle % 360 > nightStart && forceSunAngle % 360 < nightEnd)
            {
                dayTime = true;
            }
            else
            {
                dayTime = false;
            }

            if (dayTime)//the sun has no position in skybox, so vector from sun to player not possible
            {
                player.GetComponent<player_Script>().playerLit = true;
            }//else the playerlit will be false when out of light during night time
        }
        else
        {

            if (dayTime)//the sun has no position in skybox, so vector from sun to player not possible
            {              
                player.GetComponent<player_Script>().playerLit = true;
            }//else the playerlit will be false when out of light during night time

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
    }
}
