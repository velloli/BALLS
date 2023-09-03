using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BuildDisplayer : MonoBehaviour
{
    [ReadOnly]public GameObject WeaponHolderBase;
    [ReadOnly]public GameObject PrimaryWeaponHolder;
    [ReadOnly]public GameObject SecondaryWeaponHolder;
    public GameObject weaponPrefab;

    private GameObject primaryWeaponMesh,secondaryWeaponMesh;


    public Vector3 PrimaryWeaponOffset = new Vector3(1.0f, 1.0f, 0);
    void Start()
    {
        init();

    }

    private void init()
    {
        //base transform stuff
        WeaponHolderBase = new GameObject("WeaponHolderBase");
        WeaponHolderBase.transform.position = GameManager.Instance.player.transform.position;

        //primary weapon holder
        PrimaryWeaponHolder = new GameObject("PrimaryWeaponHolder");
        PrimaryWeaponHolder.transform.position = WeaponHolderBase.transform.position + PrimaryWeaponOffset;
        PrimaryWeaponHolder.transform.parent = WeaponHolderBase.transform;

        //secondary weapon holder
        SecondaryWeaponHolder = new GameObject("SecondaryWeaponHolder");
        SecondaryWeaponHolder.transform.position = WeaponHolderBase.transform.position + new Vector3(
           -PrimaryWeaponOffset.x,
            PrimaryWeaponOffset.y,
            PrimaryWeaponOffset.z);

        SecondaryWeaponHolder.transform.parent = WeaponHolderBase.transform;


        //primary weapon object
        primaryWeaponMesh = Instantiate(weaponPrefab);
        primaryWeaponMesh.GetComponentInChildren<MeshFilter>().mesh = null;

        //secondary weapon object
        secondaryWeaponMesh = Instantiate(weaponPrefab);
        secondaryWeaponMesh.GetComponentInChildren<MeshFilter>().mesh = null;

    }
    private void FixedUpdate()
    {
        Vector3 velocity = new Vector3(0, 0, 0);

        Vector3 primaryDesiredPosition = PrimaryWeaponHolder.transform.position;
        Vector3 primarySmoothedPosition = Vector3.SmoothDamp(primaryWeaponMesh.transform.position, primaryDesiredPosition, ref velocity, 0.025f);
        primaryWeaponMesh.transform.position = primarySmoothedPosition;

        Vector3 secondaryDesiredPosition = SecondaryWeaponHolder.transform.position;
        Vector3 secondarySmoothedPosition = Vector3.SmoothDamp(secondaryWeaponMesh.transform.position, secondaryDesiredPosition, ref velocity, 0.025f);
        secondaryWeaponMesh.transform.position = secondarySmoothedPosition;
    }
    // Update is called once per frame
    void Update()
    {
        WeaponHolderBase.transform.position = GameManager.Instance.player.transform.position;
        WeaponHolderBase.transform.forward = Camera.main.transform.forward;
        //Debug.Log(PrimaryWeaponHolder.transform.position);

      

        primaryWeaponMesh.transform.forward = Camera.main.transform.forward;
        secondaryWeaponMesh.transform.forward = Camera.main.transform.forward;
        //primaryWeaponMesh.transform.RotateAroundLocal(new Vector3(0, 1, 0), 90);

    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawSphere(PrimaryWeaponHolder.transform.position, 0.1f);

    }

    public void refreshPrimaryWeapon()
    {
        primaryWeaponMesh.GetComponentInChildren<MeshFilter>().mesh = GetComponent<Build>().PrimaryWeapon.displayMesh;
    }

    public void refreshWeapons()
    {
        if(GetComponent<Build>().PrimaryWeapon)
        primaryWeaponMesh.GetComponentInChildren<MeshFilter>().mesh = GetComponent<Build>().PrimaryWeapon.displayMesh;

        if (GetComponent<Build>().SecondaryWeapon)
            secondaryWeaponMesh.GetComponentInChildren<MeshFilter>().mesh = GetComponent<Build>().SecondaryWeapon.displayMesh;

    }
}
