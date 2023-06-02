using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CamerMovement : MonoBehaviour
{
    public GameObject follower;
    public bool viewOnAwake = true;
    public float smoothTime = 0.05f;
    public float rotationSpeed = 5f;
    public Vector3 offset ;

    private Vector3 velocity = Vector3.zero;


    private void Awake()
    {
        if (viewOnAwake)
        {
            transform.position = follower.transform.position;
            transform.position += offset;
            transform.LookAt(follower.transform.position);
        }
    }
    void FixedUpdate()
    {
        Vector3 targetPosition = follower.transform.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        Quaternion targetRotation = Quaternion.LookRotation(follower.transform.position - transform.position);

        // Smoothly rotate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
