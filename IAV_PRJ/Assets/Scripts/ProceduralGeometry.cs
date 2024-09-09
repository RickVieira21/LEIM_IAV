using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeometry : MonoBehaviour
{
    public Material material;

    private Mesh mesh;
    private MeshFilter mf;

    IEnumerator AnimateUVs()
    {
        float moveUV = 1f/8f;
        float frameRate = 7f;

        Vector2[] uvs = new Vector2[4];

        while (true)
        {
            uvs[0] = new Vector2(mesh.uv[0].x + moveUV, mesh.uv[0].y);
            uvs[1] = new Vector2(mesh.uv[1].x + moveUV, mesh.uv[1].y);
            uvs[2] = new Vector2(mesh.uv[2].x + moveUV, mesh.uv[2].y);
            uvs[3] = new Vector2(mesh.uv[3].x + moveUV, mesh.uv[3].y);

            mesh.uv = uvs;

            yield return new WaitForSeconds(1f / frameRate);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        mf = gameObject.AddComponent<MeshFilter>();
        MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();

        mr.material = material;

        mesh = new Mesh();

        Vector3[] vertices = {
            new Vector3(0, 0, 0), 
            new Vector3(1, 0, 0), 
            new Vector3(1, 1, 0), 
            new Vector3(0, 1, 0) };

        mesh.vertices = vertices;

        int[] triangles = {
            0, 2, 1,
            0, 3, 2,
            0, 1, 2,
            0, 2, 3
        };

        mesh.triangles = triangles;

        //UV coordinates
        Vector2[] uvs = {
        
            new Vector2(0, 6f/7f),
            new Vector2(1f/8f, 6f/7f),
            new Vector2(1f/8f, 1),
            new Vector2(0, 1)
        };

        mesh.uv = uvs;

        mf.mesh = mesh;

        StartCoroutine(AnimateUVs());
    }

    // Update is called once per frame
    void Update()
    {
    }
}
