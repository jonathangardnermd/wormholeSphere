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