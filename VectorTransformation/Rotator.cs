using UnityEngine;

// using System.Numerics;

public class Rotator
{
    private Matrix4x4 rotationMatrix;
    public Rotator(Vector3 origNormal, Vector3 newNormal)
    {
        // Normalize input vectors
        origNormal = origNormal.normalized;
        newNormal = newNormal.normalized;

        // Step 1: Calculate the rotation axis
        Vector3 rotationAxis = Vector3.Cross(origNormal, newNormal).normalized;

        // Step 2: Calculate the rotation angle
        float rotationAngle = Mathf.Acos(Vector3.Dot(origNormal, newNormal));

        // Step 3: Construct the rotation quaternion
        Quaternion rotationQuaternion = Quaternion.AngleAxis(rotationAngle * Mathf.Rad2Deg, rotationAxis);

        // Step 4: Convert the quaternion to a rotation matrix
        rotationMatrix = Matrix4x4.Rotate(rotationQuaternion);
    }

    // Function to rotate a Vector3 using the rotation matrix
    public Vector3 RotateVector(Vector3 vectorToRotate)
    {
        return rotationMatrix.MultiplyPoint(vectorToRotate);
    }

    public void RotateMeshData(MeshData meshData)
    {
        for (int i = 0; i < meshData.vertices.Count; i++)
        {
            meshData.vertices[i] = rotationMatrix.MultiplyPoint(meshData.vertices[i]);
        }
    }
}