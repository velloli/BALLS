using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : MonoBehaviour
{
    public string itemName;
    public ConsumableType type; // Type of the consumable (INSTANT or EFFECTDURATION)
    public int eventDuration; // Event Duration in seconds (for EFFECTDURATION type)
    public float usageCooldown; // Usage Cooldown in seconds

    public enum ConsumableType
    {
        INSTANT,
        EFFECTDURATION
    }
}
