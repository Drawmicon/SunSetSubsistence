using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightVisibility : MonoBehaviour
{

    public GameObject player;
    public bool playerInLight;
    public float lightDistance;
    public Vector3 light2playerVec;
    private Renderer playerRenderer;
    private Color originalColor;

    private dayNightCycle_Script dn;

    public Light lt;
    public bool manualLightSwitch = false;
    public bool lightSwitch = true;

        // Start is called before the first frame update
    void Start()
    {
        playerInLight = false;
        player = GameObject.FindGameObjectWithTag("Player");

       // Get the Renderer component from the new cube
       playerRenderer = player.GetComponent<Renderer>();
        originalColor = playerRenderer.material.color;

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
    }

    // Update is called once per frame
    void Update()
    {

        if(dn.dayTime && !manualLightSwitch)
        {
            lightSwitch = true;
        }
        else
        {
            lightSwitch = false;
        }

        if(!lightSwitch)
        {
            lt.range = 0;
        }
        else
        {
            lt.range = lightDistance*1.33f;
        }
        

        light2playerVec = (player.transform.position - this.transform.position).normalized;
        //light2playerVec = targetPoint - initialPoint;
        Debug.DrawRay(this.transform.position, light2playerVec*lightDistance, Color.green);
        // Declare a raycast hit to store information about what our raycast has hit
        Ray lightHit = new Ray(this.transform.position, light2playerVec);
        RaycastHit hit;
        if (Physics.Raycast(lightHit, out hit, lightDistance))
        {
            if (hit.collider.tag == "Player")
            {
                //Call SetColor using the shader property name "_Color" and setting the color to red
                playerRenderer.material.SetColor("_Color", Color.red);
                //playerRenderer.material.SetColor("OriginalColor", originalColor);
            }
            else
            {
                playerRenderer.material.SetColor("_Color", originalColor);
                //playerRenderer.material.SetColor("_Color", Color.red);
            }

        }
        else
        {
            playerRenderer.material.SetColor("_Color",originalColor);
            //playerRenderer.material.SetColor("_Color", Color.red);
        }
    }
}
