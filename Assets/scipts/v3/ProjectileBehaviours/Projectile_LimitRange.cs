using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile_LimitRange : MonoBehaviour
{
    //THIS CLASS WILL ENSURE THAT THE PROJECTILE DISSAPEARS AFTER A CERTAIN RANGE
    //THIS CLASS ALSO ASSUMES THAT THE PROJECTILE IS ALREADY MOVING
    private Vector3 startPos,deltaPos;
    private float range;
    private bool rangeLimiterActive = false;
    public void ExecuteRangeLimiter(float _range){

        rangeLimiterActive = true;
        startPos = transform.position;
        range = _range;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rangeLimiterActive)
        {
            deltaPos = startPos - transform.position;
            if (deltaPos.magnitude > range)
            {
                GetComponent<Rigidbody>().velocity  = Vector3.zero;
                rangeLimiterActive = false;
                gameObject.SetActive(false);
                Debug.Log("range reached");
            }
        }
    }
}
