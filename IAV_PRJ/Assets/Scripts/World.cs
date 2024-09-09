using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;


    public class World : MonoBehaviour
    {
        public GameObject player;
        public Material material;
        public static int chunkSize = 16;
        public static int radius = 3;
        public static ConcurrentDictionary<string, Chunk> chunkDict;
        public static List<string> toRemove = new List<string>();
        Vector3 lastBuildPos;
        bool drawing;

        public static string CreateChunkName(Vector3 v)
        {
            return (int)v.x + "_" + (int)v.y + "_" + (int)v.z;
        }

        IEnumerator BuildRecursiveWorld(Vector3 chunkPos, int rad)
        {
            int x = (int)chunkPos.x;
            int y = (int)chunkPos.y;
            int z = (int)chunkPos.z;

            BuildChunkAt(chunkPos);
            yield return null;

            if (--rad < 0) yield break;

            Building(new Vector3 (x  - chunkSize, y, z), rad);

            Building(new Vector3 (x  + chunkSize, y, z), rad);
            
            Building(new Vector3 (x, y + chunkSize, z), rad);
            
            Building(new Vector3 (x, y - chunkSize, z), rad);
            
            Building(new Vector3 (x, y, z + chunkSize), rad);
            
            Building(new Vector3 (x, y, z - chunkSize), rad);
            yield return null;
        }

        void BuildChunkAt(Vector3 chunkPos)
        {
            string chunkName = CreateChunkName(chunkPos);
            Chunk c;
            if(!chunkDict.TryGetValue(chunkName, out c))
            {
                c = new Chunk(chunkPos, material, this, player);
                c.goChunk.transform.parent = this.transform;
                chunkDict.TryAdd(c.goChunk.name, c);
            }
        }

        void Removing()
        {
            StartCoroutine(RemoveChunks());
        }

        IEnumerator RemoveChunks()
        {
            for(int i = 0; i < toRemove.Count; i++)
            {
                string name = toRemove[i];
                Chunk c;
                if (chunkDict.TryGetValue(name, out c))
                {
                    Destroy(c.goChunk);
                    chunkDict.TryRemove(name, out c);
                    yield return null;
                }

            }
        }

        IEnumerator DrawChunks()
        {
            drawing = true;
            foreach (KeyValuePair<string, Chunk> c in chunkDict)
            {
                if (c.Value.status == Chunk.chunkStatus.DRAW)
                {
                    c.Value.DrawChunk();
                    yield return null;
                }
                if (c.Value.goChunk && Vector3.Distance(player.transform.position, c.Value.goChunk.transform.position) > chunkSize * radius)
                {
                    toRemove.Add(c.Key);
                    Chunk removedChunk;
                    chunkDict.TryRemove(c.Key, out removedChunk);   // <-- Remover o nome do chunk da lista de chunks visíveis
                }                                                   // Com esta alteração, o nome do chunk é removido da lista de chunks visíveis e também é removido do ConcurrentDictionary (resolve o bug de renderização)

            }
            StartCoroutine(RemoveChunks() );
            drawing = false;
        }
        
        public Chunk GetChunkAt(Vector3 pos)
        {
            Chunk chuck;
            if (chunkDict.TryGetValue(CreateChunkName(pos), out chuck)) return chuck;

            BuildChunkAt(pos);
            chunkDict.TryGetValue(CreateChunkName(pos), out chuck);
            return chuck;
        }


        void Building(Vector3 chunkPos, int rad)
        {
            StartCoroutine(BuildRecursiveWorld(chunkPos,rad));
        }

        void Drawing()
        {
            StartCoroutine(DrawChunks());
        }

        Vector3 WhichChunk(Vector3 position)
        {
            Vector3 chunkPos = new Vector3();
            chunkPos.x = Mathf.Floor(position.x / chunkSize) * chunkSize;
            chunkPos.y = Mathf.Floor(position.y / chunkSize) * chunkSize;
            chunkPos.z = Mathf.Floor(position.z / chunkSize) * chunkSize;

            return chunkPos;
        }

        // Start is called before the first frame update
        void Start()
        {
            chunkDict = new ConcurrentDictionary<string, Chunk>();
            this.transform.position = Vector3.zero;
            this.transform.rotation = Quaternion.identity;
            Vector3 ppos = player.transform.position;
            player.transform.position =new Vector3(ppos.x, Utils.GenerateHeight(ppos.x, ppos.z)+1,ppos.z);
            lastBuildPos = WhichChunk(player.transform.position);

            Building(WhichChunk(lastBuildPos), radius);
            Drawing();

            player.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 movement = player.transform.position-lastBuildPos;
            if (movement.magnitude > chunkSize)
            {
                lastBuildPos = player.transform.position;
                Building(WhichChunk(lastBuildPos), radius);
                Drawing();
            }
            if(!drawing)
                Drawing();
        }
    }
