using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableStats : MonoBehaviour, IBattleable
{
    public Stats Stat { get; private set; }

    public void TakeDamage(float damage)
    {
        Stat.CurrentHealth -= damage;
        //healthDisplay.UpdateText(Stat.CurrentHealth + " / " + Stat.MaxHealth + " HP");
    }

    public string ChangeMode()
    {
        return "Battle Ally";
    }

    public bool IsDead() { return Stat.CurrentHealth <= 0;}

    public void PlayTurn()
    {
        throw new NotImplementedException();
    }
}
