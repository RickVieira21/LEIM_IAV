using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldOfCubes : MonoBehaviour
{
    public int size = 4;

    IEnumerator BuildWorld()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                for (int z = 0; z < size; z++)
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(x, y, z);
                    cube.transform.parent = this.transform;
                    cube.transform.name = x + "" + y + "" + z;

                    //yield return null;

                }
                //yield return null;
            }
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(BuildWorld());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
