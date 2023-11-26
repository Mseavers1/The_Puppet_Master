using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IComparable<GameObject>
{ 
    public int CompareTo(GameObject other)
    {
        var Stat = StaticHolder.PlayerStats;
        Stats otherStat;
        if (other.GetComponent<EnemyInfo>() != null) otherStat = other.GetComponent<PlayableStats>().Stat;
        else otherStat = other.GetComponent<EnemyInfo>().Stat;

        if (Stat.Agility > otherStat.Agility) return 1;
        if (Stat.Agility <= otherStat.Agility) return 0;

        throw new Exception("Unable to determine Order...");
    }
}
