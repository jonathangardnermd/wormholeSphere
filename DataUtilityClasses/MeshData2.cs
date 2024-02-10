using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
public class MeshData2
{
    public List<Vector3> vertices;
    public List<int> triangleIdxs;

    // public List<MeshDataComponent> components;

    public MeshData2()
    {
        vertices = new List<Vector3>();
        triangleIdxs = new();
        // components = new();
    }

    public void AddVertex(Vector3 v)
    {
        // idxs.Add(vertices.Count);
        vertices.Add(v);
    }

    public void AddTriangleIdxs(int i1, int i2, int i3)
    {
        triangleIdxs.Add(i1);
        triangleIdxs.Add(i2);
        triangleIdxs.Add(i3);
    }

    // public Triangle[] Triangles
    // {
    //     get
    //     {
    //         int triangleCount = triangleIdxs.Count / 3;
    //         Triangle[] triangleArray = new Triangle[triangleCount];
    //         for (int i = 0; i < triangleCount; i++)
    //         {
    //             int idx = i * 3;
    //             triangleArray[i] = new Triangle(vertices[triangleIdxs[idx]], vertices[triangleIdxs[idx + 1]], vertices[triangleIdxs[idx + 2]]);
    //         }
    //         return triangleArray;
    //     }
    // }

    // public string TrianglesToString()
    // {
    //     string result = "";
    //     foreach (Triangle t in Triangles)
    //     {
    //         result += t.ToString() + "\n";
    //     }
    //     return result;
    // }
    // public static Mesh CreateMesh(params MeshData2[] meshDatas)
    // {
    //     return CreateMesh(meshDatas);
    // }

    public static Mesh CreateMesh(IEnumerable<MeshData2> meshDatas)
    {
        int totNumVs = meshDatas.Sum(meshData => meshData.vertices.Count);
        int totNumTidxs = meshDatas.Sum(meshData => meshData.triangleIdxs.Count);

        Vector3[] vertices = new Vector3[totNumVs];
        int[] tIdxs = new int[totNumTidxs];

        int vertIndex = 0;
        int tIdxIndex = 0;

        foreach (var meshData in meshDatas)
        {
            for (int i = 0; i < meshData.vertices.Count; i++)
            {
                vertices[vertIndex + i] = meshData.vertices[i];
            }
            for (int i = 0; i < meshData.triangleIdxs.Count; i++)
            {
                tIdxs[tIdxIndex + i] = meshData.triangleIdxs[i] + vertIndex;
            }

            // Update indices
            vertIndex += meshData.vertices.Count;
            tIdxIndex += meshData.triangleIdxs.Count;
        }
        Mesh mesh = new Mesh
        {
            vertices = vertices,
            triangles = tIdxs,
        };
        mesh.RecalculateNormals();
        return mesh;
    }
}