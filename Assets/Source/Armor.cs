using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : Item
{
    public Armor(string name, double weight, string description) : base(name, weight, "Armor", description, false)
    {

    }

    public override void UseItem() { }
}
