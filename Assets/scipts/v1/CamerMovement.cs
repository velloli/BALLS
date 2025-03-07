using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CamerMovement : MonoBehaviour
{
    // Is true when the user wants to rotate the camera
    public bool holdUp = false;
    public float rotationSpeed = 1f;
    public float radius = 10f;
    public float mouseSens = 20.0f;
    public GameObject SPHERE;


    // The mouse cursor's position during the last frame
    Vector3 last = new Vector3();
    // The target that the camera looks at
    Vector3 target = new Vector3(0, 0, 0);
    // The spherical coordinates
    Vector3 sc = new Vector3();


    void Start()
    {
        if(SPHERE == null)
        {
            SPHERE = GameObject.FindGameObjectWithTag("Player");
            if(SPHERE == null)
            {
                Debug.LogWarning("CAMERA IS UNABLE TO FIND PLAYER");
                GetComponent<CamerMovement>().enabled = false;
                return;
            }
        }
        target = SPHERE.transform.position;
        this.transform.position = new Vector3(radius, 0.0f, 0.0f);
        this.transform.LookAt(target);
        sc = getSphericalCoordinates(this.transform.position);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    Vector3 getSphericalCoordinates(Vector3 cartesian)
    {
        float r = Mathf.Sqrt(
            Mathf.Pow(cartesian.x, 2) +
            Mathf.Pow(cartesian.y, 2) +
            Mathf.Pow(cartesian.z, 2)
        );

        float phi = Mathf.Atan2(cartesian.z / cartesian.x, cartesian.x);
        float theta = Mathf.Acos(cartesian.y / r);

        if (cartesian.x < 0)
            phi += Mathf.PI;

        return new Vector3(r, phi, theta);
    }

    Vector3 getCartesianCoordinates(Vector3 spherical)
    {
        Vector3 ret = new Vector3();

        ret.x = spherical.x * Mathf.Cos(spherical.z) * Mathf.Cos(spherical.y);
        ret.y = spherical.x * Mathf.Sin(spherical.z);
        ret.z = spherical.x * Mathf.Cos(spherical.z) * Mathf.Sin(spherical.y);

        return ret;
    }
    void mouseSchmovement()
    {
        Vector3 targetDelta = new Vector3(0.0f,1.0f, 0.0f);
        target = SPHERE.transform.position + targetDelta;

        // Get the deltas that describe how much the mouse cursor got moved between frames
        float dx = Input.GetAxis("Mouse X") * rotationSpeed * mouseSens * -1;
        float dy = Input.GetAxis("Mouse Y") * rotationSpeed * mouseSens * -1;

        // Zoom in and out with the mouse scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        radius = Mathf.Clamp(radius - scroll * 0.5f, 0.05f, 0.8f);

        // Rotate the camera left and right
        sc.y += dx * Time.deltaTime;

        // Rotate the camera up and down
        // Prevent the camera from turning upside down (1.5f = approx. Pi / 2)
        sc.z = Mathf.Clamp(sc.z + dy * Time.deltaTime, -1.5f, 1.5f);

        // Calculate the cartesian coordinates for unity
        transform.position = getCartesianCoordinates(sc) * radius + target;

        // Make the camera look at the target
        transform.LookAt(target);
    }

    // Update is called once per frame
    void Update()
    {
        if(!holdUp)
        mouseSchmovement();

        

    }
}



















    //public GameObject follower;
    //public bool viewOnAwake = true;
    //public float smoothTime = 0.05f;
    //public float rotationSpeed = 5f;
    //public Vector3 offset ;

    //private Vector3 velocity = Vector3.zero;


    //private void Awake()
    //{
    //    if (viewOnAwake)
    //    {
    //        transform.position = follower.transform.position;
    //        transform.position += offset;
    //        transform.LookAt(follower.transform.position);
    //    }
    //}
    //void FixedUpdate()
    //{
    //    Vector3 targetPosition = follower.transform.position + offset;
    //    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

    //    Quaternion targetRotation = Quaternion.LookRotation(follower.transform.position - transform.position);

    //    // Smoothly rotate towards the target rotation
    //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    //}

    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
//}
