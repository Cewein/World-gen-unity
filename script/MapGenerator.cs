using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public enum DrawMode
    {
        Noise,
        Biome,
        Mesh
    }
    public DrawMode mode;

    public int seed;

    public const int mapSize = 105;

    [Range(0,6)]
    public int LevelOfDetail;

    public Vector2 coord;

    public float heightMultiplier;
    public AnimationCurve HeightGradiant;

    public float noiseScale;
    [Range(1, 10)]
    public int octave;

    [Range(0, 1)]
    public float percistence;
    public float lacunarity;

    public MapColor[] biomes;
    public int density;
    public float InGround;

    public bool AutoUpdate;
    public bool addvegetation;
    public bool AutoMoving;

    public void Start()
    {
        UpdateMapInEditor();
    }

    public void UpdateMapInEditor()
    {
        MapData mapData = Generator();

        MapDisplay display = FindObjectOfType<MapDisplay>();

        if (mode == DrawMode.Biome)
        {
            display.DisplayTexture(TextureGen.FromBiome(mapData.colorMap, mapSize, mapSize));
        }
        else if (mode == DrawMode.Noise)
        {
            display.DisplayTexture(TextureGen.FromNoiseScale(mapData.noiseMap));
        }
        else if (mode == DrawMode.Mesh)
        {
            MeshData meshData = MeshGen.TerrainMesh(mapData.noiseMap, heightMultiplier, HeightGradiant, LevelOfDetail);
            display.DisplayMesh(meshData, TextureGen.FromBiome(mapData.colorMap, mapSize, mapSize));
            if(addvegetation)
            {
                List<GameObject> spawnedVeget = new List<GameObject>();

                spawnedVeget = VegetationGenerator.PlaceVegetation(mapData.noiseMap, meshData, LevelOfDetail, biomes, density, spawnedVeget);

                spawnedVeget = RaycastSpawning.Adjust(spawnedVeget, InGround);

                spawnedVeget.Clear();
            }
        }
    }

    private MapData Generator()
    {
        

        float[,] noiseMap = Noise.Generate(mapSize, mapSize, noiseScale, coord, octave, lacunarity, percistence, seed);

        Color[] colorMap = new Color[mapSize * mapSize];
        for(int x = 0; x < mapSize; x++)
        {
            for(int y = 0; y < mapSize; y++)
            {
                float actualPos = noiseMap[x, y];
                for(int i = 0; i < biomes.Length; i++)
                {
                    if(actualPos <= biomes[i].range)
                    {
                        colorMap[y * mapSize + x] = biomes[i].color;
                        break;
                    }
                }
            }
        }

        return new MapData(noiseMap, colorMap);

    }

    private void Update()
    {
        if(AutoMoving)
        {
            coord.x += 0.01f;
            Generator();
        }
    }

    public void Moving()
    {

    }

    [System.Serializable]
    public struct MapColor
    {
        public string name;
        public float range;
        public Color color;
        public GameObject[] population;
    }

    public struct MapData
    {
        public float[,] noiseMap;
        public Color[] colorMap;

        public MapData(float[,] arrayNoise, Color[] arrayColor)
        {
            noiseMap = arrayNoise;
            colorMap = arrayColor;
        }
    }

    private void OnValidate()
    {
        if(noiseScale < 1)
        {
            noiseScale = 1;
        }

        if(lacunarity < 1)
        {
            lacunarity = 1;
        }

    }

    public void CheckBiomes()
    {
        for (int pos = 1; pos < biomes.Length; pos++)
        {
            if (biomes[pos].range < biomes[pos - 1].range)
            {
                Swap(biomes, pos);
                Generator();
            }
        }
    }

    MapColor[] Swap(MapColor[] colorRule, int pos)
    {
        MapColor temp;

        temp = colorRule[pos];
        colorRule[pos] = colorRule[pos - 1];
        colorRule[pos - 1] = temp;

        if(pos - 2 >= 0 &&colorRule[pos - 1].range < colorRule[pos - 2].range)
        {
            Swap(colorRule,pos - 1);
        }

        return colorRule;
    }
}


