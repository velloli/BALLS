using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    bool reversed = false;
    Material material;
    public float dmg = 15.0f;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.bulletCount++;
        GameManager.playerPhase += PhaseEvent;
        GameManager.playerUnPhase += UnPhaseEvent;
        if (GameManager.Instance.phaseActive)
            PhaseEvent();
        material = GetComponentInChildren<MeshRenderer>().material;
        GetComponentInChildren<CapsuleCollider>().isTrigger = false;
        StartCoroutine(despawn());    
    }

    // Update is called once per frame
    private void PhaseEvent()
    {
        //Debug.Log("Phase Event Started");
        GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * 0.5f;
        GetComponentInChildren<Collider>().isTrigger = true;
        // Code to handle the bullet event goes here
    }

    private void UnPhaseEvent()
    {
        //Debug.Log("Phase Event Started");
        GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * 2.0f;
        GetComponentInChildren<Collider>().isTrigger = false;

        // Code to handle the bullet event goes here
    }
    private IEnumerator despawn()
    {
        yield return new WaitForSeconds(20);
        Destroy(gameObject);
        
    }
    private void OnDestroy()
    {
        GameManager.playerPhase -= PhaseEvent;
        GameManager.playerUnPhase -= UnPhaseEvent;
        GameManager.Instance.bulletCount--;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            if(!reversed && GameManager.Instance.reverseUnlocked)
            {
                GetComponent<Rigidbody>().velocity *= -3;
                material.color = Color.blue;

            }

        }

        if (other.gameObject.CompareTag("bullet"))
        {
            Debug.Log("Bullet to Bullet");
            Destroy(other.gameObject);
            Destroy(this.gameObject);

        }

        if ( other.gameObject.CompareTag("enemy"))
        {
            dealDamage(other.gameObject);
            Destroy(this.gameObject);

        }

    }

    private void dealDamage(GameObject other)
    {
        health healthScript = other.GetComponentInParent<health>();
        if(healthScript)
            healthScript.TakeDamage(dmg);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Dealing dmg to player");
            dealDamage(collision.gameObject);
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag("bullet"))
        {
            //Debug.Log("Bullet to Bullet");

            Destroy(collision.gameObject);
            Destroy(this.gameObject);

        }

        if (collision.gameObject.CompareTag("Ground"))
        { 
            Destroy(this.gameObject);
        }

        if (collision.gameObject.CompareTag("enemy"))
        {
            dealDamage(collision.gameObject);
            Destroy(this.gameObject);

        }
    }
}
