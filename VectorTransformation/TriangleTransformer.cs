using System.Collections.Generic;
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

        Vector3 rotationAxis = Vector3.Cross(startNormal, endNormal).normalized;

        float rotationAngle = Mathf.Acos(Vector3.Dot(startNormal, endNormal));

        Quaternion firstRotationQuaternion = Quaternion.AngleAxis(rotationAngle * Mathf.Rad2Deg, rotationAxis);
        firstRotationMatrix = Matrix4x4.Rotate(firstRotationQuaternion);

        centerVector = CalcCenter();

        var startFirstV = startVertices[0].normalized;
        var endFirstV = (endVertices[0] - centerVector).normalized;
        var partiallyRotatedStartFirstV = RotateVector(firstRotationMatrix, startFirstV).normalized;

        Vector3 secondRotationAxis = Vector3.Cross(partiallyRotatedStartFirstV, endFirstV).normalized;

        var dot = Vector3.Dot(partiallyRotatedStartFirstV, endFirstV);

        const float epsilon = 0.0001f; // Define a small tolerance
        if (secondRotationAxis.magnitude < epsilon)
        {
            // Handle the case where the rotation axis is close to zero bc the vectors are parallel/anti-parallel
            secondRotationAxis = endNormal;
        }

        float secondRotationAngle = Mathf.Acos(dot);
        Quaternion secondRotationQuaternion = Quaternion.AngleAxis(secondRotationAngle * Mathf.Rad2Deg, secondRotationAxis);
        secondRotationMatrix = Matrix4x4.Rotate(secondRotationQuaternion);
        this.combinedRotationMatrix = Matrix4x4.Rotate(secondRotationQuaternion * firstRotationQuaternion);
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

    public IEnumerable<Vector3> TransformVectors(IEnumerable<Vector3> vectorsToTransform)
    {
        foreach (Vector3 vectorToTransform in vectorsToTransform)
        {
            yield return TransformVector(combinedRotationMatrix, centerVector, vectorToTransform);
        }
    }
}