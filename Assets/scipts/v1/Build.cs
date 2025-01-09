using System.Collections.Generic;
using UnityEngine;



/*
 * 
 * THIS SCCRIPT TAKES CARE OF EQUIPPING AND USING ALL THE WEAPONS/CONSUMABLES
 * 
 */

public class Build : MonoBehaviour
{
    // The maximum number of slots in the backpack
    public int maxBackpackSlots = 30;

    // Lists to store different types of items in inventory
    [SerializeField]
    public Backpack backpack;

    // Variables to store the equipped weapons and consumables
    public ConsumableItem equippedConsumable1, equippedConsumable2, equippedConsumable3;

    public WeaponItem PrimaryWeapon;
    public WeaponItem SecondaryWeapon;

    public ModItem PrimaryMod, SecondaryMod;
    // Function to add a consumable item to the backpack
   
    private void Start()
    {
        backpack = new Backpack(maxBackpackSlots);

    }

    public bool FirePrimaryWeapon()
    {

        if (PrimaryWeapon !=null)
        {
            PrimaryWeapon.Fire();
        }
        return true;
    }

    public bool FireSecondaryWeapon()
    {

        if (SecondaryWeapon!=null)
        {
            SecondaryWeapon.Fire();
        }
        return true;
    }

    public bool EquipConsumable_slot1(ConsumableItem consumable)
    {

        equippedConsumable1 = consumable;
        // Refresh UI
        return true;
    }

    public bool EquipConsumable_slot2(ConsumableItem consumable)
    {

        equippedConsumable2 = consumable;
        // Refresh UI
        return true;
    }

    public bool EquipConsumable_slot3(ConsumableItem consumable)
    {

        equippedConsumable3 = consumable;
        // Refresh UI
        return true;
    }

    public bool EquipPrimaryMod(ModItem mod)
    {

        PrimaryMod = mod;
        // Refresh UI
        return true;
    }

    public bool EquipSecondaryMod(ModItem mod)
    {
        SecondaryMod = mod;
        // Refresh UI
        return true;
    }

    public bool EquipSecondaryWeapon(WeaponItem weapon)
    {
        SecondaryWeapon = weapon;
        // Refresh UI
        return true;
    }

    public bool EquipWeapon(WeaponItem weapon)
    {
        if(PrimaryWeapon == null)
        {
            PrimaryWeapon = weapon;
        }
        else if (SecondaryWeapon == null)
        {
            Debug.Log("Assigning a secondary weapon");
            SecondaryWeapon = weapon;
        }
        else
        {
            bool v = AddItemToBackpack(PrimaryWeapon);
            return false;
        }

        GetComponent<BuildDisplayer>().refreshWeaponsDisplay();
        weapon.Preload(gameObject);

        return true;
    }

    public bool EquipPrimaryWeapon(WeaponItem weapon)
    {
        PrimaryWeapon = weapon;
        GetComponent<BuildDisplayer>().refreshWeaponsDisplay();
        weapon.Preload(gameObject);
        // Refresh UI
        return true;
    }

    public bool AddItemToBackpack(interfaces.IBackpackItem item)
    {
        //automatically eqyuipping items if we have none
        if (item is ModItem)
        {
            if (PrimaryMod == null)
            {
                PrimaryMod = item as ModItem;
            }
            else if (SecondaryMod == null)
            {
                SecondaryMod = item as ModItem;
            }
            else
            {
                return backpack.AddItem(item);
            }
        }
        else if (item is WeaponItem)
        {
            if (PrimaryWeapon == null)
            {
                PrimaryWeapon = item as WeaponItem;
            }
            else if (SecondaryWeapon == null)
            {
                SecondaryWeapon = item as WeaponItem;
            }
            else
            {
                return backpack.AddItem(item);
            }
        }
        else if (item is ConsumableItem)
        {
            if (equippedConsumable1 == null)
            {
                equippedConsumable1 = item as ConsumableItem;
            }
            else if (equippedConsumable2 == null)
            {
                equippedConsumable2 = item as ConsumableItem;
            }
            else if (equippedConsumable3 == null)
            {
                equippedConsumable3 = item as ConsumableItem;
            }
            else
            {
                return backpack.AddItem(item);
            }
        }

        // Refresh UI or perform any other necessary actions
        return true;


    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {//left click
            FirePrimaryWeapon();
        }
        else if(Input.GetMouseButtonDown(1))
        {
            FireSecondaryWeapon();
        }
    }

    public void reloadPrimaryWeapon(){
        //StartCoroutine(PrimaryWeapon.reloadRoutine());

    }

}
