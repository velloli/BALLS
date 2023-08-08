using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponItem : MonoBehaviour
{
    public string itemName;
    public bool isLongRange; // True for long-range, false for slash weapon
    public int damage; // Weapon damage
    public Mesh displayMesh;
    public Material displayMaterial;
    // Add more weapon stats here as needed
}
