using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;

    public Vector3 velocity = Vector3.zero; // SmoothDamp velocity
    private Vector3 initialOffset; // Initial offset based on starting positions

    void Start()
    {
        if (target != null)
        {
            initialOffset = transform.position - target.position;
        }
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + initialOffset;
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
            transform.position = smoothedPosition;

            transform.LookAt(Camera.main.transform);
        }
    }
}