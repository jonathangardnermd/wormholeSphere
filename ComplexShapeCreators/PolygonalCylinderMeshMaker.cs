using System.Collections.Generic;
using UnityEngine;

public class PolygonCylinder
{
    public int numSides;
    public float length;
    public float polygonVertexRadius;
    public float zEnd;

    public Polygon polygon;

    public MeshData meshData;

    public PolygonCylinder(int numSides, float length, float polygonVertexRadius, float zEnd)
    {
        meshData = new();
        this.numSides = numSides;
        this.length = length;
        this.polygonVertexRadius = polygonVertexRadius;
        this.zEnd = zEnd;
    }

    public void BuildMeshData()
    {
        AddPolygonCylinderToMesh(meshData);
    }

    /*
    This function will create the sides of the cylinder by "stacking" two polygons: 
    one at z=-length and the other at z=0.
    */
    private void AddPolygonCylinderToMesh(MeshData meshData)
    {
        polygon = new Polygon(numSides); // calculate the vertices of a unit regular polygon with the specified number of sides

        // "stacking" the polygons creates the triangles and vertices to connect them, forming the sides of the cylinder
        StackPolygons(meshData, polygon, length, polygonVertexRadius, polygonVertexRadius, zEnd + length, zEnd + 0);
    }

    public static void StackPolygons(MeshData meshData, Polygon polygon, float totLength, float vertexRadius1, float vertexRadius2, float z1, float z2)
    {
        // calculate the vertices of the two polygons with the two (potentially different) radii
        Vector3[] poly1Vs = polygon.GetVertices(vertexRadius1);
        Vector3[] poly2Vs = polygon.GetVertices(vertexRadius2);

        for (int i1 = 0; i1 < polygon.numSides; i1++)
        {
            // add the vertices and triangles that form the quad between poly1's i1 and i2 vertices and poly2's i1 and i2 vertices
            int i2 = (i1 + 1) % polygon.numSides;

            var startIdx = meshData.vertices.Count;
            List<int> vIdxs = new();
            // add the first triangle of the quad
            meshData.AddVertex(new Vector3(poly1Vs[i2].x, poly1Vs[i2].y, z1));
            meshData.AddVertex(new Vector3(poly1Vs[i1].x, poly1Vs[i1].y, z1));
            meshData.AddVertex(new Vector3(poly2Vs[i1].x, poly2Vs[i1].y, z2));
            meshData.AddTriangleIdxsReverseNormal(startIdx + 0, startIdx + 1, startIdx + 2);

            // add the second triangle of the quad
            meshData.AddVertex(new Vector3(poly2Vs[i2].x, poly2Vs[i2].y, z2));
            meshData.AddVertex(new Vector3(poly1Vs[i2].x, poly1Vs[i2].y, z1));
            meshData.AddVertex(new Vector3(poly2Vs[i1].x, poly2Vs[i1].y, z2));
            meshData.AddTriangleIdxsReverseNormal(startIdx + 3, startIdx + 4, startIdx + 5);
        }
    }
}

