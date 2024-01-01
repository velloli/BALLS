using UnityEngine;

public class justSchmovement : MonoBehaviour
{
    private Rigidbody rb;
    public bool isGrounded = true;
    public bool isWalled = false;
    private bool freezeInput = false;

    public GameObject Camerara;
    public float maxVelocity = 10.0f;
    public Vector3 movementDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Camerara = Camera.main.gameObject;
    }

    void handleInput()
    {
        movementDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movementDirection += Camerara.transform.forward.normalized;
        }
        if (Input.GetKey(KeyCode.S))
        {
            movementDirection -= Camerara.transform.forward.normalized;
        }
        if (Input.GetKey(KeyCode.A))
        {
            movementDirection -= Camerara.transform.right.normalized;
        }
        if (Input.GetKey(KeyCode.D))
        {
            movementDirection += Camerara.transform.right.normalized;
        }

        // Ensure the movementDirection vector is normalized to maintain consistent speed
        if (movementDirection != Vector3.zero)
        {
            movementDirection.y = 0;
            doMovement(movementDirection.normalized);
        }

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded))
        {
            float jumpForce = 10f;
            Vector3 jumpVelocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            rb.velocity = jumpVelocity;
            doMovement(Vector3.up);
            isGrounded = false;
        }

        if (!Input.anyKey)
        {
            doMovement(Vector3.zero);
        }
    }

    void doMovement(Vector3 dir)
    {
        if (freezeInput)
            return;

        rb.useGravity = true;

        if (isWalled && dir == Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }

        if (!isGrounded)
        {
            return;
        }

        Vector3 desiredVelocity = dir.normalized * maxVelocity;

        Vector3 velocityChange = desiredVelocity - rb.velocity;
        velocityChange.y += rb.velocity.y;
        rb.AddForce(velocityChange * 0.5f);

        //Debug.Log(velocityChange.magnitude);

        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleInput();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }

        if (collision.collider.gameObject.CompareTag("wall"))
        {
            isWalled = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

        if (collision.collider.gameObject.CompareTag("wall"))
        {
            isWalled = false;
        }
    }
}