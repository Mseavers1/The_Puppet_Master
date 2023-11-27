using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatUI_Updater : MonoBehaviour
{
    public TMP_Text mainDisplay, removeCost, buyingCost;
    public GameObject negitive;

    private int currentPoint = 0;

    private void Start()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        mainDisplay.text = currentPoint.ToString();
        buyingCost.text = "-" + CalcCost() + " SP";

        if (currentPoint > 0)
        {
            removeCost.text = "+" + CalcGain() + " SP";

            if (!removeCost.gameObject.activeSelf)
            {
                removeCost.gameObject.SetActive(true);
                negitive.SetActive(true);
            }
        }
        else
        {
            removeCost.gameObject.SetActive(false);
            negitive.SetActive(false);
        }
    }

    public int BuyPoint(int SP)
    {
        var cost = CalcCost();
        if (SP >= cost)
        {
            currentPoint++;
            UpdateDisplay();

            return cost;
        }

        return 0;
    }

    public int RemovePoint()
    {
        if (currentPoint <= 0) return 0;

        var cost = CalcGain();
        currentPoint--;

        UpdateDisplay();
        return cost;
    }

    private int CalcCost()
    {
        return (int) Mathf.Pow(currentPoint + 1, 2);
    }

    private int CalcGain()
    {
        var gain = Mathf.RoundToInt((int)Mathf.Pow(currentPoint, 2) / 2f);

        if (gain > 0) return gain;

        return 1;
    }
}
