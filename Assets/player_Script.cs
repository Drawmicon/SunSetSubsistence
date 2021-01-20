using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Script : MonoBehaviour
{
    public GameObject player;
    public CharacterController pCon;
    public float healthScore;
    public float MaxHealthScore;
    public Vector3 lastGroundedPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player_Controller");
        }
       pCon = player.GetComponent<CharacterController>();
        healthScore = MaxHealthScore;
    }

    // Update is called once per frame
    void Update()
    {
        if(pCon.isGrounded)
        {
           lastGroundedPosition = player.transform.position;
        }

        if(healthScore <= 0)
        {
            Debug.Log("Player Health 0");
        }
    }
}
