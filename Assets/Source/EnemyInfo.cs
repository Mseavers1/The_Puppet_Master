using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour, IComparable<GameObject>
{
    private const int Max_Level = 100;
    public Stats Stat { get; private set; }
    public int ID;
    [Range(1, Max_Level)] public int level;

    private BattleSimulator battle;
    private string mobName;
    private Dictionary<string, string> curveStats = new ();
    private Dictionary<string, float> baseStats = new();
    private Dictionary<string, float> maxstats = new();

    private void Awake()
    {
        battle = GameObject.FindGameObjectWithTag("GameManager").GetComponent<BattleSimulator>();
    }

    private void Start()
    {
        // Get Mob info from json
        Mobs mob = LoadData(ID);
        mobName = mob.Name;

        curveStats.Add("health", CheckJSONName(mob.CurveHealth, "health"));
        curveStats.Add("mana", CheckJSONName(mob.CurveMana, "mana"));
        curveStats.Add("stamina", CheckJSONName(mob.CurveStamina, "stamina"));
        curveStats.Add("luck", CheckJSONName(mob.CurveLuck, "luck"));
        curveStats.Add("speed", CheckJSONName(mob.CurveSpeed, "speed"));
        curveStats.Add("agility", CheckJSONName(mob.CurveAgility, "agility"));
        curveStats.Add("pStrength", CheckJSONName(mob.CurvePStrength, "pStrength"));
        curveStats.Add("mStrength", CheckJSONName(mob.CurveMStrength, "mStrength"));
        curveStats.Add("pDefense", CheckJSONName(mob.CurvePDefense, "pDefense"));
        curveStats.Add("mDefense", CheckJSONName(mob.CurveMDefense, "mDefense"));

        baseStats.Add("health", mob.MaxHealth);
        baseStats.Add("mana", mob.MaxMana);
        baseStats.Add("stamina", mob.MaxStamina);
        baseStats.Add("luck", mob.Luck);
        baseStats.Add("speed", mob.Speed);
        baseStats.Add("agility", mob.Agility);
        baseStats.Add("pStrength", mob.PStrength);
        baseStats.Add("mStrength", mob.MStrength);
        baseStats.Add("pDefense", mob.PDefense);
        baseStats.Add("mDefense", mob.MDefense);

        maxstats.Add("health", mob.MaxedHealth);
        maxstats.Add("mana", mob.MaxedMana);
        maxstats.Add("stamina", mob.MaxedStamina);
        maxstats.Add("luck", mob.MaxedLuck);
        maxstats.Add("speed", mob.MaxedSpeed);
        maxstats.Add("agility", mob.MaxedAgility);
        maxstats.Add("pStrength", mob.MaxedPStrength);
        maxstats.Add("mStrength", mob.MaxedMStrength);
        maxstats.Add("pDefense", mob.MaxedPDefense);
        maxstats.Add("mDefense", mob.MaxedMDefense);

        // Generate and sets new stats based on current level
        GenerateNewStats();
        Debug.Log(Stat);
    }

    private void GenerateNewStats()
    {
        var newValues = new Dictionary<string, float>();
        foreach (var item in baseStats)
        {
            newValues.Add(item.Key, GenerateNewValue(curveStats[item.Key], item.Value, maxstats[item.Key])); 
        }

        Stat = new Stats(newValues["health"], newValues["mana"], newValues["stamina"], newValues["luck"], newValues["speed"], newValues["agility"], newValues["pStrength"], newValues["mStrength"], newValues["pDefense"], newValues["mDefense"]);
    }

    private float GenerateNewValue(string type, float baseValue, float maxedValue)
    {
        float slope;
        switch (type)
        {
            case "pow": // Power
                slope = (float) ((maxedValue - baseValue) / (Math.Pow(Max_Level - 1f, 2)));
                return (float) Math.Round((slope * Math.Pow(level - 1, 2)) + baseValue, 3);
            case "log": // Logarithm 
                slope = (float) ((maxedValue - baseValue) / (Math.Log10(Max_Level)));
                return (float) Math.Round((slope * Math.Log10(level)) + baseValue, 3);
            default: // Linear
                slope = (maxedValue - baseValue) / (Max_Level - 1f);
                return (float) Math.Round((slope * (level - 1)) + baseValue, 3);
        }
    }

    private string CheckJSONName(string name, string type)
    {
        if (name != "linear" && name != "pow" && name != "log") throw new Exception("Error in Mob JSON - The curve for [" + type + "] was named [" + name + "] instead of [linear, pow, log]!");

        return name;
    }

    private Mobs LoadData(int id)
    {
        TextAsset txt = (TextAsset)Resources.Load("Mobs", typeof(TextAsset));
        List<Mobs> mobs = JsonConvert.DeserializeObject<List<Mobs>>(txt.text) ?? throw new Exception("Empty Json!");

        foreach (Mobs mob in mobs)
        {
            if (mob.ID == id)
                return mob;
        }

        throw new Exception("Unable to find Mob with the id of " + id);
    }

    public void StartBattle(GameObject[] playables)
    {
        GameObject[] x = {gameObject}; // TEMP
        battle.BattleSetup(playables, x);
    }

    public int CompareTo(GameObject other)
    {
        Stats otherStat;
        if (other.GetComponent<EnemyInfo>() != null) otherStat = other.GetComponent<PlayableStats>().Stat;
        else otherStat = other.GetComponent<EnemyInfo>().Stat;

        if (Stat.Agility > otherStat.Agility) return 1;
        if (Stat.Agility <= otherStat.Agility) return 0;

        throw new Exception("Unable to determine Order...");
    }
}

internal class Mobs
{
    public int ID;
    public string Name;
    public float MaxHealth;
    public float MaxedHealth;
    public string CurveHealth;
    public float MaxMana;
    public float MaxedMana;
    public string CurveMana;
    public float MaxStamina;
    public float MaxedStamina;
    public string CurveStamina;
    public float Agility;
    public float MaxedAgility;
    public string CurveAgility;
    public float Speed;
    public float MaxedSpeed;
    public string CurveSpeed;
    public float Luck;
    public float MaxedLuck;
    public string CurveLuck;
    public float PStrength;
    public float MaxedPStrength;
    public string CurvePStrength;
    public float MStrength;
    public float MaxedMStrength;
    public string CurveMStrength;
    public float PDefense;
    public float MaxedPDefense;
    public string CurvePDefense;
    public float MDefense;
    public float MaxedMDefense;
    public string CurveMDefense;
}
