using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<Card> cards;
    private Stack<Card> dispile;

    public Deck(bool isPlayer)
    {
        cards = new List<Card>();
        dispile = new Stack<Card>();
        
        if(isPlayer)
            AddCards(HoldingOfSkills.CreateCards("Hands", 1));
    }

    public void AddCards(List<Card> c)
    {
        foreach (var card in c) cards.Add(card);
    }

    // Generate 5 cards with the parm of W, D, S, S, R
    public Card[] GenerateHand()
    {
        var hand = new Card[5];

        hand[0] = PullCard('W');
        hand[1] = PullCard('D');
        hand[2] = PullCard('S');
        hand[3] = PullCard('S');
        hand[4] = PullCard('R');

        return hand;
    }

    public char GetTypeIndex(int index)
    {
        switch (index)
        {
            case 0:
                return 'W';
            case 1:
                return 'D';
            case 2:
                return 'S';
            case 3:
                return 'S';
            case 4:
                return 'R';
            default: throw new Exception("The index [" + index + "] does not exsist!");
        }
    }

    public bool IsCorrectType(char type)
    {
        if (type != 'W' && type != 'D' && type != 'S' && type != 'R') return false;

        return true;
    }

    public void RandomizeDeck()
    {
        var randDeck = new List<Card>();

        // Add dispile cards back into the card
        foreach(var card in dispile)
        {
            cards.Add(card);
        }

        while (cards.Count > 0)
        {
            randDeck.Add(GetRandomCard());
        }

        dispile.Clear();
        cards = randDeck;
    }

    private void DiscardCard(Card card)
    {
        cards.Remove(card);
        dispile.Push(card);
    }

    private Card GetRandomCard()
    {
        int rand = UnityEngine.Random.Range(0, cards.Count);
        Card card = cards[rand];
        DiscardCard(card);
        return card;
    }

    public Card PullCard(char type)
    {
        // Checks if deck needs a reshuffle
        if (cards.Count == 0)
        {
            while (dispile.Count > 0)
            {
                var card = dispile.Pop();
                cards.Add(card);
            }

            UnityEngine.Debug.Log("Shuffling!");
            RandomizeDeck();
        }

        if (type == 'R') return GetRandomCard();

        // Keeps going through the deck until it finds the same type of card
        foreach(var card in cards)
        {
            if (card.IsTypeOf(type))
            {
                DiscardCard(card);
                return card;
            }
        }

        // If type is not in the deck, select the next card
        var nextCard = cards[0];
        DiscardCard(nextCard);
        return nextCard;
    }

    public override string ToString()
    {
        string str = "[" + cards.Count + "] - ";

        foreach(Card card in cards)
        {
            str += card.GetName() + " ";
        }

        return str;
    }
}
