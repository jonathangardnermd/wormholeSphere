



using UnityEngine;
// using System.Linq;
// using System.Collections.Generic;

public class PolygonBounds
{
    public readonly float minX;
    public readonly float minY;
    public readonly float maxX;
    public readonly float maxY;

    public PolygonBounds(float minX, float minY, float maxX, float maxY)
    {
        this.minX = minX;
        this.minY = minY;
        this.maxX = maxX;
        this.maxY = maxY;
    }

    public float GetWidth()
    {
        return maxX - minX;
    }
    public float GetHeight()
    {
        return maxY - minY;
    }
}
public class PolygonBoxBorder
{
    public MeshData meshData;
    public Vector3[] polygonVertices;
    public PolygonBounds polygonBounds;

    public PolygonBoxBorder(Vector3[] polygonVertices)
    {
        this.polygonVertices = polygonVertices;
        polygonBounds = GetPolygonBounds(polygonVertices);
        meshData = new MeshData();
    }
    private static PolygonBounds GetPolygonBounds(Vector3[] polygonVertices)
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;

        // Find the minimum and maximum x and y coordinates
        foreach (Vector2 vertex in polygonVertices)
        {
            minX = Mathf.Min(minX, vertex.x);
            minY = Mathf.Min(minY, vertex.y);
            maxX = Mathf.Max(maxX, vertex.x);
            maxY = Mathf.Max(maxY, vertex.y);
        }

        return new PolygonBounds(minX, minY, maxX, maxY);
    }

    private static Vector2[] GetBoundingRect(PolygonBounds polyBounds)
    {
        var boundingSquareVertices = new Vector2[4];
        boundingSquareVertices[0] = new Vector2(polyBounds.maxX, polyBounds.maxY);
        boundingSquareVertices[1] = new Vector2(polyBounds.minX, polyBounds.maxY);
        boundingSquareVertices[2] = new Vector2(polyBounds.minX, polyBounds.minY);
        boundingSquareVertices[3] = new Vector2(polyBounds.maxX, polyBounds.minY);

        return boundingSquareVertices;
    }

    public void BuildMeshData()
    {
        var boundingVerts = GetBoundingRect(polygonBounds);

        var numSides = polygonVertices.Length;
        float quadIdxSize = numSides / 4f;
        for (int i1 = 0; i1 < numSides; i1++)
        {
            var i2 = (i1 + 1) % numSides;
            var v1 = polygonVertices[i1];
            var v2 = polygonVertices[i2];

            int quad1 = (int)Mathf.Floor(i1 / quadIdxSize);
            int quad2 = (int)Mathf.Floor(i2 / quadIdxSize);
            var isAmbiguous2 = (i2 % quadIdxSize) == 0;


            var meshDataStartCt = meshData.vertices.Count;
            if (isAmbiguous2 || quad1 == quad2)
            {
                var boundingVert = boundingVerts[quad1];
                meshData.AddVertex(v1);
                meshData.AddVertex(v2);
                meshData.AddVertex(boundingVert);
                meshData.AddTriangleIdxsReverseNormal(meshDataStartCt + 0, meshDataStartCt + 2, meshDataStartCt + 1); // note the ORDER!
            }
        }
    }
}
