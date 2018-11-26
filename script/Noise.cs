using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise : MonoBehaviour {

	public static float[,] Generate(int width, int heigth, float scale, Vector2 coord, int octave, float lacunarity, float percistence, int seed)
    {

        System.Random prng = new System.Random(seed);
        Vector2[] coordinate = new Vector2[octave];
        for(int i = 0; i < octave; i++)
        {
            float offsetX = prng.Next(-10000, 10000) + coord.x;
            float offsetY = prng.Next(-10000, 10000) + coord.y;

            coordinate[i] = new Vector2(offsetX, offsetY);
        }

        float[,] noise = new float [width,heigth];

        if (scale < 0f) scale = 0.00001f;

        float halfWidth = width / 2f;
        float halfHeight = heigth / 2f;

        float maxValue = float.MinValue;
        float minValue = float.MaxValue;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < heigth; y++)
            {
                for (int z = 0; z < octave; z++)
                {
                    float cordX = (x - halfWidth) / scale * Mathf.Pow(lacunarity, z) + coordinate[z].x;
                    float cordY = (y - halfHeight) / scale * Mathf.Pow(lacunarity, z) + coordinate[z].y;

                    noise[x, y] += Mathf.PerlinNoise(cordX, cordY) * Mathf.Pow(percistence, z) * 2 - 1;
                }

                if (noise[x, y] > maxValue) maxValue = noise[x, y];
                if (noise[x, y] < minValue) minValue = noise[x, y]; 
            }

        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < heigth; y++)
            {
                noise[x, y] = Mathf.InverseLerp(minValue, maxValue, noise[x, y]);
            }
        }


                return noise;
    }
}
