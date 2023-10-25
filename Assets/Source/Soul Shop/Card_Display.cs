using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;
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
        selectedLevel = level;

        var skill = LoadData(name);
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
            combatText.text = skill.Combat[level - 1];
            noncombatText.text = skill.Noncombat;
        }

        if (isHover)
        {
            combatText.fontStyle = FontStyles.Italic;
            noncombatText.fontStyle = FontStyles.Italic;
            levelName.fontStyle = FontStyles.Italic;
            return;
        }

        if (level > 0 && level < 10) 
        {
            Unlock.SetActive(false);
            Downgrade.SetActive(true);
            Upgrade.SetActive(true);
            upgradeText.text = "-" + skill.LevelCost[level] + " SP";
            downgradeText.text = "+" + skill.DowngradeGains[level - 1] + " SP";
            downGain = int.Parse(skill.DowngradeGains[level - 1]);
            nextCost = int.Parse(skill.LevelCost[level]);
        }
        else if (level >= 10)
        {
            Unlock.SetActive(false);
            Downgrade.SetActive(true);
            Upgrade.SetActive(false);
            downgradeText.text = "+" + skill.DowngradeGains[level - 1] + " SP";
            downGain = int.Parse(skill.DowngradeGains[level - 1]);
        } 
        else 
        {
            nextCost = int.Parse(skill.LevelCost[level]);
            unlockText.text = "-" + skill.LevelCost[level] + " SP";
            Unlock.SetActive(true);
            Downgrade.SetActive(false);
            Upgrade.SetActive(false);
        }
    }

    private SkillType LoadData(string name)
    {
        // Find the mob being spawned
        TextAsset txt = (TextAsset)Resources.Load("Skills", typeof(TextAsset));
        //string json = File.ReadAllText(".\\Assets\\Src\\Jsons\\Mobs.json");
        List<SkillType> skills = JsonConvert.DeserializeObject<List<SkillType>>(txt.text) ?? throw new Exception("Empty Json!");

        foreach (SkillType skill in skills)
        {
            if (skill.Name == name)
                return skill;
        }

        throw new Exception("Unable to find skill with the name of " + name);
    }
}

internal class SkillType
{
    public int ID;
    public String Name;
    public String Flavor;
    public String[] Combat;
    public String Noncombat;
    public String[] LevelCost;
    public String[] DowngradeGains;
    public float[] ManaCost;
    public float[] StaminaCost;
}

