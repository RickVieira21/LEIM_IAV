using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPlane : MonoBehaviour
{
    //private GameObject plane;
    private int N = 7;

    private Mesh mesh;
    private Vector3[] vertices;
    
    [SerializeField] private Material material;
    [SerializeField] private float height = 10;
    [Range(0.01f, 0.1f)] [SerializeField] private float smooth = 0.1f;
    [SerializeField] private float offSet = 23456;
    [Range(1f, 6f)] [SerializeField] private int octaves = 3;
    [SerializeField] private float[] ts;
    [SerializeField] private float[] fs;
    
    // Start is called before the first frame update
    void Start()
    {
        //plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
        mr.material = material;
        for (int x = 0; x < N; x++)
        {
            for (int z = 0; z < N; z++)
            {
                Vector3 position = new Vector3(x * 10, 0, z * 10);
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);
                go.transform.position = position;
                go.transform.parent = this.transform;
            }
        }

        CombineMesh();
    }

    void CombineMesh()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        
        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }

        mesh = new Mesh();
        mesh.CombineMeshes(combine);
        
        MeshFilter mf = gameObject.AddComponent<MeshFilter>();
        mf.mesh = mesh;
        
        vertices = mesh.vertices;

        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
        transform.gameObject.SetActive(true);
    }
    
    float RandomNoise2D(float x, float z)
    {
        return Random.Range(0f, 1f);
    }
    
    float SinosoidalNoise2D(float x, float z)
    {
        float valx = (Mathf.Sin(x * smooth + offSet) + 1) * 0.5f;
        float valz = (Mathf.Cos(z * smooth + offSet) + 1) * 0.5f; ;
        return valx * valz;
    }
    
    float PerlinNoise2D(float x, float z)
    {
        float total = 0;
        float frequency = smooth;
        float amplitude = 1f;
        float persistence = 0.75f;
        float maxValue = 0;
        
        for (int i = 0; i < octaves; i++)
        {
            total += Mathf.PerlinNoise(x * frequency + offSet,
                z * frequency + offSet) * amplitude;
            frequency *= 2;
            amplitude *= persistence;
            maxValue += amplitude;
        }
        return total/maxValue;
    }

    float ContinentalnessMap(float t)
    {
        for (int i = 1; i < ts.Length; i++)
        {
            if (t <= ts[i])
            {
                float val = fs[i - 1] + (fs[i] - fs[i - 1]) *
                    (t - ts[i - 1]) / (ts[i] - ts[i - 1]);
                Debug.Log(t + " " + val);
                return val;
            }
        }
        return 1f;
    }

    // Update is called once per frame
    void Update()
    {
        Random.InitState(2341);
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 v = vertices[i];
            //float h = RandomNoise2D(v.x, v.z) * height;
            //float h = SinosoidalNoise2D(v.x, v.z) * height;
            float h = ContinentalnessMap(PerlinNoise2D(v.x, v.z));
            vertices[i] = new Vector3(v.x, h, v.z);
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        
    }
}
