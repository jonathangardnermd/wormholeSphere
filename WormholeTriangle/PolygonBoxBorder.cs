



using UnityEngine;
using System.Linq;
// using Math;
public class PolygonBounds
{
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;
    // private Vector2[] vertices;
    // public float[] angularUvs;

    // private float angle;

    // public float vertexRadius;

    public PolygonBounds(float minX, float minY, float maxX, float maxY)
    {
        this.minX = minX;
        this.minY = minY;
        this.maxX = maxX;
        this.maxY = maxY;
        // this.numSides = numSides;
        // SetUnitVertices();
    }

    // public Vector2[] GetVertices(float vertexRadius)
    // {
    //     return vertices.Select(v => v * vertexRadius).ToArray();
    // }

    // void SetUnitVertices() // vertices will be exactly 1 unit from the origin (by rotating the point <x=1,y=0> around the origin)
    // {
    //     angle = 2 * Mathf.PI / numSides;
    //     vertices = new Vector2[numSides];
    //     angularUvs = new float[numSides];

    //     for (int i = 0; i < numSides; i++)
    //     {
    //         // rotate the pt <1,0> by the rotationAngle = i * angle to form the ith vertex of the unit polygon
    //         float x = Mathf.Cos(i * angle);
    //         float y = Mathf.Sin(i * angle);
    //         vertices[i] = new Vector2(x, y);
    //         angularUvs[i] = (float)i / numSides;
    //     }
    // }

    // public MeshData GetMeshData(float vertexRadius)
    // {
    //     var meshData = new MeshData();

    //     var originVertex = new Vector3(0, 0, 0);
    //     var originIdx = meshData.AddVertex(originVertex, new Vector2(0, 0));
    //     var sizedVertices = GetVertices(vertexRadius);
    //     for (int i1 = 0; i1 < numSides; i1++)
    //     {
    //         var i2 = (i1 + 1) % numSides;
    //         var v1 = sizedVertices[i1];
    //         var v2 = sizedVertices[i2];

    //         var vIdx1 = meshData.AddVertex(v1, new Vector2(1, angularUvs[i1]));
    //         var vIdx2 = meshData.AddVertex(v2, new Vector2(1, angularUvs[i2]));
    //         meshData.AddTriangleIdxs(0, vIdx1, vIdx2);
    //     }
    //     return meshData;

    //     // return meshData;
    // }
}
public static class PolygonBoxBorder
{

    public static PolygonBounds GetPolygonBounds(Vector2[] polygonVertices)
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;

        // Find the minimum and maximum x and y coordinates
        foreach (Vector2 vertex in polygonVertices)
        {
            minX = Mathf.Min(minX, vertex.x);
            minY = Mathf.Min(minY, vertex.y);
            maxX = Mathf.Max(maxX, vertex.x);
            maxY = Mathf.Max(maxY, vertex.y);
        }

        return new PolygonBounds(minX, minY, maxX, maxY);
    }
    public static Vector2[] GetBoundingSquare(PolygonBounds polyBounds)
    {
        // Calculate the other three vertices of the bounding square
        var boundingSquareVertices = new Vector2[4];
        boundingSquareVertices[0] = new Vector2(polyBounds.maxX, polyBounds.maxY);
        boundingSquareVertices[1] = new Vector2(polyBounds.minX, polyBounds.maxY);
        boundingSquareVertices[2] = new Vector2(polyBounds.minX, polyBounds.minY);
        boundingSquareVertices[3] = new Vector2(polyBounds.maxX, polyBounds.minY);

        // Do something with boundingSquareVertices...
        return boundingSquareVertices;
    }

    public static PolygonBounds GetBoundingSquareMeshWithHole(MeshData meshData, Vector2[] polygonVertices)
    {
        var polyBounds = GetPolygonBounds(polygonVertices);
        var boundingVerts = GetBoundingSquare(polyBounds);

        var numSides = polygonVertices.Length;
        float quadIdxSize = numSides / 4f;
        // Debug.Log($"quadIdxSize quadIdxSize={quadIdxSize}");
        for (int i1 = 0; i1 < numSides; i1++)
        {
            var i2 = (i1 + 1) % numSides;
            var v1 = polygonVertices[i1];
            var v2 = polygonVertices[i2];

            int quad1 = (int)Mathf.Floor(i1 / quadIdxSize);
            int quad2 = (int)Mathf.Floor(i2 / quadIdxSize);
            var isAmbiguous2 = (i2 % quadIdxSize) == 0;
            if (isAmbiguous2 || quad1 == quad2)
            {
                var boundingVert = boundingVerts[quad1];
                int vIdx1 = meshData.AddVertex(v1, new Vector2(0, 0));
                int vIdx2 = meshData.AddVertex(v2, new Vector2(0, 0));
                int vIdx3 = meshData.AddVertex(boundingVert, new Vector2(0, 0));
                meshData.AddTriangleIdxs(vIdx2, vIdx1, vIdx3); // note the ORDER!
            }
        }
        // return meshData;
        return polyBounds;
    }
}

// public class BoxedPolygon
// {
//     public Vector3[] vertices;
//     public Vector3[] center;

//     public Vector3 unitNormal;

//     public BoxedPolygon(Vector3 center, Vector3 hDirection, int numSides, )
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