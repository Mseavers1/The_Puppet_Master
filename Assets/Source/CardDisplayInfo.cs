using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDisplayInfo : MonoBehaviour
{
    private Card card;
    private char type;

    public void SetCardType(char type) 
    {
        this.type = type; 
    }
    public char GetCardType() { return type; }

    public void SetDesc(Card card)
    {
        transform.GetChild(2).GetComponent<TMP_Text>().text = card.GetDesc();
        this.card = card;
    }

    public Card GetCard() { return card; }
}
