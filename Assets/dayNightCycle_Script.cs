using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dayNightCycle_Script : MonoBehaviour
{
    public GameObject ground;
    public GameObject sun;
    public GameObject moon;
    public float radius;
    public float sunAngle;
    public float sunRate;

    RaycastHit hit;
    bool cycle;
    Vector3 parallel;
    // Start is called before the first frame update
    void Start()
    {
        cycle = true;
        //set position to same position, aka center
        this.transform.position = ground.transform.position;
        sun.transform.position = transform.position + Vector3.forward * radius;
        /*
         •	Light sources rotate around centerTarget at a rate of minutes per hour *hours per day (60*24 = 1440) / 360 degrees = 4 degrees per real time second 
         •   angle += Time.deltaTime * 4f; // 4 degrees per second

         */
        parallel = (sun.transform.position - this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(!cycle)
        {
            sun.transform.RotateAround(this.transform.position, Vector3.forward, 0f * Time.deltaTime);
        }
        else
        {
            sun.transform.RotateAround(this.transform.position, Vector3.forward, (sunRate / (60 * 60 * 24)) * Time.deltaTime);
            
        }
        Debug.DrawRay(sun.transform.position, sun.transform.eulerAngles*100, Color.green);
        //sunAngle = ((Vector3.Angle(sun.transform.eulerAngles, transform.forward))/360)/360;
        //sunAngle = ((Vector3.Angle(sun.transform.eulerAngles, transform.forward)) * ((Vector3.Angle(sun.transform.eulerAngles, transform.forward))/360));

       /* Vector3 sun2d = Vector3.Project(sun.transform.position, Vector3.forward);
        Vector3 center2d = Vector3.Project(transform.position, Vector3.forward);

        sunAngle = ((Vector3.Angle(sun2d, center2d)));*/

        //sun.transform.LookAt(transform.position);//light source is just vector of light direction
        //RotateAround(Vector3 point, Vector3 axis, float angle);
    }
}
