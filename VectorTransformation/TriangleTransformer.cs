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

        // calculate the normals of the starting and ending triangles
        Vector3 startNormal = CalcUnitNormal(startVertices);
        Vector3 endNormal = CalcUnitNormal(endVertices);

        // Rotation #1: Rotate the starting triangle to align its normal with the ending triangle's normal
        // the correct rotation axis is given by the cross product of the two normals
        Vector3 firstRotationAxis = Vector3.Cross(startNormal, endNormal).normalized;
        // and the correct rotation angle is given by the angle between the two normals
        float firstRotationAngle = Mathf.Acos(Vector3.Dot(startNormal, endNormal));

        // calculate the first quaternion and firstRotationMatrix corresponding to 
        // a rotation of firstRotationAngle around the firstRotationAxis
        Quaternion firstRotationQuaternion = Quaternion.AngleAxis(firstRotationAngle * Mathf.Rad2Deg, firstRotationAxis);
        firstRotationMatrix = Matrix4x4.Rotate(firstRotationQuaternion);

        // calculate the center of the ending triangle, which will also be the amount 
        // we need to translate the starting triangle after the rotations are complete
        centerVector = CalcCenter();

        // Rotation #2: Rotate the starting triangle around the common normal 
        // to align the first vertices of the two triangles (relative to their respective centers)
        var startFirstV = startVertices[0].normalized; // this triangle is already centered at the origin, so no need to subtract the center vector for it
        var endFirstV = (endVertices[0] - centerVector).normalized; // align the first vertex RELATIVE TO THE CENTER OF THE TRIANGLE

        // rotate the normal of the first triangle to match the normal of the second triangle before calculating the second rotation
        var partiallyRotatedStartFirstV = RotateVector(firstRotationMatrix, startFirstV).normalized;

        // the correct rotation axis for the second rotation is either parallel or anti-parallel 
        // to the common normal. Taking the cross product instead of simply using the common normal 
        // allows for the correct anti-parallel axis where appropriate.
        Vector3 secondRotationAxis = Vector3.Cross(partiallyRotatedStartFirstV, endFirstV).normalized;

        /* 
         If the partiallyRotatedStartFirstV and endFirstV are parallel or anti-parallel, the cross 
         product will be zero (or near-zero), and the rotation axis is essentially undefined. 
         If they are parallel, the secondRotationAngle will be zero, so any secondRotationAxis will do.
         If they are anti-parallel, the secondRotationAngle will be 180 degrees, so using 
         secondRotationAxis=endNormal or secondRotationAxis=-1*endNormal yield the same result.
         Therefore, in these cases, we can simply use the endNormal as the secondRotationAxis.
        */
        if (secondRotationAxis.magnitude < 0.0001f)
        {
            secondRotationAxis = endNormal;
        }

        // calc the second rotation angle
        var dot = Vector3.Dot(partiallyRotatedStartFirstV, endFirstV);
        float secondRotationAngle = Mathf.Acos(dot);

        // calculate the second quaternion and secondRotationMatrix corresponding to 
        // a rotation of secondRotationAngle around the secondRotationAxis
        Quaternion secondRotationQuaternion = Quaternion.AngleAxis(secondRotationAngle * Mathf.Rad2Deg, secondRotationAxis);
        secondRotationMatrix = Matrix4x4.Rotate(secondRotationQuaternion);
        this.combinedRotationMatrix = Matrix4x4.Rotate(secondRotationQuaternion * firstRotationQuaternion);
    }

    private Vector3 CalcCenter()
    {
        // Calculates the center of the ending triangle.
        // This is also the amount we need to translate 
        // the starting triangle after the rotations are complete
        Vector3 totV = Vector3.zero;
        for (int i = 0; i < 3; i++)
        {
            totV += endVertices[i];
        }
        return totV / 3;
    }

    private Vector3 CalcUnitNormal(Vector3[] triangleVertices)
    {
        // calculate the (normalized) normal of the triangle formed by the three vertices
        Vector3 edge1 = triangleVertices[0] - triangleVertices[1];
        Vector3 edge2 = triangleVertices[1] - triangleVertices[2];
        return Vector3.Cross(edge1, edge2).normalized;
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

    public IEnumerable<Vector3> TransformVectors(IEnumerable<Vector3> vectorsToTransform)
    {
        // rotate and translate the vectors in vectorsToTransform according to 
        // the transformation that maps the starting triangle to the ending triangle
        foreach (Vector3 vectorToTransform in vectorsToTransform)
        {
            yield return TransformVector(combinedRotationMatrix, centerVector, vectorToTransform);
        }
    }
}