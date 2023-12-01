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
    private string desc, AOE;
    private int currentLevel;
    private string requiredWeaponSlot;
    private string[] specials, spellAttributes;
    private bool isAllyOnly, isNoCombat;

    public Card(string name, string type, float manaCost, float staminaCost, string[] damageTypes, float[] damageRatio, float totalDamage, string special, string aoe, int currentLevel, string requiredWeaponSlot)
    {
        this.manaCost = manaCost;
        this.staminaCost = staminaCost;
        this.name = name;
        this.damageTypes = damageTypes;
        this.damageRatio = damageRatio;
        this.totalDamage = totalDamage;
        specials = special.Split(',');
        this.currentLevel = currentLevel;
        this.requiredWeaponSlot = requiredWeaponSlot;
        AOE = aoe;
        isAllyOnly = false;
        isNoCombat = false;

        this.type = char.Parse(type);
        CheckType();

        if (!isNoCombat && damageTypes.Length > 0)
            desc = "Deals " + totalDamage + " " + damageTypes[0] + " damage!";

        SpecialParser();
    }

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
    public string[] GetSpellAttributes() { return spellAttributes; }

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

    private void SpecialParser()
    {
        List<string> nonusedSpecials = new List<string>();
        int index = 0;
        foreach(var special in specials)
        {
            bool found = false;
            switch (special)
            {
                case "ALLY":
                    found = true;
                    isAllyOnly = true;
                    break;
                case "NO COMBAT":
                    found = true;
                    isNoCombat = true;
                    break;
            }

            if (!found)
                nonusedSpecials.Add(special);

            index++;
        }

        spellAttributes = nonusedSpecials.ToArray();

        foreach (var att in spellAttributes)
        {
            var command = att.Split(' ');
            switch (command[0])
            {
                case "HEAL":
                    SpecialType = "Heals";
                    break;
            }
        }


    }
}
