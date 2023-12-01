using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Gamemanager_World : MonoBehaviour
{
    public GameObject itemPrefab;
    public Dictionary<int, GameObject> ItemIcons = new();
    public Sprite[] allItemSprites;
    public TMP_Text inventoryTextClose, inventoryTextOpen;

    public DisplayStatTop[] displays;
    public GameObject InventoryPanel;
    public Button endOfTurn;
    public GameObject bar;
    public Image[] icons;
    private double[] topSizes;

    [Range(0.4f, 1.3f)]
    public float scale = 0.8f;
    public string Mode = "None";

    private int currentI = 0, currentJ = 0, totalSlots = 0;

    private void Start()
    {
        topSizes = new double[icons.Length];
        for (int i = 0; i < icons.Length; i++) topSizes[i] = icons[i].rectTransform.sizeDelta.y;


        if (bar.gameObject.activeSelf) UpdateIconsText();

        // Create empty spots based on physical strength
        ExpandInventoryDisplay();

        //StaticHolder.InventoryManagement.AddItem("Bambo Sword", "Weapon", GetNextAvailableSlot());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            StaticHolder.InventoryManagement.AddItem("Bambo Sword", "Weapon", GetNextAvailableSlot());
            StaticHolder.InventoryManagement.AddItem("Iron Sword", "Weapon", GetNextAvailableSlot());
            StaticHolder.InventoryManagement.AddItem("Ninja Sword", "Weapon", GetNextAvailableSlot());
        }
    }

    public void ClickInventoryButton()
    {
        if (InventoryPanel.gameObject.activeSelf) CloseInventory();
        else OpenInventory();

    }

    public void SpawnItem(Item item)
    {
        var freeSlot = GetNextAvailableSlot();
        var slot = ItemIcons[freeSlot].GetComponent<SlotContainer>();
        slot.transform.GetChild(2).GetChild(0).GetComponent<TMP_Text>().text = item.Name;
        slot.transform.GetChild(2).GetChild(1).GetComponent<TMP_Text>().text = item.Description;

        slot.CurrentItem = item;
        slot.transform.GetChild(1).GetComponent<Image>().sprite = FindItemPicture(item.Name);
        slot.transform.GetChild(1).gameObject.SetActive(true);
        slot.name = item.Name;
    }

    public void UpdateIconsText()
    {
        var stats = StaticHolder.PlayerStats;
        displays[0].UpdateText(stats.CurrentHealth + " / " + stats.GetStatValue("Health") + " HP"); // Health
        displays[1].UpdateText(stats.CurrentMana + " / " + stats.GetStatValue("Mana") + " MP"); // Mana
        displays[2].UpdateText(stats.CurrentStamina + " / " + stats.GetStatValue("Stamina") + " SP"); // Stamina
    }

    public void UseItem(int index, bool isEquipment)
    {
        var iv = StaticHolder.InventoryManagement;
        if (isEquipment)
        {
            // Either equips or unequips the item
            iv.EquipeItem(index);
            iv.PrintAllEquipment();
            return;
        }


        iv.UseItem(index);
        ClearSlot(index);
    }

    public void EndTurn()
    {
        var battle = GetComponent<BattleSimulator>();

        // Button should not be active but just in case
        if (!battle.IsPlayerTurn()) return;

        endOfTurn.interactable = false;
        battle.NextTurn();
    }

    public void TurnButtonOn()
    {
        endOfTurn.interactable = true;
        StaticHolder.PlayerStats.StartOfTurn();

        var playerStats = StaticHolder.PlayerStats;

        displays[1].UpdateText(playerStats.CurrentMana + " / " + playerStats.GetStatValue("Mana") + " MP");
        displays[2].UpdateText(playerStats.CurrentStamina + " / " + playerStats.GetStatValue("Stamina") + " SP");
    }

    public void UpdateIcon()
    {
        int i = 0;
        foreach (var icon in icons)
        {
            double current = 0, max = 0;
            var goodIcon = icons[i];
            var topSize = topSizes[i];
            var stats = StaticHolder.PlayerStats;

            switch (i)
            {
                case 0:
                    current = stats.CurrentHealth;
                    max = stats.GetStatValue("Health");
                    break;
                case 1:
                    current = stats.CurrentMana;
                    max = stats.GetStatValue("Mana");
                    break;
                case 2:
                    current = stats.CurrentStamina;
                    max = stats.GetStatValue("Stamina");
                    break;
                default: throw new System.Exception("Exceeded the number icons -- internal error!");
            }

            if (current <= 0) goodIcon.rectTransform.sizeDelta = new Vector2(goodIcon.rectTransform.sizeDelta.x, 0);
            if (current > max) goodIcon.rectTransform.sizeDelta = new Vector2(goodIcon.rectTransform.sizeDelta.x, (float)topSize);

            var slope = topSize / max;
            goodIcon.rectTransform.sizeDelta = new Vector2(goodIcon.rectTransform.sizeDelta.x, Mathf.Clamp((float)(slope * current), 0, (float)topSize));

            i++;
        }

    }

    public void DisgardItem(int index)
    {
        StaticHolder.InventoryManagement.RemoveItem(index);
        ClearSlot(index);
    }

    private void OpenInventory()
    {
        inventoryTextClose.text = "Close Inventory";
        inventoryTextOpen.text = "Close Inventory";
        InventoryPanel.SetActive(true);
    }

    private void CloseInventory()
    {
        inventoryTextClose.text = "Open Inventory";
        inventoryTextOpen.text = "Open Inventory";
        InventoryPanel.SetActive(false);

        GetComponent<BattleSimulator>().UpdateHandDisplay();
    }

    private void ClearSlot(int index)
    {
        ItemIcons[index].GetComponent<SlotContainer>().CurrentItem = null;
        ItemIcons[index].GetComponent<SlotContainer>().transform.GetChild(1).gameObject.SetActive(false);

        foreach (var ic in ItemIcons.Values)
        {
            ic.transform.GetChild(3).gameObject.SetActive(false);
            ic.transform.GetChild(2).gameObject.SetActive(false);
        }

        GameObject.FindGameObjectWithTag("Player").GetComponent<Mouse_Handler>().SwitchHoverSlots(-1);
    }

    private int GetNextAvailableSlot()
    {
        foreach (var icon in ItemIcons)
        {
            var slot = icon.Value.GetComponent<SlotContainer>();
            if (slot.CurrentItem != null) continue;

            
            return icon.Key;
        }

        return -1;
    }

    private Sprite FindItemPicture(string name)
    {
        foreach (var sprite in allItemSprites)
        {
            //print(sprite.name);
            if (sprite.name == name) return sprite;
        }

        throw new System.Exception("Unable to location the item picture for " + name);
    }

    private void ExpandInventoryDisplay()
    {
        var iv = StaticHolder.InventoryManagement;
        var newSlots = iv.ExpandSlots();

        // Spawn new slots
        for (int i = 0; i < newSlots;  i++)
        {
            var l = Instantiate(itemPrefab, Vector2.zero, Quaternion.identity);
            l.name = "Empty Slot";
            l.transform.SetParent(InventoryPanel.transform);
            l.GetComponent<RectTransform>().anchoredPosition = StaticHolder.InventoryManagement.FindSpawningPosition(currentI, currentJ);
            l.GetComponent<SlotContainer>().SlotIndex = totalSlots;

            ItemIcons.Add(totalSlots, l);
            totalSlots++;

            var nextSpot = (currentI + 1) % 11;
            currentI = nextSpot;

            if (nextSpot == 0) currentJ++;
        }
    }
}
