using System;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public string SpecialType { get; private set; }

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
    
    public Card(string name, string type, float manaCost, float staminaCost, string[] damageTypes, float[] damageRatio, float totalDamage, int currentLevel, string requiredWeaponSlot, string[] inGameDescription)
    {
        this.manaCost = manaCost;
        this.staminaCost = staminaCost;
        this.name = name;
        this.damageTypes = damageTypes;
        _typeName = type;
        this.damageRatio = damageRatio;
        this.totalDamage = totalDamage;
        _inGameDescription = inGameDescription;

        this.currentLevel = currentLevel;
        this.requiredWeaponSlot = requiredWeaponSlot;
        isAllyOnly = false;

        foreach (var t in damageTypes)
        {
            if (t == "Heal")
                isNoCombat = true;
        }

        this.type = type[0];
        CheckType();
        
        if (!isNoCombat)
            desc = inGameDescription[0] + " " + totalDamage + " " + damageTypes[0] + " " + inGameDescription[1];
        else
            desc = inGameDescription[0] + " " + totalDamage + " " + inGameDescription[1];
    }

    public string[] GetExtra() { return _inGameDescription; }

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
