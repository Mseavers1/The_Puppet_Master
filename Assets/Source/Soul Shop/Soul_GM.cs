using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Source.Soul_Shop;
using Source.Utility;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Soul_GM : MonoBehaviour
{
    public GameObject[] menuCanvases;
    
    public TMP_Text spText, affinitiesText;
    public Card_Display display;
    private bool OnTutorial = true;
    public GameObject TutorialPrefab;

    public TMP_Text[] pointValues;
    private int availableAffinities = 30;
    private int soulPoints = 100;

    private void Start()
    {
        soulPoints = 10000;
        soulPoints += StaticHolder.SP;

        print(StaticHolder.ShowTutorialSoul);
        if(!StaticHolder.ShowTutorialSoul)
        {
            ExitTutorial();
            TutorialPrefab.SetActive(false);
        }

        UpdateSPText();
        UpdateAffinityText();
    }

    public void SwitchCanvas(int id)
    {
        menuCanvases[id].SetActive(true);

        for (var i = 0; i < menuCanvases.Length; i++)
        {
            if (i == id) continue;
            
            menuCanvases[i].SetActive(false);
        }
    }

    public int GetCanvasID(string canvas)
    {
        return canvas switch
        {
            "Home" => 0,
            "Stats" => 1,
            "Affinity" => 2,
            "Transcend" => 3,
            "Skills" => 4,
            "Item Shop" => 5,
            "Curses" => 6,
            _ => throw new Exception("Unable to find the correct ID from the name " + canvas)
        };
    }

    public void ExitTutorial()
    {
        OnTutorial = false;

        StaticHolder.ShowTutorialSoul = false;
    }

    public bool IsTutorialOn() { return OnTutorial; }

    public int GetSP() { return soulPoints; }
    public int GetAffinityCount() { return availableAffinities; }

    public void UpdateSPText()
    {
        spText.text = GlobalResources.SoulEssences.ToString(CultureInfo.CurrentCulture);
    }

    public void UpdateAffinityText()
    {
        var txt = affinitiesText.text.Split(" ");
        affinitiesText.text = txt[0] + " " + txt[1] + " " + availableAffinities.ToString();
    }

    public void AddAffinity(int SP)
    {
        soulPoints -= SP;
        availableAffinities -= 1;
        UpdateAffinityText();
        UpdateSPText();
    }

    public void RemoveAffinity(int SP)
    {
        soulPoints += SP;
        availableAffinities += 1;
        UpdateAffinityText();
        UpdateSPText();
    }

    public void Buy(int SP)
    {
        soulPoints -= SP;
        UpdateSPText();
    }

    public void Gain(int SP)
    {
        soulPoints += SP;
        UpdateSPText();
    }

    // Method called when game starts (after selecting skills)
    public void StartGame()
    {
        if (OnTutorial) return;

        var allSkills = GameObject.FindGameObjectsWithTag("SkillOption");
        var skillNames = new List<string>();

        foreach (var skill in allSkills)
        {
            var div = skill.name.Split(' ');
            var name = div[0];
            var level = int.Parse(div[1]);

            if (level <= 0) continue;


            skillNames.Add(name + " " + level);
        }

        // Get points from pointTexts
        int[] arr = new int[pointValues.Length];
        int index = 0;

        foreach (var txt in pointValues)
        {
            arr[index] = int.Parse(txt.text);
            index++;
        }

        StaticHolder.StartOfGame(arr);
        HoldingOfSkills.StartOfGame(skillNames);

        SceneManager.LoadScene(2);
    }
}
