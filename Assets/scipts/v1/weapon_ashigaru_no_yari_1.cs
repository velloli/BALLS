
using UnityEngine;

public class weapon_ashigaru_no_yari_1 : WeaponItem
{
    public override void Fire()
    {
        // Implement your custom rifle firing logic here
        Debug.Log("Ashigaru fired");
    }

    // Override the preload method if needed
    public override void preload()
    {
        // Implement your custom preload logic here
        Debug.Log("Ashigaru loaded");

    }
}
