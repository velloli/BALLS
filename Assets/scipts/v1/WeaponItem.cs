using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static interfaces;

[System.Serializable]
public abstract class WeaponItem : MonoBehaviour ,IBackpackItem
{
    public string itemName;
    public bool isLongRange; // True for long-range, false for slash weapon
    public int damage; // Weapon damage
    public Mesh displayMesh;
    public Material displayMaterial;

    private bool reloading = false;

    public void UseItem()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            onPickup();
        }
    }


    private IEnumerator reloadRoutine()
    {
        yield return null;
    }

    public void onPickup()
    {
        GameManager.Instance.player.GetComponent<Build>().EquipPrimaryWeapon(this);
        Debug.Log("Equipped " + itemName);
        //Destroy(this);
    }


    public abstract void preload();
    // Add more weapon stats here as needed
    public abstract void Fire();
}
