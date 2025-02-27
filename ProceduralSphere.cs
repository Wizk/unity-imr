using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SphereAnimator : MonoBehaviour
{
    public int resolution = 20; // Résolution de la sphère
    public float amplitude = 0.1f; // Amplitude de l'animation
    public float frequency = 1f; // Fréquence de l'animation
    public float speed = 1f; // Vitesse de l'animation

    private Mesh mesh;
    private Vector3[] baseVertices;
    private Vector3[] vertices;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GenerateSphere();
    }

    void Update()
    {
        AnimateSphere();
    }

    void GenerateSphere()
    {
        int vertCount = (resolution + 1) * (resolution + 1);
        int triCount = resolution * resolution * 6;

        Vector3[] vertices = new Vector3[vertCount];
        int[] triangles = new int[triCount];
        Vector2[] uv = new Vector2[vertCount];

        int vertexIndex = 0;
        int triangleIndex = 0;

        for (int i = 0; i <= resolution; i++)
        {
            for (int j = 0; j <= resolution; j++)
            {
                float theta = Mathf.PI * i / resolution;
                float phi = 2 * Mathf.PI * j / resolution;

                float x = Mathf.Sin(theta) * Mathf.Cos(phi);
                float y = Mathf.Cos(theta);
                float z = Mathf.Sin(theta) * Mathf.Sin(phi);

                vertices[vertexIndex] = new Vector3(x, y, z);
                uv[vertexIndex] = new Vector2((float)j / resolution, (float)i / resolution);

                if (i < resolution && j < resolution)
                {
                    int topLeft = (i * (resolution + 1)) + j;
                    int topRight = topLeft + 1;
                    int bottomLeft = ((i + 1) * (resolution + 1)) + j;
                    int bottomRight = bottomLeft + 1;

                    triangles[triangleIndex++] = topLeft;
                    triangles[triangleIndex++] = bottomLeft;
                    triangles[triangleIndex++] = topRight;

                    triangles[triangleIndex++] = topRight;
                    triangles[triangleIndex++] = bottomLeft;
                    triangles[triangleIndex++] = bottomRight;
                }

                vertexIndex++;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();

        baseVertices = vertices;
        this.vertices = new Vector3[vertCount];
        System.Array.Copy(vertices, this.vertices, vertices.Length);
    }

    void AnimateSphere()
    {
        float time = Time.time * speed;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseVertices[i];
            float wave = Mathf.Sin(time + vertex.x * frequency) + Mathf.Sin(time + vertex.y * frequency) + Mathf.Sin(time + vertex.z * frequency);
            vertices[i] = vertex + vertex * wave * amplitude;
        }

        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}
