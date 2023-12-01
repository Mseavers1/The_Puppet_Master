using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public double MaxDamage { get; private set; }
    public int SkillLevel { get; private set; }

    private bool isEquipped { get; set; }

    public Weapon(string name, double weight, string description, string subType, double maxDamage, int skillLevel) : base(name, weight, "Weapon", description, false, true, subType)
    {
        MaxDamage = maxDamage;
        SkillLevel = skillLevel;
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
