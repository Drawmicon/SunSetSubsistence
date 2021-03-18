using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyViewTarget : MonoBehaviour
{
    public GameObject ed;
    private enemy_detection edScript;
    public float speed;
    private Vector3 goHere;

    // Start is called before the first frame update
    void Start()
    {
        if(ed == null)
        {
            ed = GameObject.FindGameObjectWithTag("Enemy_Detect");
        }
        edScript = ed.GetComponent<enemy_detection>();
    }

    // Update is called once per frame
    void Update()
    {
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(transform.position, 1);

        goHere = (edScript.enemyViewPoint - ed.transform.localPosition).normalized;
        goHere = goHere * edScript.distance;

        if (edScript.enemyViewPoint != null || edScript.enemyViewPoint != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, ( edScript.enemyViewPoint), speed);
        }
    }
}