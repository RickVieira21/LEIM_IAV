using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinWorms : MonoBehaviour
{
    [SerializeField] private Transform cubeStart;
    [SerializeField] private Transform cubeEnd;
    [SerializeField] [Range(0.01f, 0.4f)] private float smooth;
    [SerializeField] private float offSet = 23456;
    [SerializeField] private float maxTurningAngle = 30f;
    [SerializeField] [Range(0f, 1f)] private float weightTarget;

    private Vector3 start, end;
    public List<Vector3> wormSequence;
    private Vector3[] dirNeighbors = new Vector3[6]
    {
        Vector3.up, Vector3.down,
        Vector3.left, Vector3.right,
        Vector3.forward, Vector3.back
    };
    
    [SerializeField] private bool RandomWalk = false;

    List<Vector3>PerlinWorm(Vector3 start, Vector3 end, int numMax = 40)
    {
        List<Vector3> sequence = new List<Vector3>();

        Vector3 pos = start;
        Vector3 dirRef = (end - start).normalized;
        Vector3 dir = dirRef;

        for (int i = 0; i<numMax; i++)
        {
            float yawRotation = convert2Angle(Mathf.PerlinNoise(pos.x * smooth + offSet, pos.z * smooth + offSet));
            float pitchRotation = convert2Angle(Mathf.PerlinNoise(pos.y * smooth + offSet, pos.z * smooth + offSet));

            if (RandomWalk)
                dir = Quaternion.AngleAxis(yawRotation, Vector3.up) * dir;
            else
                dir = Quaternion.AngleAxis(yawRotation, Vector3.up) * dirRef;
            //dir = Quaternion.AngleAxis(yawRotation, Vector3.up) * dirRef;
            dir = Quaternion.AngleAxis(pitchRotation, Vector3.right) * dir;
            
            dir = (1 - weightTarget) * dir + weightTarget * dirRef;

            pos += dir;
            
            if(Vector3.Distance(pos, end) < 1) break;
            
            sequence.Add(roundVector3(pos));
        }

        while (Vector3.Distance(pos, end) > 1.5f)
        {
            Vector3 dirToTarget = (end - pos).normalized;
            pos += dirToTarget;
            sequence.Add(roundVector3(pos));
        }

        return sequence;
    }

    private void InitAndBuildWorm()
    {
        start = cubeStart.position;
        end = cubeEnd.position;
        wormSequence = PerlinWorm(start, end);
    }

    IEnumerator renderWorm()
    {
        foreach (Vector3 v in wormSequence)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = this.transform;
            cube.transform.name = v.x + " " + v.y + " " + v.z;
            cube.GetComponent<MeshRenderer>().material.color = Color.yellow;
            cube.transform.position = v;
            //cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(EnlargeWorm());
        }
    }
    
    IEnumerator EnlargeWorm()
    {
        foreach (Vector3 v in wormSequence)
        {
            foreach (Vector3 dir in dirNeighbors)
            {
                Vector3 neighPos = v + dir;
                
                GameObject newgo = GameObject.CreatePrimitive(PrimitiveType.Cube);
                newgo.transform.parent = this.transform;
                newgo.transform.position = neighPos;
                newgo.GetComponent<MeshRenderer>().material.color = Color.blue;
                newgo.transform.name = neighPos.x + " " + neighPos.y + " " + neighPos.z;
                yield return null;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cubeStart.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
        cubeEnd.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        InitAndBuildWorm();
        StartCoroutine(renderWorm());
    }

    float convert2Angle(float p)
    {
        return (2 * p - 1) * maxTurningAngle;
    }

    Vector3 roundVector3(Vector3 pos)
    {
        float posX = Mathf.Floor(pos.x + 0.5f);
        float posY = Mathf.Floor(pos.y + 0.5f);
        float posZ = Mathf.Floor(pos.z + 0.5f);

        return new Vector3(posX, posY, posZ);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
