using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualNovel_Settings : MonoBehaviour
{
    public Slider textSpeed, autoSpeed;

    public void UpdateTextSpeed()
    {
        PlayerPrefs.SetFloat("TextSpeed", textSpeed.value);
    }
    
    public void UpdateAutoSpeed()
    {
        PlayerPrefs.SetFloat("AutoSpeed", autoSpeed.value);
    }
}
