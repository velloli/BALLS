using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Schmovement : MonoBehaviour
{
    private Rigidbody rb;
    private bool isPhasing = false;
    private bool isGrounded = true;
    private bool isWalled = false;


    public GameObject Camerara;
    public float maxVelocity = 10.0f;




    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Camerara = Camera.main.gameObject;
        GameManager.playerUnPhase += unphase;
    }

    void unphase()
    {
        isPhasing = false;
    }
    void handleInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 dir = Camerara.transform.forward.normalized;
            dir.y = 0;
            doMovement(dir.normalized);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 dir = -Camerara.transform.forward.normalized;
            dir.y = 0;

            doMovement(dir.normalized);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Vector3 dir = -Camerara.transform.right.normalized;

            doMovement(dir);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 dir = Camerara.transform.right.normalized;
            doMovement(dir);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if ((isGrounded || isWalled) && GameManager.Instance.jumpUnlocked)
            {
                float jumpForce = 10f;
                Vector3 jumpVelocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                rb.velocity = jumpVelocity;
                doMovement(Vector3.up);
                isGrounded = false;
                //teleport mechanic
                //rb.velocity = Vector3.zero;
                //transform.position = new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z);
            }
            else
            {
                doMovement(Vector3.zero);
            }

        }
            if (!Input.anyKey)
        {
            doMovement(Vector3.zero);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)  && GameManager.Instance.phaseUnlocked)
        {
            if (!isPhasing)
            {
                isPhasing = true;
                phase();
            }
        }


    }

    void phase()
    {
        //Time.timeScale = 0.5f;
        GameManager.Instance.ExecutePhase();
    }
    void Bugger()
    {
        Debug.Log(rb.velocity.y);

    }

    // Update is called once per frame
    void Update()
    {

        //Bugger();
        //Debug.Log("Walled "+ isWalled);
        //Debug.Log("Ground " + isGrounded);

        handleInput();
    }

    void doMovement(Vector3 dir)
    {
        //Vector3 dir = Camerara.transform.right.normalized;
        rb.useGravity = true;

        if (isWalled && dir == Vector3.zero)
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = false;
        }
        else
        {
            //Debug.Log("Use Gravity");
            rb.useGravity = true;
        }
        if (!isGrounded && !GameManager.Instance.jumpStrafeUnlocked)
        {
            return;
        }

        Vector3 desiredVelocity = dir.normalized * maxVelocity;


        Vector3 velocityChange = desiredVelocity - rb.velocity;
        velocityChange.y += rb.velocity.y;
        rb.AddForce(velocityChange * 0.5f);
        


        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = rb.velocity.normalized * maxVelocity;
        }

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

        if (collision.collider.gameObject.CompareTag("enemy"))
        {
            //apply damage based on velocity
            rb.velocity = Vector3.zero;
            rb.velocity = Vector3.up * 10.0f;
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
