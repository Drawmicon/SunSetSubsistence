using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyRock : MonoBehaviour
{
    public GameObject rubble;
    // Start is called before the first frame update
    void Start()
    {
        rubble.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        rubble.SetActive(true);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "playerAttack" || collision.gameObject.name == "Arm")
        {
            Destroy(this);
        }
    }
}
