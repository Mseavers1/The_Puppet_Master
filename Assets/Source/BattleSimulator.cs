using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class BattleSimulator : MonoBehaviour
{
    public GameObject DeckArea;
    public GameObject[] Cards;

    private GameObject[] playables, enemies;
    private Queue<GameObject> order = new ();
    private const float Smooth_Factor = 3f;
    private bool movePlayables = false;
    private Deck deck;
    private Card[] hand;
    private Gamemanager_World gm;

    private void Awake()
    {
        gm = gameObject.GetComponent<Gamemanager_World>();
    }

    // Get the nessessary info needed for the battle
    public void BattleSetup(GameObject[] playables, GameObject[] enemies)
    {
        // Get playables and enemies
        this.playables = playables;
        this.enemies = enemies;

        // Restrict Player Movement
        playables[0].GetComponent<Player_Movement>().EnableMovement(false);

        // Move playables to their spot TODO - Finish
        movePlayables = true;

        // Generate Cards
        deck = HoldingOfSkills.GenerateDeck();
        Debug.Log(deck);

        // Generate Hand
        hand = deck.GenerateHand();
        foreach(var card in hand) Debug.Log(card.GetName());

        // Display Hand
        DeckArea.SetActive(true);
        for (int i = 0; i < hand.Length; i++)
        {
            Cards[i].GetComponent<CardDisplayInfo>().SetDesc(hand[i]);
            Cards[i].GetComponent<CardDisplayInfo>().SetCardType(deck.GetTypeIndex(i));
        }

        // Battle Order
        GenerateBattleOrder();

    }

    public GameObject GetRandomPlayable()
    {
        int rand = UnityEngine.Random.Range(0, playables.Length);

        return playables[rand];
    }

    public void NextTurn()
    {
        var last = order.Dequeue();
        order.Enqueue(last);
        // TODO - Skip if person is dead

        if (IsPlayerTurn()) gm.Mode = "Battle Player";
        else if (order.Peek().CompareTag("Enemy")) 
        {
            gm.Mode = "Battle Mob";
            order.Peek().GetComponent<EnemyInfo>().PlayTurn();
        }
        else throw new Exception("Allies Turn - Not Implemented yet.");
    }

    public Card DrawCard(char type)
    {
        if (!deck.IsCorrectType(type)) throw new Exception(type + " is not a valid type! Has to be either W D S R");

        return deck.PullCard(type);
    }

    public bool IsPlayerTurn()
    {
        if (order.Peek().CompareTag("Player")) return true;

        return false;
    }

    private void GenerateBattleOrder()
    {
        var list = new List<GameObject>();
        
        // Add all playables in the order
        foreach (var playable in playables)
        {
            list.Add(playable);
        }

        // Add all the enemies in the order
        foreach (var enemy in enemies)
        {
            list.Add(enemy);
        }

        // Sort everyone based on agility
        SortOrder(list);
    }

    private void SortOrder(List<GameObject> list)
    {
        var newOrder = new GameObject[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            if ((i + 1) > list.Count && Compare(GetAgility(list[i]), GetAgility(list[i + 1])) > 0)
            {
                Swap(i, i + 1, list);
            }

            if (i == 0) 
                newOrder[i] = list[i]; 
            else
            {
                newOrder[i] = list[i];
                for (int j = i - 1; j >= 0; j--)
                {
                    if (Compare(GetAgility(list[i]), GetAgility(list[j])) < 0)
                    {
                        Swap(i, j, newOrder);
                        continue;
                    }

                    break;
                }
            }

        }

        order.Clear();
        foreach (var item in newOrder)
        {
            order.Enqueue(item);
        }
    }

    private int Compare(float x, float y)
    {
        if (x >= y) return -1;
        if (x < y) return 1;

        throw new Exception("Something weird happening when comparing... Shouldnt be here...");
    }

    private void Swap(int l, int r, List<GameObject> arr)
    {
        var element = arr[l];
        arr[l] = arr[r];
        arr[r] = element;
    }

    private void Swap(int l, int r, GameObject[] arr)
    {
        var element = arr[l];
        arr[l] = arr[r];
        arr[r] = element;
    }

    private float GetAgility(GameObject o)
    {
        if (o.GetComponent<EnemyInfo>() != null) return o.GetComponent<EnemyInfo>().Stat.Agility;
        
        if (o.GetComponent<PlayableStats>() != null) return o.GetComponent<PlayableStats>().Stat.Agility;

        if (o.GetComponent<PlayerStats>()) return o.GetComponent<PlayerStats>().playerStats.Agility;

        throw new Exception("Object does not have any stats");
    }

    // Move players and enemies to their location
    private void MoveActors()
    {
        // TODO: Enemy location and spawning
        // TODO: Other players location and spawning

        // Move the main character
        var player = playables[0];
        var playerPoint = enemies[0].transform.GetChild(0);
        player.transform.position = Vector3.Lerp(player.transform.position, playerPoint.transform.position, Smooth_Factor * Time.fixedDeltaTime);

        if (Vector3.Distance(player.transform.position, playerPoint.transform.position) <= .3f) movePlayables = false;
    }

    private void FixedUpdate()
    {
        if (movePlayables)
        {
            MoveActors();
        }
    }
}
