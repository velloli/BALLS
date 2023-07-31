using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Schmovement : MonoBehaviour
{
    private Rigidbody rb;
    private bool isPhasing = false;
    private bool isDashing = false;

    [NonSerialized]
    public bool isGrounded = true;
    public bool isWalled = false;
    public bool isTouchingEnemy = false;
    public bool doubleJumpAvailable = false;
    public bool freezeInput = false;
    public bool stompInProgress = false;
    public bool velocityOverride = false;
    public int enemiesTouching = 0;

    private List<string> conditions;
    public CircularBuffer<Vector3> positions;
    public GameObject Camerara;
    public float maxVelocity = 10.0f;
    Transform closestSelectedObject = null;


    [Header("Damage Vars")]
    public float bounceDMG = 10.0f;
    public float stompDMG = 70.0f;
    public float dashDMG = 5.0f;





    // Start is called before the first frame update
    void Start()
    {
        positions = new CircularBuffer<Vector3>(10);
        rb = GetComponent<Rigidbody>();
        Camerara = Camera.main.gameObject;
        GameManager.playerUnPhase += unphase;
        conditions = new List<string>();
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
            if ((isGrounded || isWalled || doubleJumpAvailable) && GameManager.Instance.jumpUnlocked)
            {
                float jumpForce = 10f;
                Vector3 jumpVelocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                rb.velocity = jumpVelocity;
                doMovement(Vector3.up);
                isGrounded = false;
                if(doubleJumpAvailable)
                {
                    doubleJumpAvailable = false;
                }
                //teleport mechanic
                //rb.velocity = Vector3.zero;
                //transform.position = new Vector3(transform.position.x, transform.position.y + 5.0f, transform.position.z);
            }
            else
            {
                doMovement(Vector3.zero);
            }

        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (!isGrounded)
            {
                stomp();
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            dash();
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

    void dash()
    {
        if(!isDashing)
        StartCoroutine(runDash());
        //ANITEJ
        //get the velocity
        //multiply the velocity with a global float defined on top of this file, so that we can tweak this float later
        //try impl

    }

    public void stopMovement()
    {
        freezeInput = true;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;
     
    }

    public void startMovement()
    {
        freezeInput = false;
        rb.constraints = RigidbodyConstraints.None;

    }

    private IEnumerator runDash(float interval = 0.2f)
    {
        velocityOverride = true;
        freezeInput = true;
        isDashing = true;
        rb.useGravity = false;
        Debug.Log("Boost started");


        //rb.velocity *= 3;

        rb.velocity = Vector3.zero;
        rb.AddForce((closestSelectedObject.position - transform.position).normalized * 1000.0f);

        yield return new WaitForSeconds(interval);

        velocityOverride = false;
        freezeInput = false;
        isDashing = false;
        rb.useGravity = true;

        Debug.Log("Boost ended");

    }

    void stomp()
    {
        if(!stompInProgress)
            StartCoroutine(freezePlayerCoroutine());
    }
    private IEnumerator freezePlayerCoroutine(float interval = 1)
    {
        stompInProgress = true;
        freezeInput = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationZ;
        rb.velocity = Vector3.up * 0.5f;
        Physics.IgnoreLayerCollision(6, 3,true);

        yield return new WaitForSeconds(interval);

        rb.useGravity = true;
        rb.velocity = Vector3.down * 20.0f;

        conditions.Add("end-stomp");

    }
    void executeConditions()
    {
        //Debug.Log("Conditions Size: " + conditions.Count);
        for (int i = conditions.Count - 1; i >= 0; i--)
        {
          
            if (conditions[i].Equals("end-stomp"))
            {
                if (isGrounded || isTouchingEnemy)
                {
                    freezeInput = false;
                    stompInProgress = false;
                    Physics.IgnoreLayerCollision(6, 3,false);
                    rb.constraints = RigidbodyConstraints.None;
                    conditions.RemoveAt(i);
                    //Debug.Log("Stomp Reset");


                }
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



        executeConditions();
        handleInput();
        //Debug.Log(sizeof(int));

    }

    private void LateUpdate()
    {
        precalc();
        positions.Push(this.transform.position);
    }

    void precalc()
    {
        //dash calc
        float radiusOfSphere = 10.0f;
        float smallesDistance = 0.0f;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radiusOfSphere);
        if (hitColliders.Length > 0)
        {
            smallesDistance = radiusOfSphere;
            foreach (Collider obj in hitColliders)
            {
                if (obj.gameObject.CompareTag("enemy"))
                {

                    float tempDistance = Vector3.Distance(obj.transform.position, transform.position);
                    if (tempDistance <= smallesDistance)
                    {
                        smallesDistance = tempDistance;
                        closestSelectedObject = obj.transform;
                    }
                }
            }
            /*other stuff that your code does*/
        }
        else
        {
            Debug.Log("NO COLLIDERS HIT");
        }
        if (closestSelectedObject)
        {
            Debug.DrawLine(transform.position, closestSelectedObject.position, Color.red);
            Debug.Log(closestSelectedObject.name);
        }
    }

    void doMovement(Vector3 dir)
    {
        //Vector3 dir = Camerara.transform.right.normalized;

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
        


        if (rb.velocity.magnitude > maxVelocity && !velocityOverride)
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

            enemiesTouching++;
            isTouchingEnemy = true;
            rb.velocity = Vector3.zero;
            rb.velocity = Vector3.up * 20.0f;
            doubleJumpAvailable = true;


            health healthScript = collision.collider.GetComponentInParent<health>();
            if (healthScript)
            {
                if (isGrounded)
                {
                    if (isDashing)
                    {
                        //Debug.Log("Dash DMG");
                        healthScript.TakeDamage(dashDMG);
                    }
                }
                else // u hit the enemy in mid-air
                {
                    

                    if (stompInProgress)
                    {
                        //Debug.Log("Stomp DMG");
                        if (healthScript.TakeDamage(stompDMG) == 0)
                        {
                            freezeInput = false;
                            stompInProgress = false;
                            Physics.IgnoreLayerCollision(6, 3, false);
                            rb.constraints = RigidbodyConstraints.None;
                            enemiesTouching--;
                            if (enemiesTouching <= 0)
                                isTouchingEnemy = false;
                        }
                        
                    }
                    else
                    {
                        healthScript.TakeDamage(bounceDMG);
                    }
                }
            }
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

        if (collision.collider.gameObject.CompareTag("enemy"))
        {
            enemiesTouching--;
            if(enemiesTouching <= 0)
                isTouchingEnemy = false;
        }
    }

}
