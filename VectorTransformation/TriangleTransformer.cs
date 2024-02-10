using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriangleTransformer
{
    public Vector3[] startVertices;
    public Vector3[] endVertices;
    public Matrix4x4 firstRotationMatrix;
    public Matrix4x4 secondRotationMatrix;
    public Matrix4x4 combinedRotationMatrix;
    public Vector3 centerVector;
    public TriangleTransformer(Vector3[] startVertices, Vector3[] endVertices)
    {
        this.startVertices = startVertices;
        this.endVertices = endVertices;
        Vector3 startNormal = CalcUnitNormal(startVertices);
        Vector3 endNormal = CalcUnitNormal(endVertices);

        // Debug.Log($"Start Normal: {startNormal}");
        // Debug.Log($"End Normal: {endNormal}");

        Vector3 rotationAxis = Vector3.Cross(startNormal, endNormal).normalized;

        float rotationAngle = Mathf.Acos(Vector3.Dot(startNormal, endNormal));

        Quaternion firstRotationQuaternion = Quaternion.AngleAxis(rotationAngle * Mathf.Rad2Deg, rotationAxis);
        firstRotationMatrix = Matrix4x4.Rotate(firstRotationQuaternion);

        // Debug.Log($"Rotation Axis: {rotationAxis}");
        // Debug.Log($"Rotation Angle: {rotationAngle}");

        centerVector = CalcCenter();

        var startFirstV = startVertices[0].normalized;
        var endFirstV = (endVertices[0] - centerVector).normalized;
        var partiallyRotatedStartFirstV = RotateVector(firstRotationMatrix, startFirstV).normalized;

        Vector3 secondRotationAxis = Vector3.Cross(partiallyRotatedStartFirstV, endFirstV).normalized;

        var dot = Vector3.Dot(partiallyRotatedStartFirstV, endFirstV);

        const float epsilon = 0.0001f; // Define a small tolerance
        if (secondRotationAxis.magnitude < epsilon)
        {
            secondRotationAxis = endNormal;
            // Handle the case where the rotation axis is close to zero
        }

        float secondRotationAngle = Mathf.Acos(dot);
        Quaternion secondRotationQuaternion = Quaternion.AngleAxis(secondRotationAngle * Mathf.Rad2Deg, secondRotationAxis);
        secondRotationMatrix = Matrix4x4.Rotate(secondRotationQuaternion);
        this.combinedRotationMatrix = Matrix4x4.Rotate(secondRotationQuaternion * firstRotationQuaternion);

        // Debug.Log($"Start First V: {startFirstV}");
        // Debug.Log($"End First V: {endFirstV}");
        // Debug.Log($"centerVector: {centerVector}");
        // Debug.Log($"Second Rotation Axis: {secondRotationAxis}");
        // Debug.Log($"Partially Rotated Start First Edge: {partiallyRotatedStartFirstV}");
        // Debug.Log($"Second Rotation Angle: {secondRotationAngle}");
    }
    private Vector3 CalcCenter()
    {
        Vector3 totV = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            totV += endVertices[i];
        }
        return totV / 3;
    }

    private Vector3 RotateVector(Matrix4x4 rotationMatrix, Vector3 vectorToRotate)
    {
        return rotationMatrix.MultiplyPoint(vectorToRotate);
    }

    private Vector3 TranslateVector(Vector3 v, Vector3 translationV)
    {
        return v + translationV;
    }

    private Vector3 TransformVector(Matrix4x4 rotationMatrix, Vector3 translationV, Vector3 vectorToTransform)
    {
        var rotatedV = RotateVector(rotationMatrix, vectorToTransform);
        var transformedV = TranslateVector(rotatedV, translationV);
        return transformedV;
    }

    private Vector3 CalcUnitNormal(Vector3[] triangleVertices)
    {
        Vector3 edge1 = triangleVertices[0] - triangleVertices[1];
        Vector3 edge2 = triangleVertices[1] - triangleVertices[2];
        return Vector3.Cross(edge1, edge2).normalized;
    }

    // public void RotateVectors(Matrix4x4 rotationMatrix, Vector3[] vectorsToRotate)
    // {
    //     for (int i = 0; i < vectorsToRotate.Length; i++)
    //     {
    //         vectorsToRotate[i] = RotateVector(rotationMatrix, vectorsToRotate[i]);
    //     }
    //     // return rotationMatrix.MultiplyPoint(vectorToRotate);

    // }

    public IEnumerable<Vector3> TransformVectors(IEnumerable<Vector3> vectorsToTransform)
    {
        foreach (Vector3 vectorToTransform in vectorsToTransform)
        {
            yield return TransformVector(combinedRotationMatrix, centerVector, vectorToTransform);
        }
    }



    // public MeshData2 BuildMesh(MeshData meshData)
    // {
    //     // var meshData = new MeshData();

    //     // startVs before rotation
    //     List<int> vIdxs = new();
    //     meshData.AddVertex(startVertices[0], vIdxs);
    //     meshData.AddVertex(startVertices[1], vIdxs);
    //     meshData.AddVertex(startVertices[2], vIdxs);
    //     meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);


    //     // startVs after rotation
    //     RotateVectors(firstRotationMatrix, startVertices);
    //     // RotateVectors(firstRotationMatrix, startVertices);

    //     for (int i = 0; i < 3; i++)
    //     {
    //         Debug.Log($"After 1st rotation: start vertex #{i}: {startVertices[i]}");
    //     }
    //     vIdxs = new();
    //     meshData.AddVertex(startVertices[0], vIdxs);
    //     meshData.AddVertex(startVertices[1], vIdxs);
    //     meshData.AddVertex(startVertices[2], vIdxs);
    //     meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);

    //     TransformVectors(secondRotationMatrix, centerVector, startVertices);
    //     // RotateVectors(secondRotationMatrix, startVertices);
    //     // RotateVectors(firstRotationMatrix, startVertices);

    //     for (int i = 0; i < 3; i++)
    //     {
    //         Debug.Log($"After 2nd rotation: start vertex #{i}: {startVertices[i]}");
    //     }
    //     vIdxs = new();
    //     meshData.AddVertex(startVertices[0], vIdxs);
    //     meshData.AddVertex(startVertices[1], vIdxs);
    //     meshData.AddVertex(startVertices[2], vIdxs);
    //     meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);

    //     // endVs, purposely unchanged
    //     vIdxs = new();
    //     meshData.AddVertex(endVertices[0], vIdxs);
    //     meshData.AddVertex(endVertices[1], vIdxs);
    //     meshData.AddVertex(endVertices[2], vIdxs);
    //     meshData.AddTriangleIdxs(vIdxs[0], vIdxs[1], vIdxs[2]);

    //     return meshData;
    // }
}