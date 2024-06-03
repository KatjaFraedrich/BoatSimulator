using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[RequireComponent(typeof(MeshFilter))]
public class GeneratePlane : MonoBehaviour
{
    [Tooltip("Number of vertices along the x and y axis respectively.")]
    [SerializeField] private Vector2Int resolution = new Vector2Int(100, 100);
    [Tooltip("Size of the mesh in unty unity in x and y direction. (Will be affected by transform.scale)")]
    [SerializeField] private Vector2 size = new Vector2(100, 100);
    private MeshFilter planeMesh;

    [ContextMenu("Generate Plane")]
    void GeneratePlaneMesh()
    {
        if (planeMesh == null)
        {
            planeMesh = GetComponent<MeshFilter>();
        }
        (List<Vector3> vertices, List<Vector2> uvs) = GenerateVerticesAndUVs();
        List<int> triangles = GenerateEdges();

        Mesh newPlaneMesh = new Mesh();
        newPlaneMesh.vertices = vertices.ToArray();
        newPlaneMesh.triangles = triangles.ToArray();
        newPlaneMesh.uv = uvs.ToArray();


        newPlaneMesh.RecalculateNormals();
        newPlaneMesh.RecalculateBounds();
        planeMesh.mesh = newPlaneMesh;
        Debug.Log($"Created Mesh with {vertices.Count} vertices");
    }

    /// <summary>
    /// Generates a list of vertices and uvs based on the scripts resolution and size parameters
    /// </summary>
    /// <returns>The list of vector3 vertices in the first component and vector2 uvs in the second component</returns>
    private (List<Vector3>, List<Vector2>) GenerateVerticesAndUVs()
    {
        
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();


        for (int i = 0; i < resolution.x; i++)
        {
            for (int j = 0; j < resolution.y; j++)
            {
                float x = (float)i / (resolution.x - 1) * size.x - size.x / 2;
                float z = (float)j / (resolution.y - 1) * size.y - size.y / 2;

                vertices.Add(new Vector3(x, 0, z));
                uvs.Add(new Vector2((float)i / (resolution.x - 1), (float)j / (resolution.y - 1)));

            }
        }
        return (vertices, uvs);
    }

    /// <summary>
    /// Generates a list of edges based on the scripts resolution parameter. More specifically by creating a list of vertex indices that correspond to the triangles of the mesh.
    /// </summary>
    /// <returns>A list of ints being the vertex indices of the triangles in order.</returns>
    private List<int> GenerateEdges()
    {
        List<int> triangles = new List<int>();
        for (int i = 0; i < resolution.x - 1; i++)
        {
            for (int j = 0; j < resolution.y - 1; j++)
            {
                int bottomLeftIndex = (i * (resolution.y)) + j;
                int bottomRightIndex = bottomLeftIndex + 1;
                int topLeftIndex = bottomLeftIndex + resolution.y;
                int topRightIndex = topLeftIndex + 1;

                // Add first triangle 
                triangles.Add(bottomLeftIndex);
                triangles.Add(bottomRightIndex);
                triangles.Add(topRightIndex);


                // Add second triangle 
                triangles.Add(bottomLeftIndex);
                triangles.Add(topRightIndex);
                triangles.Add(topLeftIndex);


            }
        }
        return triangles;
    }

}
