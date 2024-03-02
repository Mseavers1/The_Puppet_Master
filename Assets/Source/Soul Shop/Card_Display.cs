using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;
using Source.Soul_Shop;
using UnityEngine;

public class Card_Display : MonoBehaviour
{
    private int nextCost, downGain;
    private string selectedName;
    private int selectedLevel;

    private const string Default_Level_0_Text = "You currently do not have this skill yet!";

    public Soul_GM gm;
    public TMP_Text cardName, levelName, flavorText, combatText, noncombatText, upgradeText, downgradeText, unlockText;
    public GameObject Upgrade, Downgrade, Unlock;

    private void Start()
    {
        Unlock.SetActive(false);
        Downgrade.SetActive(false);
        Upgrade.SetActive(false);
    }

    public int ClickUpgrade()
    {
        if (gm.GetSP() < nextCost) return selectedLevel;
        
        gm.Buy(nextCost);
        selectedLevel++;
        DisplayCard(selectedName, selectedLevel, false);

        return selectedLevel;
    }

    public int ClickDowngrade()
    {
        gm.Gain(downGain);
        selectedLevel--;
        DisplayCard(selectedName, selectedLevel, false);

        return selectedLevel;
    }

    public void DisplayCard(string name, int level, bool isHover)
    {
        combatText.fontStyle = FontStyles.Normal;
        noncombatText.fontStyle = FontStyles.Normal;
        levelName.fontStyle = FontStyles.Normal;

        selectedName = name;

        if(!isHover) selectedLevel = level;

        var skill = LoadData(name);

        int maxUpgradedLevel = skill.Levels.Length;
        levelName.text = "Lv. " + level.ToString();
        cardName.text = skill.Name;
        flavorText.text = skill.Flavor;

        if (level == 0)
        {
            combatText.text = Default_Level_0_Text;
            noncombatText.text = Default_Level_0_Text;
        } 
        else
        {
            combatText.text = skill.Levels[level - 1].Combat;
            noncombatText.text = skill.Levels[level - 1].NonCombat;
        }

        if (isHover)
        {
            combatText.fontStyle = FontStyles.Italic;
            noncombatText.fontStyle = FontStyles.Italic;
            levelName.fontStyle = FontStyles.Italic;
            return;
        }

        if (level > 0 && level < maxUpgradedLevel) 
        {
            Unlock.SetActive(false);
            Downgrade.SetActive(true);
            Upgrade.SetActive(true);
            upgradeText.text = "-" + skill.Levels[level].LevelCost + " SP";
            downgradeText.text = "+" + skill.Levels[level - 1].DowngradeGains + " SP";
            downGain = skill.Levels[level - 1].DowngradeGains;
            nextCost = skill.Levels[level].LevelCost;
        }
        else if (level >= maxUpgradedLevel)
        {
            Unlock.SetActive(false);
            Downgrade.SetActive(true);
            Upgrade.SetActive(false);
            downgradeText.text = "+" + skill.Levels[level - 1].DowngradeGains + " SP";
            downGain = skill.Levels[level - 1].DowngradeGains;
        } 
        else 
        {
            nextCost = skill.Levels[level].LevelCost;
            unlockText.text = "-" + skill.Levels[level].LevelCost + " SP";
            Unlock.SetActive(true);
            Downgrade.SetActive(false);
            Upgrade.SetActive(false);
        }
    }

    private SkillType LoadData(string name)
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

    public List<Card> CreateCards(string name, int level)
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
                //cards.Add(new Card(name, skill.Type, skill.Levels[currentLevel - 1].ManaCost, skill.Levels[currentLevel - 1].StaminaCost, effect.DamageTypes, effect.DamageRatio, effect.TotalDamage, effect.Special, effect.AOE, currentLevel, skill.Weapon));
            }

            currentLevel--;
        }

        return cards;
    }
}
