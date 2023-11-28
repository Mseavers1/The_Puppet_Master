
using System;
using System.Collections.Generic;
using UnityEngine.InputSystem.Utilities;

public class Stats
{
    public double CurrentHealth { get; set; }
    public double CurrentMana { get; set; }
    public double CurrentStamina { get; set; }
    public int Level { get; private set; }

    private Dictionary<string, int> StatPoints = new();
    public Dictionary<string, double> StatValues = new();

    public Stats (int health, int mana, int stamina, int agility, int speed, int luck, int pStrength, int mStrength, int pDefense, int mDefense, int level)
    {
        StatPoints.Add("Health", health);
        StatPoints.Add("Mana", mana);
        StatPoints.Add("Stamina", stamina);
        StatPoints.Add("Agility", agility);
        StatPoints.Add("Speed", speed);
        StatPoints.Add("Luck", luck);
        StatPoints.Add("PStrength", pStrength);
        StatPoints.Add("MStrength", mStrength);
        StatPoints.Add("PDefense", pDefense);
        StatPoints.Add("MDefense", mDefense);

        StatValues.Add("Health", 0);
        StatValues.Add("Mana", 0);
        StatValues.Add("Stamina", 0);
        StatValues.Add("Agility", 0);
        StatValues.Add("Speed", 0);
        StatValues.Add("Luck", 0);
        StatValues.Add("PStrength", 0);
        StatValues.Add("MStrength", 0);
        StatValues.Add("PDefense", 0);
        StatValues.Add("MDefense", 0);

        Level = level;

        CalculatStats();
        Heal();
        RestoreMana();
        RestoreStamina();
    }

    public double GetStatValue(string name)
    {
        if (!StatValues.ContainsKey(name)) throw new Exception(name + " is not a valid stat!");

        return StatValues[name];
    }

    public void Heal()
    {
        CurrentHealth = StatValues["Health"];
    }

    public void Heal(float percent)
    {
        if (percent >= 1) Heal();

        CurrentHealth += Math.Round(StatValues["Health"] * percent, 3);
    }

    public void RestoreMana()
    {
        CurrentMana = StatValues["Mana"];
    }

    public void RestoreMana(float percent)
    {
        if (percent >= 1) RestoreMana();

        CurrentHealth += Math.Round(StatValues["Mana"] * percent, 3);
    }

    public void RestoreStamina()
    {
        CurrentStamina = StatValues["Stamina"];
    }

    public void RestoreStamina(float percent)
    {
        if (percent >= 1) RestoreStamina();

        CurrentHealth += Math.Round(StatValues["Stamina"] * percent, 3);
    }

    public void CalculatStats()
    {
        foreach (var x in StatPoints)
        {
            var stat = x.Key;
            var type = StaticHolder.curves[stat];
            var defaultValue = StaticHolder.defaultStatValues[stat];
            var maxValues = StaticHolder.maxValues[stat];
            var maxPoints = StaticHolder.Max_Point_Size;
            var totalPoints = Level + x.Value;

            double slope;

            // Calculations
            switch (type)
            {
                case "power":
                    slope = (maxValues - defaultValue) / Math.Pow(maxPoints - 1f, 2);
                    StatValues[stat] = Math.Round((slope * Math.Pow(totalPoints - 1, 2)) + defaultValue, 1);
                    break;
                case "log":
                    slope = (maxValues - defaultValue) / Math.Log10(maxPoints);
                    StatValues[stat] = Math.Round((slope * Math.Log10(totalPoints)) + defaultValue, 1); ;
                    break;
                default: // linear
                    slope = (maxValues - defaultValue) / (maxPoints - 1f);
                    StatValues[stat] = Math.Round((slope * (totalPoints - 1)) + defaultValue, 1);
                    break;
            }
        }


    }
}
