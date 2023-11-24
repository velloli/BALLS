
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class OrientationVisualizer : MonoBehaviour
{
    [SerializeField][Range(0.5F, 2)] private float cubeSize = 1.0F;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public static Vector3 WeightedAverage(Vector3 vector1, Vector3 vector2, float weight1, float weight2)
    {
        if (weight1 + weight2 == 0)
        {
            Debug.LogError("Total weight is zero. Returning Vector3.zero.");
            return Vector3.zero;
        }

        float totalWeight = weight1 + weight2;
        float weight1Normalized = weight1 / totalWeight;
        float weight2Normalized = weight2 / totalWeight;

        Vector3 weightedAverage = (vector1 * weight1Normalized + vector2 * weight2Normalized)/2;
        return weightedAverage;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        var position = transform.position;
        var velocity = _rigidbody.velocity;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraForwardParallelToGround = new Vector3(cameraForward.x, 0f, cameraForward.z).normalized;

        var cubePosition = position;


        Vector3 finalLookVector = WeightedAverage(cameraForwardParallelToGround.normalized, velocity.normalized, 0.9f, 0.1f);
        Quaternion cubeRot = Quaternion.LookRotation(finalLookVector, Vector3.up);

        Gizmos.matrix = Matrix4x4.TRS(cubePosition, cubeRot, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(cubeSize, cubeSize, cubeSize));
        Gizmos.matrix = Matrix4x4.identity; // Reset the matrix to avoid affecting other Gizmos

        Handles.color = Color.green;
        Handles.ArrowHandleCap(0, position, cubeRot, 1, EventType.Repaint);

    }
}
