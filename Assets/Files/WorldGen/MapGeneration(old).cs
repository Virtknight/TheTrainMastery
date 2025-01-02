using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MapGeneration : MonoBehaviour
{

    Mesh mesh;

    Vector3[] Vertices;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;

    public bool GenerateNewMesh = true;

    [Header("PearlNoise")]

    public float amp1;
    public float amp2;
    public float amp3;
    public float scale1;
    public float scale2;
    public float scale3;


    void Update()
    {
        if(GenerateNewMesh == true){
            GenerateMesh();
            GenerateNewMesh = false;
        }
    }

    void GenerateMesh()
    {
        mesh = new Mesh();
        gameObject.GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }
    void CreateShape()
    {
        Vertices = new Vector3[(xSize + 1) * (zSize + 1)];


        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .3f, z * .3f) * 2f;
             
                Vertices[i] = new Vector3(x, y, z);
                i++;

            }
        }

        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = Vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {

        if (Vertices == null)
        {
            return;
        }
        for (int i = 0; i < Vertices.Length; i++)
        {
            Gizmos.DrawSphere(Vertices[i], .1f);
        }

    }

}
