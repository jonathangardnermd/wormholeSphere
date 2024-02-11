using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeshMaker : MonoBehaviour
{
    public bool autoUpdate = false;


    [Range(2, 30)]
    public int halfNumSides = 3; // the numSides in the polygon (e.g. 6 means the cross-section is hexagonal)

    [Range(10, 100)]
    public float sphereSizeFactor = 100;

    [Range(10, 500)]
    public float baseCylinderLength = 10;

    public void MakeMesh()
    {
        // Config.debugModeEnabled = debugModeEnabled;
        MakeIco();
    }
    public void ClearMesh()
    {
        Mesh meshData = new();
        MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
        var texture = GetTexture();
        drawer.DrawMesh(meshData, texture);
        Debug.Log("cleared");
    }
    public void MakeIco()
    {
        // var ico = new Icosahedron();
        var meshData = BuildWormholeSphereMesh();
        MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
        var texture = GetTexture();
        drawer.DrawMesh(meshData, texture);
        Debug.Log("done");
    }

    public Mesh BuildWormholeSphereMesh()
    {
        // Init();
        var ico = new Icosahedron(sphereSizeFactor);
        // var meshData = ico.BuildFunIco(halfNumSides * 2, sphereSizeFactor);
        List<WormholeTriangle> ts = new();
        for (int i = 0; i < Icosahedron.icoTriangleIdxs.Length; i++)
        {
            var triVertIdxs = Icosahedron.icoTriangleIdxs[i];
            Vector3[] triVerts = new Vector3[3];
            for (int j = 0; j < 3; j++)
            {
                triVerts[j] = ico.vertices[triVertIdxs[j]];
            }

            var splayLength = sphereSizeFactor / 10;
            var baseCylinderRadius = sphereSizeFactor / 100;
            var polyNumSides = halfNumSides * 2;

            var wt = new WormholeTriangle(ico.SideLength, triVerts, polyNumSides, baseCylinderLength, baseCylinderRadius, splayLength);
            wt.BuildMeshData();
            ts.Add(wt);
        }

        IEnumerable<MeshData> meshDataList = ts
            .SelectMany(wt => wt.GetMeshes());
        var mesh = MeshData.CreateMesh(meshDataList);
        return mesh;
    }

    private static Texture2D GetTexture()
    {
        Texture2D texture = new Texture2D(1, 1);
        return texture;
    }
}