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
        // MakeWormholeSphereMesh();
        // MakePolygonBorderBox();
        MakeWormholeTriangle();
    }

    public void DrawMesh(Mesh mesh)
    {
        MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
        var texture = GetTexture();
        drawer.DrawMesh(mesh, texture);
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

    public void MakePolygonBorderBox()
    {
        // calc vertices/triangles for polygon box border around the polygonal hole
        var poly = new Polygon(halfNumSides * 2);
        var pb = new PolygonBoxBorder(poly.GetVertices(10));
        pb.BuildMeshData();
        var mesh = MeshData.CreateMesh(new[] { pb.meshData });
        DrawMesh(mesh);
        // var b = pb.polygonBounds;
    }
    public void MakeWormholeTriangle()
    {
        // calc vertices/triangles for polygon box border around the polygonal hole
        var wt = new WormholeTriangle(40, 6, 10, 1, 10);
        wt.BuildMeshData();
        var meshes = wt.GetMeshes();
        var mesh = MeshData.CreateMesh(meshes);
        DrawMesh(mesh);
        // var b = pb.polygonBounds;
    }
    public Mesh BuildWormholeSphereMesh()
    {
        // replaces the triangles in an icosahedron with "WormholeTriangles" 
        // that contain a womrhole-like structure in the middle of each triangle

        // create an icosahedron
        var ico = new Icosahedron(sphereSizeFactor);

        // Build a single wormhole triangle centered at the origin and unrotated.
        // For each triangle of the icosahedron, we will clone this wormhole triangle 
        // and transform it (rotate and translate it) to its proper place on the icosahedron.
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
            // get the vertices of each triangle in the icosahedron
            var triVertIdxs = Icosahedron.icoTriangleIdxs[i];
            Vector3[] triVerts = new Vector3[3];
            for (int j = 0; j < 3; j++)
            {
                triVerts[j] = ico.vertices[triVertIdxs[j]];
            }

            int index = i; // Create a local copy of 'i' so the debug statements print the correct index (necessary due to multi-threading)

            // clone and transform the original wormhole triangle 
            // in a separate thread for each triangle in the icosahedron
            tasks.Add(Task.Run(() =>
            {
                Debug.Log("Building triangle " + index);

                // we need to clone the meshData for the separate objects that make up the wormhole triangle (cylinder, splay, polygonBoxBorder, and the rest of the triangle)
                MeshData[] clonedMeshDatas = new MeshData[meshes.Length];
                for (int j = 0; j < meshes.Length; j++)
                {
                    // calculate the transform to rotate and translate the wormhole triangle to its proper place on the icosahedron
                    var tt = WormholeTriangle.CalcTransform(wormholeTriangleVertexRadius, triVerts);

                    // clone and transform
                    clonedMeshDatas[j] = MeshData.Clone(meshes[j]);
                    clonedMeshDatas[j].vertices = tt.TransformVectors(clonedMeshDatas[j].vertices).ToList();
                }
                Debug.Log("Done building triangle " + index);
                return clonedMeshDatas;
            }));
        }

        // wait for all the threads to complete
        Task.WaitAll(tasks.ToArray());

        // combine the meshDatas from all the "wormhole triangles" into a single mesh
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