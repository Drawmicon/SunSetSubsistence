using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_Script : MonoBehaviour
{
    public Camera mainCam;
    public float camDistance;
    public float camRotSpeed;

    public bool gargVisible = true;
    public GameObject garg;
    public Slider healthBar;
    public GameObject player;
    public CharacterController pCon;
    public float healthScore;
    public float MaxHealthScore;
    public float solarRegenRate;
    public float lunarDegenRate;
    public bool healthModeActive;
    public bool playerLit;
    public Vector3 lastGroundedPosition;
    //private lightVisibility dn;
    public dayNightCycle_Script dnc;

    public Vector3 step1, step2;
    public float stepCounter,stepCounterMax;

    public bool detected;
    public float stepTimer, maxStepTimer;
    public Vector3 position1, position2, lastDetectedDirection;

    public cinemaCameraController ccc;
    public cinemaCameraExtra cce;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Gargoyle") != null)
        {
            garg = GameObject.FindGameObjectWithTag("Gargoyle");
            garg.SetActive(true);
        }
        detected = false;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player_Controller");
        }
       pCon = player.GetComponent<CharacterController>();
        healthScore = MaxHealthScore;
        healthBar.value = playerHealthSlider();
        step1 = this.transform.position;

        if(ccc == null)
        {
            ccc = GetComponentInChildren<cinemaCameraController>();
        }

        if (cce == null)
        {
            cce = GetComponentInChildren<cinemaCameraExtra>();
        }
    }

    public float playerHealthSlider()
    {
        if(healthScore <= 0)
        {
            return 0f;
        }
        return Mathf.Abs(healthScore/MaxHealthScore);
    }

    public void setGargoyleStatue(bool set)
    {
        if(garg != null)
        {
            garg.SetActive(set);
        }
        else { Debug.LogError("Gargoyle Statue Gameobject not set in player_script"); }
    }

    // Update is called once per frame
    void Update()
    {
        if(cce.atParentPosition)
        {
            garg.SetActive(false);
        }
        else
        {
            garg.SetActive(true);
        }

        healthBar.value = playerHealthSlider();

        if (pCon.isGrounded)
        {
           lastGroundedPosition = player.transform.position;
        }

        if (healthModeActive)
        {
            if (healthScore <= 0)
            {
                Debug.Log("Player Health 0");
            }
            else
            { 
                if (dnc.dayTime && Time.timeScale == 1)//if player's health is active and is greater than 0, and its daytime, and time scale is normal
                {
                    dnc.normalSpeed = false;

                    if (healthScore < MaxHealthScore)
                    {
                        healthScore += solarRegenRate * Time.deltaTime;
                    }
                    ccc.switchCamMode = false;
                    
                }
                else
                {
                    if (Time.timeScale == 1)
                    {
                        dnc.normalSpeed = true;
                        healthScore -= lunarDegenRate * Time.deltaTime;
                    
                    ccc.switchCamMode = true;
                    }
                }
            }
        }

        /*if (stepCounter <= 0f)//gets current and former positions every time interval
        {
            step2 = step1;
            step1 = this.transform.position;
            stepCounter = stepCounterMax;
        }
        else
        {
            stepCounter -= Time.deltaTime;   
        }*/
        
        //
        if(detected)//if player is found
        {
            if (stepTimer == 0f) //at each time interval
            { 
                position1 = this.transform.position;//get current position
                position2 = position1;//get last current position
                lastDetectedDirection = (position2 - position1).normalized;//set direction from 2 points
            }
            else//count down timer if not 0
            {
                stepTimer -= Time.deltaTime;
            }
        }
        else//if not detected, set timer as max value
        {
            stepTimer = maxStepTimer;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //start fire delay damage timer
        //once <= 0, take damage based on size of object: Collider.bounds.size;

        //on collision water: teleport to last onground position
    }

    private void OnCollisionExit(Collision collision)
    {
        //reset fire damage delay timer
    }
}
