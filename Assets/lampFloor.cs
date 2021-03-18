using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lampFloor : MonoBehaviour
{

    public lightVisibility lv;
    public float sideLength;
    // Start is called before the first frame update
    void Start()
    {
        /*
        if (GetComponentInParent<lightVisibility>() != null)
        {
            lv = GetComponentInParent<lightVisibility>();
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (lv != null)
        {
            sideLength = Mathf.Sqrt(2 * lv.lightDistance * lv.lightDistance);
            this.transform.localScale = new Vector3(sideLength, 0, sideLength);
        }
    }
}
