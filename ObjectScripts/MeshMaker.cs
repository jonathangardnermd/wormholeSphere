using System;
using System.Collections.Generic;
using UnityEngine;

public class MeshMaker : MonoBehaviour
{
    public bool debugModeEnabled = false;
    public bool autoUpdate = false;
    public bool addWormhole = true;

    [Range(2, 10)]
    public int numSides = 50; // the numSides in the polygon (e.g. 6 means the cross-section is hexagonal)

    // [Min(0.01f)]
    // public float length = 20; // the length of the polygonal cylinder

    public void MakeMesh()
    {
        Config.debugModeEnabled = debugModeEnabled;
        // MakeTriangleWithPolygonHole();
        MakeIco();
    }

    public void MakeTriangleWithPolygonHole()
    {
        float polySize = 1f;
        Polygon p = new Polygon(numSides * 2);
        var meshData = new MeshData();
        var b = PolygonBoxBorder.AddMeshData(meshData, p.GetVertices(polySize));

        var wt = new EquilateralTriangleWithRectHole(polySize * 10, b.GetHeight(), b.GetWidth());
        wt.AddMeshData(meshData);

        var origN = new Vector3(0, 0, 1);
        var newN = new Vector3(1, 1, 1);
        var t = new Transformer(origN, newN, new Vector3(100, 100, 100));
        t.TransformMeshData(meshData, 0);

        MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
        var texture = GetTexture();
        drawer.DrawMesh(meshData, texture);
    }

    public void MakeIco()
    {
        var meshData = Icosahedron.BuildFunIco();
        MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
        var texture = GetTexture();
        drawer.DrawMesh(meshData, texture);
        Debug.Log("done");
    }

    private static Texture2D GetTexture()
    {
        Texture2D texture = new Texture2D(1, 1);
        return texture;
    }
}