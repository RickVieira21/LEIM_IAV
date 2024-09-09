using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinCubes : MonoBehaviour
{
    public int size = 50;
    private int height = 20;

    private float smooth = 0.05f;
    private float smooth3D = 0.05f;
    private int waterLevel = 5;

    public float offset = 2345;

    void BuildWorld()
    {
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                int h = (int)(Mathf.PerlinNoise(x*smooth + offset, z*smooth + offset) * height);

                for (int y = 0; y < height; y++)
                {
                    if (y == h)
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = new Vector3(x, y, z);
                        cube.transform.parent = this.transform;
                        cube.transform.name = x + "" + y + "" + z;
                        cube.GetComponent<MeshRenderer>().material.color = Color.green;
                    }
                    else if (y < h)
                    {
                        float density = Perlin3D(x*smooth3D + offset, y*smooth3D + offset, z*smooth3D + offset);

                        if (Mathf.Abs(density) > 0.08f)
                        {
                            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            cube.transform.position = new Vector3(x, y, z);
                            cube.transform.parent = this.transform;
                            cube.transform.name = x + "" + y + "" + z;
                            cube.GetComponent<MeshRenderer>().material.color = Color.black;
                        }

                        
                    }
                    else if (y <= waterLevel)
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = new Vector3(x, y, z);
                        cube.transform.parent = this.transform;
                        cube.transform.name = x + "" + y + "" + z;
                        cube.GetComponent<MeshRenderer>().material.color = Color.blue;
                    }


                    //yield return null;

                }
                //yield return null;
            }
            //yield return null;
        }
    }

    float Perlin3D(float x, float y, float z)
    {
        float ab = Mathf.PerlinNoise(x, y);
        float ba = Mathf.PerlinNoise(y, x);
        float ac = Mathf.PerlinNoise(x, z);
        float ca = Mathf.PerlinNoise(z, x);
        float bc = Mathf.PerlinNoise(y, z);
        float cb= Mathf.PerlinNoise(z, y);

        float value = (ab + ba + ac + ca + bc + cb) / 6.0f;
        return (value - 0.5f) * 2f;
    }
    // Start is called before the first frame update
    void Start()
    {
        BuildWorld();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
