
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.InputSystem.Utilities;

public class Stats
{
    public double CurrentHealth { get; set; }
    public double CurrentMana { get; set; }
    public double CurrentStamina { get; set; }
    public int Level { get; private set; }

    public float StaminaRecoveryRate { get; private set; }
    public float ManaRecoveryRate { get; private set; }

    private Dictionary<string, int> StatPoints = new();
    public Dictionary<string, double> StatValues = new();

    public Stats (int health, int mana, int stamina, int strength, int agility, int speed, int luck, int level)
    {
        StatPoints.Add("Health", health);
        StatPoints.Add("Mana", mana);
        StatPoints.Add("Stamina", stamina);
        StatPoints.Add("Agility", agility);
        StatPoints.Add("Speed", speed);
        StatPoints.Add("Luck", luck);
        StatPoints.Add("Strength", strength);

        StatValues.Add("Health", 0);
        StatValues.Add("Mana", 0);
        StatValues.Add("Stamina", 0);
        StatValues.Add("Agility", 0);
        StatValues.Add("Speed", 0);
        StatValues.Add("Luck", 0);
        StatValues.Add("Strength", 0);

        Level = level;

        StaminaRecoveryRate = StaticHolder.Default_Stamina_Rate;
        ManaRecoveryRate = StaticHolder.Default_Mana_Rate;

        CalculatStats();
        Heal();
        RestoreMana();
        RestoreStamina();
    }
    

    public void StartOfTurn()
    {
        // Restore mana and stamina if applicable
        if (CurrentMana < StatValues["Mana"]) RestoreMana(ManaRecoveryRate);
        if (CurrentStamina < StatValues["Stamina"]) RestoreStamina(StaminaRecoveryRate);
    }

    public double GetStatValue(string name)
    {
        if (!StatValues.ContainsKey(name)) throw new Exception(name + " is not a valid stat!");

        return StatValues[name];
    }

    public bool PlayCard(double manaCost, double staminaCost)
    {
        if (CurrentMana < manaCost || CurrentStamina < staminaCost) return false;

        CurrentMana -= manaCost;
        CurrentStamina -= staminaCost;

        return true;
    }

    public void Heal()
    {
        CurrentHealth = StatValues["Health"];
    }

    public void Heal(float percent)
    {
        if (percent >= 1) Heal();

        CurrentHealth += Math.Round(StatValues["Health"] * percent, 1);

        if (CurrentHealth > StatValues["Health"]) Heal();
    }

    public void RestoreMana()
    {
        CurrentMana = StatValues["Mana"];
    }

    public void RestoreMana(float percent)
    {
        if (percent >= 1) RestoreMana();

        CurrentMana += Math.Round(StatValues["Mana"] * percent, 1);

        if (CurrentMana > StatValues["Mana"]) RestoreMana();
    }

    public void RestoreStamina()
    {
        CurrentStamina = StatValues["Stamina"];
    }

    public void RestoreStamina(float percent)
    {
        if (percent >= 1) RestoreStamina();

        CurrentStamina += Math.Round(StatValues["Stamina"] * percent, 1);

        if (CurrentStamina > StatValues["Stamina"]) RestoreStamina();
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
