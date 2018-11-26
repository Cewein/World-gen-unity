using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGen : MonoBehaviour
{

    public static Texture2D FromNoiseScale(float[,] noise)
    {
        int width = noise.GetLength(0);
        int heigth = noise.GetLength(1);

        Texture2D texture = new Texture2D(width, heigth);

        Color[] colorMap = new Color[width * heigth];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < heigth; y++)
            {
                colorMap[y * width + x] = Color.Lerp(Color.black, Color.white, noise[x, y]);
            }
        }

        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;

    }

    public static Texture2D FromBiome(Color[] colorMap, int width, int height)
    {

        Texture2D texture = new Texture2D(width, height)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;

    }

}
