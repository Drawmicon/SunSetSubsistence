using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class wave_maker : MonoBehaviour
{
    [SerializeField]
    public Octave[] octave;
    private dayNightCycle_Script dn;
    private float originalY;
    //******
    public int Dimensions = 10;
    protected MeshFilter MeshFilter;
    protected Mesh mesh;

    

    // Start is called before the first frame update
    void Start()
    {
        dn = (GameObject.FindGameObjectWithTag("SunMoonController")).GetComponent<dayNightCycle_Script>();
        originalY = this.transform.position.y;
        //****
        mesh = new Mesh();
        mesh.name = gameObject.name;
        mesh.vertices = GenerateVertices();
        mesh.triangles = GenerateTriangles();
        mesh.RecalculateBounds();

        MeshFilter = gameObject.AddComponent<MeshFilter>();
        MeshFilter.mesh = mesh;
    }

    private Vector3[] GenerateVertices()
    {
        var verts = new Vector3[(Dimensions + 1) * (Dimensions + 1)];
        for(int x =0; x <= Dimensions; x++)
        {
            for(int z = 0; z <= Dimensions; z++)
            {
                verts[index(x, z)] = new Vector3(x, 0, z);
            }
        }
        return verts;
    }

    private int index(int x, int y)
    {
        int i = (x * (Dimensions+1)) + y;
        return i;
    }

    private int[]GenerateTriangles()
    {
        var tri = new int[mesh.vertices.Length * 6];

        for(int x= 0; x < Dimensions; x++)
        {
            for(int z = 0; z < Dimensions; z++)
            {
                tri[index(x, z) * 6 + 0] = index(x, z);
                tri[index(x, z) * 6 + 1] = index(x+1, z+1);
                tri[index(x, z) * 6 + 2] = index(x+1, z);

                tri[index(x, z) * 6 + 3] = index(x, z);
                tri[index(x, z) * 6 + 4] = index(x, z+1);
                tri[index(x, z) * 6 + 5] = index(x+1, z+1);

            }
        }

        return tri;
    }
    // Update is called once per frame
    void Update()
    {
        var verts = mesh.vertices;
        for(int x = 0; x <= Dimensions; x++)
        {
            for(int z = 0; z <= Dimensions; z++)
            {
                var y = 0f;
                for(int o = 0; o < octave.Length; o++)
                {
                    if(octave[o].alternate)
                    {
                        var perl = Mathf.PerlinNoise((x * octave[o].scale.x) / Dimensions, (z * octave[o].scale.y) / Dimensions)*2*Mathf.PI;
                        if (dn.dayTime)
                        {
                            y += Mathf.Cos(perl + octave[o].speed.magnitude * Time.time) * octave[o].height;
                        }
                        else 
                        {
                            y += Mathf.Cos(perl + octave[o].speed.magnitude * Time.time) * octave[o].height*10;
                        }
                    }
                    else
                    {
                        var perl = Mathf.PerlinNoise((x * octave[o].scale.x + Time.time * octave[o].speed.x) / Dimensions, (z * octave[o].scale.y + Time.time * octave[o].speed.y) / Dimensions) - 0.5f;
                        y += perl * octave[o].height;
                    }
                }
                verts[index(x, z)] = new Vector3(x, y, z);
            }
        }
        mesh.vertices = verts;
        //transform.position = new Vector3(this.transform.position.x,Mathf.Sin(dn.sunAngle)* waveHeight+originalY, this.transform.position.z);
    }

    [Serializable]
    public struct Octave
    {
        public Vector2 speed;
        public Vector2 scale;
        public float height;
        public bool alternate;
    }
}
