using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStatTop : MonoBehaviour
{
    public Image goodIcon;

    public void UpdateText(string text)
    {
        GetComponent<TMP_Text>().text = text;

        GameObject.FindGameObjectWithTag("GameManager").GetComponent<Gamemanager_World>().UpdateIcon();
    }
}
