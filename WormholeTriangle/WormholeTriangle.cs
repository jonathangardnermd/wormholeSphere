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
    // public static float sqrt3 = Mathf.Sqrt(3);
    public static float sqrt2 = Mathf.Sqrt(2);

    public WormholeTriangle(float triangleVertexRadius, int polyNumSides, float baseCylinderLength, float baseCylinderRadius, float splayLength)
    {
        Init(triangleVertexRadius, polyNumSides, baseCylinderLength, baseCylinderRadius, splayLength);
        // BuildMeshData();
        // CalcTransform(triangleVertexRadius, triangleVerts);
    }
    private void Init(float triangleVertexRadius
    , int polyNumSides, float baseCylinderLength, float baseCylinderRadius, float splayLength)
    {
        // calc polygonal hole where the wormhole will go
        float polyVertexRadius = baseCylinderRadius + splayLength;
        var poly = new Polygon(polyNumSides);

        // calc vertices/triangles for polygon box border around the polygonal hole
        pb = new PolygonBoxBorder(poly.GetVertices(polyVertexRadius));
        var b = pb.polygonBounds;

        // calc vertices/triangles for triangle with rect hole that fits the polygon box border 
        triangle = new EquilateralTriangleWithRectHole(triangleVertexRadius,
            b.GetHeight(), b.GetWidth());

        // calc vertices/triangles for cylindrical piece of the wormhole
        var zEnd = splayLength;
        cylinder = new PolygonCylinder(polyNumSides, baseCylinderLength, baseCylinderRadius, zEnd);

        // calc vertices/triangles for wormhole splay
        var splayData = new SplayData(polyNumSides, splayLength * sqrt2);
        splay = new PolygonalCylinderSplay(poly, baseCylinderRadius, splayData, zEnd);
    }

    public static TriangleTransformer CalcTransform(float triangleVertexRadius, Vector3[] triangleVerts)
    {
        var equilateralTriangle = new Polygon(3);
        var startVertices3D = equilateralTriangle.GetVertices(triangleVertexRadius);
        return new TriangleTransformer(startVertices3D, triangleVerts);
    }

    public void BuildMeshData()
    {
        pb.BuildMeshData();
        triangle.BuildMeshData();
        // pb.meshData.vertices = tt.TransformVectors(pb.meshData.vertices).ToList();
        // triangle.meshData.vertices = tt.TransformVectors(triangle.meshData.vertices).ToList();

        cylinder.BuildMeshData();
        splay.BuildMeshData();
        // cylinder.meshData.vertices = tt.TransformVectors(cylinder.meshData.vertices).ToList();
        // splay.meshData.vertices = tt.TransformVectors(splay.meshData.vertices).ToList();
    }

    public MeshData[] GetMeshes()
    {
        return new[] { pb.meshData, triangle.meshData, cylinder.meshData, splay.meshData };
    }
}


