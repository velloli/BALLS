using UnityEngine;

public class MidAirStrafe : MonoBehaviour
{
    private Rigidbody rb;
    public bool midAirStrafeUnlocked = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void handleMidAirStrafe()
    {
        float maxVelocity = GetComponent<justSchmovement>().maxVelocity;
        Vector3 dir = GetComponent<justSchmovement>().movementDirection;

        if (!midAirStrafeUnlocked)
            return;

        if (!GetComponent<justSchmovement>().isGrounded)
        {
            Vector3 desiredVelocity = dir.normalized * GetComponent<justSchmovement>().maxVelocity;
            Vector3 velocityChange = desiredVelocity - rb.velocity;
            velocityChange.y += rb.velocity.y;
            rb.AddForce(velocityChange * 0.5f);
            if (rb.velocity.magnitude > maxVelocity)
            {
                rb.velocity = rb.velocity.normalized * maxVelocity;
            }
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        handleMidAirStrafe();
    }
}