using UnityEngine;

namespace DefaultNamespace
{
    public class CreateQuad : MonoBehaviour
    {
        enum CubeSide {Bottom, Top, Left, Right, Front, Back};
        public enum BlockType {Grass, Dirt, Stone};
        public Material material;
        public BlockType bType;
        
        static Vector2 GrassSide_LBC = new Vector2(3f, 15f)/16f;
        static Vector2 GrassTop_LBC = new Vector2(2f, 6f)/16f;
        static Vector2 Dirt_LBC = new Vector2(2f, 15f)/16f;
        static Vector2 Stone_LBC = new Vector2(0f, 14f)/16f;
        
        Vector2[,] blockUVs = {
            /*Grass Top*/{GrassTop_LBC, GrassTop_LBC, new Vector2(1f, 0f)/16, GrassTop_LBC,
                new Vector2(0f, 1f)/16, GrassTop_LBC, new Vector2(1f, 1f)/16},
            
            /*Grass Side*/{GrassSide_LBC, GrassSide_LBC, new Vector2(1f, 0f)/16, GrassSide_LBC,
                new Vector2(0f, 1f)/16, GrassSide_LBC, new Vector2(1f, 1f)/16},
            
            /*Dirt*/{Dirt_LBC, Dirt_LBC, new Vector2(1f, 0f)/16, Dirt_LBC, 
                new Vector2(0f, 1f)/16, Dirt_LBC, new Vector2(1f, 1f)/16},
            
            /*Stone*/{Stone_LBC, Stone_LBC, new Vector2(1f, 0f)/16, Stone_LBC, 
                new Vector2(0f, 1f)/16, Stone_LBC, new Vector2(1f, 1f)/16},
        };

        void Quad(CubeSide side)
        {
            Mesh mesh = new Mesh();
            
            Vector3 v0 = new Vector3(-0.5f, -0.5f, 0.5f);
            Vector3 v1 = new Vector3(0.5f, -0.5f, 0.5f);
            Vector3 v2 = new Vector3(0.5f, -0.5f, -0.5f);
            Vector3 v3 = new Vector3(-0.5f, -0.5f, -0.5f);
            Vector3 v4 = new Vector3(-0.5f, 0.5f, 0.5f);
            Vector3 v5 = new Vector3(0.5f, 0.5f, 0.5f);
            Vector3 v6 = new Vector3(0.5f, 0.5f, -0.5f);
            Vector3 v7 = new Vector3(-0.5f, 0.5f, -0.5f);
            
            Vector2 uv00 = new Vector2(0, 0);
            Vector2 uv10 = new Vector2(1, 0);
            Vector2 uv01 = new Vector2(0, 1);
            Vector2 uv11 = new Vector2(1, 1);

            if (bType == BlockType.Grass && side == CubeSide.Top)
            {
                uv00 = blockUVs[0, 0];
                uv10 = blockUVs[0, 1];
                uv01 = blockUVs[0, 2];
                uv11 = blockUVs[0, 3];
            }
            else if (bType == BlockType.Grass && side == CubeSide.Bottom)
            {
                uv00 = blockUVs[2, 0];
                uv10 = blockUVs[2, 1];
                uv01 = blockUVs[2, 2];
                uv11 = blockUVs[2, 3];
            }
            else
            {
                uv00 = blockUVs[(int)(bType + 1), 0];
                uv10 = blockUVs[(int)(bType + 1), 1];
                uv01 = blockUVs[(int)(bType + 1), 2];
                uv11 = blockUVs[(int)(bType + 1), 3];
            }
        }
    }
}