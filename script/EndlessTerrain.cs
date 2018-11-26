using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour {

    public const int maxViewDistance = 500;
    public Transform viewer;

    public static Vector2 viewerPos;
    int chunkSize;
    int chunkVisibleInViewDistance;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> TerrainChunkLastUpdate = new List<TerrainChunk>();


	void Start () {
        chunkSize = MapGenerator.mapSize;
        chunkVisibleInViewDistance = Mathf.RoundToInt(maxViewDistance / chunkSize);
	}

    private void Update()
    {
        viewerPos = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }

    void UpdateVisibleChunks()
    {
        for(int i = 0; i < TerrainChunkLastUpdate.Count; i++)
        {
            TerrainChunkLastUpdate[i].SetVisible(false);
        }
        TerrainChunkLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPos.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPos.y / chunkSize);

        for(int xOffset = -chunkVisibleInViewDistance;xOffset <= chunkVisibleInViewDistance; xOffset++)
        {
            for(int yOffset = -chunkVisibleInViewDistance; yOffset <= chunkVisibleInViewDistance; yOffset++)
            {
                Vector2 viewedChunk = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if(terrainChunkDictionary.ContainsKey(viewedChunk))
                {
                    terrainChunkDictionary[viewedChunk].UpdateTerrainChunk();
                    if(terrainChunkDictionary[viewedChunk].IsActive())
                    {
                        TerrainChunkLastUpdate.Add(terrainChunkDictionary[viewedChunk]);
                    }
                }
                else
                {
                    terrainChunkDictionary.Add(viewedChunk, new TerrainChunk(viewedChunk, chunkSize));
                }
            }
        }

    }

    public class TerrainChunk
    {
        GameObject meshObj;
        Vector2 position;
        Bounds bounds;

        public TerrainChunk(Vector2 coord, int size)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObj = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObj.transform.position = positionV3;
            meshObj.transform.localScale = Vector3.one * size / 10f;
            SetVisible(false);
        }

        public void UpdateTerrainChunk()
        {
            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPos));
            bool visible = viewerDstFromNearestEdge <= maxViewDistance;
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            meshObj.SetActive(visible);
        }

        public bool IsActive()
        {
            return meshObj.activeSelf;
        }
    }

}
