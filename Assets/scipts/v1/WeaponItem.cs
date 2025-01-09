using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static interfaces;
public struct WeaponStatus{
    public int state;
    //STATE 0: weapon initialized
    //STATE 1: weapon firing
    //STATE 2: weapon reloading
}
[System.Serializable]
public abstract class WeaponItem : MonoBehaviour,IBackpackItem
{
    public string itemName;
    public bool isLongRange; // True for long-range, false for slash weapon
    public int damage; // Weapon damage
    public Mesh displayMesh;
    public Material displayMaterial;
    public List<Material> equipMaterials;
    public WeaponStatus status;

    private bool reloading = false;

    public void UseItem()
    {
    }

    public abstract void Preload(GameObject player);
    // Add more weapon stats here as needed
    public abstract void Fire();
    public abstract void Reload();
    public abstract void Unload();


}
