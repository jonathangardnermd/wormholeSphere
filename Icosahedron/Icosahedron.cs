using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Icosahedron
{
    public static readonly float SIZE_FACTOR = 30;
    public static float SIDE_LENGTH = 0;

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

    public static List<Vector3> vertices = new List<Vector3>()
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
    // private List<TriangleIdxs> triangles;
    // private List<Vector3> vertices;

    // public List<TriangleIdxs> TriangleIdxs { get => triangles; private set => triangles = value; }
    // public List<Vector3> Vertices { get => vertices; private set => vertices = value; }

    // public Icosahedron(float sizeFactor)
    // {

    // }

    public static void Init()
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

    public static MeshData BuildFunIco()
    {
        Init();
        MeshData meshData = new MeshData();

        for (int i = 0; i < icoTriangleIdxs.Length; i++)
        {
            var triVertIdxs = icoTriangleIdxs[i];
            Vector3[] triVerts = new Vector3[3];
            for (int j = 0; j < 3; j++)
            {
                triVerts[j] = vertices[triVertIdxs[j]];
            }
            WormholeTriangle.AddTriangleWithPolygonHoleToMesh2(meshData, SIDE_LENGTH, triVerts, 6, 1f);
        }
        return meshData;
    }
    // public static MeshData BuildFunIco()
    // {
    //     // build and use TriangleTransformer
    //     // make sure sideLength of ending triangles is the same as beginning!
    //     Init();
    //     MeshData meshData = new MeshData();

    //     for (int i = 0; i < icoTriangleIdxs.Length; i++)
    //     {
    //         var triVertIdxs = icoTriangleIdxs[i];
    //         Vector3[] triVerts = new Vector3[3];
    //         for (int j = 0; j < 3; j++)
    //         {
    //             triVerts[j] = vertices[triVertIdxs[j]];
    //         }
    //         WormholeTriangle.AddTriangleWithPolygonHoleToMesh2(meshData, SIDE_LENGTH, triVerts, 6, 1f);
    //     }
    //     return meshData;
    // }
}