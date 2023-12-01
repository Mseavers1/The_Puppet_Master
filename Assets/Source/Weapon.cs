using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    private bool isEquipped { get; set; }

    public Weapon(string name, double weight, string description, string subType) : base(name, weight, "Weapon", description, false, true, subType)
    {

    }

    public override void UseItem() {}

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
