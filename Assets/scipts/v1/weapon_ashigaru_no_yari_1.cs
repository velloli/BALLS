
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class weapon_ashigaru_no_yari_1 : WeaponItem
{
    private BuildDisplayer bd;
    private Build b;
    private GameObject weaponColliderHost;
    private static readonly int MaskCompletionID = Shader.PropertyToID("_MaskCompletion");
    private GameObject weaponProjectileInstance;

    public GameObject weaponProjectile;
    public float reloadDuration = 3;//maybe this should be in WeaponItem?
    public float range = 20;
    public float displayProjectileVelocity = 100;

    public override void Fire()
    {
        //check if weapon is ready to be fired
        if (status.state != 0)
            return;
    
        status.state = 1;

        // Implement your custom rifle firing logic here
        Debug.Log("Ashigaru fired");

        weaponProjectileInstance.transform.position = weaponColliderHost.transform.position = bd.GetWeaponHolder(this).transform.position;
        weaponProjectileInstance.transform.rotation = weaponColliderHost.transform.rotation= bd.GetWeaponHolder(this).transform.rotation;
        weaponProjectileInstance.SetActive(true);
        weaponProjectileInstance.GetComponent<Rigidbody>().velocity = weaponProjectileInstance.transform.forward * displayProjectileVelocity;
        weaponProjectileInstance.GetComponent<Projectile_LimitRange>().ExecuteRangeLimiter(range);
        Debug.DrawLine(weaponColliderHost.transform.position, weaponColliderHost.transform.position + weaponColliderHost.transform.forward * range, Color.red, 1f);

        // Get the forward direction from the weapon's transform
        Vector3 rayDirection = weaponColliderHost.transform.forward;
        RaycastHit[] hits;

        // Perform the raycast and store all hits
        // Parameters: origin point, direction, array to store hits, max distance
        // You can adjust the maxDistance value based on your weapon's range
        //hits = Physics.RaycastAll(weaponColliderHost.transform.position, rayDirection, range);
        hits = Physics.CapsuleCastAll(weaponColliderHost.transform.position, weaponColliderHost.transform.position, 0.15f, rayDirection);
        // Iterate through all objects hit by the ray
        foreach (RaycastHit hit in hits)
        {
            Debug.Log(hit.collider.name);

            // Check the hit object and its hierarchy for health component
            health healthComponent = hit.collider.gameObject.GetComponent<health>();

            // If no health component found, check parents
            if (healthComponent == null)
            {
                healthComponent = hit.collider.gameObject.GetComponentInParent<health>();
            }

            // If still no health component found, check children
            if (healthComponent == null)
            {
                healthComponent = hit.collider.gameObject.GetComponentInChildren<health>();
            }

            // If we found a health component anywhere in the hierarchy, apply damage
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(50);
                Debug.Log($"Hit health component on {healthComponent.gameObject.name}");
                Debug.DrawLine(weaponColliderHost.transform.position, hit.point, Color.red, 1f);
            }

        }

        status.state = 2;
        Reload();
    }

    public override void Unload()
    {
        Destroy(weaponColliderHost.GetComponent<BoxCollider>());
        Debug.Log("Ashigaru Unloaded");
        Destroy(this);

    }

    public override void Reload()
    {
        // Implement your custom reload logic here
        //Debug.Log("Ashigaru reloading...");
        status.state = 2;
        //SUPER IMPORTANT TO LET BUILD COMPONENT START THE COROUTINE
        b.StartCoroutine(reloadRoutine());
    }

    private IEnumerator reloadRoutine(){

        float elapsedTime = 0f;
        Material weaponMaterial = bd.GetWeaponMeshRenderer(this).material;

        while (elapsedTime < reloadDuration)
        {
            elapsedTime += Time.deltaTime;
            float completion = elapsedTime / reloadDuration;
            weaponMaterial.SetFloat(MaskCompletionID, 1 -completion);
            yield return null;
        }

        // Ensure we end exactly at 1
        weaponMaterial.SetFloat(MaskCompletionID, 0);
        //Debug.Log("Reload is over");
        status.state = 0;

    }




    // Override the preload method if needed
    public override void Preload(GameObject player)
    {
    //TODO HIDE THE BD MESH
    //ShowDetailsOptions THE 
        bd = player.GetComponent<BuildDisplayer>();
        b = player.GetComponent<Build>();
        weaponColliderHost = PlayerHelpers.instance.GetColliderHost();
        weaponProjectileInstance = Instantiate(weaponProjectile);
        weaponProjectileInstance.SetActive(false);
        //weaponColliderHost.AddComponent<BoxCollider>();
        Debug.Log("Ashigaru loaded");

    }
}
