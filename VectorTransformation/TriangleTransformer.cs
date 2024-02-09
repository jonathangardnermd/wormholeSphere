using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


// using System.Numerics;

public class TriangleTransformer
{
    Vector3[] startVertices;
    Vector3[] endVertices;
    Matrix4x4 firstRotationMatrix;
    Matrix4x4 secondRotationMatrix;
    Matrix4x4 combinedRotationMatrix;
    public TriangleTransformer(Vector3[] startVertices, Vector3[] endVertices)
    {
        this.startVertices = startVertices;
        this.endVertices = endVertices;
        Vector3 startNormal = CalcUnitNormal(startVertices);
        Vector3 endNormal = CalcUnitNormal(endVertices);

        Debug.Log($"Start Normal: {startNormal}");
        Debug.Log($"End Normal: {endNormal}");

        // rotate normals to align
        // Step 1: Calculate the rotation axis
        Vector3 rotationAxis = Vector3.Cross(startNormal, endNormal).normalized;

        // Step 2: Calculate the rotation angle
        float rotationAngle = Mathf.Acos(Vector3.Dot(startNormal, endNormal));

        // Step 3: Construct the rotation quaternion
        // Step 4: Construct the transformation matrix with rotation to align the normal axes
        Quaternion firstRotationQuaternion = Quaternion.AngleAxis(rotationAngle * Mathf.Rad2Deg, rotationAxis);
        firstRotationMatrix = Matrix4x4.Rotate(firstRotationQuaternion);

        Debug.Log($"Rotation Axis: {rotationAxis}");
        Debug.Log($"Rotation Angle: {rotationAngle}");

        // rotate around normal to align

        // Step 1: rotate the first edge of the startTriangle to match up the normals
        var startFirstEdge = (startVertices[0] - startVertices[1]).normalized;
        var endFirstEdge = (endVertices[0] - endVertices[1]).normalized;
        var partiallyRotatedStartFirstEdge = RotateVector(firstRotationMatrix, startFirstEdge);
        partiallyRotatedStartFirstEdge = partiallyRotatedStartFirstEdge.normalized;

        // Step 2: calculate the angle between the first edges
        float secondRotationAngle = Mathf.Acos(Vector3.Dot(partiallyRotatedStartFirstEdge, endFirstEdge));

        // Step 3: Construct the rotation quaternion using the endNormal as the rotation axis
        Quaternion secondRotationQuaternion = Quaternion.AngleAxis(secondRotationAngle * Mathf.Rad2Deg, endNormal);
        secondRotationMatrix = Matrix4x4.Rotate(secondRotationQuaternion);

        this.combinedRotationMatrix = Matrix4x4.Rotate(secondRotationQuaternion * firstRotationQuaternion);

        Debug.Log($"Start First Edge: {startFirstEdge}");
        Debug.Log($"End First Edge: {endFirstEdge}");
        Debug.Log($"Partially Rotated Start First Edge: {partiallyRotatedStartFirstEdge}");
        Debug.Log($"Second Rotation Angle: {secondRotationAngle}");

        // Step 4: Construct the transformation matrix with rotation to align the normal axes
        // var secondRotationMatrix = Matrix4x4.Rotate(secondRotationQuaternion);

        // Combine the two rotation quaternions
        // Quaternion combinedRotation = ;

        // Convert the combined rotation quaternion to a rotation matrix
    }

    public Vector3 CalcUnitNormal(Vector3[] triangleVertices)
    {
        Vector3 edge1 = triangleVertices[0] - triangleVertices[1];
        Vector3 edge2 = triangleVertices[1] - triangleVertices[2];
        return Vector3.Cross(edge1, edge2).normalized;
    }

    public Vector3 RotateVector(Matrix4x4 rotationMatrix, Vector3 vectorToRotate)
    {
        return rotationMatrix.MultiplyPoint(vectorToRotate);
    }

    public void RotateVectors(Matrix4x4 rotationMatrix, Vector3[] vectorsToRotate)
    {
        for (int i = 0; i < vectorsToRotate.Length; i++)
        {
            vectorsToRotate[i] = rotationMatrix.MultiplyPoint(vectorsToRotate[i]);
        }
        // return rotationMatrix.MultiplyPoint(vectorToRotate);
    }

    public MeshData BuildMesh()
    {
        var meshData = new MeshData();

        // startVs before rotation
        List<int> vIdxs = new();
        meshData.AddVertex(startVertices[0], vIdxs);
        meshData.AddVertex(startVertices[1], vIdxs);
        meshData.AddVertex(startVertices[2], vIdxs);
        meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);


        // startVs after rotation
        RotateVectors(firstRotationMatrix, startVertices);
        // RotateVectors(firstRotationMatrix, startVertices);

        for (int i = 0; i < 3; i++)
        {
            Debug.Log($"After 1st rotation: start vertex #{i}: {startVertices[i]}");
        }
        vIdxs = new();
        meshData.AddVertex(startVertices[0], vIdxs);
        meshData.AddVertex(startVertices[1], vIdxs);
        meshData.AddVertex(startVertices[2], vIdxs);
        meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);

        RotateVectors(secondRotationMatrix, startVertices);
        // RotateVectors(firstRotationMatrix, startVertices);

        for (int i = 0; i < 3; i++)
        {
            Debug.Log($"After 2nd rotation: start vertex #{i}: {startVertices[i]}");
        }
        vIdxs = new();
        meshData.AddVertex(startVertices[0], vIdxs);
        meshData.AddVertex(startVertices[1], vIdxs);
        meshData.AddVertex(startVertices[2], vIdxs);
        meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);

        // endVs, purposely unchanged
        vIdxs = new();
        meshData.AddVertex(endVertices[0], vIdxs);
        meshData.AddVertex(endVertices[1], vIdxs);
        meshData.AddVertex(endVertices[2], vIdxs);
        meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);

        return meshData;
    }
}