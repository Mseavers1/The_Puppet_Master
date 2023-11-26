using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Stats playerStats;

    private void Start()
    {
        playerStats = StaticHolder.PlayerStats;
    }
}
