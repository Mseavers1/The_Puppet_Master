using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamemanager_World : MonoBehaviour
{
    public DisplayStatTop[] displays;
    public Button endOfTurn;

    [Range(0.4f, 1.3f)]
    public float scale = 0.8f;
    public string Mode = "None";

    private void Start()
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
}
