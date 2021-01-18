using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_Inside : MonoBehaviour
{
    public GameObject[] prefab;
    public int prefabSelection;
    public bool autoSelection;

    // Start is called before the first frame update
    void Start()
    {
        if(autoSelection)
        {
            prefabSelection=Random.Range(0, prefab.Length-1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player")
        {
            Destroy(this.gameObject);
        }
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }*/

    private void OnDestroy()
    {
        GameObject tempo = Instantiate(prefab[prefabSelection], this.transform.position, Quaternion.identity);
    }
}
