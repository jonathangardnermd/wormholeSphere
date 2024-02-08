// using UnityEngine;
// using System.Collections.Generic;

// public static class PolygonClipper
// {
//     const int INSIDE = 0; // Bitwise codes for the region
//     const int LEFT = 1;
//     const int RIGHT = 2;
//     const int BOTTOM = 4;
//     const int TOP = 8;
//     // public static (List<Vector2>, List<int>) ClipPolygon(List<Vector2> subjectPolygon, List<Vector2> clipPolygon)
//     // {
//     //     List<Vector2> outputList = new List<Vector2>(subjectPolygon);
//     //     List<int> triangles = new List<int>();

//     //     foreach (Vector2 clipVertex in clipPolygon)
//     //     {
//     //         List<Vector2> inputList = new List<Vector2>(outputList);
//     //         outputList.Clear();

//     //         if (inputList.Count == 0)
//     //             break;

//     //         Vector2 S = inputList[inputList.Count - 1];

//     //         foreach (Vector2 E in inputList)
//     //         {
//     //             if (IsInside(E, clipPolygon))
//     //             {
//     //                 if (!IsInside(S, clipPolygon))
//     //                 {
//     //                     Vector2 point = GetIntersection(S, E, clipVertex);
//     //                     outputList.Add(point);
//     //                 }
//     //                 outputList.Add(E);
//     //             }
//     //             else if (IsInside(S, clipPolygon))
//     //             {
//     //                 Vector2 point = GetIntersection(S, E, clipVertex);
//     //                 outputList.Add(point);
//     //             }

//     //             S = E;
//     //         }
//     //     }

//     //     // Generate triangles
//     //     for (int i = 1; i < outputList.Count - 1; i++)
//     //     {
//     //         triangles.Add(0);
//     //         triangles.Add(i);
//     //         triangles.Add(i + 1);
//     //     }

//     //     return (outputList, triangles);
//     // }




//     public static (List<Vector2>, List<int>) ClipPolygon(List<Vector2> subjectPolygon, List<Vector2> clipPolygon)
//     {
//         List<Vector2> outputList = new List<Vector2>(subjectPolygon);
//         List<int> triangles = new List<int>();

//         // Clip each edge of the clip polygon against the edges of the subject polygon (larger triangle)
//         for (int i = 0; i < clipPolygon.Count; i++)
//         {
//             Vector2 edgeStart = clipPolygon[i];
//             Vector2 edgeEnd = clipPolygon[(i + 1) % clipPolygon.Count];

//             outputList = ClipEdge(outputList, edgeStart, edgeEnd);
//         }

//         // Generate triangles
//         for (int i = 1; i < outputList.Count - 1; i++)
//         {
//             triangles.Add(0);
//             triangles.Add(i);
//             triangles.Add(i + 1);
//         }

//         return (outputList, triangles);
//     }

//     private static List<Vector2> ClipEdge(List<Vector2> polygon, Vector2 edgeStart, Vector2 edgeEnd)
//     {
//         List<Vector2> outputList = new List<Vector2>();

//         // Iterate over each edge of the polygon
//         for (int i = 0; i < polygon.Count; i++)
//         {
//             Vector2 vertex1 = polygon[i];
//             Vector2 vertex2 = polygon[(i + 1) % polygon.Count];

//             // Clip the edge against the current edge of the larger containing triangle
//             Vector2? clippedVertex = ClipLine(vertex1, vertex2, edgeStart, edgeEnd);

//             if (clippedVertex != null)
//                 outputList.Add(clippedVertex.Value);
//         }

//         return outputList;
//     }

//     private static Vector2? ClipLine(Vector2 start, Vector2 end, Vector2 clipStart, Vector2 clipEnd)
//     {
//         const int INSIDE = 0; // Bitwise codes for the region
//         const int LEFT = 1;
//         const int RIGHT = 2;
//         const int BOTTOM = 4;
//         const int TOP = 8;

//         // Compute region codes for start and end points
//         int codeStart = ComputeCode(start, clipStart, clipEnd);
//         int codeEnd = ComputeCode(end, clipStart, clipEnd);

//         // Initialize line to be outside of clip window
//         bool accept = false;

//         while (true)
//         {
//             if ((codeStart == 0) && (codeEnd == 0))
//             {
//                 // Line segment is completely inside the clip window
//                 accept = true;
//                 break;
//             }
//             else if ((codeStart & codeEnd) != 0)
//             {
//                 // Line segment is completely outside the clip window
//                 break;
//             }
//             else
//             {
//                 // If line segment is not completely inside or outside, it must be partially inside
//                 int codeOut = (codeStart != 0) ? codeStart : codeEnd;

//                 // Find intersection point
//                 Vector2 intersection = Vector2.zero;

//                 if ((codeOut & TOP) != 0) // Intersection with top edge
//                 {
//                     intersection.x = start.x + (end.x - start.x) * (clipEnd.y - start.y) / (end.y - start.y);
//                     intersection.y = clipEnd.y;
//                 }
//                 else if ((codeOut & BOTTOM) != 0) // Intersection with bottom edge
//                 {
//                     intersection.x = start.x + (end.x - start.x) * (clipStart.y - start.y) / (end.y - start.y);
//                     intersection.y = clipStart.y;
//                 }
//                 else if ((codeOut & RIGHT) != 0) // Intersection with right edge
//                 {
//                     intersection.y = start.y + (end.y - start.y) * (clipEnd.x - start.x) / (end.x - start.x);
//                     intersection.x = clipEnd.x;
//                 }
//                 else if ((codeOut & LEFT) != 0) // Intersection with left edge
//                 {
//                     intersection.y = start.y + (end.y - start.y) * (clipStart.x - start.x) / (end.x - start.x);
//                     intersection.x = clipStart.x;
//                 }

