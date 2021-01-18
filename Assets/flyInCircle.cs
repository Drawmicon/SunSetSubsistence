using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyInCircle : MonoBehaviour
{
    public GameObject Turret;//to get the position in worldspace to which this gameObject will rotate around.

    [Header("The axis by which it will rotate around (Y: Horizontal)")]
    public Vector3 axis;//by which axis it will rotate. x,y or z.

    [Header("Speed/Angle covered per update")]
    public float angle; //or the speed of rotation.
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Turret.transform.position, axis, angle);
    }
}
