// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;


// public static class ClipperDemo
// {
//     public static List<Vector2> GetVerticesForTriangle(float size)
//     {
//         List<Vector2> vertices = new();

//         // float size = 4f;
//         var v = new Vector2(size / 2, -size / 2);
//         vertices.Add(v);

//         v = new Vector2(-size / 2, -size / 2);
//         vertices.Add(v);
//         v = new Vector2(size / 2, size / 2);
//         vertices.Add(v);


//         return vertices;
//     }

//     public static List<Vector2> GetVerticesForPolygon(int numSides)
//     {
//         Polygon p = new Polygon(numSides);

//         var vertices = p.GetVertices(1f);

//         return vertices.ToList();
//     }

// }