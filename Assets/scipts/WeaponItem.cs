using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static interfaces;

[System.Serializable]
public class WeaponItem : MonoBehaviour ,IBackpackItem
{
    public string itemName;
    public bool isLongRange; // True for long-range, false for slash weapon
    public int damage; // Weapon damage
    public Mesh displayMesh;
    public Material displayMaterial;

    public void UseItem()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }

    // Add more weapon stats here as needed
}
