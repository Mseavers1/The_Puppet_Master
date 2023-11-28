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
        healthDisplay.UpdateText(playerStats.CurrentHealth + " / " + playerStats.GetStatValue("Health") + " HP");

    }

    public bool IsDead() { return playerStats.CurrentHealth <= 0; }

    private void Start()
    {
        playerStats = StaticHolder.PlayerStats;
    }

    public string ChangeMode()
    {
        return "Battle Player";
    }

    public void PlayTurn() { } // Only for non player users
}
