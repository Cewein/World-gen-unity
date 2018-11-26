using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGen {

	public static MeshData TerrainMesh(float [,] heightMap, float mulitplier, AnimationCurve curve, int LOD)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);

        int meshSimplification = (LOD == 0) ? 1 : LOD * 2;
        int vertexPerLine = (width - 1) / meshSimplification + 1;

        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(vertexPerLine, vertexPerLine);
        int vertexIndex = 0;

        for (int y = 0; y < height; y += meshSimplification)
        {
            for (int x = 0; x < width; x += meshSimplification)
            {
                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, curve.Evaluate(heightMap[x, y]) * mulitplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if( x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + vertexPerLine + 1, vertexIndex + vertexPerLine);
                    meshData.AddTriangle(vertexIndex + vertexPerLine + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }

        

        return meshData;
    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int trianglesIndex;

    public MeshData(int width, int height)
    {
        vertices = new Vector3[width * height];
        uvs = new Vector2[width * height];
        triangles = new int[(width - 1) * (height - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[trianglesIndex] = a;
        triangles[trianglesIndex+1] = b;
        triangles[trianglesIndex+2] = c;
        trianglesIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh
        {
            vertices = vertices,
            uv = uvs,
            triangles = triangles
        };
        mesh.RecalculateNormals();

        return mesh;
    }


}
