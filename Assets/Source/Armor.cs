using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{
    private bool isEquipped { get; set; }

    public Armor(string name, double weight, string description, string subType) : base(name, weight, "Armor", description, false, true, subType)
    {

    }

    public override void UseItem() { }

    public override bool IsEquipped()
    {
        return isEquipped;
    }

    public override void Equip()
    {
        isEquipped = true;
    }

    public override void UnEquip()
    {
        isEquipped = false;
    }
}
