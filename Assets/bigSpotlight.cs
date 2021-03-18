using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigSpotlight : MonoBehaviour
{
    public enemy_detection ed;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponentInParent<enemy_detection>() != null)
        { 
            ed = GetComponentInParent<enemy_detection>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ed != null)
        { 
            if (ed.enemyState == 1 || ed.enemyState == 2)
            {
                this.GetComponent<Light>().enabled = true;
            }
            else
            {
                this.GetComponent<Light>().enabled = false;
            }
            this.GetComponent<Light>().range = ed.distance;
            this.GetComponent<Light>().spotAngle = ed.coneDetectionRadius;
            //range, spot angle control
        }

    }
}
