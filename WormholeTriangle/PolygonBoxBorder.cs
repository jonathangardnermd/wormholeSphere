



using UnityEngine;
using System.Linq;
using System.Collections.Generic;
// using Math;
public class PolygonBounds
{
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;

    public PolygonBounds(float minX, float minY, float maxX, float maxY)
    {
        this.minX = minX;
        this.minY = minY;
        this.maxX = maxX;
        this.maxY = maxY;
    }

    public float GetWidth()
    {
        return this.maxX - this.minX;
    }
    public float GetHeight()
    {
        return this.maxY - this.minY;
    }
}
public static class PolygonBoxBorder
{
    private static PolygonBounds GetPolygonBounds(Vector2[] polygonVertices)
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
    private static Vector2[] GetBoundingSquare(PolygonBounds polyBounds)
    {
        // Calculate the other three vertices of the bounding square
        var boundingSquareVertices = new Vector2[4];
        boundingSquareVertices[0] = new Vector2(polyBounds.maxX, polyBounds.maxY);
        boundingSquareVertices[1] = new Vector2(polyBounds.minX, polyBounds.maxY);
        boundingSquareVertices[2] = new Vector2(polyBounds.minX, polyBounds.minY);
        boundingSquareVertices[3] = new Vector2(polyBounds.maxX, polyBounds.minY);

        // Do something with boundingSquareVertices...
        return boundingSquareVertices;
    }

    public static PolygonBounds AddMeshData(MeshData meshData, Vector2[] polygonVertices)
    {
        var polyBounds = GetPolygonBounds(polygonVertices);
        var boundingVerts = GetBoundingSquare(polyBounds);

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
            if (isAmbiguous2 || quad1 == quad2)
            {
                var vIdxs = new List<int>();
                var boundingVert = boundingVerts[quad1];
                meshData.AddVertex(v1, vIdxs);
                meshData.AddVertex(v2, vIdxs);
                meshData.AddVertex(boundingVert, vIdxs);
                meshData.AddTriangleIdxs(vIdxs[0], vIdxs[2], vIdxs[1]); // note the ORDER!
            }
        }
        return polyBounds;
    }
}
