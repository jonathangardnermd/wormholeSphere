using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Icosahedron
{
    private float sideLength;
    public MeshData meshData;

    public static int[][] icoTriangleIdxs = new int[][]
                {
            new int[] { 0, 11, 5 },
            new int[] { 0, 5, 1 },
            new int[] { 0, 1, 7 },
            new int[] { 0, 7, 10 },
            new int[] { 0, 10, 11 },
            new int[] { 1, 5, 9 },
            new int[] { 5, 11, 4 },
            new int[] { 11, 10, 2 },
            new int[] { 10, 7, 6 },
            new int[] { 7, 1, 8 },
            new int[] { 3, 9, 4 },
            new int[] { 3, 4, 2 },
            new int[] { 3, 2, 6 },
            new int[] { 3, 6, 8 },
            new int[] { 3, 8, 9 },
            new int[] { 4, 9, 5 },
            new int[] { 2, 4, 11 },
            new int[] { 6, 2, 10 },
            new int[] { 9, 8, 1 },
            new int[] { 8, 6, 7 },
                };

    private static readonly float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

    public List<Vector3> vertices = new List<Vector3>()
    {
        new Vector3(-1, t, 0).normalized,
        new Vector3(1, t, 0).normalized,
        new Vector3(-1, -t, 0).normalized,
        new Vector3(1, -t, 0).normalized,
        new Vector3(0, -1, t).normalized,
        new Vector3(0, 1, t).normalized,
        new Vector3(0, -1, -t).normalized,
        new Vector3(0, 1, -t).normalized,
        new Vector3(t, 0, -1).normalized,
        new Vector3(t, 0, 1).normalized,
        new Vector3(-t, 0, -1).normalized,
        new Vector3(-t, 0, 1).normalized
    };

    public float SideLength { get => sideLength; set => sideLength = value; }

    public Icosahedron(float sizeFactor)
    {
        Init(sizeFactor);
        meshData = new();
    }

    private void Init(float sizeFactor)
    {
        // scale all the vertices by the sizeFactor
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] *= sizeFactor;
        }

        // Take the first triangle and calculate the side length of the first edge.
        // This side length should be the same for all edges in the symmetric icosahedron
        var tIdxs = icoTriangleIdxs[0];
        var v1 = vertices[tIdxs[0]];
        var v2 = vertices[tIdxs[1]];
        sideLength = Vector3.Distance(v1, v2);
    }

    public void BuildMeshData()
    {
        for (int i = 0; i < icoTriangleIdxs.Length; i++)
        {
            var triVertIdxs = icoTriangleIdxs[i];
            var v1 = vertices[triVertIdxs[0]];
            var v2 = vertices[triVertIdxs[1]];
            var v3 = vertices[triVertIdxs[2]];

            meshData.AddVertex(new Vector3(v1.x, v1.y, v1.z));
            meshData.AddVertex(new Vector3(v2.x, v2.y, v2.z));
            meshData.AddVertex(new Vector3(v3.x, v3.y, v3.z));
            meshData.AddTriangleIdxs(i * 3, i * 3 + 1, i * 3 + 2);
            // meshData.AddVertex(vertices[i]);
        }
        // meshData.vertices = new List<Vector3>(vertices);
        // meshData.triangleIdxs = icoTriangleIdxs.SelectMany(subArray => subArray).ToList();
    }
}