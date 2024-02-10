// using UnityEngine;
// using System.Collections.Generic;

// public class MeshData
// {
//     public List<Vector3> vertices;
//     public List<int> triangleIdxs;
//     // public List<Vector2> uvs;

//     private int lastVertexIdx = -1;

//     public MeshData()
//     {
//         vertices = new List<Vector3>();
//         triangleIdxs = new List<int>();
//         // uvs = new List<Vector2>();
//         // lastVertexIdx = -1;
//     }

//     public void AddVertex(Vector3 v, List<int> idxs)
//     {
//         idxs.Add(vertices.Count);
//         vertices.Add(v);

//         // lastVertexIdx++;
//         // return lastVertexIdx;
//     }

//     public void AddTriangleIdxs(int i1, int i2, int i3)
//     {
//         triangleIdxs.Add(i1);
//         triangleIdxs.Add(i2);
//         triangleIdxs.Add(i3);
//     }

//     public Triangle[] Triangles
//     {
//         get
//         {
//             int triangleCount = triangleIdxs.Count / 3;
//             Triangle[] triangleArray = new Triangle[triangleCount];
//             for (int i = 0; i < triangleCount; i++)
//             {
//                 int idx = i * 3;
//                 triangleArray[i] = new Triangle(vertices[triangleIdxs[idx]], vertices[triangleIdxs[idx + 1]], vertices[triangleIdxs[idx + 2]]);
//             }
//             return triangleArray;
//         }
//     }

//     public string TrianglesToString()
//     {
//         string result = "";
//         foreach (Triangle t in Triangles)
//         {
//             result += t.ToString() + "\n";
//         }
//         return result;
//     }

//     public Mesh CreateMesh()
//     {
//         Mesh mesh = new Mesh
//         {
//             vertices = vertices.ToArray(),
//             triangles = triangleIdxs.ToArray(),
//             // uv = uvs.ToArray()
//         };
//         mesh.RecalculateNormals();
//         return mesh;
//     }
// }

