using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item
{
    public string Name { get; private set; }
    public string ItemType { get; private set; }
    public double Weight { get; private set; }
    public string Description { get; private set; }
    public bool CanUse { get; private set; }

    public Item (string name, double weight, string itemType, string description, bool canUse)
    {
        Name = name;
        Weight = weight;
        ItemType = itemType;
        Description = description;
        CanUse = canUse;
    }

    public abstract void UseItem(); 
}
