using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class Gamemanager_World : MonoBehaviour
{
    public DisplayStatTop[] displays;
    public Button endOfTurn;
    public GameObject bar;
    public Image[] icons;
    private double[] topSizes;

    [Range(0.4f, 1.3f)]
    public float scale = 0.8f;
    public string Mode = "None";

    private void Start()
    {
        topSizes = new double[icons.Length];
        for (int i = 0; i < icons.Length; i++) topSizes[i] = icons[i].rectTransform.sizeDelta.y;


        if (bar.gameObject.activeSelf) UpdateIconsText();
    }

    public void UpdateIconsText()
    {
        var stats = StaticHolder.PlayerStats;
        displays[0].UpdateText(stats.CurrentHealth + " / " + stats.GetStatValue("Health") + " HP"); // Health
        displays[1].UpdateText(stats.CurrentMana + " / " + stats.GetStatValue("Mana") + " MP"); // Mana
        displays[2].UpdateText(stats.CurrentStamina + " / " + stats.GetStatValue("Stamina") + " SP"); // Stamina
    }

    public void EndTurn()
    {
        var battle = GetComponent<BattleSimulator>();

        // Button should not be active but just in case
        if (!battle.IsPlayerTurn()) return;

        endOfTurn.interactable = false;
        battle.NextTurn();
    }

    public void TurnButtonOn()
    {
        endOfTurn.interactable = true;
        StaticHolder.PlayerStats.StartOfTurn();

        var playerStats = StaticHolder.PlayerStats;

        displays[1].UpdateText(playerStats.CurrentMana + " / " + playerStats.GetStatValue("Mana") + " MP");
        displays[2].UpdateText(playerStats.CurrentStamina + " / " + playerStats.GetStatValue("Stamina") + " SP");
    }

    public void UpdateIcon()
    {
        int i = 0;
        foreach (var icon in icons)
        {
            double current = 0, max = 0;
            var goodIcon = icons[i];
            var topSize = topSizes[i];
            var stats = StaticHolder.PlayerStats;

            switch (i)
            {
                case 0:
                    current = stats.CurrentHealth;
                    max = stats.GetStatValue("Health");
                    break;
                case 1:
                    current = stats.CurrentMana;
                    max = stats.GetStatValue("Mana");
                    break;
                case 2:
                    current = stats.CurrentStamina;
                    max = stats.GetStatValue("Stamina");
                    break;
                default: throw new System.Exception("Exceeded the number icons -- internal error!");
            }

            if (current <= 0) goodIcon.rectTransform.sizeDelta = new Vector2(goodIcon.rectTransform.sizeDelta.x, 0);
            if (current > max) goodIcon.rectTransform.sizeDelta = new Vector2(goodIcon.rectTransform.sizeDelta.x, (float)topSize);

            var slope = topSize / max;
            goodIcon.rectTransform.sizeDelta = new Vector2(goodIcon.rectTransform.sizeDelta.x, Mathf.Clamp((float)(slope * current), 0, (float)topSize));

            i++;
        }

    }
}
