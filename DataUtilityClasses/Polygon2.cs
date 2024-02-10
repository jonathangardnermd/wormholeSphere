using UnityEngine;
using System.Linq;
using System.Collections.Generic;
// using Math;
public class Polygon2
{
    public readonly int numSides;
    private Vector3[] vertices;
    private float[] angularUvs;

    private float angle;

    // public float vertexRadius;

    public Polygon2(int numSides)
    {
        this.numSides = numSides;
        SetUnitVertices();
    }

    public Vector3[] GetVertices(float vertexRadius)
    {
        return vertices.Select(v => v * vertexRadius).ToArray();
    }

    private void SetUnitVertices() // vertices will be exactly 1 unit from the origin (by rotating the point <x=1,y=0> around the origin)
    {
        angle = 2 * Mathf.PI / numSides;
        vertices = new Vector3[numSides];
        angularUvs = new float[numSides];

        for (int i = 0; i < numSides; i++)
        {
            // rotate the pt <1,0,0> by the rotationAngle = i * angle to form the ith vertex of the unit polygon
            float x = Mathf.Cos(i * angle);
            float y = Mathf.Sin(i * angle);
            vertices[i] = new Vector3(x, y, 0);
            angularUvs[i] = (float)i / numSides;
        }
    }

    // public MeshData GetMeshData(float vertexRadius)
    // {
    //     var meshData = new MeshData();

    //     var originVertex = new Vector3(0, 0, 0);
    //     List<int> vIdxs = new();
    //     meshData.AddVertex(originVertex, vIdxs);
    //     var sizedVertices = GetVertices(vertexRadius);
    //     for (int i1 = 0; i1 < numSides; i1++)
    //     {
    //         var i2 = (i1 + 1) % numSides;
    //         var v1 = sizedVertices[i1];
    //         var v2 = sizedVertices[i2];

    //         meshData.AddVertex(v1, vIdxs);
    //         meshData.AddVertex(v2, vIdxs);
    //         meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);
    //     }
    //     return meshData;
    // }

    // public Vector2[] GetBoundingSquare()
    // {
    //     float minX = float.MaxValue;
    //     float minY = float.MaxValue;
    //     float maxX = float.MinValue;
    //     float maxY = float.MinValue;

    //     // Find the minimum and maximum x and y coordinates
    //     foreach (Vector2 vertex in this.vertices)
    //     {
    //         minX = Mathf.Min(minX, vertex.x);
    //         minY = Mathf.Min(minY, vertex.y);
    //         maxX = Mathf.Max(maxX, vertex.x);
    //         maxY = Mathf.Max(maxY, vertex.y);
    //     }

    //     // Calculate the other three vertices of the bounding square
    //     var boundingSquareVertices = new Vector2[4];
    //     boundingSquareVertices[0] = new Vector2(maxX, maxY);
    //     boundingSquareVertices[1] = new Vector2(minX, maxY);
    //     boundingSquareVertices[2] = new Vector2(minX, minY);
    //     boundingSquareVertices[3] = new Vector2(maxX, minY);

    //     // Do something with boundingSquareVertices...
    //     return boundingSquareVertices;
    // }

    // public MeshData GetBoundingSquareMeshWithHole(MeshData meshData)
    // {
    //     var boundingVerts = GetBoundingSquare();

    //     float quadIdxSize = numSides / 4f;
    //     for (int i1 = 0; i1 < numSides; i1++)
    //     {
    //         var i2 = (i1 + 1) % numSides;
    //         var v1 = vertices[i1];
    //         var v2 = vertices[i2];

    //         int quad1 = (int)Mathf.Floor(i1 / quadIdxSize);
    //         int quad2 = (int)Mathf.Floor(i2 / quadIdxSize);
    //         var isAmbiguous2 = (i2 % quadIdxSize) == 0;
    //         if (isAmbiguous2 || quad1 == quad2)
    //         {
    //             var boundingVert = boundingVerts[quad1];
    //             int vIdx1 = meshData.AddVertex(v1, new Vector2(0, 0));
    //             int vIdx2 = meshData.AddVertex(v2, new Vector2(0, 0));
    //             int vIdx3 = meshData.AddVertex(boundingVert, new Vector2(0, 0));
    //             meshData.AddTriangleIdxs(vIdx1, vIdx2, vIdx3);
    //         }
    //     }
    //     return meshData;
    // }
}