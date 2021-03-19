using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reposition_collider : MonoBehaviour
{

    public GameObject player;
    public CharacterController pCon;
    public player_Script pScript;
    public float waterDamage;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pCon = player.GetComponent<CharacterController>();
        pScript = player.gameObject.GetComponent<player_Script>();
    }

    private void OnCollisionEnter(Collision collision)
    {       
        if(collision.transform.tag == "Player_Controller" || collision.transform.tag == "Player")
        {
            Vector3 xr = new Vector3(pScript.lastGroundedPosition.x, pScript.lastGroundedPosition.y + 1, pScript.lastGroundedPosition.z);
            //reposition player to last on-ground position, stored in player script
            pCon.enabled = false; 
            player.transform.position = xr;
            pCon.enabled = true;
            pScript.healthScore -= waterDamage;
            Debug.Log("Player Collision With Water!");
        }
        else
        {
            if (collision.transform.tag == "test")
            {
                Debug.Log("TEst Collision!");
                Vector3 xr = new Vector3(0f, 3, 0f);
                //reposition player to last on-ground position, stored in player script
                collision.transform.position = xr;
            }
            else
            {
                Destroy(collision.transform.gameObject);
            }
        }
    }
}
