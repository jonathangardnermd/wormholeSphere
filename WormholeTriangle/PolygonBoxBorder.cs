



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
        // find the vertices of the rectangle that bounds the polygon
        var boundingSquareVertices = new Vector2[4];
        boundingSquareVertices[0] = new Vector2(polyBounds.maxX, polyBounds.maxY);
        boundingSquareVertices[1] = new Vector2(polyBounds.minX, polyBounds.maxY);
        boundingSquareVertices[2] = new Vector2(polyBounds.minX, polyBounds.minY);
        boundingSquareVertices[3] = new Vector2(polyBounds.maxX, polyBounds.minY);

        return boundingSquareVertices;
    }

    public void BuildMeshData()
    {
        // build the mesh data for a rectangular border around a polygonal hole
        // this allows us to build a triangular mesh with a rectangular hole in the middle later on,
        // which is far easier than building a triangular mesh with a polygonal hole in the middle.
        // That is, we turn a polygonal hole into a rectangular hole by building a PolygonBoxBorder around the polygonal hole.
        var boundingVerts = GetBoundingRect(polygonBounds);

        var numSides = polygonVertices.Length;
        float quadIdxSize = numSides / 4f; // the number of vertices in each quadrant of the polygon

        // loop through each vertex of the polygon and draw triangles to connect it to the next vertex and the corners of the bounding rectangle
        for (int i1 = 0; i1 < numSides; i1++)
        {
            var i2 = (i1 + 1) % numSides; // the "next" idx after i1
            var v1 = polygonVertices[i1];
            var v2 = polygonVertices[i2];

            int quad1 = (int)Mathf.Floor(i1 / quadIdxSize); // calculate the quadrant i1 belongs to
            int quad2 = (int)Mathf.Floor(i2 / quadIdxSize); // calculate the quadrant i2 belongs to
            var isAmbiguous2 = (i2 % quadIdxSize) == 0; // if isAmbiguous2==true, i2 could belong to either quadrant because it's right at the intersection

            if (isAmbiguous2 || quad1 == quad2)
            {
                var meshDataStartCt = meshData.vertices.Count;
                var boundingVert = boundingVerts[quad1];

                // draw a triangle between v1, v2, and the corners of the bounding rectangle
                meshData.AddVertex(v1);
                meshData.AddVertex(v2);
                meshData.AddVertex(boundingVert);
                meshData.AddTriangleIdxs(meshDataStartCt + 0, meshDataStartCt + 1, meshDataStartCt + 2);
            }
        }
    }
}
