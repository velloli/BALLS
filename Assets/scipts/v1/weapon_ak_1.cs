using UnityEngine;

public class weapon_ak_1 : WeaponItem
{
    public override void Fire()
    {
        // Implement your custom rifle firing logic here
        Debug.Log("ak-1 fired");
    }

    // Override the preload method if needed
    public override void preload()
    {
        // Implement your custom preload logic here
        Debug.Log("ak-1 loaded");

    }
}
