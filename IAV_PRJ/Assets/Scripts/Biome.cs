using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome
{
    public enum BiomeType
    {
        FOREST,
        DESERT,
        SNOW
    };

    public static BiomeType getBiome(int temperature)
    {
        if (temperature < 30 && temperature > 10)
        {
            return BiomeType.FOREST;
        }

        if (temperature < 10)
        {
            return BiomeType.SNOW;
        }

        if (temperature > 30)
        {
            return BiomeType.DESERT;
        }

        return BiomeType.FOREST;
    }

    public static Block.BlockType getBiomeDirt(BiomeType type)
    {
        switch (type)
        {
            case BiomeType.FOREST:
                return Block.BlockType.Grass;
            case BiomeType.DESERT:
                return Block.BlockType.Sand;
            case BiomeType.SNOW:
                return Block.BlockType.Snow;
            default:
                return Block.BlockType.Grass;
        }
    }

    public static bool getVegProb(BiomeType type)
    {
        switch (type)
        {
            case BiomeType.FOREST:
                return Random.Range(0, 1f) < 0.2f;
            case BiomeType.DESERT:
                return Random.Range(0, 1f) < 0.05f;
            default:
                return Random.Range(0, 1f) < 0.1f;
        }
    }

    public static Block.BlockType getBiomeTree(BiomeType type)
    {
        switch (type)
        {
            case BiomeType.FOREST:
                return Block.BlockType.WoodForest;
            case BiomeType.DESERT:
                return Block.BlockType.Cactus;
            case BiomeType.SNOW:
                return Block.BlockType.WoodForestSnow;
            default:
                return Block.BlockType.WoodForest;
        }
    }

    public static Block.BlockType getBiomeVeg(BiomeType type)
    {
        switch (type)
        {
            case BiomeType.FOREST:
                float randFlowerOrGrass = Random.Range(0f, 1f);
                if (randFlowerOrGrass < 0.5f)
                {
                    float randWhichFlower = Random.Range(0f, 1f);
                    switch (randWhichFlower)
                    {
                        case < 0.25f:
                            return Block.BlockType.RedFlower;
                        case >= 0.25f and < 0.5f:
                            return Block.BlockType.YellowFlower;
                        case >= 0.5f and < 0.75f:
                            return Block.BlockType.RedMushroom;
                        default:
                            return Block.BlockType.BrownMushroom;
                    }
                }
                else
                {
                    return Block.BlockType.Weeds;
                }
            case BiomeType.DESERT:
                return Block.BlockType.DeadBush;
            case BiomeType.SNOW:
                return Block.BlockType.RedMushroom;
            default:
                return Block.BlockType.Weeds;
        }
    }
}
