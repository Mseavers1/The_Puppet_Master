using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Card
{
    private float manaCost, staminaCost;
    private char type; // W - Weapon // S - Spell // D - Defense // M - Misc
    private string desc;

    public Card(string name, int level)
    {

    }


    private void CheckType()
    { 
        if (type != 'W' && type != 'S' && type != 'D' && type != 'M') throw new Exception("Type of card is not valid! Type " + type + " does not exsist!");
    }
}
