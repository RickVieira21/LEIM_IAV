using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree
{
    public Vector3 pos;
    public Chunk chunk;
    public Material material;
    public Biome.BiomeType biome;
    
    private List<Chunk> occupiedChunks = new List<Chunk>();
    
    public Tree(Vector3 pos, Chunk chunk, Material material, Biome.BiomeType biome)
    {
        this.pos = pos;

        if (pos.y >= World.chunkSize)
        {
            this.chunk = chunk.world.GetChunkAt(chunk.goChunk.transform.position + new Vector3(0, World.chunkSize, 0));
            this.pos.y = 0;
        }
        else
            this.chunk = chunk;

        this.material = material;
        this.biome = biome;
    }

    public void DrawTree()
    {
        int h = 3;
        for (int i = 0; i <= h; i++)
        {
            BuildTreeBlock(pos + new Vector3(0, i, 0));
        }
    }

    private void BuildTreeBlock(Vector3 treeBlockPos)
    {
        int x = (int)treeBlockPos.x;
        int y = (int)treeBlockPos.y;
        int z = (int)treeBlockPos.z;

        int chunkOffsetX = (x < 0) ? -1 : (x > 15) ? 1 : 0;
        int chunkOffsetY = (y < 0) ? -1 : (y > 15) ? 1 : 0;
        int chunkOffsetZ = (z < 0) ? -1 : (z > 15) ? 1 : 0;

        int dataPosX = (x < 0) ? World.chunkSize + x : (x > 15) ? x - World.chunkSize : x;
        int dataPosY = (y < 0) ? World.chunkSize + y : (y > 15) ? y - World.chunkSize : y;
        int dataPosZ = (z < 0) ? World.chunkSize + z : (z > 15) ? z - World.chunkSize : z;

        Vector3 chunkOffset = new Vector3(World.chunkSize * chunkOffsetX, World.chunkSize * chunkOffsetY, World.chunkSize * chunkOffsetZ);
        Chunk occupiedChunk = chunk.world.GetChunkAt(chunk.goChunk.transform.position + chunkOffset);
        Block block = new Block("",Biome.getBiomeTree(biome), new Vector3(dataPosX, dataPosY, dataPosZ), occupiedChunk, material);
        occupiedChunk.chunkdata[dataPosX, dataPosY, dataPosZ] = block;

        if (!occupiedChunks.Contains(occupiedChunk) && occupiedChunk != chunk)
            occupiedChunks.Add(occupiedChunk);
    }


    public void DrawTreeChunks()
    {
        foreach (Chunk chunkAux in occupiedChunks)
        {
            chunkAux.goChunk.SetActive(chunk.goChunk.active);
            if (chunk.goChunk.active)
            {
                //chunkAux.goChunk.gameObject.tag = "Tree";
                Object.DestroyImmediate(chunkAux.goChunk.GetComponent<MeshFilter>());
                Object.DestroyImmediate(chunkAux.goChunk.GetComponent<MeshRenderer>());
                Object.DestroyImmediate(chunkAux.goChunk.GetComponent<MeshCollider>());
                chunkAux.DrawChunk();
            }
        }
    }
}
