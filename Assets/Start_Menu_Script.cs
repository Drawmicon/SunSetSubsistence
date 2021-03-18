﻿using System.Collections;
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

    public int menu; //0:no menu, 1:title menu, 2: options menu, 3:exit game, 4:pause
    public Text subtitle;
    private string[] subOptions;

    public int previousMode = 1;
    private bool devOp;
    public bool playerGhostMode;

    public Camera main, cinema;
    public cinemaCameraController ccc;
    public cinemaCameraExtra cce;
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
        playerGhostMode = false;
        devOp = false;
        menu = 1;

        //
        
       // main.enabled = false;
        //cinema.enabled = true;

    }
    
    public void swapCams()
    {
        ccc.switchCamMode = true;
        /*
        if(main.enabled)
        {
            ccc.currentAngle = 180f;
            ccc.switchCamMode = false ;
            main.enabled = false;
            cinema.enabled = true;
            
        }
        else
        {
            ccc.switchCamMode = false;
            if (cce.atParentPosition)
            {
                main.enabled = true;
                cinema.enabled = false;
            }
        }
        */
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
        resumeGameWorld();
        swapCams();
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
        resumeGameWorld();
        swapCams();
    }

    public void forceEndGame()
    {
        Application.Quit();
    }//does not work in editor

    public void newGame()
    {
        previousMode = 1;
        SceneManager.LoadScene("GameWorld_0");
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
    }

    public void resumeButton()//resume game, exit ui
    {
        previousMode = menu;
        menu = 0;
        resumeGameWorld();
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        exitMenu.SetActive(false);
        pauseMenu.SetActive(false);
        HUD.SetActive(true);
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
        //0:no menu, 1:title menu, 2: options menu, 3:exit game, 4:pause
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

            default:
                resumeButton() ;
                break;
        }
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
