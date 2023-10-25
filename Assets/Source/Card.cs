using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Card
{
    private string name;
    private float manaCost, staminaCost;
    private char type; // W - Weapon // S - Spell // D - Defense // M - Misc
    private string desc;

    public Card(string name, float manaCost, float staminaCost)
    {
        this.manaCost = manaCost;
        this.staminaCost = staminaCost;
        this.name = name;
    }

    public string GetName() { return name; }


    private void CheckType()
    { 
        if (type != 'W' && type != 'S' && type != 'D' && type != 'M') throw new Exception("Type of card is not valid! Type " + type + " does not exsist!");
    }
}
