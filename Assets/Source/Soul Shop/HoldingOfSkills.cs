using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HoldingOfSkills
{
    private static List<string> skills = new ();

    public static void StartOfGame(List<string> s)
    {
        skills = s;
    }

    private static SkillType LoadData(string name)
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

    private static List<Card> CreateCards(string name, int level)
    {
        // Get Skill
        var skill = LoadData(name);

        // Generate the cards
        var cards = new List<Card>();
        var currentLevel = level;

        while (currentLevel > 0)
        {
            var numCards = skill.Levels[currentLevel - 1].Effects[0].NumberOfCards;

            for (var i = 0; i < numCards; i++)
            {
                var effect = skill.Levels[currentLevel - 1].Effects[0];
                cards.Add(new Card(name, skill.Type, skill.Levels[currentLevel - 1].ManaCost, skill.Levels[currentLevel - 1].StaminaCost, effect.DamageTypes, effect.DamageRatio, effect.TotalDamage, effect.Special, effect.AOE));
            }

            currentLevel--;
        }

        return cards;
    }

    // Generate the player's deck based on the skills the player has selected
    public static Deck GenerateDeck()
    {
        Deck deck = new();

        foreach (var skill in skills)
        {
            var div = skill.Split(' ');
            var name = div[0];
            var level = int.Parse(div[1]);

            var cards = CreateCards(name, level);
            deck.AddCards(cards);

        }

        return deck;
    }
}
