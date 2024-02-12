using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

public class MeshMaker : MonoBehaviour
{
    public bool autoUpdate = false;


    [Range(2, 11)]
    public int halfNumSides = 3; // the numSides in the polygon (e.g. 6 means the cross-section is hexagonal)

    [Range(10, 500)]
    public float sphereSizeFactor = 100;

    [Range(10, 500)]
    public float baseCylinderLength = 10;

    public void MakeMesh()
    {
        MakeWormholeSphereMesh();
    }

    public void DrawMesh(Mesh mesh)
    {
        MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
        var texture = GetTexture();
        drawer.DrawMesh(mesh, texture);
        Debug.Log("cleared");
    }
    public void ClearMesh()
    {
        Mesh emptyMesh = new();
        DrawMesh(emptyMesh);
        Debug.Log("cleared");
    }
    public void MakeWormholeSphereMesh()
    {
        var mesh = BuildWormholeSphereMesh();
        DrawMesh(mesh);
        Debug.Log("done");
    }

    public Mesh BuildWormholeSphereMesh()
    {
        var ico = new Icosahedron(sphereSizeFactor);
        List<Task<MeshData[]>> tasks = new List<Task<MeshData[]>>();
        float wormholeTriangleVertexRadius = ico.SideLength / Mathf.Sqrt(3);
        var polyNumSides = halfNumSides * 2;
        var splayLength = sphereSizeFactor / 10;
        var baseCylinderRadius = sphereSizeFactor / 100;
        var wt = new WormholeTriangle(wormholeTriangleVertexRadius, polyNumSides, baseCylinderLength, baseCylinderRadius, splayLength);
        wt.BuildMeshData();
        var meshes = wt.GetMeshes();

        for (int i = 0; i < Icosahedron.icoTriangleIdxs.Length; i++)
        {
            var triVertIdxs = Icosahedron.icoTriangleIdxs[i];
            Vector3[] triVerts = new Vector3[3];
            for (int j = 0; j < 3; j++)
            {
                triVerts[j] = ico.vertices[triVertIdxs[j]];
            }

            int index = i; // Create a local copy of 'i'
            tasks.Add(Task.Run(() =>
            {
                Debug.Log("Building triangle " + index);
                MeshData[] clonedMeshDatas = new MeshData[meshes.Length];
                for (int j = 0; j < meshes.Length; j++)
                {
                    var tt = WormholeTriangle.CalcTransform(wormholeTriangleVertexRadius, triVerts);
                    clonedMeshDatas[j] = MeshData.Clone(meshes[j]);
                    clonedMeshDatas[j].vertices = tt.TransformVectors(clonedMeshDatas[j].vertices).ToList();
                }
                Debug.Log("Done building triangle " + index);
                return clonedMeshDatas;
            }));
        }

        Task.WaitAll(tasks.ToArray());
        List<MeshData> meshDataList = tasks.SelectMany(task => task.Result).ToList();
        var mesh = MeshData.CreateMesh(meshDataList);
        return mesh;
    }


    private static Texture2D GetTexture()
    {
        Texture2D texture = new Texture2D(1, 1);
        return texture;
    }
}