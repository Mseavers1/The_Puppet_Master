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
    public bool CanEquip { get; private set; }

    public string SubType { get; private set; }

    public Item (string name, double weight, string itemType, string description, bool canUse, bool canEquip, string subType)
    {
        Name = name;
        Weight = weight;
        ItemType = itemType;
        Description = description;
        CanUse = canUse;
        CanEquip = canEquip;
        SubType = subType;
    }

    public abstract void UseItem(); 
    public abstract bool IsEquipped();

    public abstract void Equip();
    public abstract void UnEquip();
}
