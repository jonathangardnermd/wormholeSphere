// using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WormholeTriangle
{
    public TriangleTransformer tt;
    public PolygonBoxBorder pb;
    public EquilateralTriangleWithRectHole triangle;
    public PolygonCylinder cylinder;
    public PolygonalCylinderSplay splay;
    public static float sqrt3 = Mathf.Sqrt(3);
    public static float sqrt2 = Mathf.Sqrt(2);

    public WormholeTriangle(float sideLength, Vector3[] triangleVerts, int polyNumSides, float baseCylinderLength, float baseCylinderRadius, float splayLength)
    {
        Init(sideLength, triangleVerts, polyNumSides, baseCylinderLength, baseCylinderRadius, splayLength);
    }
    private void Init(float sideLength
    , Vector3[] triangleVerts, int polyNumSides, float baseCylinderLength, float baseCylinderRadius, float splayLength)
    {
        // create the big equilateral triangle
        float vertexRadius = sideLength / sqrt3; // applies to an equilateral triangle
        var equilateralTriangle = new Polygon(3);
        var startVertices3D = equilateralTriangle.GetVertices(vertexRadius);

        tt = new TriangleTransformer(startVertices3D, triangleVerts);

        float polyVertexRadius = baseCylinderRadius + splayLength;
        var poly = new Polygon(polyNumSides);
        pb = new PolygonBoxBorder(poly.GetVertices(polyVertexRadius));
        var b = pb.polygonBounds;

        triangle = new EquilateralTriangleWithRectHole(vertexRadius,
            b.GetHeight(), b.GetWidth());

        var zEnd = splayLength;
        cylinder = new PolygonCylinder(polyNumSides, baseCylinderLength, baseCylinderRadius, zEnd);
        var splayData = new SplayData(polyNumSides, splayLength * sqrt2);
        splay = new PolygonalCylinderSplay(poly, baseCylinderRadius, splayData, zEnd);

    }

    public void BuildMeshData()
    {
        pb.BuildMeshData();
        triangle.BuildMeshData();
        pb.meshData.vertices = tt.TransformVectors(pb.meshData.vertices).ToList();
        triangle.meshData.vertices = tt.TransformVectors(triangle.meshData.vertices).ToList();

        cylinder.BuildMeshData();
        splay.BuildMeshData();
        cylinder.meshData.vertices = tt.TransformVectors(cylinder.meshData.vertices).ToList();
        splay.meshData.vertices = tt.TransformVectors(splay.meshData.vertices).ToList();
    }

    public MeshData[] GetMeshes()
    {
        return new[] { pb.meshData, triangle.meshData, cylinder.meshData, splay.meshData };
    }
}


