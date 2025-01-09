using Unity.Mathematics;
using UnityEditor.AssetImporters;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode{NoiseMap, Colormap, Mesh}
    public DrawMode drawmode;
    public const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    public int seed;
    public Vector2 offset;
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] Regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);
        Color[] colormap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < Regions.Length; i++)
                {
                    if (currentHeight <= Regions[i].height)
                    {
                        colormap[y * mapChunkSize + x] = Regions[i].color;
                        break;
                    }
                }
            }
        }
        MapDisplay display = FindFirstObjectByType<MapDisplay>();
        if(drawmode == DrawMode.NoiseMap){
            display.DrawTexture(TextureGenerator.TexturefromHeightMap(noiseMap));
        }else if(drawmode == DrawMode.Colormap){
            display.DrawTexture(TextureGenerator.TextureFromColormap(colormap, mapChunkSize, mapChunkSize));
        }else if(drawmode == DrawMode.Mesh){
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColormap(colormap, mapChunkSize, mapChunkSize));
        }
        
    }

    void OnValidate()
    {
        
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }

    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
