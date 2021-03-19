using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Start_Menu_Script : MonoBehaviour
{
    public AudioMixer musisk;
    public Slider volume;
    public GameObject startMenu;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject exitMenu;
    public GameObject HUD;
    public GameObject gameOver;

    public int menu; //0:no menu, 1:title menu, 2: options menu, 3:exit game, 4:pause, 5:gameover
    public Text subtitle, survivalTime;
    private string[] subOptions;

    public int previousMode = 1;
    private bool devOp;
    public bool playerGhostMode;

    public Camera main, cinema;
    public cinemaCameraController ccc;
    public cinemaCameraExtra cce;

    public Image healthIcon;
    public Sprite[] healthIconImages; //health Icon images == 0:safe, 1:detected, 2:lit up, 3: low health

    public player_Script ps;

    public dayNightCycle_Script dnc;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Confined;
        subOptions = new string[] { "The Reign in Spain Falls Neatly in Distain", "Find Truth, Regardless On How Uncouth ", "Still in a dream, Snake Eater" };
        subtitle.text = subOptions[Random.Range(0, subOptions.Length)];
        stopGameWorld();
        startMenu.SetActive(true);
        optionsMenu.SetActive (false);
        exitMenu.SetActive (false);
        pauseMenu.SetActive (false);
        HUD.SetActive(false);
        gameOver.SetActive(false);
        playerGhostMode = false;
        devOp = false;
        menu = 1;     
        
       // main.enabled = false;
        //cinema.enabled = true;
        if(ps == null)
        {
            ps = GameObject.FindGameObjectWithTag("Player").GetComponent<player_Script>();
        }
        ps.healthModeActive = false;

        dnc = (GameObject.FindGameObjectWithTag("SunMoonController")).GetComponent<dayNightCycle_Script>();
    }
    
    public void swapCams()
    {
        ccc.switchCamMode = true;
    }

    public void disableUI()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        previousMode = menu;
        menu = 0;
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        exitMenu.SetActive(false);
        pauseMenu.SetActive(false);
        HUD.SetActive(false);
        gameOver.SetActive(false);
        resumeGameWorld();
        swapCams();
        ps.healthModeActive = true;
    }

    public void disableUINotHUD()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        previousMode = menu;
        menu = 0;
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        exitMenu.SetActive(false);
        pauseMenu.SetActive(false);
        HUD.SetActive(true);
        gameOver.SetActive(false);
        resumeGameWorld();
        swapCams();
        ps.healthModeActive = true;
    }

    public void forceEndGame()
    {
        Application.Quit();
    }//does not work in editor

    public void newGame()
    {
        previousMode = 1;
        SceneManager.LoadScene("GameWorld_1");
    }//restarts scene

    public void startButton()//exit UI and starts game world time
    {
        menu = 0;
        disableUINotHUD();//removes ui and starts game world time
    }

    public void gameOverButton()
    {
        swapCams();
    }

    public void optionsButton()
    {
        //sound
        previousMode = menu;
        menu = 2;
        stopGameWorld();
        startMenu.SetActive(false);
        exitMenu.SetActive(false);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        HUD.SetActive(false);
        gameOver.SetActive(false);
    }

    public void exitButton()
    {
        //goodbye
        menu = 3;
        stopGameWorld();
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        exitMenu.SetActive(true);
        gameOver.SetActive(false);
    }

    public void resumeButton()//resume game, exit ui
    {
        previousMode = menu;
        disableUINotHUD();
       /* menu = 0;
        resumeGameWorld();
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        exitMenu.SetActive(false);
        pauseMenu.SetActive(false);
        HUD.SetActive(true);
        gameObject.SetActive(false);*/
    }

    public void stopGameWorld()
    {
        Time.timeScale = 0;
    }

    public void resumeGameWorld()
    {
        Time.timeScale = 1;
    }

    public void devMode_button()
    {
        menu = 4;
        devOp = true;
    }

    public void observeMode()//enable function
    {
        if(devOp)
        {
            //disable all player triggers
        }
    }

    public void setVolume(float val)//Error: Uninteractable when UI text was in front of slider
    {
       // Debug.Log("Volume: " + val);
        musisk.SetFloat("Musick", Mathf.Log10(val)*20);
        //https://www.youtube.com/watch?v=xNHSGMKtlv4&ab_channel=JohnFrench
    }

    public void setFullScreen(bool set)
    {
        Screen.fullScreen = set;
    }

    public void setQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
    public void pauseButton()
    {
        previousMode = menu;
        menu = 4;
        stopGameWorld();
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        exitMenu.SetActive(false);
        pauseMenu.SetActive(true);
        HUD.SetActive(false);
    }

    public void goBackMenu()
    {
        //0:no menu, 1:title menu, 2: options menu, 3:exit game, 4:pause, 5:gameover
        menu = previousMode;
        switch (previousMode)
        {
            case 0://no menu
                resumeButton();               
                break;
            case 1://title menu
                startMenu.SetActive(true);
                optionsMenu.SetActive(false);
                exitMenu.SetActive(false);
                pauseMenu.SetActive(false);
                HUD.SetActive(false);
                stopGameWorld();
                break;
            case 2://options menu
                startMenu.SetActive(false);
                optionsMenu.SetActive(true);
                exitMenu.SetActive(false);
                pauseMenu.SetActive(false);
                HUD.SetActive(false);
                stopGameWorld();
                break;
            case 3://end game menu
                startMenu.SetActive(false);
                optionsMenu.SetActive(false);
                exitMenu.SetActive(true);
                pauseMenu.SetActive(false);
                HUD.SetActive(false);
                stopGameWorld();
                break;
            case 4://pause menu
                startMenu.SetActive(false);
                optionsMenu.SetActive(false);
                exitMenu.SetActive(false);
                pauseMenu.SetActive(true);
                HUD.SetActive(false);
                stopGameWorld();
                break;

            case 5://game over menu
                gameOverMenu();
                break;

            default:
                resumeButton() ;
                break;
        }
    }

    public void gameOverMenu()//enable gameover UI
    {
        previousMode = menu;
        menu = 5;
        resumeGameWorld();
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        exitMenu.SetActive(false);
        pauseMenu.SetActive(false);
        HUD.SetActive(false);
        gameOver.SetActive(true);
        ccc.switchCamMode = false;
        //ps.garg.SetActive(false);
        ps.rubble.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape"))//enable pause menu while no other menu is enabled
        {
            if (menu == 0)
            {
                pauseButton();            
            }
            else
            {
                if (menu != 1)
                {
                    disableUINotHUD();
                }
            }
        }

        //health Icon images == 0:safe, 1:detected, 2:lit up, 3: low health, 4:lowHealth&detected
        if (ps.healthScore < ps.MaxHealthScore / 3)//low health and detected
        {
            healthIcon.sprite = healthIconImages[4];
        }
        else { 
            if (ps.healthScore < ps.MaxHealthScore / 3)//low health
            {
                healthIcon.sprite = healthIconImages[3];
            }
            else
            {
                if (ps.detected)//detected
                {
                    healthIcon.sprite = healthIconImages[1];
                }
                else
                {
                    if (ps.playerLit)//lit up
                    {
                        healthIcon.sprite = healthIconImages[2];
                    }
                    else
                    {
                        healthIcon.sprite = healthIconImages[0];//normal mode
                    }
                }
            }
        }

        if(ps.healthScore <= 0f && ps.healthModeActive)
        {
            gameOverMenu();
        }

        survivalTime.text = "Survival Time: " + dnc.dayCounter + " days...";

        /*
        if (Input.GetKey(KeyCode.F1) && menu == 0 && devOp)
        {
            playerGhostMode = !playerGhostMode;
        }
        
        if(menu != 0)
        {
            stopGameWorld();
        }

        if (menu != previousMode)
        {
            switch (menu)
            {
                case 0://no menu
                    disableUI();                   
                    previousMode = 0;
                    break;
                case 1://title menu
                    subtitle.text = subOptions[Random.Range(0, subOptions.Length)];
                    optionsMenu.SetActive(false);
                    previousMode = 1;
                    break;
                case 2://pause menu
                    optionsMode();
                    break;
                case 3://end game menu
                    forceEndGame();
                    optionsMenu.SetActive(false);
                    previousMode = 3;
                    break;
                case 4://dev options
                    observeMode();
                    optionsMenu.SetActive(false);
                    previousMode = 4;
                    break;

                case 5://game options
                    optionsMode();
                    optionsMenu.SetActive(true);
                    startMenu.SetActive(false);
                    previousMode = 5;
                    break;
                default:
                    previousMode = 0;
                    break;
            }
        }*/
    }
}
