using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static interfaces;

public class ConsumableItem : MonoBehaviour, IBackpackItem
{
    public string itemName;
    public ConsumableType type; // Type of the consumable (INSTANT or EFFECTDURATION)
    public int eventDuration; // Event Duration in seconds (for EFFECTDURATION type)
    public float usageCooldown; // Usage Cooldown in seconds

    public void UseItem()
    {
        throw new System.NotImplementedException();
    }

    public enum ConsumableType
    {
        INSTANT,
        EFFECTDURATION
    }
}
