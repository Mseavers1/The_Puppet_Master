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
    

    public Card GetCard() { return card; }

    public float CalculateCardDamage(Card card)
    {
        if (card.GetRequiredWeapon() == "None") return card.GetDamage();

        var iv = StaticHolder.InventoryManagement;

        if (!iv.CheckEquipment(card.GetRequiredWeapon())) return 0;

        var weapon = (Weapon)iv.GetEquipment(card.GetRequiredWeapon());

        if (card.GetLevel() >= weapon.SkillLevel) return (float)weapon.MaxDamage;

        var slope = weapon.MaxDamage / weapon.SkillLevel;

        return (float)Math.Round(slope * card.GetLevel(), 1);
    }
    
    public float CalculateHealing(Card card)
    {
        /*
        float healAmount = 0;
        foreach (var x in card.GetSpellAttributes())
        {
            if (x.Split(' ')[0] != "HEAL") continue;

            //healAmount = float.Parse(x.Split(' ')[1]);
            break;
        }

        return healAmount * 100; */
        return 0;
    }
}
