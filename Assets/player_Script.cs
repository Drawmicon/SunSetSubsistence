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
    public GameObject garg, rubble;
    public Slider healthBar;
    public GameObject player;
    public CharacterController pCon;
    public float healthScore;
    public float MaxHealthScore;
    public float solarRegenRate;
    public float lunarDegenRate;
    public bool healthModeActive;
    public bool playerLit;

    public bool isGrounded;
    public Vector3 lastGroundedPosition;
    //private lightVisibility dn;
    public dayNightCycle_Script dnc;

    public Vector3 step1, step2;
    public float stepCounter,stepCounterMax;

    public bool detected, alertDetected;//detected == not visibly seen, alert detected == visibly seen
    public float stepTimer, maxStepTimer;
    public Vector3 position1, position2, lastDetectedDirection;

    public cinemaCameraController ccc;
    public cinemaCameraExtra cce;

    public Transform groundChecker;
    public float groundDistance;
    public LayerMask groundMask;

    public LayerMask waterMask;

    public Collider playerCollider;
    public float waterDamage;

    public float difficultyLevel;//multiplier value for how much health is drained during night

    public GameObject[] lsArray;

    public GameObject[] enemies;

    public bool isLoud, isMoving, isQuiet;

    public movementController mc;
    public bool isAttacking;
    public Vector3 playerForward;

    // Start is called before the first frame update
    void Start()
    {
        mc = GetComponentInChildren<movementController>();
        enemies = GameObject.FindGameObjectsWithTag("EnemyHead");

            /**/
            lsArray = GameObject.FindGameObjectsWithTag("Lantern");
        //**************************
        healthScore = MaxHealthScore;
        solarRegenRate = (MaxHealthScore / 2) / 6.5f;// gives player half of full health extra every day
        lunarDegenRate = (MaxHealthScore * difficultyLevel) / 300; // removes player health at rate of 3 full lives per night
        //*************************

        rubble.SetActive(false);
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
        if (pCon == null)
        {
            pCon = player.GetComponent<CharacterController>();
        }
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

    public bool checkLightSources()//check all light sources, if at least one returns that the player is lit, the player is lit, else all false == player unlit
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<triggerDetection>().playerAlertDetected)
            {
                return true;
            }
        }
        for (int i = 0; i < lsArray.Length; i++)
        {
            lightVisibility lv = lsArray[i].GetComponent<lightVisibility>();
            if (lv.playerInLight)
            {
                return true;
            }
        }
        return false;
    }

    public bool checkEnemySuspicion()//check all enemies, if at least one returns that the player is detected, the player is detected, else all false
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<triggerDetection>().playerSusDetected)
            {
                return true;
            }
        }
        return false;
    }

    public bool checkEnemyAlert()//check all enemies, if at least one returns that the player is detected, the player is detected, else all false
    {
        foreach(GameObject enemy in enemies)
        {
            if (enemy.GetComponent<triggerDetection>().playerAlertDetected)
            {
                return true;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        playerForward = this.transform.forward;
        isLoud = mc.isLoud; 
        isMoving = mc.playerMoving;
        isAttacking = mc.isAttacking;

        alertDetected= checkEnemyAlert();
        if (alertDetected)//if already alert detected, set sus detected to false
        {

            detected = false;
        }
        else
        {
            detected = checkEnemySuspicion();
        }

        //if night time, then check light sources or if enemy is alert and looking at player, to see if player is lit
        if(!dnc.dayTime)
        {
            playerLit = checkLightSources();
        }

        //if ground is detected, last grounded position is current position
       if( isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask))
        {
            lastGroundedPosition = this.transform.position;
        }

       //cinemeatic camera control
        if(cce.atParentPosition)
        {           
                garg.SetActive(false);
        }
        else
        {
            if (healthScore > 0)
            {
                garg.SetActive(true);
            }
        }

        //UI health meter
        healthBar.value = playerHealthSlider();

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

        //if ground checker detects being on water, teleport to last on ground position
        if (Physics.CheckSphere(groundChecker.position, groundDistance, waterMask))
        {
            this.transform.position = lastGroundedPosition;
            //add water damage
            healthScore -= waterDamage;
        }

        //
        if (detected)//if player is found
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
    
}
