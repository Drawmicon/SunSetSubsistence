using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wave_maker : MonoBehaviour
{

    private dayNightCycle_Script dn;
    public float waveHeight;
    private float originalY;
    // Start is called before the first frame update
    void Start()
    {
        dn = (GameObject.FindGameObjectWithTag("SunMoonController")).GetComponent<dayNightCycle_Script>();
        originalY = this.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(this.transform.position.x,Mathf.Sin(dn.sunAngle)* waveHeight+originalY, this.transform.position.z);
    }
}
