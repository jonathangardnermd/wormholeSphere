
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
// using Math;
public class EquilateralTriangleWithRectHole
{
    public static float slope = 1 / Mathf.Sqrt(3); // slope of the outer equilateral triangle from the first vertex (at <r,0>) to the second vertex (rotated 120 degrees counterclockwise from <r,0>)
    public float vertexRadius;
    public float halfRectHeight;
    public float halfRectWidth;
    public Vector2[] triangleVertices;
    public MeshData2 meshData;

    public EquilateralTriangleWithRectHole(float vertexRadius, float rectHeight, float rectWidth)
    {
        meshData = new();
        this.vertexRadius = vertexRadius;
        this.halfRectHeight = rectHeight / 2f;
        this.halfRectWidth = rectWidth / 2f;

        var polygon = new Polygon(3);
        triangleVertices = polygon.GetVertices(vertexRadius);
    }

    public void BuildMeshData()
    {
        var deltaX1 = vertexRadius - halfRectWidth; // change in x from the (r,0) triangle vertex to the right side of rect hole
        var deltaX2 = vertexRadius + halfRectWidth; // ... to the LEFT side of rect hole
        var deltaY1 = slope * deltaX1; // change in y from (r,0) to the right side of the rect hole
        var deltaY2 = slope * deltaX2; // ... to the LEFT side of the rect hole

        // List<int> newIdxs = new();
        meshData.AddVertex(triangleVertices[0]); // 0
        meshData.AddVertex(triangleVertices[1]); // 1
        meshData.AddVertex(triangleVertices[2]);// 2
        meshData.AddVertex(new Vector2(halfRectWidth, deltaY1)); // 3
        meshData.AddVertex(new Vector2(halfRectWidth, -deltaY1)); // 4
        meshData.AddVertex(new Vector2(-halfRectWidth, deltaY2)); // 5
        meshData.AddVertex(new Vector2(-halfRectWidth, -deltaY2)); // 6
        meshData.AddVertex(new Vector2(halfRectWidth, halfRectHeight)); // 7
        meshData.AddVertex(new Vector2(-halfRectWidth, halfRectHeight)); // 8
        meshData.AddVertex(new Vector2(-halfRectWidth, -halfRectHeight)); // 9
        meshData.AddVertex(new Vector2(halfRectWidth, -halfRectHeight)); // 10

        meshData.AddTriangleIdxs(0, 3, 4);
        meshData.AddTriangleIdxs(7, 3, 5);
        meshData.AddTriangleIdxs(5, 8, 7);
        meshData.AddTriangleIdxs(10, 6, 4);
        meshData.AddTriangleIdxs(10, 9, 6);
        meshData.AddTriangleIdxs(6, 5, 1);
        meshData.AddTriangleIdxs(1, 2, 6);
        meshData.AddTriangleIdxs(0, 3, 4);
        // return meshData;
    }

}