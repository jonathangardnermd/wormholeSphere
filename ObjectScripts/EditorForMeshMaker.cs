using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshMaker))]
public class EditorForMeshMaker : Editor
{
    public override void OnInspectorGUI()
    {
        MeshMaker maker = (MeshMaker)target;

        if (DrawDefaultInspector())
        {
            // if (maker.autoUpdate)
            // {
            //     maker.MakeMesh("");
            // }
        }

        if (GUILayout.Button("Generate Icosahedron"))
        {
            maker.MakeMesh("ico");
        }
        if (GUILayout.Button("Generate WormholeTriangle"))
        {
            maker.MakeMesh("tri");
        }
        if (GUILayout.Button("Generate PolyBoxBorder"))
        {
            maker.MakeMesh("border");
        }
        if (GUILayout.Button("Clear Mesh"))
        {
            maker.ClearMesh();
        }
    }
}