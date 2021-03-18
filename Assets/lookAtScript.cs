using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtScript : MonoBehaviour
{
    public GameObject lookAtTargetObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(lookAtTargetObject.transform, Vector3.up);
    }
}
