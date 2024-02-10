using UnityEngine;

public class MeshDrawer : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        // meshFilter.sharedMesh.Clear();
        if (Config.debugModeEnabled) Debug.Log("Creating mesh...");
        meshFilter.sharedMesh = meshData.CreateMesh();

        if (Config.debugModeEnabled) Debug.Log("Setting texture...");
        meshRenderer.sharedMaterial.mainTexture = texture;

        if (Config.debugModeEnabled) Debug.Log("Done Drawing Mesh");
    }
    public void DrawMesh(Mesh mesh, Texture2D texture)
    {
        // meshFilter.sharedMesh.Clear();
        if (Config.debugModeEnabled) Debug.Log("Creating mesh...");
        meshFilter.sharedMesh = mesh;

        if (Config.debugModeEnabled) Debug.Log("Setting texture...");
        meshRenderer.sharedMaterial.mainTexture = texture;

        if (Config.debugModeEnabled) Debug.Log("Done Drawing Mesh");
    }
}