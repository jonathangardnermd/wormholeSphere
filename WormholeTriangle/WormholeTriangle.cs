using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.WSA;

public class WormholeTriangle
{

    public PolygonBoxBorder pb;
    public EquilateralTriangleWithRectHole triangle;
    public static float sqrt3 = Mathf.Sqrt(3);

    public WormholeTriangle(float sideLength, Vector3[] triangleVerts, int polyNumSides, float polyVertexRadius)
    {
        AddTriangleWithPolygonHoleToMesh(sideLength, triangleVerts, polyNumSides, polyVertexRadius);
        // pb = new PolygonBoxBorder(new Vector3[] { });
        // triangle = new EquilateralTriangleWithRectHole(0, 0, 0);
    }
    private void AddTriangleWithPolygonHoleToMesh(float sideLength
    , Vector3[] triangleVerts, int polyNumSides, float polyVertexRadius)
    {
        // create the big equilateral triangle
        float vertexRadius = sideLength / sqrt3; // applies to an equilateral triangle
        var equilateralTriangle = new Polygon2(3);
        var startVertices3D = equilateralTriangle.GetVertices(vertexRadius);

        // Calculate the transformation from startVertices to endVertices
        TriangleTransformer tt = new TriangleTransformer(startVertices3D, triangleVerts);

        // set the breakpoint for adding new vertices
        // var startVertexCt = meshData.vertices.Count;

        // calc the polygon border to turn it into a rect
        // AND add it to the mesh (TODO: divide this up)
        var poly = new Polygon2(polyNumSides);
        pb = new PolygonBoxBorder(poly.GetVertices(polyVertexRadius));
        // pb.BuildMeshData();
        var b = pb.polygonBounds;
        // var b = PolygonBoxBorder.AddMeshData(meshData, poly.GetVertices(polyVertexRadius));

        // calc the triangles to go around the polyBoxBorder
        triangle = new EquilateralTriangleWithRectHole(vertexRadius,
            b.GetHeight(), b.GetWidth());

        // add the triangles to the mesh
        // triangle.AddMeshData(meshData);

        pb.BuildMeshData();
        triangle.BuildMeshData();
        pb.meshData.vertices = tt.TransformVectors(pb.meshData.vertices).ToList();
        triangle.meshData.vertices = tt.TransformVectors(triangle.meshData.vertices).ToList();
        // var mesh = MeshData2.CreateMesh(pb.meshData, triangle.meshData);
        // transform ALL the vertices we've added since setting the breakpoint
        // var transformedVertices = tt.TransformVectors(tt.combinedRotationMatrix, tt.centerVector, meshData.vertices.Skip(startVertexCt).ToArray());
        // for (int i = startVertexCt; i < meshData.vertices.Count; i++)
        // {
        //     meshData.vertices[i] = transformedVertices[i - startVertexCt];
        // }

        // List<int> vIdxs = new();
        // for (int i = 0; i < triangleVerts.Length; i++)
        // {
        //     meshData.AddVertex(triangleVerts[i], vIdxs);
        // }
        // meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);


        // vIdxs = new();
        // for (int i = 0; i < startVertices3D.Length; i++)
        // {
        //     meshData.AddVertex(startVertices3D[i], vIdxs);
        // }
        // meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);

    }
    // public static void AddTriangleWithPolygonHoleToMesh2(MeshData2 meshData, float sideLength
    // , Vector3[] triangleVerts, int polyNumSides, float polyVertexRadius)
    // {
    //     float vertexRadius = sideLength / sqrt3;
    //     Polygon2 p = new Polygon2(3);
    //     var startVertices3D = p.GetVertices(vertexRadius);

    //     TriangleTransformer tt = new TriangleTransformer(startVertices3D, triangleVerts);
    //     tt.TransformVectors(tt.combinedRotationMatrix, tt.centerVector * 1f, startVertices3D);


    //     List<int> vIdxs = new();


    //     vIdxs = new();
    //     for (int i = 0; i < startVertices3D.Length; i++)
    //     {
    //         meshData.AddVertex(startVertices3D[i], vIdxs);
    //     }
    //     meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);

    // }
    // public static void AddTriangleWithPolygonHoleToMesh3(MeshData meshData, float sideLength
    // , Vector3[] triangleVerts, int polyNumSides, float polyVertexRadius)
    // {
    //     float vertexRadius = sideLength / sqrt3;
    //     Polygon2 p = new Polygon2(3);
    //     var startVertices3D = p.GetVertices(vertexRadius);

    //     TriangleTransformer tt = new TriangleTransformer(startVertices3D, triangleVerts);
    //     tt.BuildMesh(meshData);
    // }


}