//                 // Replace the point outside of the clip window with the intersection point
//                 if (codeOut == codeStart)
//                 {
//                     start = intersection;
//                     codeStart = ComputeCode(start, clipStart, clipEnd);
//                 }
//                 else
//                 {
//                     end = intersection;
//                     codeEnd = ComputeCode(end, clipStart, clipEnd);
//                 }
//             }
//         }

//         if (accept)
//         {
//             // Return the portion of the line segment inside the clip window
//             return start;
//         }
//         else
//         {
//             // Line segment is completely outside the clip window
//             return null;
//         }
//     }

//     // Helper function to compute the region code for a point
//     private static int ComputeCode(Vector2 point, Vector2 clipStart, Vector2 clipEnd)
//     {
//         int code = INSIDE; // Initialize as being inside of clip window

//         if (point.x < clipStart.x) // to the left of clip window
//             code |= LEFT;
//         else if (point.x > clipEnd.x) // to the right of clip window
//             code |= RIGHT;
//         if (point.y < clipStart.y) // below the clip window
//             code |= BOTTOM;
//         else if (point.y > clipEnd.y) // above the clip window
//             code |= TOP;

//         return code;
//     }



//     // public static (List<Vector2>, List<int>) ClipPolygon(List<Vector2> subjectPolygon, List<Vector2> clipPolygon)
//     // {
//     //     List<Vector2> outputList = new List<Vector2>(subjectPolygon);
//     //     List<int> triangles = new List<int>();

//     //     Debug.Log("Subject polygon vertices: " + string.Join(", ", subjectPolygon));
//     //     Debug.Log("Clip polygon vertices: " + string.Join(", ", clipPolygon));

//     //     foreach (Vector2 clipVertex in clipPolygon)
//     //     {
//     //         List<Vector2> inputList = new List<Vector2>(outputList);
//     //         outputList.Clear();

//     //         if (inputList.Count == 0)
//     //             break;

//     //         Vector2 S = inputList[inputList.Count - 1];

//     //         // foreach (Vector2 E in inputList)
//     //         // {
//     //         //     if (IsInside(E, clipPolygon))
//     //         //     {
//     //         //         if (!IsInside(S, clipPolygon))
//     //         //         {
//     //         //             Vector2 point = GetIntersection(S, E, clipVertex);
//     //         //             outputList.Add(point);
//     //         //         }
//     //         //         outputList.Add(E);
//     //         //     }
//     //         //     else if (IsInside(S, clipPolygon))
//     //         //     {
//     //         //         Vector2 point = GetIntersection(S, E, clipVertex);
//     //         //         outputList.Add(point);
//     //         //     }

//     //         //     S = E;
//     //         // }
//     //         foreach (Vector2 E in inputList)
//     //         {
//     //             Debug.Log("S: " + S);
//     //             Debug.Log("E: " + E);

//     //             if (IsInside(E, clipPolygon))
//     //             {
//     //                 if (!IsInside(S, clipPolygon))
//     //                 {
//     //                     Vector2 point = GetIntersection(S, E, clipVertex);
//     //                     Debug.Log("Intersection point: " + point);
//     //                     outputList.Add(point);
//     //                 }
//     //                 outputList.Add(E);
//     //             }
//     //             else if (IsInside(S, clipPolygon))
//     //             {
//     //                 Vector2 point = GetIntersection(S, E, clipVertex);
//     //                 Debug.Log("Intersection point: " + point);
//     //                 outputList.Add(point);
//     //             }

//     //             S = E;
//     //         }

//     //     }

//     //     // Generate triangles
//     //     for (int i = 1; i < outputList.Count - 1; i++)
//     //     {
//     //         triangles.Add(0);
//     //         triangles.Add(i);
//     //         triangles.Add(i + 1);
//     //     }

//     //     Debug.Log("Output vertices: " + string.Join(", ", outputList));
//     //     Debug.Log("Triangles: " + string.Join(", ", triangles));

//     //     return (outputList, triangles);
//     // }


//     // public static bool IsInside(Vector2 test, List<Vector2> polygon)
//     // {
//     //     bool inside = false;
//     //     int j = polygon.Count - 1;
//     //     for (int i = 0; i < polygon.Count; j = i++)
//     //     {
//     //         if (((polygon[i].y > test.y) != (polygon[j].y > test.y)) &&
//     //             (test.x < (polygon[j].x - polygon[i].x) * (test.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x))
//     //         {
//     //             inside = !inside;
//     //         }
//     //     }
//     //     return inside;
//     // }

//     // public static Vector2 GetIntersection(Vector2 p1, Vector2 p2, Vector2 clipVertex)
//     // {
//     //     Vector2 intersection;
//     //     float m = (p2.y - p1.y) / (p2.x - p1.x);

//     //     if (Mathf.Abs(p1.x - p2.x) < Mathf.Epsilon) // Vertical line
//     //     {
//     //         intersection = new Vector2(p1.x, clipVertex.y);
//     //     }
//     //     else
//     //     {
//     //         float b = p1.y - m * p1.x;
//     //         float y = m * clipVertex.x + b;
//     //         intersection = new Vector2(clipVertex.x, y);
//     //     }

//     //     return intersection;
//     // }
// }
