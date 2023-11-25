using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardDisplayInfo : MonoBehaviour
{
    public void SetDesc(string desc)
    {
        transform.GetChild(2).GetComponent<TMP_Text>().text = desc;
    }
}
