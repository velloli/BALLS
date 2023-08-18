using System.Collections.Generic;
using UnityEngine;





public class Build : MonoBehaviour
{
    // The maximum number of slots in the backpack
    public int maxBackpackSlots = 30;

    // Lists to store different types of items in inventory
    private Backpack backpack;

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

    public bool EquipConsumable_slot1(ConsumableItem consumable)
    {
        if (equippedConsumable1 != null)
        {
            if (!backpack.AddItem(equippedConsumable1))
            {
                //unable to equip
                //drop the item
            }
        }
        equippedConsumable1 = consumable;
        // Refresh UI
        return true;
    }

    public bool EquipConsumable_slot2(ConsumableItem consumable)
    {
        if (equippedConsumable2 != null)
        {
            if (!backpack.AddItem(equippedConsumable2))
            {
                // Unable to equip, you might want to drop the item
                // DropLogic(equippedConsumable2);
            }
        }
        equippedConsumable2 = consumable;
        // Refresh UI
        return true;
    }

    public bool EquipConsumable_slot3(ConsumableItem consumable)
    {
        if (equippedConsumable3 != null)
        {
            if (!backpack.AddItem(equippedConsumable3))
            {
                // Unable to equip, you might want to drop the item
                // DropLogic(equippedConsumable3);
            }
        }
        equippedConsumable3 = consumable;
        // Refresh UI
        return true;
    }

    public bool EquipPrimaryMod(ModItem mod)
    {
        if (PrimaryMod != null)
        {
            if (!backpack.AddItem(PrimaryMod))
            {
                // Unable to equip, you might want to drop the item
                // DropLogic(PrimaryMod);
            }
        }
        PrimaryMod = mod;
        // Refresh UI
        return true;
    }

    public bool EquipSecondaryMod(ModItem mod)
    {
        if (SecondaryMod != null)
        {
            if (!backpack.AddItem(SecondaryMod))
            {
                // Unable to equip, you might want to drop the item
                // DropLogic(SecondaryMod);
            }
        }
        SecondaryMod = mod;
        // Refresh UI
        return true;
    }



}
