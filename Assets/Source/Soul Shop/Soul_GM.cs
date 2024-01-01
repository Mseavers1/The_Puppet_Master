using System;
using System.Collections;
using System.Collections.Generic;
using Source.Soul_Shop;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Soul_GM : MonoBehaviour
{
    public TMP_Text spText, affinitiesText;
    public Card_Display display;
    public TMP_Text[] pointText;
    private bool OnTutorial = true;
    public GameObject TutorialPrefab;

    private int availableAffinities = 30;
    private int soulPoints = 100;

    private void Start()
    {
        soulPoints = Int32.MaxValue;
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
        spText.text = soulPoints.ToString();
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
        int[] arr = new int[pointText.Length];
        int index = 0;

        foreach (var txt in pointText)
        {
            arr[index] = int.Parse(txt.text);
            index++;
        }

        StaticHolder.StartOfGame(arr);
        HoldingOfSkills.StartOfGame(skillNames);

        SceneManager.LoadScene(2);
    }
}
