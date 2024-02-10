using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.WSA;

public class WormholeTriangle
{
    public TriangleTransformer tt;
    public PolygonBoxBorder pb;
    public EquilateralTriangleWithRectHole triangle;
    public static float sqrt3 = Mathf.Sqrt(3);

    public WormholeTriangle(float sideLength, Vector3[] triangleVerts, int polyNumSides, float polyVertexRadius)
    {
        Init(sideLength, triangleVerts, polyNumSides, polyVertexRadius);
        // BuildMeshData();
    }
    private void Init(float sideLength
    , Vector3[] triangleVerts, int polyNumSides, float polyVertexRadius)
    {
        // create the big equilateral triangle
        float vertexRadius = sideLength / sqrt3; // applies to an equilateral triangle
        var equilateralTriangle = new Polygon(3);
        var startVertices3D = equilateralTriangle.GetVertices(vertexRadius);

        tt = new TriangleTransformer(startVertices3D, triangleVerts);

        var poly = new Polygon(polyNumSides);
        pb = new PolygonBoxBorder(poly.GetVertices(polyVertexRadius));
        var b = pb.polygonBounds;

        triangle = new EquilateralTriangleWithRectHole(vertexRadius,
            b.GetHeight(), b.GetWidth());
    }

    public void BuildMeshData()
    {
        pb.BuildMeshData();
        triangle.BuildMeshData();
        pb.meshData.vertices = tt.TransformVectors(pb.meshData.vertices).ToList();
        triangle.meshData.vertices = tt.TransformVectors(triangle.meshData.vertices).ToList();
    }
}


