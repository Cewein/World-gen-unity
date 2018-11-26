using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VegetationGenerator : MonoBehaviour
{
    public static List<GameObject> PlaceVegetation(float[,] heightMap, MeshData meshData, int LOD, MapGenerator.MapColor[] Vegetation, int density, List<GameObject> spawnedVeget)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        int meshSimplification = (LOD == 0) ? 1 : LOD * 2;
        int vertexPerLine = (width - 1) / meshSimplification + 1;

        int vertexIndex = 0;

        for (int y = 0; y < height-1; y += meshSimplification)
        {
            for (int x = 0; x < width; x += meshSimplification)
            {
                for (int i = 0; i < Vegetation.Length; i++)
                {
                    if (Vegetation[i].range >= heightMap[x, y])
                    {
                        if (Vegetation[i].population.Length > 0)
                        {
                            for (int j = 0; j < density; j++)
                            {
                                int choose = Random.Range(0, Vegetation[i].population.Length - 1);
                                /*if (y >= 239)
                                {
                                    Debug.Log("coord = " + y);
                                    Debug.Log("coord = " + x);
                                    Debug.Log("vertex = " + vertexIndex);
                                    Debug.Log("the limit = " + vertexIndex + vertexPerLine + 1);
                                    Debug.Log("length of vertices = " + meshData.vertices.Length);
                                    Debug.Log("rand = " + choose);
                                    Debug.Log("veget = " + Vegetation[i].range);
                                }*/
                                
                             

                                spawnedVeget.Add(
                                    Instantiate(
                                        Vegetation[i].population[choose],
                                        new Vector3
                                        (
                                            Random.Range(meshData.vertices[vertexIndex].x * 5, meshData.vertices[vertexIndex + vertexPerLine + 1].x * 5),
                                            1000,
                                            Random.Range(meshData.vertices[vertexIndex].z * 5, meshData.vertices[vertexIndex + vertexPerLine + 1].z * 5)
                                        )
                                        ,
                                        new Quaternion
                                        (
                                            0, Random.Range(0f, 360f), 0, 0
                                        )
                                    )
                                );
                            

                            }
                        }

                        break;
                    }
                }

                vertexIndex++;

            }
        }

        return spawnedVeget;
    }
}
