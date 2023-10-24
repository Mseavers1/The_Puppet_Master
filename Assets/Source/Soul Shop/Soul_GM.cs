using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Soul_GM : MonoBehaviour
{
    public TMP_Text spText;
    private int soulPoints = 10;

    

    private void Start()
    {
        UpdateSPText();
    }

    public int GetSP() { return soulPoints; }

    public void UpdateSPText()
    {
        spText.text = soulPoints.ToString();
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
