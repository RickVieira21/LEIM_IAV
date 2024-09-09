using UnityEngine;
    
public class Utils
    {

        //ALTURA
        static float smooth = 0.001f;
        static float smooth3D = 10 * smooth;
        static int maxHeight = 150;
        static int octaves = 6;
        static float persistence = 0.7f;
        static float offset = 32000;

        //TEMPERATURA
         static float smoothT = 0.0005f;
        static int maxHeightT = 60;
        static int octavesT = 6;
        static float persistenceT = 3f;




        //Gera a altura do terreno
        public static int GenerateHeight(float x, float z){
             return (int)Map(0, maxHeight, 0, 1, fBM(x * smooth, z*smooth, octaves, persistence));
        }


        //Gera a temperatura do terreno (Usa parâmetros diferentes)
        public static int GenerateTemperature(float x, float z)
        {
            return (int)Map(0, maxHeightT, 0, 1, fBM(x * smoothT, z*smoothT, octavesT, persistenceT));
        }


        // (Não está a ser usado)
        public static int GenerateStoneHeight(float x, float z){
            return (int)Map(0, maxHeight - 10, 0, 1, fBM(x * 3*smooth, z*3*smooth, octaves-1, 1.2f*persistence));
        }

        static float Map(float newmin, float newmax, float orimin, float orimax, float val)
        {
            return Mathf.Lerp(newmin, newmax, Mathf.InverseLerp(orimin, orimax, val));
        }

        public static float fBM3D(float x, float y, float z, int octaves, float persistence){
            float xy = fBM(x*smooth3D, y*smooth3D, octaves, persistence);
            float yx = fBM(y*smooth3D, x*smooth3D, octaves, persistence);
            float xz = fBM(x*smooth3D, z*smooth3D, octaves, persistence);
            float zx = fBM(z*smooth3D, x*smooth3D, octaves, persistence);
            float yz = fBM(y*smooth3D, z*smooth3D, octaves, persistence);
            float zy = fBM(z*smooth3D, y*smooth3D, octaves, persistence);

            return (xy + yx + xz + zx + yz + zy)/6;

        }

        static float fBM(float x, float z, int octaves, float persistence)
        {
            float total = 0;
            float amplitude = 1;
            float frequency = 1;
            float maxValue = 0;
            for(int i=0; i < octaves; i++)
            {
                total += Mathf.PerlinNoise((x+offset) * frequency , (z+offset) * frequency) * amplitude;
                maxValue += amplitude;
                amplitude *= persistence;
                frequency *= 2;
            
            }
            return total/maxValue;
        }
}
