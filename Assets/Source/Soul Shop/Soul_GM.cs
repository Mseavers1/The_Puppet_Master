using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Soul_GM : MonoBehaviour
{
    public TMP_Text spText, affinitiesText;

    private int availableAffinities = 30;
    private int soulPoints = 10;

    

    private void Start()
    {
        UpdateSPText();
        UpdateAffinityText();
    }

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
}
