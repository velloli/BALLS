using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleEnemy : MonoBehaviour
{
    private GameObject player;
    private GameManager gameManager;
    private bool canShoot = true;
    [SerializeField] private float smoothSpeed = 10f;



    public bool burst = false;
    public bool lock_verticalAiming = false;

    public GameObject bullet;
    public Transform barrelLocation;
    public float timeBetweenShots = 0.5f;
    bool is_shooting = false;

    public float BulletForce = 200.0f;


    [Header("Simple Fire Vars")]
    public float interval = 1.0f;

    [Header("burst Fire Vars")]
    public int bulletsinEachBurst = 4;
    public float burstInterval = 2.0f;
    public float burstBulletInterval = 0.1f;




    // Start is called before the first frame update

    private void Awake()
    {
        //Debug.Log("TRYING TO FIND GM");
        gameManager = GameManager.Instance;
        player = gameManager.player;
        GameManager.playerPhase += PhaseEvent;
        GameManager.playerUnPhase += UnPhaseEvent;

    }
    private void LateUpdate()
    {
        aim();
        //canShoot = true;
        if(!is_shooting)
        {
            shoot();
        }
        
    }
    private void FixedUpdate()
    {
    }

    private void PhaseEvent()
    {
        //Debug.Log("Phase Event Started");
        canShoot = false;
        // Code to handle the bullet event goes here
    }

    private void UnPhaseEvent()
    {
        //Debug.Log("Phase Event Started");
        canShoot = true;
        // Code to handle the bullet event goes here
    }

    void aim()
    {
        //transform.LookAt(player.transform.position);

        // Get the direction to the player
        Vector3 targetDirection = player.transform.position - transform.position;
        //targetDirection.y = 0f; // Lock the gun's rotation on the y-axis
        if(lock_verticalAiming)
        targetDirection.y = 0f; // Lock the gun's rotation on the y-axis

        // Smoothly rotate the gun to face the player
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);

    }

    private IEnumerator randomWait()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 2.0f));
    }

    void shoot()
    {
        randomWait(); 
        if (burst)
        {
            StartCoroutine(BurstFireCoroutine(bulletsinEachBurst,burstBulletInterval,burstInterval));
        }
        else
        {
            StartCoroutine(FireCoroutine(timeBetweenShots));
        }

    }

    private IEnumerator BurstFireCoroutine( int _bulletsinEachBurst = 4, float _burstBulletInterval = 4, float _burstInterval = 4)
    {
        while (canShoot)
        {
            is_shooting = true;
            for (int i = 0; i < _bulletsinEachBurst; i++)
            {
                
                GameObject spawnBullet = Instantiate(bullet);

                spawnBullet.transform.position = barrelLocation.position;
                spawnBullet.transform.forward = barrelLocation.forward;
                spawnBullet.GetComponent<Rigidbody>().AddForce(spawnBullet.transform.forward * BulletForce);
                //Debug.Log("Shot a bullet");
                yield return new WaitForSeconds(_burstBulletInterval);
            }
            yield return new WaitForSeconds(_burstInterval);
            is_shooting = false;
        }
    }

    private IEnumerator  FireCoroutine(float _interval = 1.0f)
    {
        while (canShoot)
        {
            is_shooting = true;   
            GameObject spawnBullet = Instantiate(bullet);
            spawnBullet.transform.position = barrelLocation.position;
            spawnBullet.transform.forward = barrelLocation.forward;
            spawnBullet.GetComponent<Rigidbody>().AddForce(spawnBullet.transform.forward * BulletForce);
            yield return new WaitForSeconds(_interval);        
            is_shooting = false;
        }
        
    }
}
