using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Icosahedron
{
    public static readonly float SIZE_FACTOR = 30;
    public float SIDE_LENGTH = 0;

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

    public void Init()
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            vertices[i] *= SIZE_FACTOR;
        }
        // set side length
        var tIdxs = icoTriangleIdxs[0];
        var v1 = vertices[tIdxs[0]];
        var v2 = vertices[tIdxs[1]];
        SIDE_LENGTH = Vector3.Distance(v1, v2);
    }

    public Mesh BuildFunIco(int numSides)
    {
        Init();
        List<WormholeTriangle> ts = new();
        for (int i = 0; i < icoTriangleIdxs.Length; i++)
        {
            var triVertIdxs = icoTriangleIdxs[i];
            Vector3[] triVerts = new Vector3[3];
            for (int j = 0; j < 3; j++)
            {
                triVerts[j] = vertices[triVertIdxs[j]];
            }
            var wt = new WormholeTriangle(SIDE_LENGTH, triVerts, numSides * 2, 1f);
            wt.BuildMeshData();
            ts.Add(wt);
        }

        IEnumerable<MeshData> meshDataList = ts
            .SelectMany(wt => new[] { wt.pb.meshData, wt.triangle.meshData });
        var mesh = MeshData.CreateMesh(meshDataList);
        return mesh;
    }
}