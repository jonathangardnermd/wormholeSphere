using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using Stopwatch = System.Diagnostics.Stopwatch;

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
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var ico = new Icosahedron(sphereSizeFactor);
        List<Task<WormholeTriangle>> tasks = new List<Task<WormholeTriangle>>();

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

            int index = i; // Create a local copy of 'i'
            tasks.Add(Task.Run(() =>
            {
                Debug.Log("Building triangle " + index);
                var wt = new WormholeTriangle(ico.SideLength, triVerts, polyNumSides, baseCylinderLength, baseCylinderRadius, splayLength);
                wt.BuildMeshData();
                Debug.Log("Done building triangle " + index);
                return wt;
            }));
        }

        Task.WaitAll(tasks.ToArray());
        List<WormholeTriangle> ts = tasks.Select(task => task.Result).ToList();

        IEnumerable<MeshData> meshDataList = ts.SelectMany(wt => wt.GetMeshes());
        var mesh = MeshData.CreateMesh(meshDataList);

        stopwatch.Stop();
        Debug.Log("Time taken to build wormhole sphere: " + stopwatch.ElapsedMilliseconds + " milliseconds");

        return mesh;
    }


    private static Texture2D GetTexture()
    {
        Texture2D texture = new Texture2D(1, 1);
        return texture;
    }
}