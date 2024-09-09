using System;
using System.Collections.Generic;
using UnityEngine;

public class Block
{ 
    enum Cubeside { BOTTOM, TOP, LEFT, RIGHT, FRONT, BACK, DIAG1FRONT, DIAG1BACK, DIAG2FRONT, DIAG2BACK }; 
    public enum BlockType {Grass, Dirt, Stone, Sand, Snow, Cactus, WoodForest, WoodForestSnow, RedFlower, YellowFlower, RedMushroom, BrownMushroom, Weeds, DeadBush, Air};

    Material material;
    BlockType bType;
    Chunk owner;
    Vector3 pos;
    private bool isSolid;
    public List<GameObject> quads = new List<GameObject>();
    
    public GameObject gameObject;

    static Vector2 GrassSide_LBC = new Vector2 (3f,15f) / 16;
    static Vector2 GrassTop_LBC = new Vector2(2f, 6f) / 16;
    
    static Vector2 Dirt_LBC = new Vector2(2f, 15f) / 16;
    static Vector2 Stone_LBC = new Vector2(0f, 14f) / 16;
    
    static Vector2 Sand_LBC = new Vector2(2f, 14f) / 16;
    
    static Vector2 SnowSide_LBC = new Vector2(4f, 11f) / 16;
    static Vector2 SnowTop_LBC = new Vector2(2f, 11f) / 16;
    
    static Vector2 CactusSide_LBC = new Vector2 (6f,11f) / 16;
    static Vector2 CactusTop_LBC = new Vector2(5f, 11f) / 16;
    static Vector2 CactusBottom_LBC = new Vector2(7f, 11f) / 16;
    
    static Vector2 WoodForestSide_LBC = new Vector2 (4f,14f) / 16;
    static Vector2 WoodForestTop_LBC = new Vector2(5f, 14f) / 16;
    
    static Vector2 WoodForestSnowSide_LBC = new Vector2 (4f,8f) / 16;

    static Vector2 RedFlower_LBC = new Vector2(12f, 15f) / 16;
    
    static Vector2 YellowFlower_LBC = new Vector2(13f, 15f) / 16;

    static Vector2 RedMushroom_LBC = new Vector2(12f, 14f) / 16;
    
    static Vector2 BrownMushroom_LBC = new Vector2(13f, 14f) / 16;
    
    static Vector2 Weeds_LBC = new Vector2(12f, 10f) / 16;
    
    static Vector2 DeadBush_LBC = new Vector2(7f, 12f) / 16;


