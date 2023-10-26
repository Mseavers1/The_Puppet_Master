using System;
using System.Collections;
using System.Collections.Generic;

public class Deck
{
    private List<Card> cards;
    private Stack<Card> dispile;

    public Deck()
    {
        cards = new List<Card>();
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
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

    private Card PullCard(char type)
    {
        if (type == 'R') return GetRandomCard();

        foreach(var card in cards)
        {
            if (card.IsTypeOf(type))
            {
                DiscardCard(card);
                return card;
            }
        }

        // TODO - Reshuffle discard pile
        throw new Exception("Not Implemented yet...");
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
