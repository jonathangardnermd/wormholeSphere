// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Linq;

// public class IcosahedronGenerator
// {
//     private List<TriangleIdxs> triangles;
//     private List<Vector3> vertices;

//     public List<TriangleIdxs> TriangleIdxs { get => triangles; private set => triangles = value; }
//     public List<Vector3> Vertices { get => vertices; private set => vertices = value; }

//     public void Initialize()
//     {
//         triangles = new List<TriangleIdxs>();
//         vertices = new List<Vector3>();

//         // An icosahedron has 12 vertices, and
//         // since it's completely symmetrical the
//         // formula for calculating them is kind of
//         // symmetrical too:

//         float t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

//         vertices.Add(new Vector3(-1, t, 0).normalized);
//         vertices.Add(new Vector3(1, t, 0).normalized);
//         vertices.Add(new Vector3(-1, -t, 0).normalized);
//         vertices.Add(new Vector3(1, -t, 0).normalized);
//         vertices.Add(new Vector3(0, -1, t).normalized);
//         vertices.Add(new Vector3(0, 1, t).normalized);
//         vertices.Add(new Vector3(0, -1, -t).normalized);
//         vertices.Add(new Vector3(0, 1, -t).normalized);
//         vertices.Add(new Vector3(t, 0, -1).normalized);
//         vertices.Add(new Vector3(t, 0, 1).normalized);
//         vertices.Add(new Vector3(-t, 0, -1).normalized);
//         vertices.Add(new Vector3(-t, 0, 1).normalized);

//         // And here's the formula for the 20 sides,
//         // referencing the 12 vertices we just created.
//         triangles.Add(new TriangleIdxs(0, 11, 5));
//         triangles.Add(new TriangleIdxs(0, 5, 1));
//         triangles.Add(new TriangleIdxs(0, 1, 7));
//         triangles.Add(new TriangleIdxs(0, 7, 10));
//         triangles.Add(new TriangleIdxs(0, 10, 11));
//         triangles.Add(new TriangleIdxs(1, 5, 9));
//         triangles.Add(new TriangleIdxs(5, 11, 4));
//         triangles.Add(new TriangleIdxs(11, 10, 2));
//         triangles.Add(new TriangleIdxs(10, 7, 6));
//         triangles.Add(new TriangleIdxs(7, 1, 8));
//         triangles.Add(new TriangleIdxs(3, 9, 4));
//         triangles.Add(new TriangleIdxs(3, 4, 2));
//         triangles.Add(new TriangleIdxs(3, 2, 6));
//         triangles.Add(new TriangleIdxs(3, 6, 8));
//         triangles.Add(new TriangleIdxs(3, 8, 9));
//         triangles.Add(new TriangleIdxs(4, 9, 5));
//         triangles.Add(new TriangleIdxs(2, 4, 11));
//         triangles.Add(new TriangleIdxs(6, 2, 10));
//         triangles.Add(new TriangleIdxs(8, 6, 7));
//         triangles.Add(new TriangleIdxs(9, 8, 1));
//     }

//     public void Subdivide(int recursions)
//     {
//         var midPointCache = new Dictionary<int, int>();

//         for (int i = 0; i < recursions; i++)
//         {
//             var newTriangleIdxs = new List<TriangleIdxs>();
//             foreach (var triangle in triangles)
//             {
//                 int a = triangle.vertices[0];
//                 int b = triangle.vertices[1];
//                 int c = triangle.vertices[2];
//                 // Use GetMidPointIndex to either create a
//                 // new vertex between two old vertices, or
//                 // find the one that was already created.
//                 int ab = GetMidPointIndex(midPointCache, a, b);
//                 int bc = GetMidPointIndex(midPointCache, b, c);
//                 int ca = GetMidPointIndex(midPointCache, c, a);
//                 // Create the four new polygons using our original
//                 // three vertices, and the three new midpoints.
//                 newTriangleIdxs.Add(new TriangleIdxs(a, ab, ca));
//                 newTriangleIdxs.Add(new TriangleIdxs(b, bc, ab));
//                 newTriangleIdxs.Add(new TriangleIdxs(c, ca, bc));
//                 newTriangleIdxs.Add(new TriangleIdxs(ab, bc, ca));
//             }
//             // Replace all our old polygons with the new set of
//             // subdivided ones.
//             triangles = newTriangleIdxs;
//         }
//     }

//     public MeshData GetMeshData()
//     {
//         MeshData mesh = new MeshData
//         {
//             vertices = vertices,
//             triangleIdxs = triangles.SelectMany(triangle => triangle.vertices).ToList(),
//             // uv = uvs
//         };
//         // mesh.RecalculateNormals();
//         return mesh;
//     }

//     public int GetMidPointIndex(Dictionary<int, int> cache, int indexA, int indexB)
//     {
//         // We create a key out of the two original indices
//         // by storing the smaller index in the upper two bytes
//         // of an integer, and the larger index in the lower two
//         // bytes. By sorting them according to whichever is smaller
//         // we ensure that this function returns the same result
//         // whether you call
//         // GetMidPointIndex(cache, 5, 9)
//         // or...
//         // GetMidPointIndex(cache, 9, 5)
//         int smallerIndex = Mathf.Min(indexA, indexB);
//         int greaterIndex = Mathf.Max(indexA, indexB);
//         int key = (smallerIndex << 16) + greaterIndex;
//         // If a midpoint is already defined, just return it.
//         int ret;
//         if (cache.TryGetValue(key, out ret))
//             return ret;
//         // If we're here, it's because a midpoint for these two
//         // vertices hasn't been created yet. Let's do that now!
//         Vector3 p1 = vertices[indexA];
//         Vector3 p2 = vertices[indexB];
//         Vector3 middle = Vector3.Lerp(p1, p2, 0.5f).normalized;

//         ret = vertices.Count;
//         vertices.Add(middle);

//         cache.Add(key, ret);
//         return ret;
//     }
// }