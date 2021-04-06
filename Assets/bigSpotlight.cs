using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigSpotlight : MonoBehaviour
{
    public triggerDetection td;
    //public player_Script ps;
    public Light light;
    public bool lightON;
    // Start is called before the first frame update
    void Start()
    {
        /*if (GetComponentInParent<enemy_detection>() != null)
        { 
            ed = GetComponentInParent<enemy_detection>();
        }*/
        td = GetComponentInParent<triggerDetection>();
        light = GetComponent<Light>();
        //ps = GameObject.FindGameObjectWithTag("Player").GetComponent<player_Script>();
        light.enabled = false;
        light.range = td.enemy2Player.magnitude;
        light.spotAngle = td.detectionRadius;
    }

    // Update is called once per frame
    void Update()
    {
        if (td != null)
        { 
            if (td.alertMode)
            {
                this.GetComponent<Light>().enabled = true;
                lightON = true;
            }
            else
            {
                this.GetComponent<Light>().enabled = false;
                lightON = false;
            }
            /*this.GetComponent<Light>().range = //ed.distance;
            this.GetComponent<Light>().spotAngle = *///ed.coneDetectionRadius;
            //range, spot angle control
        }

    }
}
