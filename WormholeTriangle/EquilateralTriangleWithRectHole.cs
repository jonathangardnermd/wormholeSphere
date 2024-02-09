
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

    public EquilateralTriangleWithRectHole(float vertexRadius, float rectHeight, float rectWidth)
    {
        this.vertexRadius = vertexRadius;
        this.halfRectHeight = rectHeight / 2f;
        this.halfRectWidth = rectWidth / 2f;

        var polygon = new Polygon(3);
        triangleVertices = polygon.GetVertices(vertexRadius);
    }

    public MeshData AddMeshData(MeshData meshData)
    {
        var deltaX1 = vertexRadius - halfRectWidth; // change in x from the (r,0) triangle vertex to the right side of rect hole
        var deltaX2 = vertexRadius + halfRectWidth; // ... to the LEFT side of rect hole
        var deltaY1 = slope * deltaX1; // change in y from (r,0) to the right side of the rect hole
        var deltaY2 = slope * deltaX2; // ... to the LEFT side of the rect hole

        List<int> newIdxs = new();
        meshData.AddVertex(triangleVertices[0], newIdxs); // 0
        meshData.AddVertex(triangleVertices[1], newIdxs); // 1
        meshData.AddVertex(triangleVertices[2], newIdxs);// 2
        meshData.AddVertex(new Vector2(halfRectWidth, deltaY1), newIdxs); // 3
        meshData.AddVertex(new Vector2(halfRectWidth, -deltaY1), newIdxs); // 4
        meshData.AddVertex(new Vector2(-halfRectWidth, deltaY2), newIdxs); // 5
        meshData.AddVertex(new Vector2(-halfRectWidth, -deltaY2), newIdxs); // 6
        meshData.AddVertex(new Vector2(halfRectWidth, halfRectHeight), newIdxs); // 7
        meshData.AddVertex(new Vector2(-halfRectWidth, halfRectHeight), newIdxs); // 8
        meshData.AddVertex(new Vector2(-halfRectWidth, -halfRectHeight), newIdxs); // 9
        meshData.AddVertex(new Vector2(halfRectWidth, -halfRectHeight), newIdxs); // 10

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

}