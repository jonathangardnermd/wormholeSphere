// using UnityEngine;
// using System.Linq;
// using System.Collections.Generic;
// // using Math;
// public class Polygon
// {
//     public readonly int numSides;
//     private Vector2[] vertices;
//     private float[] angularUvs;

//     private float angle;

//     // public float vertexRadius;

//     public Polygon(int numSides)
//     {
//         this.numSides = numSides;
//         SetUnitVertices();
//     }

//     public Vector2[] GetVertices(float vertexRadius)
//     {
//         return vertices.Select(v => v * vertexRadius).ToArray();
//     }

//     void SetUnitVertices() // vertices will be exactly 1 unit from the origin (by rotating the point <x=1,y=0> around the origin)
//     {
//         angle = 2 * Mathf.PI / numSides;
//         vertices = new Vector2[numSides];
//         angularUvs = new float[numSides];

//         for (int i = 0; i < numSides; i++)
//         {
//             // rotate the pt <1,0> by the rotationAngle = i * angle to form the ith vertex of the unit polygon
//             float x = Mathf.Cos(i * angle);
//             float y = Mathf.Sin(i * angle);
//             vertices[i] = new Vector2(x, y);
//             angularUvs[i] = (float)i / numSides;
//         }
//     }

//     public MeshData GetMeshData(float vertexRadius)
//     {
//         var meshData = new MeshData();

//         var originVertex = new Vector3(0, 0, 0);
//         List<int> vIdxs = new();
//         meshData.AddVertex(originVertex, vIdxs);
//         var sizedVertices = GetVertices(vertexRadius);
//         for (int i1 = 0; i1 < numSides; i1++)
//         {
//             var i2 = (i1 + 1) % numSides;
//             var v1 = sizedVertices[i1];
//             var v2 = sizedVertices[i2];

//             meshData.AddVertex(v1, vIdxs);
//             meshData.AddVertex(v2, vIdxs);
//             meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);
//         }
//         return meshData;
//     }
// }