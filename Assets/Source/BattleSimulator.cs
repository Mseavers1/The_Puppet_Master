using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Source.Game;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class BattleSimulator
{
    public GameObject DeckArea;
    public GameObject[] Cards;

    private EnemyInfo _enemy;
    private GameGm _gm;
    private int _currentTurn;
    private int[] _order; // 0 - player // 1 - enemy
    private bool _isBattle = false;

    public BattleSimulator(GameGm gm, int mobID, int enemyLevel)
    {
        _gm = gm;
        _enemy = new EnemyInfo(mobID, gm, enemyLevel);

        // Battle Order
        GenerateBattleOrder();
        _isBattle = true;
    }

    public bool IsPlayerTurn()
    {
        return _order[Mathf.Abs(_currentTurn - 1) % 2] == 0;
    }

    public EnemyInfo GetCurrentEnemy()
    {
        return _enemy;
    }

    public void NextTurn()
    {
        _currentTurn++;
        // Player Dead
        if (StaticHolder.HasDied)
        {
            Debug.Log("Player Died");
            _isBattle = false;
            var length = _gm.PlayLoseSound();

            _gm.curtainCommands.CloseCurtains(1.5f);
            _gm.StartCoroutine(_gm.Lose(length));
            //_gm.InventoryPanel.transform.parent.GetChild(7).gameObject.SetActive(true);
            //_gm.InventoryPanel.transform.parent.GetChild(7).GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "You gained <color=red>" + _gm.NextSP + "</color> SP!";
            //_gm.Mode = "Dead";
            //_gm.TurnOnTutorial();
            //GameObject.FindGameObjectWithTag("Player").GetComponent<Player_Movement>().EnableMovement(false);
            return;
        }

        // Enemy Dead
        if (_enemy.IsDead())
        {
            Debug.Log("Enemy Died");
            _isBattle = false;
            var length = _gm.PlayWinSound();
            
            // Calculate Earned SP
            _gm.currentGameSP += _enemy.holdingSP;
            
            _gm.curtainCommands.CloseCurtains(1.5f);
            _gm.StartCoroutine(_gm.ReturnToSelector(length));
            
            return;
        }
        
        // Find who turn it is
        var turn = _order[Mathf.Abs(_currentTurn - 1) % 2];

        // Enemy Turn
        if (turn == 1)
        {
            _gm.StartCoroutine(_gm.DoEnemyTurn(_enemy, 1f));
            _gm.HideEndTurnSign();
        }
        // Player Turn
        else
        {
            StaticHolder.PlayerStats.StartOfTurn();
            _gm.ShowEndTurnSign();
            _gm.UpdateManaIcon();
            _gm.UpdateStaminaIcon();
            // Shuffle Cards?
        }
        
        // Play Turn Sound Effect
        _gm.PlayTurnSound();
    }

    private void GenerateBattleOrder()
    {
        // Sort everyone based on agility
        _order = StaticHolder.PlayerStats.GetStatValue("Agility") >= _enemy.Stat.GetStatValue("Agility") ? new[] {0, 1} : new[] {1, 0};
    }
    
}
