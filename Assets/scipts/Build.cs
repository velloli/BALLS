using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;





public class Build : MonoBehaviour
{
    // The maximum number of slots in the backpack
    public int maxBackpackSlots = 30;

    // Lists to store different types of items
    private List<ConsumableItem> consumables = new List<ConsumableItem>();
    private List<WeaponItem> weapons = new List<WeaponItem>();
    private List<ModItem> mods = new List<ModItem>();

    // The maximum number of mods the player can have (2 max)
    private const int maxModSlots = 2;
    // The maximum number of weapons the player can have equipped (2 max)
    private const int maxEquippedWeapons = 2;
    // The maximum number of consumables the player can have (3 max)
    private const int maxConsumableSlots = 3;

    // Variables to store the equipped weapons and consumables
    private List<WeaponItem> equippedWeapons = new List<WeaponItem>();
    private List<ConsumableItem> equippedConsumables = new List<ConsumableItem>();
    private List<ModItem> equippedMods = new List<ModItem>();

    // Function to add a consumable item to the backpack
    public void AddConsumableItem(ConsumableItem item)
    {
        consumables.Add(item);
    }

    // Function to remove a consumable item from the backpack
    public void RemoveConsumableItem(ConsumableItem item)
    {
        consumables.Remove(item);
    }

    // Function to add a weapon item to the backpack
    public void AddWeaponItem(WeaponItem item)
    {
        weapons.Add(item);
    }

    // Function to remove a weapon item from the backpack
    public void RemoveWeaponItem(WeaponItem item)
    {
        weapons.Remove(item);
    }

    // Function to add a mod item to the backpack
    public void AddModItem(ModItem item)
    {
        if (mods.Count < maxModSlots)
        {
            mods.Add(item);
        }
        else
        {
            Debug.LogWarning("Cannot add more mods. Max mod slots reached.");
        }
    }

    // Function to remove a mod item from the backpack
    public void RemoveModItem(ModItem item)
    {
        mods.Remove(item);
    }

    // Function to equip a weapon
    public void EquipWeapon(WeaponItem item)
    {
        if (weapons.Contains(item))
        {
            if (equippedWeapons.Count < maxEquippedWeapons)
            {
                equippedWeapons.Add(item);
            }
            else
            {
                Debug.LogWarning("Cannot equip more weapons. Max equipped weapons reached.");
            }
        }
        else
        {
            Debug.LogWarning("Weapon not found in the backpack.");
        }
    }

    // Function to unequip a weapon
    public void UnequipWeapon(WeaponItem item)
    {
        equippedWeapons.Remove(item);
    }

    // Function to equip a mod
    public void EquipMod(ModItem item)
    {
        if (mods.Contains(item))
        {
            if (equippedMods.Count < maxModSlots)
            {
                equippedMods.Add(item);
            }
            else
            {
                Debug.LogWarning("Cannot equip more mods. Max equipped mods reached.");
            }
        }
        else
        {
            Debug.LogWarning("Mod not found in the backpack.");
        }
    }

    // Function to unequip a mod
    public void UnequipMod(ModItem item)
    {
        equippedMods.Remove(item);
    }

    // Function to use a consumable
    public void UseConsumable(ConsumableItem item)
    {
        if (consumables.Contains(item))
        {
            if (equippedConsumables.Count < maxConsumableSlots)
            {
                equippedConsumables.Add(item);
                consumables.Remove(item);
            }
            else
            {
                Debug.LogWarning("Cannot equip more consumables. Max equipped consumables reached.");
            }
        }
        else
        {
            Debug.LogWarning("Consumable not found in the backpack.");
        }
    }
     
}
