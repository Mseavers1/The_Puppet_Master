using System;

public class Card
{
    private string name;
    private float manaCost, staminaCost;
    private string[] damageTypes;
    private float[] damageRatio;
    private float totalDamage;
    private char type; // W - Weapon // S - Spell // D - Defense // M - Misc
    private string desc, special, AOE;
    private int currentLevel;

    public Card(string name, string type, float manaCost, float staminaCost, string[] damageTypes, float[] damageRatio, float totalDamage, string special, string aoe, int currentLevel)
    {
        this.manaCost = manaCost;
        this.staminaCost = staminaCost;
        this.name = name;
        this.damageTypes = damageTypes;
        this.damageRatio = damageRatio;
        this.totalDamage = totalDamage;
        this.special = special;
        this.currentLevel = currentLevel;
        AOE = aoe;

        this.type = char.Parse(type);
        CheckType();

        desc = "Deals " + totalDamage + " " + damageTypes[0] + " damage!";
    }

    public float GetDamage() { return totalDamage; }

    public int GetLevel() { return currentLevel; }

    public string GetName() { return name; }

    public string GetDesc() { return desc; }

    public bool IsTypeOf(char type)
    {
        if (this.type == type) return true;

        return false;
    }


    private void CheckType()
    { 
        if (type != 'W' && type != 'S' && type != 'D' && type != 'M') throw new Exception("Type of card is not valid! Type " + type + " does not exsist!");
    }
}
