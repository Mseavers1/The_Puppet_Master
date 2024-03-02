using System;
using System.Collections.Generic;
using UnityEngine;

public static class StaticHolder
{
    // Player Stuff
    public static Stats PlayerStats { get; private set; }

    public static bool HasDied { get; set; } = false;
    public static bool ShowTutorialSoul { get; set; } = true;
    public static bool ShowTutorialGame { get; set; } = true;

    public static int SP { get; set; }
    public static InventoryManagement InventoryManagement { get; private set; }

    // Defaults
    public const int Max_Point_Size = 200, Max_Buyable_Point_Size = 100, Max_Level = 100;
    public const float Default_Stamina_Rate = 0.1f, Default_Mana_Rate = 0.1f;

    // Linear, Power, or Log -- Used in the entire game
    public static readonly Dictionary<string, string> curves = new();
    public static readonly Dictionary<string, double> defaultStatValues = new();
    public static readonly Dictionary<string, double> maxValues = new();

    public static void TakeDamage(float damage)
    {
        PlayerStats.CurrentHealth -= damage;

        if (PlayerStats.CurrentHealth <= 0) HasDied = true;
    }

    public static void StartOfGame(int[] points)
    {
        if (HasDied)
        {
            HasDied = false;
            PlayerStats = null; InventoryManagement = null;
            PlayerStats = new Stats(points[0], points[1], points[2], points[3], points[4], points[5], points[6], 1);

            InventoryManagement = new InventoryManagement();
            return;
        }

        SetCurves();
        SetDefaultValues();
        SetMaxValues();

        // Check if there are any errors in curves
        foreach (var curve in curves) if (curve.Value != "linear" && curve.Value != "power" && curve.Value != "log") throw new Exception("Curve array contains something that is not LINEAR, POWER, or LOG!! -> " + curve);

        PlayerStats = new Stats(points[0], points[1], points[2], points[3], points[4], points[5], points[6], 1);

        InventoryManagement = new InventoryManagement();
    }

    private static void SetCurves()
    {
        curves.Add("Health", "linear");
        curves.Add("Mana", "linear");
        curves.Add("Stamina", "linear");
        curves.Add("Luck", "linear");
        curves.Add("Agility", "linear");
        curves.Add("Speed", "linear");
        curves.Add("Strength", "linear");
    }

    private static void SetDefaultValues()
    {
        defaultStatValues.Add("Health", 10);
        defaultStatValues.Add("Mana", 10);
        defaultStatValues.Add("Stamina", 10);
        defaultStatValues.Add("Luck", 10);
        defaultStatValues.Add("Agility", 10);
        defaultStatValues.Add("Speed", 10);
        defaultStatValues.Add("Strength", 10);
    }

    private static void SetMaxValues()
    {
        maxValues.Add("Health", 1000);
        maxValues.Add("Mana", 1000);
        maxValues.Add("Stamina", 1000);
        maxValues.Add("Luck", 1000);
        maxValues.Add("Agility", 1000);
        maxValues.Add("Speed", 1000);
        maxValues.Add("Strength", 1000);
    }
}
