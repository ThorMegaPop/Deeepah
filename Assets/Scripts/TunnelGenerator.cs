using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TunnelGenerator : MonoBehaviour
{
    public int tunnelLength = 50; // Number of segments along the tunnel's depth
    public int segments = 16;     // Number of sides for each cross-section
    public float radius = 2f;     // Radius of the tunnel
    [SerializeField] private float noiseValue = 1; 

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private void Start() {
        CreateMesh();
    }

    private void CreateMesh()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateVertices();
        GenerateTriangles();
        ApplyNoise(noiseValue);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals(); // Ensure lighting works correctly
    } 

    private void GenerateVertices()
    {
        vertices = new Vector3[tunnelLength * segments]; 

        for (int i = 0; i < tunnelLength; i++)
        {
            float z = i; 
            for (int j = 0; j < segments; j++)
            {
                float angle = j * Mathf.PI * 2 / segments; 
                float x = Mathf.Cos(angle) * radius; 
                float y = Mathf.Sin(angle) * radius; 
                vertices[i*segments + j] = new Vector3(x,y,z); 
            }
        }
    }

    private void GenerateTriangles()
    {
        triangles = new int[(tunnelLength - 1) * segments * 6]; 

        int triIndex = 0;

        for (int i = 0; i < tunnelLength -1; i++)
        {
            for (int j = 0; j < segments; j++)
            {
                int current = i * segments + j;
                int next = i * segments + (j + 1) % segments;
                int nextRow = current + segments;
                int nextRowNext = next + segments;

                triangles[triIndex++] = current;
                triangles[triIndex++] = nextRow;
                triangles[triIndex++] = next;

                triangles[triIndex++] = next;
                triangles[triIndex++] = nextRow;
                triangles[triIndex++] = nextRowNext;
            }
        }
    }

    private void ApplyNoise(float noiseScale)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            float noiseValue = Mathf.PerlinNoise(vertices[i].z * noiseScale, 0) * 0.5f;
            vertices[i] += vertices[i].normalized * noiseValue;
        }
    }
}
