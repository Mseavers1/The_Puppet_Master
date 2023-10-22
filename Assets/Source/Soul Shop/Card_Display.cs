using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;
using UnityEngine;

public class Card_Display : MonoBehaviour
{
    private int l = 3;
    private int nextCost, downGain;

    public Soul_GM gm;
    public TMP_Text cardName, levelName, flavorText, combatText, noncombatText, upgradeText, downgradeText, unlockText;
    public GameObject Upgrade, Downgrade, Unlock;

    private void Start()
    {
        DisplayCard("Swordsmanship", l);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(2))
        {
            gm.Gain(10);
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Downgrade
            gm.Gain(downGain);
            l--;
            DisplayCard("Swordsmanship", l);
            gm.UpdateSPText();
        }

        if (Input.GetMouseButtonDown(1))
        {
            // Upgrade
            if (gm.GetSP() < nextCost) return;
            gm.Buy(nextCost);
            l++;
            DisplayCard("Swordsmanship", l);
            gm.UpdateSPText();
        }
    }

    private void DisplayCard(string name, int level)
    {
        var skill = LoadData(name);
        levelName.text = "Lv. " + level.ToString();
        cardName.text = skill.Name;
        flavorText.text = skill.Flavor;
        combatText.text = skill.Combat;
        noncombatText.text = skill.Noncombat;

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
    public String Combat;
    public String Noncombat;
    public String[] LevelCost;
    public String[] DowngradeGains;
}

