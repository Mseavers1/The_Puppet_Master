using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BattleSimulator : MonoBehaviour
{
    public GameObject DeckArea;
    public GameObject[] Cards;

    private GameObject[] playables, enemies;
    private List<GameObject> order;
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
            Cards[i].GetComponent<CardDisplayInfo>().SetDesc(hand[i].GetDesc());
        }

        // Battle Order
        GenerateBattleOrder();

        foreach(var x in order)
        {
            print(x.name);
        }
    }

    private void GenerateBattleOrder()
    {
        order = new List<GameObject>();
        
        // Add all playables in the order
        foreach (var playable in playables)
        {
            order.Add(playable);
        }

        // Add all the enemies in the order
        foreach (var enemy in enemies)
        {
            order.Add(enemy);
        }

        // Sort everyone based on agility
        foreach (var person in order)
        {
            order.Sort();
        }
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
