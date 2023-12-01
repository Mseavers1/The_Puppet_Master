using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public Consumable(string name, double weight, string description) : base(name, weight, "Consumable", description, true, false, "None")
    {
    }

    public override void Equip()
    {
        throw new System.Exception("How did you here? Consumables are equipable!");
    }

    public override bool IsEquipped()
    {
        return false;
    }

    public override void UnEquip()
    {
        
    }

    public override void UseItem() 
    {
       
    }
}
