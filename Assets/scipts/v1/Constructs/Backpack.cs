using System;
using System.Collections.Generic;
using UnityEngine;

using static interfaces;

public class Backpack
{
    [SerializeField]
    public List<IBackpackItem> items = new List<IBackpackItem>();
    private int maxSlots;

    public Backpack(int maxSlots)
    {
        this.maxSlots = maxSlots;
    }

    public bool AddItem(IBackpackItem item)
    {
        if (items.Count < maxSlots)
        {
            items.Add(item);
            return true;
        }
        else
        {
            Debug.LogWarning("Backpack is full.");
            return false;
        }
    }

    public void RemoveItem(IBackpackItem item)
    {
        items.Remove(item);
    }

    public bool Contains(IBackpackItem item)
    {
        return items.Contains(item);
    }

    public List<IBackpackItem> GetItems()
    {
        return items;
    }
}