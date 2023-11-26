using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayStatTop : MonoBehaviour
{
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void UpdateText(string text)
    {
        this.text.text = text;
    }
}
