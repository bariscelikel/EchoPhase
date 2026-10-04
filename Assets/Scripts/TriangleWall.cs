using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TriangleWall : MonoBehaviour
{
    void Awake()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = new Vector3[]
        {
            new Vector3(-9f, 0f, 0f),
            new Vector3(9f, 0f, 0f),
            new Vector3(0f, 4f, 0f)
        };

        mesh.triangles = new int[]
        {
            0, 1, 2
        };

        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}