using UnityEngine;
using System.Collections.Generic;
using System;
public class EndlessTerrain : MonoBehaviour
{
    public const float maxViewDist = 300;
    public Transform viewer;

    public static Vector2 viewerPosition;
    int chunkSize;
    int chunksInView;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new();
    List<TerrainChunk> TerrainChunksVisibleLastUpdate = new();
    void Start()
    {
        for(int i = 0; i< TerrainChunksVisibleLastUpdate.Count; i++){
            TerrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        TerrainChunksVisibleLastUpdate.Clear();
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunksInView = Mathf.RoundToInt(maxViewDist / chunkSize);

    }
    void Update(){
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        updateTerrainChunkVisibleChunks();
    }
    void updateTerrainChunkVisibleChunks()
    {
        int currentChunkCordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int yOffset = -chunksInView; yOffset <= chunksInView; yOffset++)
        {
            for (int xOffset = -chunksInView; xOffset <= chunksInView; xOffset++)
            {

                Vector2 viewedChunkCord = new Vector2(currentChunkCordX + xOffset, currentChunkCordY + yOffset);
                if (terrainChunkDictionary.ContainsKey(viewedChunkCord))
                {
                    terrainChunkDictionary[viewedChunkCord].updateTerrainChunk();
                    if(terrainChunkDictionary[viewedChunkCord].IsVisible()){
                        TerrainChunksVisibleLastUpdate.Add(terrainChunkDictionary[viewedChunkCord]);
                    }
                }
                else
                {
                    terrainChunkDictionary.Add(viewedChunkCord, new TerrainChunk(viewedChunkCord, chunkSize));
                }

            }
        }


    }

    public class TerrainChunk
    {

        Vector2 position;
        GameObject meshObject;
        Bounds bounds;

        public TerrainChunk(Vector2 cord, int size)
        {
            position = cord * size;
            bounds = new(position, Vector2.one * size);
            Vector3 positionV3 = new(position.x, 0, position.y);
            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = positionV3;
            meshObject.transform.localScale = Vector3.one * size / 10f;
            SetVisible(false);
        }

        public void updateTerrainChunk()
        {
            float viewerDistFromNearestEdge = MathF.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDistFromNearestEdge <= maxViewDist;
            SetVisible(visible);
        }
        public void SetVisible(bool visible){
            meshObject.SetActive(visible);
        }
        public bool IsVisible(){
            return meshObject.activeSelf;
        }
    }
}
