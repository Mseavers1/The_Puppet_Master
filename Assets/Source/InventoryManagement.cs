using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryManagement
{
    private Dictionary<int, Item> inventory = new ();
    private const int Total_Slots = 66, Min_Slots = 3;
    private int currentMaxSlots = 0;

    private Dictionary<string, Item> equippedItems = new ();

    public InventoryManagement()
    {
        equippedItems.Add("Helmet", null);
        equippedItems.Add("Body", null);
        equippedItems.Add("Boots", null);
        equippedItems.Add("Sword", null);
        equippedItems.Add("Spear", null);
        equippedItems.Add("Bow", null);
    }

    public void PrintAllEquipment()
    {
        string x = "Equipment: ";
        foreach (var item in equippedItems.Values)
        {
            if (item != null) { x += item.Name + " "; }
        }

        Debug.Log(x);
    }

    public void EquipeItem(int slotID)
    {
        var item = inventory[slotID];
        if (item.ItemType != "Weapon" && item.ItemType != "Armor") throw new Exception("An error occured... " + item.ItemType + " are not equipable!");

        if (item.IsEquipped())
        {
            UnEquip(slotID);
            return;
        }

        if (equippedItems[item.SubType] != null)
        {
            var otherItem = equippedItems[item.SubType];
            otherItem.UnEquip();
        }

        item.Equip();
        equippedItems[item.SubType] = item;
    }

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

    public bool CheckEquipment(string type)
    {
        return equippedItems[type] != null;
    }

    public Item GetEquipment(string type)
    {
        return equippedItems[type];
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

    public void UseItem(int slot)
    {
        // Use item
        Debug.Log("You used the item -> " + inventory[slot].Name);

        RemoveItem(slot);
    }

    private void UnEquip(int slotID)
    {
        var item = inventory[slotID];
        item.UnEquip();
        equippedItems[item.SubType] = null;
    }

    private Item FindConsumable(string name)
    {
        TextAsset txt = (TextAsset)Resources.Load("Consumables", typeof(TextAsset));
        List<ConsumableItem> items = JsonConvert.DeserializeObject<List<ConsumableItem>>(txt.text) ?? throw new Exception("Empty Json!");

        foreach (ConsumableItem item in items)
        {
            if (item.Name == name)
                return new Consumable(name, item.Weight, item.Description);
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
                return new Weapon(name, item.Weight, item.Description, item.SubType, item.PotentialDamage, item.SkillLevel);
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
                return new Armor(name, item.Weight, item.Description, item.SubType);
        }

        throw new Exception("Unable to find the armor item with the name of " + name);
    }
}

internal class ConsumableItem
{
    public int ID;
    public string Name;
    public string Description;
    public double Weight;
    public double HealthRecovery, StaminaRecovery, ManaRecovery;
}

internal class WeaponItem
{
    public int ID;
    public string Name;
    public double Weight;
    public string Description;
    public double PotentialDamage;
    public int SkillLevel;
    public string SkillName;
    public string SubType;
}

internal class ArmorItem
{
    public int ID;
    public string Name;
    public double Weight;
    public string Description;
    public string[] Resistances;
    public double[] ResistancesValues;
    public string SubType;
}
