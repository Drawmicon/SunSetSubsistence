using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemyTextController : MonoBehaviour
{
    public GameObject player;
    public EnemyAI eai;
    public triggerDetection td;
    public string textOutput;
    public TextMesh enemyText;
    public string[] names;
    public int nameChoice;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyText = GetComponentInChildren<TextMesh>();
        names = new string[] { "Evan", "Connor", "Jared", "Larry"};
        nameChoice = Random.Range(0, names.Length);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(player.transform.position);
        
        /*if (td.playerAlertDetected)//if in alert mode (timer based)
        {
            Debug.Log("AlertMode: " + td.alertMode);
            if (td.alertMode)//if enemy is in view of player
            {
                Debug.Log("PlayerAlertDetected: " + td.playerAlertDetected);
                textOutput = "!!!";
            }
            else
            {
                textOutput = "!";
            }
        }
        else     
        {
            if (td.susMode)//if in sus mode (timer based)
            {
                Debug.Log("SusMode: " + td.susMode);
                if (td.playerSusDetected)//if player is in enemy collider detector
                {
                    Debug.Log("PlayerSusDetected: " + td.playerSusDetected);
                    textOutput = "???";
                }
                else
                {
                    textOutput = "?";
                }
            }
            else
            {
                //Debug.Log("Enemy ... " );
                textOutput = "\n"+ names[nameChoice] +"(" + eai.enemyHealth + "/" + eai.maxEnemyHealth + ")";
            }
        }
        */

        enemyText.text = textOutput;

        /*if(eai != null)
        {
            textOutput += "\n("+eai.enemyHealth+"/"+eai.maxEnemyHealth+")";
        }
        else
        {
            textOutput += "\n(" + names[Random.Range(0, names.Length)] + ")";
        }*/
    }
}
