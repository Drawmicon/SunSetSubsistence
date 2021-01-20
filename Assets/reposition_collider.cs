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
        player = GameObject.FindGameObjectWithTag("Player_Controller");
        pCon = player.GetComponent<CharacterController>();
        pScript = player.gameObject.GetComponent<player_Script>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player_Controller")
        {
            //reposition player to last on-ground position, stored in player script
            collision.transform.position = pScript.lastGroundedPosition;
            pScript.healthScore -= waterDamage;

        }
        else
        {
            Destroy(collision.transform.gameObject);
        }
    }
}
