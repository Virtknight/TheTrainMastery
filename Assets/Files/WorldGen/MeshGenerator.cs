using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        int meshSimplificationIncrement = (levelOfDetail ==0)?1:levelOfDetail * 2;
        int verticiesPerLine = (width-1)/meshSimplificationIncrement + 1;

        MeshData meshData = new MeshData(verticiesPerLine, verticiesPerLine);
        int vertexIndex = 0;

        for (int y = 0; y < height; y+=meshSimplificationIncrement)
        {
            for (int x = 0; x < width; x+=meshSimplificationIncrement)
            {
                meshData.Vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x,y]) * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x/(float)width, y/(float)height);

                if(x < width - 1 && y < height - 1){
                    meshData.AddTriangle(vertexIndex, vertexIndex + verticiesPerLine + 1, vertexIndex + verticiesPerLine);
                    meshData.AddTriangle(vertexIndex + verticiesPerLine +1 , vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }
        return meshData;
    }
}

public class MeshData
{
    public Vector3[] Vertices;
    public int[] triangles;

    public Vector2[] uvs;

    int TriIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        Vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth*meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];

    }
    public void AddTriangle(int a, int b, int c)
    {
        triangles[TriIndex] = a;
        triangles[TriIndex + 1] = b;
        triangles[TriIndex + 2] = c;
        TriIndex += 3;
    }

    public Mesh CreateMesh(){
        Mesh mesh = new Mesh();
        mesh.vertices = Vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }
}