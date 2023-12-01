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
        if (!card.IsNoCombat())
            card.SetDesc("Deal <color=yellow>" + CalculateCardDamage(card) + "</color> " + card.GetDamageTypes()[0] + " damage.");
        else
        {
            var description = card.SpecialType;

            switch (description)
            {
                case "Heals":
                    card.SetDesc("Recover " + CalculateHealing(card) + "% of HP.");
                    break;
            }
        }

        transform.GetChild(0).GetComponent<TMP_Text>().text = card.GetName();
        transform.GetChild(1).GetComponent<TMP_Text>().text = "Lv. " + card.GetLevel();
        transform.GetChild(2).GetComponent<TMP_Text>().text = card.GetDesc();
        transform.GetChild(3).GetComponent<TMP_Text>().text = card.GetStaminaCost() + " SP";
        transform.GetChild(4).GetComponent<TMP_Text>().text = card.GetManaCost() + " MP";
        transform.GetChild(5).GetComponent<TMP_Text>().text = card.GetTypeName().ToString();
        this.card = card;
    }

    public Card GetCard() { return card; }

    public float CalculateCardDamage(Card card)
    {
        var iv = StaticHolder.InventoryManagement;

        if (!iv.CheckEquipment(card.GetRequiredWeapon())) return 0;

        var weapon = (Weapon)iv.GetEquipment(card.GetRequiredWeapon());

        if (card.GetLevel() >= weapon.SkillLevel) return (float)weapon.MaxDamage;

        var slope = weapon.MaxDamage / weapon.SkillLevel;

        return (float)Math.Round(slope * card.GetLevel(), 1);
    }
    
    public float CalculateHealing(Card card)
    {
        float healAmount = 0;
        foreach (var x in card.GetSpellAttributes())
        {
            if (x.Split(' ')[0] != "HEAL") continue;

            healAmount = float.Parse(x.Split(' ')[1]);
            break;
        }

        return healAmount * 100;
    }
}
