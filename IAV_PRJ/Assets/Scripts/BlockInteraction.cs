using System.Collections.Generic;
using UnityEngine;

public class BlockInteraction : MonoBehaviour
{ 
    public Camera cam;
    enum InteractionType { DESTROY, BUILD };
    InteractionType interactionType;
    public GameObject player;
    Block.BlockType lastDestroyedBlock = Block.BlockType.Air; // Variável para armazenar o último bloco destruído

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Temperatura " + Utils.GenerateTemperature(player.transform.position.x,player.transform.position.z));
        bool interaction = Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
        if (interaction)
        {
            interactionType = Input.GetMouseButtonDown(0) ? InteractionType.DESTROY : InteractionType.BUILD;


            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10))
            {
                string chunkName = hit.collider.gameObject.name;
                var position = hit.collider.gameObject.transform.position;
                float chunkx = position.x;
                float chunky = position.y;
                float chunkz = position.z;
                Vector3 hitBlock;

                //POSIÇÃO DO BLOCO
                if (interactionType == InteractionType.DESTROY)
                {
                    hitBlock = hit.point - hit.normal / 2f;
                }
                else
                {
                    hitBlock = hit.point + hit.normal / 2f;
                }

                int blockx = (int)(Mathf.Round(hitBlock.x) - chunkx);
                int blocky = (int)(Mathf.Round(hitBlock.y) - chunky);
                int blockz = (int)(Mathf.Round(hitBlock.z) - chunkz);

                Chunk c;

                //TIPO DE INTERAÇÃO
                if (World.chunkDict.TryGetValue(chunkName, out c))
                {
                    if ((interactionType == InteractionType.DESTROY))
                    {
                        //Guarda o tipo do ultimo bloco destruido
                        lastDestroyedBlock = c.chunkdata[blockx, blocky, blockz].GetBlockType();

                        c.chunkdata[blockx, blocky, blockz].SetType(Block.BlockType.Air);
                    }
                    else
                    {
                       if (lastDestroyedBlock != Block.BlockType.Air)
                            {
                                // Construir o bloco usando o tipo em lastDestroyedBlockType
                                c.chunkdata[blockx, blocky, blockz].SetType(lastDestroyedBlock);
        
                                // Define lastDestroyedBlockType como null para evitar construir mais blocos
                                lastDestroyedBlock = Block.BlockType.Air;
                            }
                    }
                }

                List<string> updates = new List<string>();
                updates.Add(chunkName);
                if (blockx == 0)
                {
                    updates.Add(World.CreateChunkName(new Vector3(chunkx - World.chunkSize, chunky, chunkz)));
                }

                if (blockx == World.chunkSize - 1)
                {
                    updates.Add(World.CreateChunkName(new Vector3(chunkx + World.chunkSize, chunky, chunkz)));
                }

                if (blocky == 0)
                {
                    updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky - World.chunkSize, chunkz)));
                }

                if (blocky == World.chunkSize - 1)
                {
                    updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky + World.chunkSize, chunkz)));
                }

                if (blockz == 0)
                {
                    updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky, chunkz - World.chunkSize)));
                }

                if (blockz == World.chunkSize - 1)
                {
                    updates.Add(World.CreateChunkName(new Vector3(chunkx, chunky, chunkz + World.chunkSize)));
                }

                foreach (string cname in updates)
                {
                    if (World.chunkDict.TryGetValue(cname, out c))
                    {
                        DestroyImmediate(c.goChunk.GetComponent<MeshFilter>());
                        DestroyImmediate(c.goChunk.GetComponent<MeshRenderer>());
                        DestroyImmediate(c.goChunk.GetComponent<MeshCollider>());
                        c.DrawChunk();
                    }
                }
            }

        }
    }

}