    private Vector2[,,] blockUVs =
    {
        //Grass
        {
            {
                //Grass Top
                GrassTop_LBC, GrassTop_LBC + new Vector2(1, 0) / 16,
                GrassTop_LBC + new Vector2(0, 1) / 16, GrassTop_LBC + new Vector2(1, 1) / 16
            },
            {
                //Grass Side
                GrassSide_LBC, GrassSide_LBC + new Vector2(1, 0) / 16,
                GrassSide_LBC + new Vector2(0, 1) / 16, GrassSide_LBC + new Vector2(1, 1) / 16
            },
            {
                //Grass Bottom
                Dirt_LBC, Dirt_LBC + new Vector2(1, 0) / 16,
                Dirt_LBC + new Vector2(0, 1) / 16, Dirt_LBC + new Vector2(1, 1) / 16
            }
        },
        //Dirt
        {
            {
                //Dirt Top
                Dirt_LBC, Dirt_LBC + new Vector2(1, 0) / 16,
                Dirt_LBC + new Vector2(0, 1) / 16, Dirt_LBC + new Vector2(1, 1) / 16
            },
            {
                //Dirt Side
                Dirt_LBC, Dirt_LBC + new Vector2(1, 0) / 16,
                Dirt_LBC + new Vector2(0, 1) / 16, Dirt_LBC + new Vector2(1, 1) / 16
            },
            {
                //Dirt Bottom
                Dirt_LBC, Dirt_LBC + new Vector2(1, 0) / 16,
                Dirt_LBC + new Vector2(0, 1) / 16, Dirt_LBC + new Vector2(1, 1) / 16
            }
        },
        //Stone
        {
            {
                //Stone Top
                Stone_LBC, Stone_LBC + new Vector2(1, 0) / 16,
                Stone_LBC + new Vector2(0, 1) / 16, Stone_LBC + new Vector2(1, 1) / 16
            },
            {
                //Stone Side
                Stone_LBC, Stone_LBC + new Vector2(1, 0) / 16,
                Stone_LBC + new Vector2(0, 1) / 16, Stone_LBC + new Vector2(1, 1) / 16
            },
            {
                //Stone Bottom
                Stone_LBC, Stone_LBC + new Vector2(1, 0) / 16,
                Stone_LBC + new Vector2(0, 1) / 16, Stone_LBC + new Vector2(1, 1) / 16
            }
        },
        //Sand
        {
            {
                //Sand Top
                Sand_LBC, Sand_LBC + new Vector2(1, 0) / 16,
                Sand_LBC + new Vector2(0, 1) / 16, Sand_LBC + new Vector2(1, 1) / 16
            },
            {
                //Sand Side
                Sand_LBC, Sand_LBC + new Vector2(1, 0) / 16,
                Sand_LBC + new Vector2(0, 1) / 16, Sand_LBC + new Vector2(1, 1) / 16
            },
            {
                //Sand Bottom
                Sand_LBC, Sand_LBC + new Vector2(1, 0) / 16,
                Sand_LBC + new Vector2(0, 1) / 16, Sand_LBC + new Vector2(1, 1) / 16
            }
        },
        //Snow
        {
            {
                //Snow Top
                SnowTop_LBC, SnowTop_LBC + new Vector2(1, 0) / 16,
                SnowTop_LBC + new Vector2(0, 1) / 16, SnowTop_LBC + new Vector2(1, 1) / 16
            },
            {
                //Snow Side
                SnowSide_LBC, SnowSide_LBC + new Vector2(1, 0) / 16,
                SnowSide_LBC + new Vector2(0, 1) / 16, SnowSide_LBC + new Vector2(1, 1) / 16
            },
            {
                //Snow Bottom
                Dirt_LBC, Dirt_LBC + new Vector2(1, 0) / 16,
                Dirt_LBC + new Vector2(0, 1) / 16, Dirt_LBC + new Vector2(1, 1) / 16
            }
        },
        //Cactus
        {
            {
                //Cactus Top
                CactusTop_LBC, CactusTop_LBC + new Vector2(1, 0) / 16,
                CactusTop_LBC + new Vector2(0, 1) / 16, CactusTop_LBC + new Vector2(1, 1) / 16
            },
            {
                //Cactus Side
                CactusSide_LBC, CactusSide_LBC + new Vector2(1, 0) / 16,
                CactusSide_LBC + new Vector2(0, 1) / 16, CactusSide_LBC + new Vector2(1, 1) / 16
            },
            {
                //Cactus Bottom
                CactusBottom_LBC, CactusBottom_LBC + new Vector2(1, 0) / 16,
                CactusBottom_LBC + new Vector2(0, 1) / 16, CactusBottom_LBC + new Vector2(1, 1) / 16
            }
        },
        //Red Flower
        {
            {
                //WoodForest Top
                WoodForestTop_LBC, WoodForestTop_LBC + new Vector2(1, 0) / 16,
                WoodForestTop_LBC + new Vector2(0, 1) / 16, WoodForestTop_LBC + new Vector2(1, 1) / 16
            },
            {
                //WoodForest Side
                WoodForestSide_LBC, WoodForestSide_LBC + new Vector2(1, 0) / 16,
                WoodForestSide_LBC + new Vector2(0, 1) / 16, WoodForestSide_LBC + new Vector2(1, 1) / 16
            },
            {
                //WoodForest Bottom
                WoodForestTop_LBC, WoodForestTop_LBC + new Vector2(1, 0) / 16,
                WoodForestTop_LBC + new Vector2(0, 1) / 16, WoodForestTop_LBC + new Vector2(1, 1) / 16
            }
        },
        //WoodForestSnow
        {
            {
                //WoodForestSnow Top
                WoodForestTop_LBC, WoodForestTop_LBC + new Vector2(1, 0) / 16,
                WoodForestTop_LBC + new Vector2(0, 1) / 16, WoodForestTop_LBC + new Vector2(1, 1) / 16
            },
            {
                //WoodForestSnow Side
                WoodForestSnowSide_LBC, WoodForestSnowSide_LBC + new Vector2(1, 0) / 16,
                WoodForestSnowSide_LBC + new Vector2(0, 1) / 16, WoodForestSnowSide_LBC + new Vector2(1, 1) / 16
            },
            {
                //WoodForestSnow Bottom
                WoodForestTop_LBC, WoodForestTop_LBC + new Vector2(1, 0) / 16,
                WoodForestTop_LBC + new Vector2(0, 1) / 16, WoodForestTop_LBC + new Vector2(1, 1) / 16
            }
        },
        //RedFlower
        {
            {
                //RedFlower
                RedFlower_LBC, RedFlower_LBC + new Vector2(1, 0) / 16,
                RedFlower_LBC + new Vector2(0, 1) / 16, RedFlower_LBC + new Vector2(1, 1) / 16
            },
            {
                //RedFlower
                RedFlower_LBC, RedFlower_LBC + new Vector2(1, 0) / 16,
                RedFlower_LBC + new Vector2(0, 1) / 16, RedFlower_LBC + new Vector2(1, 1) / 16
            },
            {
                //RedFlower
                RedFlower_LBC, RedFlower_LBC + new Vector2(1, 0) / 16,
                RedFlower_LBC + new Vector2(0, 1) / 16, RedFlower_LBC + new Vector2(1, 1) / 16
            }
        },
        //YellowFlower
        {
            {
                //YellowFlower
                YellowFlower_LBC, YellowFlower_LBC + new Vector2(1, 0) / 16,
                YellowFlower_LBC + new Vector2(0, 1) / 16, YellowFlower_LBC + new Vector2(1, 1) / 16
            },
            {
                //YellowFlower
                YellowFlower_LBC, YellowFlower_LBC + new Vector2(1, 0) / 16,
                YellowFlower_LBC + new Vector2(0, 1) / 16, YellowFlower_LBC + new Vector2(1, 1) / 16
            },
            {
                //YellowFlower
                YellowFlower_LBC, YellowFlower_LBC + new Vector2(1, 0) / 16,
                YellowFlower_LBC + new Vector2(0, 1) / 16, YellowFlower_LBC + new Vector2(1, 1) / 16
            }
        },
        //RedMushroom
        {
            {
                //RedMushroom
                RedMushroom_LBC, RedMushroom_LBC + new Vector2(1, 0) / 16,
                RedMushroom_LBC + new Vector2(0, 1) / 16, RedMushroom_LBC + new Vector2(1, 1) / 16
            },
            {
                //RedMushroom
                RedMushroom_LBC, RedMushroom_LBC + new Vector2(1, 0) / 16,
                RedMushroom_LBC + new Vector2(0, 1) / 16, RedMushroom_LBC + new Vector2(1, 1) / 16
            },
            {
                //RedMushroom
                RedMushroom_LBC, RedMushroom_LBC + new Vector2(1, 0) / 16,
                RedMushroom_LBC + new Vector2(0, 1) / 16, RedMushroom_LBC + new Vector2(1, 1) / 16
            }
        },
        //BrownMushroom
        {
            {
                //BrownMushroom
                BrownMushroom_LBC, BrownMushroom_LBC + new Vector2(1, 0) / 16,
                BrownMushroom_LBC + new Vector2(0, 1) / 16, BrownMushroom_LBC + new Vector2(1, 1) / 16
            },
            {
                //BrownMushroom
                BrownMushroom_LBC, BrownMushroom_LBC + new Vector2(1, 0) / 16,
                BrownMushroom_LBC + new Vector2(0, 1) / 16, BrownMushroom_LBC + new Vector2(1, 1) / 16
            },
            {
                //BrownMushroom
                BrownMushroom_LBC, BrownMushroom_LBC + new Vector2(1, 0) / 16,
                BrownMushroom_LBC + new Vector2(0, 1) / 16, BrownMushroom_LBC + new Vector2(1, 1) / 16
            }
        },
        //Weeds
        {
            {
                //Weeds
                Weeds_LBC, Weeds_LBC + new Vector2(1, 0) / 16,
                Weeds_LBC + new Vector2(0, 1) / 16, Weeds_LBC + new Vector2(1, 1) / 16
            },
            {
                //Weeds
                Weeds_LBC, Weeds_LBC + new Vector2(1, 0) / 16,
                Weeds_LBC + new Vector2(0, 1) / 16, Weeds_LBC + new Vector2(1, 1) / 16
            },
            {
                //Weeds
                Weeds_LBC, Weeds_LBC + new Vector2(1, 0) / 16,
                Weeds_LBC + new Vector2(0, 1) / 16, Weeds_LBC + new Vector2(1, 1) / 16
            }
        },
        //DeadBush
        {
            {
                //DeadBush
                DeadBush_LBC, DeadBush_LBC + new Vector2(1, 0) / 16,
                DeadBush_LBC + new Vector2(0, 1) / 16, DeadBush_LBC + new Vector2(1, 1) / 16
            },
            {
                //DeadBush
                DeadBush_LBC, DeadBush_LBC + new Vector2(1, 0) / 16,
                DeadBush_LBC + new Vector2(0, 1) / 16, DeadBush_LBC + new Vector2(1, 1) / 16
            },
            {
                //DeadBush
                DeadBush_LBC, DeadBush_LBC + new Vector2(1, 0) / 16,
                DeadBush_LBC + new Vector2(0, 1) / 16, DeadBush_LBC + new Vector2(1, 1) / 16
            }
        }

    };
    
