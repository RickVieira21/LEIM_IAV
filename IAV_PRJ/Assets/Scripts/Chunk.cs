using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;


public class Chunk
    {
        public Block[,,] chunkdata;
        public GameObject goChunk;
        public enum chunkStatus { DRAW, DONE };
        public chunkStatus status = chunkStatus.DRAW;
        Material material;
        public World world;
        public Vector3 pos;
        public GameObject player;
        
        public List<Vector3> vegetationPos = new List<Vector3>();
        public List<Tree> trees = new List<Tree>();
        bool hasVeg = false;
        public List<Block> vegetation = new List<Block>();
        

        public Chunk(Vector3 pos, Material material, World world, GameObject player)
        {
            goChunk = new GameObject(World.CreateChunkName(pos));
            goChunk.transform.position = pos;
            this.material = material;
            this.world = world;
            this.pos = pos;
            this.player = player;
            BuildChunk();
        }


        void BuildChunk()
        {
            chunkdata = new Block[World.chunkSize, World.chunkSize, World.chunkSize];
            for (int z = 0; z < World.chunkSize; z++)
            {
                for (int y = 0; y < World.chunkSize; y++)
                {
                    for (int x = 0; x < World.chunkSize; x++)
                    {
                        Vector3 pos = new Vector3(x, y, z);
                        int worldX = (int) goChunk.transform.position.x + x;
                        int worldY = (int) goChunk.transform.position.y + y;
                        int worldZ = (int) goChunk.transform.position.z + z;
                        
                        int h = Utils.GenerateHeight(worldX, worldZ);
                        int hs = Utils.GenerateStoneHeight(worldX, worldZ);
                        int temperature = Utils.GenerateTemperature(worldX,worldZ); //Gerar temperatura

                        Biome.BiomeType biome = Biome.getBiome(temperature);
                        
                        if(worldY <= hs)
                        {
                            if(Utils.fBM3D(worldX,worldY,worldZ,1,0.5f)<0.510f) //Gerar a pedra em redor
                                chunkdata[x, y, z] = new Block("", Block.BlockType.Stone, pos, this, material); //Comentado para não cercar de pedra
                            else
                                chunkdata[x, y, z] = new Block("", Block.BlockType.Air, pos, this, material);
                        }
                            
                        //Passa a ir buscar o tipo de terreno correspondente 
                        else if (worldY == h)
                        {
                            //chunkdata[x, y, z] = new Block(Block.BlockType.Grass, pos, this, material);
                            chunkdata[x, y, z] = new Block("", Biome.getBiomeDirt(biome), pos, this, material);
                            if (x > 1 && z > 1 && x < 14 && z < 14)
                            {
                                if (Biome.getVegProb(biome))
                                {
                                    vegetationPos.Add(pos);
                                }
                            }
                        }

                        else if(worldY < h)
                            if (biome == Biome.BiomeType.DESERT)
                                chunkdata[x, y, z] = new Block("", Biome.getBiomeDirt(biome), pos, this, material);
                            else
                                chunkdata[x, y, z] = new Block("", Block.BlockType.Dirt, pos, this, material);

                        else if (chunkdata[x, y, z] is null)
                            chunkdata[x, y, z] = new Block("", Block.BlockType.Air, pos, this, material);                    
                    }
                }
            }
            status = chunkStatus.DRAW;
        }

        public void GenerateVeg()
        {
            foreach (Vector3 pos in vegetationPos)
            {
                int x = (int)goChunk.transform.position.x + (int)pos.x;
                int z = (int)goChunk.transform.position.z + (int)pos.z;
                
                int temp = Utils.GenerateTemperature(x, z);
                
                Biome.BiomeType biome = Biome.getBiome(temp);
                
                float rand = Random.Range(0f, 1f);

                if (rand < 0.1f)
                {
                    Tree tree = new Tree(pos + Vector3.up, this, material, biome);
                    
                    //desenha a arvore
                    tree.DrawTree();
                    trees.Add(tree);
                    //desenha a arvore nos chunks ocupados
                    tree.DrawTreeChunks();
                }
                else if (rand > 0.1f || rand < 0.3f)
                {
                    Block veg = new Block("",Biome.getBiomeVeg(biome), pos + Vector3.up, this, material);
                    veg.gameObject.tag = "Vegetation";
                    veg.gameObject.layer = LayerMask.NameToLayer("Vegetation");
                    vegetation.Add(veg);
                    //vemos se a posição é a ultima do chunk e adicionamos a vegetação ao chunk seguinte
                    if (pos.y + 1 >= World.chunkSize)
                    {
                        Chunk nextChunk = world.GetChunkAt(goChunk.transform.position + new Vector3(0, World.chunkSize, 0));
                        nextChunk.chunkdata[(int)pos.x, 0, (int)pos.z] = veg;
                        nextChunk.chunkdata[(int)pos.x, 0, (int)pos.z].gameObject.layer = LayerMask.NameToLayer("Vegetation");
                    }
                    //caso contrario criamos a vegetação no chunck atual
                    else
                    {
                        chunkdata[(int)pos.x, (int)pos.y + 1, (int)pos.z] = veg;
                        chunkdata[(int)pos.x, (int)pos.y + 1, (int)pos.z].gameObject.layer = LayerMask.NameToLayer("Vegetation");
                    }
                }
            }
            hasVeg = true;
        }
        
        public void DrawChunk() {
            
            //verificar se tem vegetação e gerar caso não tenha
            if(!hasVeg) GenerateVeg();
            
            for (int z = 0; z < World.chunkSize; z++)
                for (int y = 0; y < World.chunkSize; y++)
                    for (int x = 0; x < World.chunkSize; x++)
                        chunkdata[x, y, z].Draw();    
            CombineQuads();
            MeshCollider collider = goChunk.AddComponent<MeshCollider>();
            collider.sharedMesh = goChunk.GetComponent<MeshFilter>().mesh;
            status = chunkStatus.DONE;
        }

        void CombineQuads()
        {
            //1. Combine all children meshes
            MeshFilter[] meshFilters = goChunk.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];
            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                i++;
            }
            //2. Create a new mesh on the parent object
            MeshFilter mf = (MeshFilter)goChunk.AddComponent<MeshFilter>();
            mf.mesh = new Mesh();

            //3. Add combined meshes on children as the parent's mesh
            mf.mesh.CombineMeshes(combine);

            //4. Create a renderer for the parent
            MeshRenderer renderer = goChunk.AddComponent<MeshRenderer>();
            renderer.material = material;

            //5. Delete all uncombined children
            foreach (Transform quad in goChunk.transform)
            {
                GameObject.Destroy(quad.gameObject);
            }
        }

        //// Start is called before the first frame update
        //void Start()
        //{
        //    StartCoroutine(BuildChunk(8, 8, 8));
        //}

        //// Update is called once per frame
        //void Update()
        //{
            
        //}
    }
