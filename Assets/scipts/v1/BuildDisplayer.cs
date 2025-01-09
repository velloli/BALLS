using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Burst.CompilerServices;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BuildDisplayer : MonoBehaviour
{
    //THESE ARE THE PARENT OBJECTS
    [ReadOnly]public GameObject WeaponHolderBase;
    [ReadOnly]public GameObject PrimaryWeaponHolder;
    [ReadOnly]public GameObject SecondaryWeaponHolder;
    public GameObject weaponPrefab;

    //THESE ARE THE ACTUAL MESHES WHICH ATTACH TO THE HOLDERS
    private GameObject primaryWeaponMesh,secondaryWeaponMesh;
    private MeshFilter primaryMeshFilter, secondaryMeshFilter;
    private MeshRenderer primaryMeshRenderer,secondaryMeshRenderer;
    private Build build;


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
        primaryMeshFilter = primaryWeaponMesh.GetComponentInChildren<MeshFilter>();
        primaryMeshFilter.mesh = null;
        primaryMeshRenderer = primaryWeaponMesh.GetComponentInChildren<MeshRenderer>();

        //secondary weapon object
        secondaryWeaponMesh = Instantiate(weaponPrefab);
        secondaryMeshFilter = secondaryWeaponMesh.GetComponentInChildren<MeshFilter>();
        secondaryMeshFilter.mesh = null;
        secondaryMeshRenderer = secondaryWeaponMesh.GetComponentInChildren<MeshRenderer>();

        build = GetComponent<Build>();

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
        primaryMeshFilter.mesh = build.PrimaryWeapon.displayMesh;
    }

    public void refreshWeaponsDisplay()
    {
        if (build.PrimaryWeapon !=null)
        {
            primaryMeshFilter.mesh = build.PrimaryWeapon.displayMesh;
            primaryMeshRenderer.SetMaterials(build.PrimaryWeapon.equipMaterials);

        }

        if (build.SecondaryWeapon != null)
        {
            secondaryMeshFilter.mesh = build.SecondaryWeapon.displayMesh;
            secondaryMeshRenderer.SetMaterials(build.SecondaryWeapon.equipMaterials);
        }

    }

    public GameObject GetWeaponHolder(WeaponItem weaponItem)
    {
        GameObject ret = null;
        if(build.PrimaryWeapon == weaponItem){
            ret = PrimaryWeaponHolder;
        }
        else if(build.SecondaryWeapon == weaponItem)
        {
            ret = SecondaryWeaponHolder;
        }

        return ret;
    }

    public MeshRenderer GetWeaponMeshRenderer(WeaponItem weaponItem)
    {
        MeshRenderer ret = null;
        if (build.PrimaryWeapon == weaponItem)
        {
            ret = primaryMeshRenderer;
        }
        else if (build.SecondaryWeapon == weaponItem)
        {
            ret = secondaryMeshRenderer;
        }

        return ret;
    }

    public void HideWeaponMesh(WeaponItem weaponItem){



    }
}
