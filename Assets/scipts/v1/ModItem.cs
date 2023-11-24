using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static interfaces;

[System.Serializable]
public class ModItem : MonoBehaviour, IBackpackItem
{
    public string itemName;
    public string perkDescription; // Description of the unique perk

    public void UseItem()
    {
        //throw new System.NotImplementedException();
    }
    // Add more mod-related properties here if needed
}
