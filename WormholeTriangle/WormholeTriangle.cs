


using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;

// using System.Numerics;

public class WormholeTriangle
{
    // public Polygon polygon;
    public Vector2[] triangleVertices;
    public float sideLength;
    public float vertexRadius;
    public static float sqrt3 = Mathf.Sqrt(3);

    public static Vector2 center = new Vector2(0, 0);

    public WormholeTriangle(float vertexRadius)
    {
        this.vertexRadius = vertexRadius;
        var polygon = new Polygon(3);
        triangleVertices = polygon.GetVertices(vertexRadius);
        var tStr = polygon.GetMeshData(vertexRadius).TrianglesToString();
        Debug.Log($"tStr={tStr}");
        sideLength = sqrt3 * vertexRadius;
    }

    public MeshData GetMeshWithRectangularHole(MeshData meshData, float height, float width)
    {
        var slope = 1 / sqrt3;

        var halfWidth = width / 2;
        var halfHeight = height / 2;
        var deltaX1 = vertexRadius - halfWidth;
        // var deltaX1 = vertexRadius;
        var deltaX2 = vertexRadius + halfWidth;
        // var deltaX2 = vertexRadius;
        var deltaY1 = slope * deltaX1;
        var deltaY2 = slope * deltaX2;

        // Vector2[] extraVs = new Vector2[4];
        // extraVs[0] = new Vector2(halfWidth, deltaY1);
        // extraVs[1] = new Vector2(halfWidth, -deltaY1);
        // extraVs[2] = new Vector2(-halfWidth, deltaY2);
        // extraVs[3] = new Vector2(-halfWidth, -deltaY2);

        // var meshData = new MeshData();
        var uv = new Vector2(0, 0);

        List<int> newIdxs = new();
        var idx = meshData.AddVertex(triangleVertices[0], uv); // 0
        newIdxs.Add(idx);
        idx = meshData.AddVertex(triangleVertices[1], uv); // 1
        newIdxs.Add(idx);
        idx = meshData.AddVertex(triangleVertices[2], uv);// 2
        newIdxs.Add(idx);
        idx = meshData.AddVertex(new Vector2(halfWidth, deltaY1), uv); // 3
        newIdxs.Add(idx);
        idx = meshData.AddVertex(new Vector2(halfWidth, -deltaY1), uv); // 4
        newIdxs.Add(idx);
        idx = meshData.AddVertex(new Vector2(-halfWidth, deltaY2), uv); // 5
        newIdxs.Add(idx);
        idx = meshData.AddVertex(new Vector2(-halfWidth, -deltaY2), uv); // 6
        newIdxs.Add(idx);

        idx = meshData.AddVertex(new Vector2(halfWidth, halfHeight), uv); // 7
        newIdxs.Add(idx);
        idx = meshData.AddVertex(new Vector2(-halfWidth, halfHeight), uv); // 8
        newIdxs.Add(idx);
        idx = meshData.AddVertex(new Vector2(-halfWidth, -halfHeight), uv); // 9
        newIdxs.Add(idx);
        idx = meshData.AddVertex(new Vector2(halfWidth, -halfHeight), uv); // 10
        newIdxs.Add(idx);

        meshData.AddTriangleIdxs(newIdxs[0], newIdxs[3], newIdxs[4]);
        meshData.AddTriangleIdxs(newIdxs[7], newIdxs[3], newIdxs[5]);
        meshData.AddTriangleIdxs(newIdxs[5], newIdxs[8], newIdxs[7]);
        meshData.AddTriangleIdxs(newIdxs[10], newIdxs[6], newIdxs[4]);
        meshData.AddTriangleIdxs(newIdxs[10], newIdxs[9], newIdxs[6]);
        meshData.AddTriangleIdxs(newIdxs[6], newIdxs[5], newIdxs[1]);
        meshData.AddTriangleIdxs(newIdxs[1], newIdxs[2], newIdxs[6]);
        meshData.AddTriangleIdxs(newIdxs[0], newIdxs[3], newIdxs[4]);
        return meshData;
    }


    // public void CalcCenter()
    // {
    //     Vector3 sum = Vector3.zero;
    //     foreach (Vector3 vertex in triangleVertices)
    //     {
    //         sum += vertex;
    //     }
    //     center = new Vector3[] { sum / 3 };
    // }

    // public void CalcUnitNormal()
    // {
    //     // Get two edges of the triangle
    //     Vector3 edge1 = vertices[1] - vertices[0];
    //     Vector3 edge2 = vertices[2] - vertices[0];

    //     // Calculate the cross product of the edges to get the normal
    //     Vector3 normal = Vector3.Cross(edge1, edge2).normalized;

    //     // Assign the calculated normal to the unitNormal variable
    //     unitNormal = normal;
    // }
}