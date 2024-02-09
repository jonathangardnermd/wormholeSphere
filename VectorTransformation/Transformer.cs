using UnityEngine;

public class Transformer
{
    private Matrix4x4 transformationMatrix;

    public Transformer(Vector3 origNormal, Vector3 newNormal, Vector3 translation)
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

        // Step 4: Construct the transformation matrix with rotation and translation
        transformationMatrix = Matrix4x4.TRS(translation, rotationQuaternion, Vector3.one);
    }

    // Function to transform a Vector3 using the transformation matrix
    public Vector3 TransformVector(Vector3 vectorToTransform)
    {
        return transformationMatrix.MultiplyPoint(vectorToTransform);
    }

    public void TransformMeshData(MeshData meshData, int startIdx)
    {
        for (int i = startIdx; i < meshData.vertices.Count; i++)
        {
            meshData.vertices[i] = transformationMatrix.MultiplyPoint(meshData.vertices[i]);
        }
    }


    public static Quaternion ComputeAlignmentQuaternion(Vector3[] triangle1Vertices, Vector3[] triangle2Vertices)
    {
        // Compute the rotation quaternion
        Quaternion rotationQuaternion = Quaternion.FromToRotation(triangle1Vertices[1] - triangle1Vertices[0], triangle2Vertices[1] - triangle2Vertices[0]);

        return rotationQuaternion;
    }
}
