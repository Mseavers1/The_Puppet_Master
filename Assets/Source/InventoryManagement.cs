using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManagement
{
    private Dictionary<int, Item> inventory = new ();
    private const int Total_Slots = 66, Min_Slots = 3;
    private int currentMaxSlots = 0;


    public int ExpandSlots()
    {
        var stats = StaticHolder.PlayerStats;
        var slope = (Total_Slots - Min_Slots) / (StaticHolder.maxValues["PStrength"] - StaticHolder.defaultStatValues["PStrength"]);
        var totalSlots = Mathf.RoundToInt((float) (slope * (stats.GetStatValue("PStrength") - StaticHolder.defaultStatValues["PStrength"]) + Min_Slots));
        var sub = totalSlots - currentMaxSlots;
        currentMaxSlots += sub;

        return sub;
    }

    public int GetNumberMaxSlots () { return currentMaxSlots; }

    public bool IsSlotAvailable()
    {
        return inventory.Count < currentMaxSlots;
    }

    public void AddItem(string name, string type, int slot)
    {
        if (!IsSlotAvailable()) 
        {
            Debug.Log("No Slots Available!");
            return;
        }; // TODO - show there is no room

        switch (type)
        {
            case "Consumable":
                inventory.Add(slot, FindConsumable(name));
                break;
            case "Weapon":
                inventory.Add(slot, FindWeapon(name));
                break;
            case "Armor":
                inventory.Add(slot, FindArmor(name));
                break;
            default: throw new Exception(type + " is not a registered type! Maybe " + name + " is under a different type?");
        }

        var gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Gamemanager_World>();
        gm.SpawnItem(inventory[slot]);
    }

    public Vector2 FindSpawningPosition(int i, int j)
    {
        Vector2 pos = new (-503 + 100 * i, 229 - 100 * j);
        return pos;
    }

    public void RemoveItem(int slot)
    {
        inventory.Remove(slot);
    }

    public void UseItem(int splot)
    {
        // Use item


        RemoveItem(splot);
    }

    private Item FindConsumable(string name)
    {
        TextAsset txt = (TextAsset)Resources.Load("Consumables", typeof(TextAsset));
        List<ConsumableItem> items = JsonConvert.DeserializeObject<List<ConsumableItem>>(txt.text) ?? throw new Exception("Empty Json!");

        foreach (ConsumableItem item in items)
        {
            if (item.Name == name)
                return new Consumable(name, item.Weight);
        }

        throw new Exception("Unable to find the consumable item with the name of " + name);
    }

    private Item FindWeapon(string name)
    {
        TextAsset txt = (TextAsset)Resources.Load("Weapons", typeof(TextAsset));
        List<WeaponItem> items = JsonConvert.DeserializeObject<List<WeaponItem>>(txt.text) ?? throw new Exception("Empty Json!");

        foreach (WeaponItem item in items)
        {
            if (item.Name == name)
                return new Weapon(name, item.Weight);
        }

        throw new Exception("Unable to find the weapon item with the name of " + name);
    }

    private Item FindArmor(string name)
    {
        TextAsset txt = (TextAsset)Resources.Load("Armors", typeof(TextAsset));
        List<ArmorItem> items = JsonConvert.DeserializeObject<List<ArmorItem>>(txt.text) ?? throw new Exception("Empty Json!");

        foreach (ArmorItem item in items)
        {
            if (item.Name == name)
                return new Armor(name, item.Weight);
        }

        throw new Exception("Unable to find the armor item with the name of " + name);
    }
}

internal class ConsumableItem
{
    public int ID;
    public string Name;
    public double Weight;
    public double HealthRecovery, StaminaRecovery, ManaRecovery;
}

internal class WeaponItem
{
    public int ID;
    public string Name;
    public double Weight;
    public double PotentialDamage;
    public int SkillLevel;
    public string SkillName;
}

internal class ArmorItem
{
    public int ID;
    public string Name;
    public double Weight;
    public string[] Resistances;
    public double[] ResistancesValues;
}
