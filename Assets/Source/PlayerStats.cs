using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IBattleable
{
    public Stats playerStats;
    public DisplayStatTop healthDisplay;

    public void TakeDamage(float damage)
    {
        playerStats.CurrentHealth -= damage;
        healthDisplay.UpdateText(playerStats.CurrentHealth + " / " + playerStats.MaxHealth + " HP");

        // Death check
        if (playerStats.CurrentHealth <= 0)
            print("Player Died");

    }

    private void Start()
    {
        playerStats = StaticHolder.PlayerStats;
    }
}
