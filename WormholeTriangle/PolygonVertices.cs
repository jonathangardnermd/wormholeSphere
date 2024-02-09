// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using System.IO;

// public class PolygonVertices
// {
//     public Vector3[] vertices;
//     public Vector3 center;
//     public Vector3 hDirection;

//     public Vector3 unitNormal;

//     public PolygonVertices(Vector3 center, Vector3 unitNormal, Vector3 hDirection, int numSides, float vertexRadius)
//     {
//         // this.vertices = vertices;
//     }

//     public void CalcCenter()
//     {
//         Vector3 sum = Vector3.zero;
//         foreach (Vector3 vertex in vertices)
//         {
//             sum += vertex;
//         }
//         center = new Vector3[] { sum / 3 };
//     }

//     public void CalcUnitNormal()
//     {
//         // Get two edges of the triangle
//         Vector3 edge1 = vertices[1] - vertices[0];
//         Vector3 edge2 = vertices[2] - vertices[0];

//         // Calculate the cross product of the edges to get the normal
//         Vector3 normal = Vector3.Cross(edge1, edge2).normalized;

//         // Assign the calculated normal to the unitNormal variable
//         unitNormal = normal;
//     }


// }