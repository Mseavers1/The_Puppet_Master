using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HoldingOfSkills
{
    public static Dictionary<string, int> BoughtSkills = new();


    //private static List<string> skills = new ();

    public static void StartOfGame(List<string> s)
    {
        //skills = s;
        //skills.Add("Hand 1");
    }
    
    public static bool ContainSkill(string skillName)
    {
        return false;
    }

    public static SkillType LoadData(string name)
    {
        TextAsset txt = (TextAsset)Resources.Load("Skills", typeof(TextAsset));
        List<SkillType> skills = JsonConvert.DeserializeObject<List<SkillType>>(txt.text) ?? throw new Exception("Empty Json!");

        foreach (SkillType skill in skills)
        {
            if (skill.Name == name)
                return skill;
        }

        throw new Exception("Unable to find skill with the name of " + name);
    }

    public static List<SkillType> GetAllSkills()
    {
        TextAsset txt = (TextAsset)Resources.Load("Skills", typeof(TextAsset));
        List<SkillType> skills = JsonConvert.DeserializeObject<List<SkillType>>(txt.text) ?? throw new Exception("Empty Json!");

        return skills;
    }

    public static List<Card> CreateCards(string name, int level)
    {
        // Get Skill
        var skill = LoadData(name);

        // Generate the cards
        var cards = new List<Card>();
        var currentLevel = level;

        while (currentLevel > 0)
        {
            var numCards = skill.NumberOfCards[currentLevel - 1];

            for (var i = 0; i < numCards; i++)
            {
               cards.Add(new Card(name, skill.Type, skill.ManaCost[currentLevel - 1], skill.StaminaCost[currentLevel - 1], skill.DamageTypes, skill.DamageRatios, skill.TotalDamage[currentLevel - 1], currentLevel, null, skill.GameCardInfo, skill.Special[currentLevel - 1]));
            }

            currentLevel--;
        }

        return cards;
    }

    // Generate the player's deck based on the skills the player has selected
    public static Deck GenerateDeck()
    {
        Deck deck = new(true);

        foreach (var (skillName, level) in BoughtSkills)
        {
            if (level == 0) continue;
            
            var cards = CreateCards(skillName, level);
            deck.AddCards(cards);
        }

        deck.RandomizeDeck();
        return deck;
    }
}
