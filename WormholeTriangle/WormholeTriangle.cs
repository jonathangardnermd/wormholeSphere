using System.Collections.Generic;
using UnityEngine;

public class WormholeTriangle
{
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
        List<int> vIdxs = new();
        for (int i = 0; i < triangleVerts.Length; i++)
        {
            meshData.AddVertex(triangleVerts[i], vIdxs);
        }
        meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);
        // var startVertexCt = meshData.vertices.Count;
        // Polygon p = new Polygon(polyNumSides);
        // var b = PolygonBoxBorder.AddMeshData(meshData, p.GetVertices(polyVertexRadius));

        // float triangleVertexRadius = sideLength / Mathf.Sqrt(3);
        // var wt = new EquilateralTriangleWithRectHole(triangleVertexRadius, b.GetHeight(), b.GetWidth());
        // wt.AddMeshData(meshData);

        // // Calculate the edges of the triangle
        // Vector3 edge1 = triangleVerts[0] - triangleVerts[1];
        // Vector3 edge2 = triangleVerts[0] - triangleVerts[2];

        // // Calculate the normal using the cross product of the edges
        // Vector3 newNormal = Vector3.Cross(edge1, edge2).normalized;

        // Vector3 center = triangleVerts[0] + triangleVerts[1] + triangleVerts[2] / 3;
        // var origNormal = new Vector3(0, 0, 1);
        // // var newN = new Vector3(1, 1, 1);
        // var t = new Transformer(origNormal, newNormal, center);
        // t.TransformMeshData(meshData, startVertexCt);



        // return meshData;
        // MeshDrawer drawer = FindObjectOfType<MeshDrawer>();
        // var texture = GetTexture();
        // drawer.DrawMesh(meshData, texture);
    }

    public void CalcTriangleVertexRadiusFromVertices()
    {

    }


}


