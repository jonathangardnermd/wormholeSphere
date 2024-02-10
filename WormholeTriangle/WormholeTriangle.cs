using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.WSA;

public class WormholeTriangle
{
    public static float sqrt3 = Mathf.Sqrt(3);
    // public WormholeTriangle(Vector3[] triangleVerts)
    // {

    // }

    public static void AddTriangleWithPolygonHoleToMesh(MeshData meshData, float sideLength
        , Vector3[] triangleVerts, int polyNumSides, float polyVertexRadius)
    {
        var startVertexCt = meshData.vertices.Count;
        Polygon p = new Polygon(polyNumSides);
        var b = PolygonBoxBorder.AddMeshData(meshData, p.GetVertices(polyVertexRadius));

        float triangleVertexRadius = 2 * sideLength / Mathf.Sqrt(3);
        var triangle = new EquilateralTriangleWithRectHole(triangleVertexRadius,
            b.GetHeight(), b.GetWidth());
        triangle.AddMeshData(meshData);

        // 
        Vector3 edge1 = triangleVerts[1] - triangleVerts[2];
        // rotate the vertical edge of the new triangle to match this edge


        // Calculate the edges of the triangle

        Vector3 edge2 = triangleVerts[0] - triangleVerts[2];


        // Calculate the normal using the cross product of the edges
        Vector3 newNormal = Vector3.Cross(edge1, edge2).normalized;

        Vector3 center = triangleVerts[0] + triangleVerts[1] + triangleVerts[2] / 3;
        var origNormal = new Vector3(0, 0, 1);
        var t = new Transformer(origNormal, newNormal, center);
        t.TransformMeshData(meshData, startVertexCt);
    }

    public static void AddTriangleWithPolygonHoleToMesh2(MeshData meshData, float sideLength
    , Vector3[] triangleVerts, int polyNumSides, float polyVertexRadius)
    {
        float vertexRadius = sideLength / sqrt3;
        Polygon p = new Polygon(3);
        var startVertices = p.GetVertices(vertexRadius);

        Vector3[] startVertices3D = new Vector3[startVertices.Length];
        for (int i = 0; i < startVertices.Length; i++)
        {
            // Convert each Vector2 to Vector3, setting the Z component to the specified value
            startVertices3D[i] = new Vector3(startVertices[i].x, startVertices[i].y, 0);
        }
        TriangleTransformer tt = new TriangleTransformer(startVertices3D, triangleVerts);
        tt.TransformVectors(tt.combinedRotationMatrix, tt.centerVector * 2, startVertices3D);


        List<int> vIdxs = new();
        for (int i = 0; i < triangleVerts.Length; i++)
        {
            meshData.AddVertex(triangleVerts[i], vIdxs);
        }
        meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);


        vIdxs = new();
        for (int i = 0; i < startVertices3D.Length; i++)
        {
            meshData.AddVertex(startVertices3D[i], vIdxs);
        }
        meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);

    }
    public static void AddTriangleWithPolygonHoleToMesh3(MeshData meshData, float sideLength
    , Vector3[] triangleVerts, int polyNumSides, float polyVertexRadius)
    {
        float vertexRadius = sideLength / sqrt3;
        Polygon p = new Polygon(3);
        var startVertices = p.GetVertices(vertexRadius);

        Vector3[] startVertices3D = new Vector3[startVertices.Length];
        for (int i = 0; i < startVertices.Length; i++)
        {
            // Convert each Vector2 to Vector3, setting the Z component to the specified value
            startVertices3D[i] = new Vector3(startVertices[i].x, startVertices[i].y, 0);
        }
        TriangleTransformer tt = new TriangleTransformer(startVertices3D, triangleVerts);
        // tt.TransformVectors(tt.combinedRotationMatrix, tt.translationVector * 2, startVertices3D);
        tt.BuildMesh(meshData);

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

    public void CalcTriangleVertexRadiusFromVertices()
    {

    }


}


