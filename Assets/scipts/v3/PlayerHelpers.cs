using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script helps the build system assign weapons to slots(primary and secondary)
public class PlayerHelpers : MonoBehaviour
{
    public static PlayerHelpers instance;
    public GameObject PrimaryWeaponColliderHost;
    public GameObject SecondaryWeaponColliderHost;

    private bool PrimaryTaken,SecondaryTaken;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        PrimaryWeaponColliderHost = new GameObject("PrimaryWeaponColliderHost");
        SecondaryWeaponColliderHost = new GameObject("SecondaryWeaponColliderHost");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetColliderHost()
    {
        GameObject ret = null;
        if(!PrimaryTaken)
        {
            PrimaryTaken = true;
            ret = PrimaryWeaponColliderHost;
        }

        else if (!SecondaryTaken)
        {
            SecondaryTaken = true;
            ret = SecondaryWeaponColliderHost;
        }

        return ret;
    }
}
