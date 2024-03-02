using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card
{
    private string name;
    private float manaCost, staminaCost;
    private string[] damageTypes;
    private float[] damageRatio;
    private float totalDamage;
    private char type; // W - Weapon // S - Spell // D - Defense // M - Misc
    private string desc;
    private int currentLevel;
    private string requiredWeaponSlot;
    private bool isAllyOnly, isNoCombat;
    private string[] _inGameDescription;
    private string _typeName;
    private string _specials;
    
    public Card(string name, string type, float manaCost, float staminaCost, string[] damageTypes, float[] damageRatio, float totalDamage, int currentLevel, string requiredWeaponSlot, string[] inGameDescription, string special)
    {
        this.manaCost = manaCost;
        this.staminaCost = staminaCost;
        this.name = name;
        this.damageTypes = damageTypes;
        _typeName = type;
        this.damageRatio = damageRatio;
        this.totalDamage = totalDamage;
        _inGameDescription = inGameDescription;
        _specials = special;

        this.currentLevel = currentLevel;
        this.requiredWeaponSlot = requiredWeaponSlot;
        isAllyOnly = false;

        if (totalDamage == 0) isNoCombat = true;

        this.type = type[0];
        CheckType();
        
        if (!isNoCombat)
            desc = inGameDescription[0] + " " + totalDamage + " " + damageTypes[0] + " " + inGameDescription[1] + "/n";

        if (special != "")
        {
            desc += ParseSpecial(special);
        }
        
    }
    
    private string ParseSpecial(string special)
    {
        var commands = special.Split(',');
        var description = commands.Select(command => command.Split(' '))
            .Aggregate("", (current, sentence) => current + sentence[0] switch
            {
                "Heal" => "Heal " + sentence[1] + " % HP\n",
                _ => throw new Exception("The command " + sentence[0] + " is no been implemented.")
            });

        return description;
    }

    public string GetSpecial() { return _specials; }

    public string GetFullTypeName() { return _typeName; }

    public float GetDamage() { return totalDamage; }
    public string[] GetDamageTypes() {  return damageTypes; }
    public float[] GetDamageRatio() { return damageRatio; }
    public string GetRequiredWeapon() { return requiredWeaponSlot; }
    public int GetLevel() { return currentLevel; }
    public string GetName() { return name; }
    public string GetDesc() { return desc; }
    public void SetDesc(string desc) { this.desc = desc; }
    public float GetManaCost() { return manaCost; }
    public float GetStaminaCost() { return staminaCost; }
    public bool IsAllyOnly() { return isAllyOnly; }
    public bool IsNoCombat() { return isNoCombat; }

    public bool IsTypeOf(char type)
    {
        if (this.type == type) return true;

        return false;
    }

    public char GetTypeName() { return type; }


    private void CheckType()
    { 
        if (type != 'W' && type != 'S' && type != 'D' && type != 'M') throw new Exception("Type of card is not valid! Type " + type + " does not exsist!");
    }
}
