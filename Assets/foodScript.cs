using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class foodScript : MonoBehaviour
{
    public GameObject player;
    public player_Script ps;

    public float healthScore;
    private float healthMultiplier;
    public float speed;
    public float scaleShrinkRate;
    public float scaleMax;
    public float scaleMin;

    public float offGroundHeight;
    RaycastHit hit;
    public float moveCloserToGround;
    Vector3 vv, aa;

    public float respawnTimer;

    //public float delay;
    public float baseHealth;

    //navmeshobstacle 
    public NavMeshObstacle nmo;

    // Start is called before the first frame update
    void Start()
    {
        if(player = null)
        {
            player = GameObject.FindGameObjectWithTag("Player");            
        }
        //ps = player.GetComponent<player_Script>();

        this.transform.localScale = Random.Range(scaleMin,scaleMax) * Vector3.one;
        healthScore = transform.localScale.magnitude * healthMultiplier+baseHealth;
        speed += (Random.Range(0, 15))*.1f*speed;

        vv = new Vector3(0f, -1f, 0f);
        aa = new Vector3(0f, 1f, 0f);

        if(GetComponent<NavMeshObstacle>() != null)
        {
            nmo = GetComponent<NavMeshObstacle>();

            Mesh mesh = GetComponent<MeshFilter>().mesh;
            nmo.shape = NavMeshObstacleShape.Box;
            nmo.size = mesh.bounds.size;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.up * speed * Time.deltaTime);

        if (Physics.Raycast(transform.position, transform.TransformDirection(vv), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(vv) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit: "+ hit.distance);

            if(hit.distance > offGroundHeight)
            {
                this.transform.position += Vector3.down * moveCloserToGround;
            }
        }
        else
        {
            if (!(Physics.Raycast(transform.position, transform.TransformDirection(aa), out hit, Mathf.Infinity)))
            {
                if (hit.distance < offGroundHeight)
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(aa) * 1000, Color.blue);
                    this.transform.position += Vector3.up * (moveCloserToGround+1);
                    //Debug.Log("Did not Hit" + hit.distance);
                }
            }
        }

    }

    void shrink()
    {
        if(this.transform.localScale.magnitude >= .1f)
        {
            this.transform.localScale -= (scaleShrinkRate * (transform.localScale.magnitude / scaleMax))*Vector3.one;
        }              
        Destroy(this);
    }


    void OnTriggerEnter(Collider player)
    {
        if (player.gameObject.tag == "Player")
        {

            Debug.Log(player.name+" ("+player.tag + ") Detected!");

            if (player.GetComponent<player_Script>().healthScore < player.GetComponent<player_Script>().MaxHealthScore)
            {
                if (player.GetComponent<player_Script>().healthScore + healthScore >= player.GetComponent<player_Script>().MaxHealthScore)
                {
                    player.GetComponent<player_Script>().healthScore = player.GetComponent<player_Script>().MaxHealthScore;
                }
                else
                {
                    player.GetComponent<player_Script>().healthScore += healthScore;
                }
                //Debug.Log(player.name+" ("+player.tag + "): Health("+player.GetComponent<player_Script>().healthScore+") + "+healthScore);
                /*
                Set healthScore = 0f;
                Eat.Play();*/
                //shrink();
                /*  }
                  else
                {
                      //rejectEating.play();
                }*/
                Destroy(this.gameObject);
            }
            }
    }

    //on destroy adds timer to correct position of spawned object, so it can respawn at that time
    void OnDestroy()//Add to enemy script
    {
        if (this.transform.parent != null)
        {
            // Debug.Log("OnDestroy: "+this.name);
            int indy = (this.name).IndexOf(":");

            string subName = (this.name).Substring(0, indy);

            //Debug.Log("Spawn source name: " + subName);
            GameObject spawn_point = GameObject.Find(subName);
            if (spawn_point != null)
            {
                //script that controls spawn point
                spawning_script ss = spawn_point.GetComponent<spawning_script>();
                char x = (this.name)[(this.name).Length - 1];
                int xx = (int)char.GetNumericValue(x);
                //Debug.Log("Spawn source name" + subName + " at index " + xx + ", "+x);

                //ss.timers[xx] = ss.delayTimer;
                ss.timers[xx] = respawnTimer;
                
            }
        }
    }
}
