using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotContainer : MonoBehaviour
{
    public Item CurrentItem { get; set; }
    public int SlotIndex { get; set; }

    public void Use()
    {
        if (CurrentItem == null) return;

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<Gamemanager_World>().UseItem(SlotIndex, CurrentItem.CanEquip);

        foreach (var obj in GameObject.FindGameObjectsWithTag("InventorySlot"))
        {
            try 
            {
                if (obj.GetComponent<SlotContainer>().CurrentItem == null) continue;

                obj.transform.GetChild(4).gameObject.SetActive(obj.GetComponent<SlotContainer>().CurrentItem.IsEquipped());
                var objItem = obj.GetComponent<SlotContainer>().CurrentItem;

                if (objItem.IsEquipped()) obj.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Unequip";
                else obj.transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Equip";
            }
            catch { }
        }

        if (CurrentItem.CanUse) transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Use";
        else if (CurrentItem.IsEquipped()) transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Unequip";
        else transform.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Equip";
    }

    public void Disgard()
    {
        if (CurrentItem != null) GameObject.FindGameObjectWithTag("GameManager").GetComponent<Gamemanager_World>().DisgardItem(SlotIndex);
    }

}
