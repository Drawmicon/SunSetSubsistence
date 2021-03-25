using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Dont block player collider with anything, like the sound collider or such
public class lightVisibility : MonoBehaviour
{

    public GameObject player;
    public GameObject fireObject;
    public bool playerInLight;
    public float lightDistance;
    public Vector3 light2playerVec;
    private Renderer playerRenderer;
    private Color originalColor;

    private dayNightCycle_Script dn;

    public Light lt;
    public bool manualLightSwitch = false;
    public bool lightSwitch = true;
    private float raymond;
    private float defaultIntensity;
    public float flickerRange;
    public float dimRate;

    public player_Script ps;

        // Start is called before the first frame update
    void Start()
    {
        defaultIntensity = lt.intensity;
        playerInLight = false;

        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if(ps == null)
        {
            ps = player.GetComponent<player_Script>();
        }

        // Get the Renderer component from the new cube
        if (player.GetComponent<Renderer>() != null)
        {
            playerRenderer = player.GetComponent<Renderer>();
            originalColor = playerRenderer.material.color;
        }
        GameObject parent = (this.transform.parent.gameObject);
        
        if (lt != null)
        {
            lt.range = lightDistance*1.33f;
        }
        else
        {
            Debug.Log("Error: No light attached to the light visibility script");
        }
        dn = (GameObject.FindGameObjectWithTag("SunMoonController")).GetComponent<dayNightCycle_Script>();

        raymond = lightDistance;
    }

    void flicker(float range)
    {
        lt.intensity = Random.Range(defaultIntensity + range, defaultIntensity + -range);
    }

    void dimmer(bool onOff)
    {
        if (onOff && lt.intensity < defaultIntensity) 
        {
            lt.intensity+= dimRate;
        }
        if(!onOff && lt.intensity > 0)
        {
            lt.intensity -= dimRate;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(!dn.dayTime && !manualLightSwitch)//if it is night time, and no manual control, set to OFF
        {
            lightSwitch = true;
        }
        else
        {
            if (dn.dayTime && !manualLightSwitch)//if it is day time and manual control is OFF
            { lightSwitch = false; }
        }

        if(!lightSwitch)//if light switch is off, set light brightness range to 0, otherwise, set to standard light distance(normal range)
        {
            if (lt.range > 0)//if intensity is above 0, reduce
            {
                lt.range -= dimRate;
                fireObject.SetActive(false);
            }
            //lt.range = 0;
            //raymond = 0f;
        }
        else
        {
            if (lt.range < lightDistance * 1.3f)//if intensity is less than max, increase
            {
                lt.range += dimRate;
                fireObject.SetActive(true);
            }

            //lt.range = lightDistance * 1.3f;
            //raymond = lightDistance;
        }

        flicker(flickerRange);
        light2playerVec = (player.transform.position - this.transform.position).normalized;
        //light2playerVec = targetPoint - initialPoint;
        Debug.DrawRay(this.transform.position, light2playerVec*raymond, Color.green);//line from light to player
        // Declare a raycast hit to store information about what our raycast has hit
        Ray lightHit = new Ray(this.transform.position, light2playerVec);
        RaycastHit hit;
        if (Physics.Raycast(lightHit, out hit, raymond))
        {
            if (hit.collider.tag == "Player" || hit.collider.tag == "playerSound" || hit.collider.tag == "Gargoyle" )
            {
                Debug.Log("Raycast hit object: " + hit.collider.name + "(tag:" + hit.collider.tag + ")");
                //player.GetComponentInParent<player_Script>().playerLit = true;
                //ps.playerLit = true;
                playerInLight = true;
                //Call SetColor using the shader property name "_Color" and setting the color to red
                //playerRenderer.material.SetColor("_Color", Color.red);       
            }
            else
            {
                Debug.Log("Raycast hit object: " + hit.collider.name + "(tag:" + hit.collider.tag + ")");
                if (dn.dayTime == false)//if player is lit, but daytime, player is not lit by lamp
                {
                    //player.GetComponentInParent<player_Script>().playerLit = false;
                    //ps.playerLit = false;
                    playerInLight = false;
                }
                //playerRenderer.material.SetColor("_Color", originalColor);
            }
        }
        else
        {
            if (dn.dayTime == false)
            {
                //player.GetComponentInParent<player_Script>().playerLit = false;
                //ps.playerLit = false;
                playerInLight = false;
            }
            //playerRenderer.material.SetColor("_Color",originalColor);

        }
    }
}
