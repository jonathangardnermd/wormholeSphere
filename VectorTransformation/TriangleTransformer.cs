using UnityEngine;


// using System.Numerics;

public class TriangleTransformer
{
    Vector3[] startVertices;
    Vector3[] endVertices;
    Matrix4x4 combinedRotationMatrix;
    public TriangleTransformer(Vector3[] startVertices, Vector3[] endVertices)
    {
        Vector3 startNormal = CalcUnitNormal(startVertices);
        Vector3 endNormal = CalcUnitNormal(endVertices);

        // rotate normals to align
        // Step 1: Calculate the rotation axis
        Vector3 rotationAxis = Vector3.Cross(startNormal, endNormal).normalized;

        // Step 2: Calculate the rotation angle
        float rotationAngle = Mathf.Acos(Vector3.Dot(startNormal, endNormal));

        // Step 3: Construct the rotation quaternion
        Quaternion rotationQuaternion = Quaternion.AngleAxis(rotationAngle * Mathf.Rad2Deg, rotationAxis);

        // Step 4: Construct the transformation matrix with rotation to align the normal axes
        var firstRotationMatrix = Matrix4x4.Rotate(rotationQuaternion);


        // rotate around normal to align

        // Step 1: rotate the first edge of the startTriangle to match up the normals
        var startFirstEdge = startVertices[0] - startVertices[1];
        var endFirstEdge = endVertices[0] - endVertices[1];
        var partiallyRotatedStartFirstEdge = RotateVector(firstRotationMatrix, startFirstEdge);

        // Step 2: calculate the angle between the first edges
        float secondRotationAngle = Mathf.Acos(Vector3.Dot(partiallyRotatedStartFirstEdge, endFirstEdge));

        // Step 3: Construct the rotation quaternion using the endNormal as the rotation axis
        Quaternion secondRotationQuaternion = Quaternion.AngleAxis(secondRotationAngle * Mathf.Rad2Deg, endNormal);

        // Step 4: Construct the transformation matrix with rotation to align the normal axes
        var secondRotationMatrix = Matrix4x4.Rotate(secondRotationQuaternion);

        // Combine the two rotation quaternions
        Quaternion combinedRotation = secondRotationQuaternion * rotationQuaternion;

        // Convert the combined rotation quaternion to a rotation matrix
        combinedRotationMatrix = Matrix4x4.Rotate(combinedRotation);
        // translate
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
}