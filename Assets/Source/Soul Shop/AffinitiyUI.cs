using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AffinitiyUI : MonoBehaviour
{
    public AffinitiyUI[] Requirements;

    private bool hasBought = false;
    private Image image;
    private float defaultAplha;
    private Soul_GM gm;
    private int cost = 1;

    private void Awake()
    {
        image = GetComponent<Image>();
        defaultAplha = image.color.a;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Soul_GM>();
    }

    private void Start()
    {
        UpdateCostText(false);
    }

    protected bool HasBought() { return hasBought; }

    public void OnClicked()
    {
        // When buying the affinity
        if(hasBought == false)
        {
            if (gm.GetSP() < cost || gm.GetAffinityCount() <= 0 || !SatisfyRequirements()) return;

            gm.AddAffinity(cost);
            image.color = new Color(image.color.r, image.color.g, image.color.b, 100);
            hasBought = true;
            UpdateCostText(true);
            transform.GetChild(2).GetComponent<Image>().color = new Color(0, 0, 0, 100);
            return;
        }

        // Removing the affinity
        RemoveAffinity();
    }

    public void CheckRequirementsSatisfaction()
    {
        if (!hasBought || SatisfyRequirements()) return;

        RemoveAffinity();
    }

    private void UpdateCostText(bool hasBought)
    {
        if (hasBought)
        {
            transform.GetChild(0).GetComponent<TMP_Text>().text = "+" + cost.ToString() + " SP";
            return;
        }

        transform.GetChild(0).GetComponent<TMP_Text>().text = "-" + cost.ToString() + " SP";
    }

    private void RemoveAffinity()
    {
        gm.RemoveAffinity(cost);
        image.color = new Color(image.color.r, image.color.g, image.color.b, defaultAplha);
        hasBought = false;
        UpdateCostText(false);
        transform.GetChild(2).GetComponent<Image>().color = new Color(0, 0, 0, defaultAplha);
    }

    private bool SatisfyRequirements()
    {
        if (Requirements.Length == 0 || Requirements == null) return true;

        foreach (var requirement in Requirements)
        {
            if (!requirement.HasBought()) return false;
        }

        return true;
    }
}
