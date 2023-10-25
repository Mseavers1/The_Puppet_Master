using System.Collections;
using System.Collections.Generic;

public class Deck
{
    private List<Card> cards;

    public Deck()
    {
        cards = new List<Card>();
    }

    public void AddCard(Card card)
    {
        cards.Add(card);
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