    public Block(string type, BlockType bType, Vector3 pos, Chunk owner, Material material)
    {
        this.pos = pos;
        this.owner = owner;
        this.material = material;
        SetType(bType);
        gameObject = new GameObject(type);
    }
    
    public BlockType GetBlockType()
    {
       return bType;
    }
    
    public bool isSolidBlock()
    {
        return isSolid;
    }


    public void SetType(BlockType bType)
    {
        this.bType = bType;
        if(bType == BlockType.Air || bType == BlockType.RedFlower || bType == BlockType.YellowFlower || bType == BlockType. RedMushroom || bType == BlockType. BrownMushroom || bType == BlockType.Weeds || bType == BlockType.DeadBush)
        {
            isSolid = false;
        }
        else
        {
            isSolid = true;
        }
    }

    void CreateBlockQuad(Cubeside side)
    {
        Mesh mesh = new Mesh();

        Vector3 v0 = new Vector3(-0.5f, -0.5f, 0.5f);
        Vector3 v1 = new Vector3(0.5f,-0.5f,0.5f);
        Vector3 v2 = new Vector3(0.5f,-0.5f,-0.5f);
        Vector3 v3 = new Vector3(-0.5f, -0.5f, -0.5f);
        Vector3 v4 = new Vector3(-0.5f,0.5f, 0.5f);
        Vector3 v5 = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 v6 = new Vector3(0.5f, 0.5f, -0.5f);
        Vector3 v7 = new Vector3(-0.5f, 0.5f, -0.5f);

        Vector2 uv00 = new Vector2(0, 0);
        Vector2 uv01 = new Vector2(0, 1);
        Vector2 uv10 = new Vector2(1, 0);   
        Vector2 uv11 = new Vector2(1, 1);
        
        Debug.Log((int)bType);

        if(side == Cubeside.TOP)
        {
            uv00 = blockUVs[(int)bType, 0, 0];
            uv10 = blockUVs[(int)bType, 0, 1];
            uv01 = blockUVs[(int)bType, 0, 2];
            uv11 = blockUVs[(int)bType, 0, 3];
        }
        else if(side == Cubeside.BOTTOM) 
        {
            uv00 = blockUVs[(int)bType, 2, 0];
            uv10 = blockUVs[(int)bType, 2, 1];
            uv01 = blockUVs[(int)bType, 2, 2];
            uv11 = blockUVs[(int)bType, 2, 3];
        }
        else if(side == Cubeside.DIAG1FRONT || side == Cubeside.DIAG2FRONT)
        {
            uv00 = blockUVs[(int)bType, 0, 0];
            uv10 = blockUVs[(int)bType, 0, 1];
            uv01 = blockUVs[(int)bType, 0, 2];
            uv11 = blockUVs[(int)bType, 0, 3];
        }
        else if(side == Cubeside.DIAG1BACK || side == Cubeside.DIAG2BACK)
        {
            uv00 = blockUVs[(int)bType, 1, 0];
            uv10 = blockUVs[(int)bType, 1, 1];
            uv01 = blockUVs[(int)bType, 1, 2];
            uv11 = blockUVs[(int)bType, 1, 3];
        }
        else
        {
            uv00 = blockUVs[(int)bType, 1, 0];
            uv10 = blockUVs[(int)bType, 1, 1];
            uv01 = blockUVs[(int)bType, 1, 2];
            uv11 = blockUVs[(int)bType, 1, 3];
        }

        Vector3[] vertices = new Vector3[4]; 
        Vector3[] normals = new Vector3[4];
        int[] triangles = new int[] { 3, 1, 0, 3, 2, 1 };
        Vector2[] uv = new Vector2[] { uv11, uv01, uv00, uv10 };

        switch (side)
        {
            case Cubeside.FRONT:
                vertices = new Vector3[] { v4, v5, v1, v0 };
                normals = new Vector3[] { Vector3.forward, Vector3.forward, Vector3.forward, Vector3.forward };
                break;
            case Cubeside.BOTTOM:
                vertices = new Vector3[] { v0, v1, v2, v3 };
                normals = new Vector3[] { Vector3.down, Vector3.down, Vector3.down, Vector3.down };
                break;
            case Cubeside.TOP:
                vertices = new Vector3[] { v7, v6, v5, v4 };
                normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
                break;
            case Cubeside.LEFT:
                vertices = new Vector3[] { v7, v4, v0, v3 };
                normals = new Vector3[] { Vector3.left, Vector3.left, Vector3.left, Vector3.left };
                break;
            case Cubeside.RIGHT:
                vertices = new Vector3[] { v5, v6, v2, v1 };
                normals = new Vector3[] { Vector3.right, Vector3.right, Vector3.right, Vector3.right };
                break;
            case Cubeside.BACK:
                vertices = new Vector3[] { v6, v7, v3, v2 };
                normals = new Vector3[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                break;
            case Cubeside.DIAG1FRONT:
                vertices = new Vector3[] { v4, v6, v2, v0 };
                normals = new Vector3[]
                    { new Vector3(1, 0, 1), new Vector3(1, 0, 1), new Vector3(1, 0, 1), new Vector3(1, 0, 1) };
                break;
            case Cubeside.DIAG1BACK:
                vertices = new Vector3[] { v6, v4, v0, v2 };
                normals = new Vector3[]
                    { new Vector3(-1, 0, -1), new Vector3(-1, 0, -1), new Vector3(-1, 0, -1), new Vector3(-1, 0, -1) };
                break;
            case Cubeside.DIAG2FRONT:
                vertices = new Vector3[] { v7, v5, v1, v3 };
                normals = new Vector3[]
                    { new Vector3(1, 0, -1), new Vector3(1, 0, -1), new Vector3(1, 0, -1), new Vector3(1, 0, -1) };
                break;
            case Cubeside.DIAG2BACK:
                vertices = new Vector3[] { v5, v7, v3, v1 };
                normals = new Vector3[]
                    { new Vector3(-1, 0, 1), new Vector3(-1, 0, 1), new Vector3(-1, 0, 1), new Vector3(-1, 0, 1) };
                break;
        }
        
        mesh.vertices = vertices;
        mesh.normals = normals; 
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("quad");
        quad.transform.position = this.pos;
        quad.transform.parent = owner.goChunk.transform;

        MeshFilter mf = quad.AddComponent<MeshFilter>();
        mf.mesh = mesh;
        
        quads.Add(quad);
    }

    int ConvertToLocalIndex(int i)
    {
        if(i == -1) return World.chunkSize - 1;
        if (i == World.chunkSize) return 0;
        return i;
    }

    bool HasSolidNeighbour(int x, int y, int z)
    {

        Block[,,] chunkdata;

        if (x < 0 || x >= World.chunkSize || y < 0 || y >= World.chunkSize || z < 0 || z >= World.chunkSize)
        {
            Vector3 neighChunkPos = owner.goChunk.transform.position + new Vector3(
                (x - (int)pos.x) * World.chunkSize,
                (y - (int)pos.y) * World.chunkSize, 
                (z - (int)pos.z) * World.chunkSize);
            string chunkName = World.CreateChunkName(neighChunkPos);

            x = ConvertToLocalIndex(x);
            y = ConvertToLocalIndex(y);
            z = ConvertToLocalIndex(z);

            Chunk neighChunk;
            if (World.chunkDict.TryGetValue(chunkName, out neighChunk))
                chunkdata = neighChunk.chunkdata;
            else
                return false;

        }
        else
        
            chunkdata = owner.chunkdata;
        try
        {
            return chunkdata[x, y, z].isSolid;
        }
        catch (System.IndexOutOfRangeException ex)
        {

        }
        return false;
        

        
    }

    public void Draw()
    {
        quads = new List<GameObject>();
        if (bType == BlockType.Air) return;

        else if (bType == BlockType.RedFlower || bType == BlockType.YellowFlower || bType == BlockType. RedMushroom || bType == BlockType. BrownMushroom || bType == BlockType.Weeds || bType == BlockType.DeadBush)
        {
            CreateBlockQuad(Cubeside.DIAG1FRONT);
            CreateBlockQuad(Cubeside.DIAG1BACK);
            CreateBlockQuad(Cubeside.DIAG2FRONT);
            CreateBlockQuad(Cubeside.DIAG2BACK);
        }
        else
        { 
            if (!HasSolidNeighbour((int)pos.x - 1, (int)pos.y, (int)pos.z))
                CreateBlockQuad(Cubeside.LEFT);
            if (!HasSolidNeighbour((int)pos.x + 1, (int)pos.y, (int)pos.z))
                CreateBlockQuad(Cubeside.RIGHT);
            if (!HasSolidNeighbour((int)pos.x, (int)pos.y - 1, (int)pos.z))
                CreateBlockQuad(Cubeside.BOTTOM);
            if (!HasSolidNeighbour((int)pos.x, (int)pos.y + 1, (int)pos.z))
                CreateBlockQuad(Cubeside.TOP);
            if (!HasSolidNeighbour((int)pos.x, (int)pos.y, (int)pos.z - 1))
                CreateBlockQuad(Cubeside.BACK);
            if (!HasSolidNeighbour((int)pos.x, (int)pos.y, (int)pos.z + 1))
                CreateBlockQuad(Cubeside.FRONT);
        }
    }
}
