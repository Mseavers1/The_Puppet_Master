using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    private BattleSimulator battle;

    private void Awake()
    {
        battle = GameObject.FindGameObjectWithTag("GameManager").GetComponent<BattleSimulator>();
    }

    public void StartBattle(GameObject[] playables)
    {
        GameObject[] x = {gameObject}; // TEMP
        battle.BattleSetup(playables, x);
    }
}
