using UnityEngine;
using System.Collections.Generic;
using System.Linq;
// using System;

public class MeshData
{
    public List<Vector3> vertices;
    public List<int> triangleIdxs;

    public MeshData()
    {
        vertices = new List<Vector3>();
        triangleIdxs = new();
    }

    public static MeshData Clone(MeshData meshDataToClone)
    {
        MeshData meshData = new MeshData();
        foreach (var vertex in meshDataToClone.vertices)
        {
            meshData.vertices.Add(new Vector3(vertex.x, vertex.y, vertex.z));
        }
        meshData.triangleIdxs = new List<int>(meshDataToClone.triangleIdxs);
        return meshData;
    }

    public void AddVertex(Vector3 v)
    {
        vertices.Add(v);
    }

    public void AddTriangleIdxsReverseNormal(int i1, int i2, int i3)
    {
        triangleIdxs.Add(i2);
        triangleIdxs.Add(i1);
        triangleIdxs.Add(i3);
    }

    public static Mesh CreateMesh(IEnumerable<MeshData> meshDatas)
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