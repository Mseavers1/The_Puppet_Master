using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager_World : MonoBehaviour
{
    public DisplayStatTop[] displays;

    [Range(0.4f, 1.3f)]
    public float scale = 0.8f;
    public string Mode = "None";

    private void Start()
    {
        var stats = StaticHolder.PlayerStats;
        displays[0].UpdateText(stats.CurrentHealth + " / " + stats.MaxHealth + " HP"); // Health
        displays[1].UpdateText(stats.CurrentMana + " / " + stats.MaxMana + " MP"); // Mana
        displays[2].UpdateText(stats.CurrentStamina + " / " + stats.MaxStamina + " S"); // Stamina
    }
}
