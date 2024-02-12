
using UnityEngine;

public class EquilateralTriangleWithRectHole
{
    public static float slope = 1 / Mathf.Sqrt(3); // slope of the outer equilateral triangle from the first vertex (at <r,0>) to the second vertex (rotated 120 degrees counterclockwise from <r,0>)
    public float vertexRadius;
    public float halfRectHeight;
    public float halfRectWidth;
    public Vector3[] triangleVertices;
    public MeshData meshData;

    public EquilateralTriangleWithRectHole(float vertexRadius, float rectHeight, float rectWidth)
    {
        meshData = new();
        this.vertexRadius = vertexRadius;
        this.halfRectHeight = rectHeight / 2f;
        this.halfRectWidth = rectWidth / 2f;

        // calc the vertices for the outer equilateral triangle
        var polygon = new Polygon(3);
        triangleVertices = polygon.GetVertices(vertexRadius);
    }

    public void BuildMeshData()
    {
        // this fxn divides an equilateral triangle up into 7 triangles with a rectangular hole in the middle (of a certain height and width)
        var deltaX1 = vertexRadius - halfRectWidth; // change in x from the (r,0) triangle vertex to the right side of rect hole
        var deltaX2 = vertexRadius + halfRectWidth; // ... to the LEFT side of rect hole
        var deltaY1 = slope * deltaX1; // change in y from (r,0) to the right side of the rect hole
        var deltaY2 = slope * deltaX2; // ... to the LEFT side of the rect hole

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

        // avoid re-using vertices so that the normals are calculated correctly
        meshData.AddVertex(new Vector2(halfRectWidth, deltaY1)); // 11 same as 3
        meshData.AddVertex(new Vector2(halfRectWidth, deltaY1)); // 12 same as 3
        meshData.AddVertex(new Vector2(halfRectWidth, -deltaY1)); // 13 same as 4
        meshData.AddVertex(new Vector2(halfRectWidth, -deltaY1)); // 14 same as 4
        meshData.AddVertex(new Vector2(-halfRectWidth, deltaY2)); // 15 same as 5
        meshData.AddVertex(new Vector2(-halfRectWidth, deltaY2)); // 16 same as 5
        meshData.AddVertex(new Vector2(-halfRectWidth, -deltaY2)); // 17 same as 6 
        meshData.AddVertex(new Vector2(-halfRectWidth, -deltaY2)); // 18 same as 6
        meshData.AddVertex(new Vector2(-halfRectWidth, -deltaY2)); // 19 same as 6
        meshData.AddVertex(new Vector2(halfRectWidth, -halfRectHeight)); // 20 same as 10
        meshData.AddVertex(triangleVertices[0]); // 21 same as 0
        meshData.AddVertex(triangleVertices[1]); // 22 same as 1

        meshData.AddTriangleIdxsReverseNormal(3, 0, 4);
        meshData.AddTriangleIdxsReverseNormal(7, 11, 5);
        meshData.AddTriangleIdxsReverseNormal(15, 8, 7);
        meshData.AddTriangleIdxsReverseNormal(10, 6, 13);
        meshData.AddTriangleIdxsReverseNormal(20, 9, 17);
        meshData.AddTriangleIdxsReverseNormal(18, 16, 1);
        meshData.AddTriangleIdxsReverseNormal(22, 2, 19);
        meshData.AddTriangleIdxsReverseNormal(21, 12, 14);
    }

}