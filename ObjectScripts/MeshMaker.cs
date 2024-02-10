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
        // TestTransform();
    }

    // public void DrawMesh(MeshData meshData)
    // {
    //     MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
    //     var texture = GetTexture();
    //     drawer.DrawMesh(meshData, texture);
    // }


    // public void TestTransform()
    // {
    //     float sqrt2 = Mathf.Sqrt(2);

    //     float d = sqrt2 * 5;
    //     Vector3[] startVs = {
    //         new Vector3(0,0,0),
    //         new Vector3(20,0,0),
    //         new Vector3(0,10,0)
    //     };

    //     float yCenter = -3;
    //     Vector3[] endVs = {
    //         new Vector3(0,yCenter,0),
    //         new Vector3(sqrt2*10,yCenter,sqrt2*10),
    //         new Vector3(-sqrt2*5,yCenter,sqrt2*5),
    //     };

    //     var t = new TriangleTransformer(startVs, endVs);

    //     var meshData = t.BuildMesh(new MeshData());
    //     DrawMesh(meshData);
    // }
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
        var ico = new Icosahedron();
        var meshData = ico.BuildFunIco(numSides);
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