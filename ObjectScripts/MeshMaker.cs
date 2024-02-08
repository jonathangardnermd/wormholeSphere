using System;
using System.Collections.Generic;
using UnityEngine;

public class MeshMaker : MonoBehaviour
{
    public bool debugModeEnabled = false;
    public bool autoUpdate = false;
    public bool addWormhole = true;

    [Range(2, 100)]
    public int numSides = 50; // the numSides in the polygon (e.g. 6 means the cross-section is hexagonal)

    [Min(0.01f)]
    public float length = 20; // the length of the polygonal cylinder

    [Min(0.01f)]
    public float polygonVertexRadius = 1; // the radius of the polygonal cylinder

    [Range(0, 40)]
    public int numSplaySubdivisions = 20; // this controls how well the splay approximates a parabolic curve

    [Min(1f)]
    public float totChangeInU = 10; // the size of the splay in the wormhole


    public void MakeMesh()
    {
        Config.debugModeEnabled = debugModeEnabled;
        numSides *= 2;
        // MakeCylinderMesh();
        MakePolygonMesh();
    }

    public void MakePolygonMesh()
    {
        Polygon p = new Polygon(numSides);
        var meshData = p.GetMeshData();
        p.GetBoundingSquareMeshWithHole(meshData);
        MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
        var texture = GetTexture();
        drawer.DrawMesh(meshData, texture);
    }
    // public void MakeClippedMesh()
    // {
    //     var triangleVertices = ClipperDemo.GetVerticesForTriangle(100);
    //     var polyVertices = ClipperDemo.GetVerticesForPolygon(6);

    //     var rslt = PolygonClipper.ClipPolygon(triangleVertices, polyVertices);
    //     var clippedVerts = rslt.Item1;
    //     var clippedTriangles = rslt.Item2;

    //     Debug.Log($"VERTEX COUNT {clippedVerts.Count}");
    //     Debug.Log($"TRIANGLE COUNT {clippedTriangles.Count}");
    //     for (int i = 0; i < clippedVerts.Count; i++)
    //     {
    //         Debug.Log($"{clippedVerts[i].x}, {clippedVerts[i].y}");
    //     }
    //     for (int i = 0; i < clippedTriangles.Count / 3; i++)
    //     {
    //         Debug.Log($"{clippedTriangles[i]}, {clippedTriangles[i + 1]}, {clippedTriangles[i + 2]}");
    //     }

    //     List<Vector3> clippedVerts3D = clippedVerts.ConvertAll(v => new Vector3(v.x, v.y, 0));

    //     Mesh mesh = new Mesh
    //     {
    //         vertices = clippedVerts.ConvertAll(v => new Vector3(v.x, v.y, 0)).ToArray(),
    //         triangles = clippedTriangles.ToArray(),
    //     };
    //     mesh.RecalculateNormals();
    //     MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
    //     var texture = GetTexture();
    //     drawer.DrawRawMesh(mesh, texture);
    // }
    public void MakeCylinderMesh()
    {
        // TODO: the length used to calculate the uvs of the cylinder and the splay should both be length+totChangeInU since they share the same material/texture
        if (Config.debugModeEnabled) Debug.Log("GenerateMesh invoked...");

        if (Config.debugModeEnabled) Debug.Log("Getting texture...");
        var texture = GetTexture();
        var meshData = new MeshData();

        if (Config.debugModeEnabled) Debug.Log("Getting mesh...");
        var pc = new PolygonCylinder(numSides, length, polygonVertexRadius);
        pc.AddPolygonCylinderToMesh(meshData);

        if (addWormhole)
        {
            if (Config.debugModeEnabled) Debug.Log("Getting SPLAY data...");
            var splayData = new SplayData(numSplaySubdivisions, totChangeInU);
            if (Config.debugModeEnabled) splayData.SaveToCSV($"{Config.debugFilePath}/splayData.csv");

            if (Config.debugModeEnabled) Debug.Log("Adding SPLAY mesh...");
            var pcs = new PolygonalCylinderSplay(pc.polygon, pc.polygonVertexRadius, splayData);
            pcs.AddPolygonalCylinderSplayToMesh(meshData);
        }

        if (Config.debugModeEnabled) Debug.Log("Drawing mesh...");
        MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
        drawer.DrawMesh(meshData, texture);

        Debug.Log("Mesh generation complete"); // always prints
    }

    private static Texture2D GetTexture()
    {
        Texture2D texture = new Texture2D(1, 1);
        return texture;
    }
}