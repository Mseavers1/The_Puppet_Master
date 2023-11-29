using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotContainer : MonoBehaviour
{
    public Item CurrentItem { get; set; }
    public int SlotIndex { get; set; }

    public void Use()
    {
        print("Test");
        if (CurrentItem != null) GameObject.FindGameObjectWithTag("GameManager").GetComponent<Gamemanager_World>().UseItem(SlotIndex);
    }

}
