using System;
using System.Collections.Generic;

public static class StaticHolder
{
    // Player Stuff
    public static Stats PlayerStats { get; private set; }

    // Defaults
    public const int Max_Point_Size = 200, Max_Buyable_Point_Size = 100, Max_Level = 100;
    public const float Default_Stamina_Rate = 0.1f, Default_Mana_Rate = 0.1f;

    // Linear, Power, or Log -- Used in the entire game
    public static readonly Dictionary<string, string> curves = new();
    public static readonly Dictionary<string, double> defaultStatValues = new();
    public static readonly Dictionary<string, double> maxValues = new();

    public static void StartOfGame(int[] points)
    {
        SetCurves();
        SetDefaultValues();
        SetMaxValues();

        // Check if there are any errors in curves
        foreach (var curve in curves) if (curve.Value != "linear" && curve.Value != "power" && curve.Value != "log") throw new Exception("Curve array contains something that is not LINEAR, POWER, or LOG!! -> " + curve);

        PlayerStats = new Stats(points[0], points[1], points[2], points[3], points[4], points[5], points[6], points[7], points[8], points[9], 1);
    }

    private static void SetCurves()
    {
        curves.Add("Health", "linear");
        curves.Add("Mana", "linear");
        curves.Add("Stamina", "linear");
        curves.Add("Luck", "linear");
        curves.Add("Agility", "linear");
        curves.Add("Speed", "linear");
        curves.Add("PStrength", "linear");
        curves.Add("MStrength", "linear");
        curves.Add("PDefense", "linear");
        curves.Add("MDefense", "linear");
    }

    private static void SetDefaultValues()
    {
        defaultStatValues.Add("Health", 100000);
        defaultStatValues.Add("Mana", 10);
        defaultStatValues.Add("Stamina", 10);
        defaultStatValues.Add("Luck", 10);
        defaultStatValues.Add("Agility", 10);
        defaultStatValues.Add("Speed", 10);
        defaultStatValues.Add("PStrength", 10);
        defaultStatValues.Add("MStrength", 10);
        defaultStatValues.Add("PDefense", 10);
        defaultStatValues.Add("MDefense", 10);
    }

    private static void SetMaxValues()
    {
        maxValues.Add("Health", 1000);
        maxValues.Add("Mana", 1000);
        maxValues.Add("Stamina", 1000);
        maxValues.Add("Luck", 1000);
        maxValues.Add("Agility", 1000);
        maxValues.Add("Speed", 1000);
        maxValues.Add("PStrength", 1000);
        maxValues.Add("MStrength", 1000);
        maxValues.Add("PDefense", 1000);
        maxValues.Add("MDefense", 1000);
    }
}
