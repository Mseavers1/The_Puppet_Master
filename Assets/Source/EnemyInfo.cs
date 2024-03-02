using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Source.Game;
using Unity.Mathematics;
using UnityEngine;

public class EnemyInfo : IBattleable
{
    private const int Max_Level = 100;
    public Stats Stat { get; private set; }
    private int _id;
    public int holdingSP;
    
    private string mobName;

    private Dictionary<string, string> curveStats = new ();
    private Dictionary<string, int> bonusStats = new();

    private Deck deck;
    private Card[] hand;
    private Dictionary<string, float> skills = new ();

    private GameGm _gm;

    public EnemyInfo(int id, GameGm gm, int level)
    {
        _id = id;
        _gm = gm;
        
        // Get Mob info from json
        var mob = LoadData(_id);
        mobName = mob.Name;

        curveStats.Add("health", CheckJSONName(mob.CurveHealth, "health"));
        curveStats.Add("mana", CheckJSONName(mob.CurveMana, "mana"));
        curveStats.Add("stamina", CheckJSONName(mob.CurveStamina, "stamina"));
        curveStats.Add("strength", CheckJSONName(mob.CurveStamina, "strength"));
        curveStats.Add("luck", CheckJSONName(mob.CurveLuck, "luck"));
        curveStats.Add("speed", CheckJSONName(mob.CurveSpeed, "speed"));
        curveStats.Add("agility", CheckJSONName(mob.CurveAgility, "agility"));

        bonusStats.Add("health", mob.MaxHealth);
        bonusStats.Add("mana", mob.MaxMana);
        bonusStats.Add("stamina", mob.MaxStamina);
        bonusStats.Add("strength", mob.Strength);
        bonusStats.Add("luck", mob.Luck);
        bonusStats.Add("speed", mob.Speed);
        bonusStats.Add("agility", mob.Agility);

        // Generate and sets new stats based on current level
        Stat = new Stats(bonusStats["health"], bonusStats["mana"], bonusStats["stamina"], bonusStats["strength"], bonusStats["agility"], bonusStats["speed"], bonusStats["luck"], level);
        Debug.Log(Stat);

        // Convert skills into list
        GenerateSkillsList(mob);

        // Generate Cards
        deck = GenerateDeck();
        Debug.Log("Enemy Deck: " + deck);

        // Generate Hand
        hand = deck.GenerateHand();
        foreach (var card in hand) Debug.Log("Enemy Card in Hand: " + card.GetName());

        // Calculate SP
        var slope = (mob.MaxSP - mob.BaseSP) / (99);
        holdingSP = Mathf.RoundToInt(slope * (level-1) + mob.BaseSP);
    }

    public void PlayTurn()
    {
        // Restore Mana/Stamina
        Stat.StartOfTurn();

        // Need to make a reference since skills uses both level and the name in the key
        Dictionary<string, string> dict = new ();
        foreach (var ele in skills)
        {
            var split = ele.Key.Split(' ');
            var name = split[0];

            dict.Add(name, ele.Key);
        }


        // Calculate total from all chances in hand
        float total = 0;
        foreach (var card in hand)
        {
            total += skills[dict[card.GetName()]];
        }

        var attempts = 0;

        while (attempts < 3 && !StaticHolder.HasDied)
        {
            float chanceToStop = UnityEngine.Random.Range(0, 100);
            if (chanceToStop > 70) break;
            
            // Get Random number
            float rand = UnityEngine.Random.Range(0, total);

            // Find Card based on chances
            float currentTotal = 0;
            int index = 0;

            foreach (var c in hand)
            {
                currentTotal += skills[dict[c.GetName()]];

                // Find match
                if (rand < currentTotal)
                {
                    // Check if card is playable
                    if (!Stat.PlayCard(hand[index].GetManaCost(), hand[index].GetStaminaCost()))
                    {
                        Debug.Log("Do not have enough stamina or mp");
                        attempts++;
                        break;
                    }

                    PlayCard(index);

                    break;
                }

                index++;
            }
        }
        
        // Progress order when move is done.
        _gm.NextTurn();
    }

    private void PlayCard(int cardIndex)
    {
        var card = hand[cardIndex];
        var damage = card.GetDamage();

        // Replace card in there hand
        hand[cardIndex] = deck.PullCard(deck.GetTypeIndex(cardIndex));
        Debug.Log(mobName + " used " + card.GetName() + " at level " + card.GetLevel() + " dealing a total of " + damage + " damage!");
        
        // No combat
        if (!card.IsNoCombat())
        {
            StaticHolder.TakeDamage(damage);
            _gm.UpdateHealthIcon();
        }


    }

    private void GenerateSkillsList(Mobs mob)
    {
        foreach (var skill in mob.MobSkills)
        {
            skills.Add(skill.Name + " " + skill.Level, skill.Chance);
        }
    }

    private Deck GenerateDeck()
    {
        Deck deck = new(false);

        foreach (var skill in skills)
        {
            var div = skill.Key.Split(' ');
            var name = div[0];
            var level = int.Parse(div[1]);

            var cards = HoldingOfSkills.CreateCards(name, level);
            deck.AddCards(cards);

        }

        deck.RandomizeDeck();
        return deck;
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
    
    private void UpdateHealthDisplay()
    {
        _gm.UpdateEnemyHealthIcon(this);
    }

    public void TakeDamage(float damage)
    {
        Stat.CurrentHealth -= damage;
        UpdateHealthDisplay();
    }

    public string ChangeMode()
    {
        return "Battle Enemy";
    }

    public bool IsDead() { return Stat.CurrentHealth <= 0; }
}

internal class Mobs
{
    public int ID;
    public string Name;
    public int BaseSP;
    public int MaxSP;
    public int MaxHealth;
    public string CurveHealth;
    public int MaxMana;
    public string CurveMana;
    public int MaxStamina;
    public string CurveStamina;
    public int Agility;
    public string CurveAgility;
    public int Speed;
    public string CurveSpeed;
    public int Luck;
    public string CurveLuck;
    public int Strength;
    public string CurveStrength;
    public MobSkills[] MobSkills;
}

internal class MobSkills
{
    public string Name;
    public int Level;
    public float Chance;
}
