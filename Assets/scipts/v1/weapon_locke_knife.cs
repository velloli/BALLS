using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon_locke_knife : WeaponItem
{
    // Start is called before the first frame update
    public override void Fire()
    {
        // Implement your custom rifle firing logic here
        Debug.Log("locke knife fired");
    }

    public override void Unload()
    {

    }

    // Override the preload method if needed
    public override void Preload(GameObject player)
    {
        // Implement your custom preload logic here
        Debug.Log("locke knife loaded");

    }

    public override void Reload()
    {
        // Implement your custom preload logic here

    }
}
