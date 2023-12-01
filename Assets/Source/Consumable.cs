using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public Consumable(string name, double weight, string description) : base(name, weight, "Consumable", description, true)
    {
    }

    public override void UseItem() 
    {

    }
}
