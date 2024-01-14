using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Source.Soul_Shop;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class SoulShop_Controls : MonoBehaviour
{
    public GameObject canvas;
    public Card_Display display;
    public StatTransformer statTransformer;

    private PlayerControls controls;
    private PlayerInput input;
    private GraphicRaycaster caster;

    private PointerEventData pointerData, hoveringData;
    private List<RaycastResult> pointerResults, hoveringResults;
    private GameObject selectedSkill;

    private GameObject[] affinityUI;
    private string nameSkill = "";

    private byte hoveringItem = 0; // 0 = None, 1 = Upgrade, 2 = Downgrade, 3 = Unlock
    private byte hoveringStat, hoveringStatLabel;
    private string selectedStatName;
    public TMP_Text[] statsIconTexts;
    private Vector3 defaultLocalScale;
    public GameObject[] statsIconLabels;
    public Color defaultStatHighlight, statHighlight;

    private void Awake()
    {
        controls = new PlayerControls();
        input = GetComponent<PlayerInput>();
        input.onActionTriggered += OnClick;
        affinityUI = GameObject.FindGameObjectsWithTag("AffinitiesIcon");
    }

    private void Start()
    {
        caster = canvas.GetComponent<GraphicRaycaster>();
        pointerData = new PointerEventData(EventSystem.current);
        pointerResults = new List<RaycastResult>();

        hoveringData = new PointerEventData(EventSystem.current);
        hoveringResults = new List<RaycastResult>();

        defaultLocalScale = statsIconLabels[0].transform.localScale;
    }

    private void FixedUpdate()
    {
        //if (GetComponent<Soul_GM>().IsTutorialOn()) return;

        // Hovering
        hoveringData.position = Mouse.current.position.ReadValue();
        hoveringResults.Clear();
        caster.Raycast(hoveringData, hoveringResults);
        bool hasFound = false, hasFoundStat = false, hasFoundStatGainOrMinus = false, hasFoundMenu = false;
        
        foreach (var result in hoveringResults)
        {
         
            // Soul Card
            if (result.gameObject.CompareTag("SoulCardUI"))
            {
                switch (result.gameObject.name)
                {
                    case "Upgrade":
                        hasFound = true;
                        if (hoveringItem != 1) SwitchHover(1);
                        break;
                    case "Downgrade":
                        hasFound = true;
                        if (hoveringItem != 2) SwitchHover(2);
                        break;
                    case "Unlock":
                        hasFound = true;
                        if (hoveringItem != 3) SwitchHover(3);
                        break;
                    default: throw new Exception("Found something that doesn't belong... Word " + result.gameObject.name + " is invalid!");
                }
            }
            
            if (!hasFound) if (hoveringItem != 0) SwitchHover(0);

            // Stats
            if (result.gameObject.CompareTag("Stat"))
            {
                switch (result.gameObject.name)
                {
                    case "Vitality":
                        hasFoundStat = true;
                        if (hoveringStat != 1) SwitchHoverStat(1);
                        break;
                    case "Intelligence":
                        hasFoundStat = true;
                        if (hoveringStat != 2) SwitchHoverStat(2);
                        break;
                    case "Endurance":
                        hasFoundStat = true;
                        if (hoveringStat != 3) SwitchHoverStat(3);
                        break;
                    case "Strength":
                        hasFoundStat = true;
                        if (hoveringStat != 4) SwitchHoverStat(4);
                        break;
                    case "Agility":
                        hasFoundStat = true;
                        if (hoveringStat != 5) SwitchHoverStat(5);
                        break;
                    case "Speed":
                        hasFoundStat = true;
                        if (hoveringStat != 6) SwitchHoverStat(6);
                        break;
                    case "Luck":
                        hasFoundStat = true;
                        if (hoveringStat != 7) SwitchHoverStat(7);
                        break;
                }
            }
            
            if (!hasFoundStat) if (hoveringStat != 0) SwitchHoverStat(0);
            
            // Plus & Minus on Stats
            if (result.gameObject.CompareTag("Stat"))
            {
                switch (result.gameObject.name)
                {
                    case "Plus":
                        hasFoundStatGainOrMinus = true;
                        if (hoveringStatLabel != 1) SwitchHoverStatLabel(1);
                        break;
                    case "Minus":
                        hasFoundStatGainOrMinus = true;
                        if (hoveringStatLabel != 2) SwitchHoverStatLabel(2);
                        break;
                }
            }
            
            if (!hasFoundStatGainOrMinus) if (hoveringStatLabel != 0) SwitchHoverStatLabel(0);
        }
        
    }

    private void SwitchHover(byte item)
    {
        hoveringItem = item;

        var skill = selectedSkill.name.Split(' ');
        var skillName = skill[0];
        var skillLevel = skill[1];

        switch (item)
        {
            case 1: // Upgrade
                display.DisplayCard(skillName, int.Parse(skillLevel) + 1, true);
                break;
            case 2: // Downgrade
                display.DisplayCard(skillName, int.Parse(skillLevel) - 1, true);
                break;
            case 3: // Unlock
                display.DisplayCard(skillName, int.Parse(skillLevel) + 1, true);
                break;
            default: // None
                display.DisplayCard(skillName, int.Parse(skillLevel), false);
                break;
        }
    }
    
    private void SwitchHoverStat(byte item)
    {
        hoveringStat = item;

        if (item is > 0 and <= 7) statsIconTexts[item - 1].color = statHighlight;
        else
        {
            foreach (var stat in statsIconTexts)
            {
                if (stat.text != selectedStatName)
                    stat.color = defaultStatHighlight;
            }
        }
    }
    
    private void SwitchHoverStatLabel(byte item)
    {
        hoveringStatLabel = item;

        if (item is > 0 and <= 2) statsIconLabels[item - 1].transform.localScale = defaultLocalScale * 1.25f;
        else foreach (var label in statsIconLabels) label.transform.localScale = defaultLocalScale;
    }
    
    private void OnClick(InputAction.CallbackContext context)
    {
        if (GetComponent<Soul_GM>().IsTutorialOn()) return;

        if (context.action.name.Equals("Left Click") && context.performed)
        {
            // Raycast for UI
            pointerData.position = Mouse.current.position.ReadValue();
            pointerResults.Clear();

            caster.Raycast(pointerData, pointerResults);

            // Cycle through the results
            foreach (var result in pointerResults)
            {
                // Stats
                if (result.gameObject.CompareTag("Stat"))
                {
                    var resultName = result.gameObject.name;
                    var gm = GetComponent<Soul_GM>();

                    switch (resultName)
                    {
                        case "Plus":
                            var cost = statTransformer.GetCurrentCost();
                            if (string.IsNullOrEmpty(selectedStatName) || gm.GetSP() < cost || SoulGmSettings.IsBuyableMaxedOut() || statTransformer.IsStatMaxed(selectedStatName)) return;
                            
                            SoulGmSettings.BuyStat();
                            gm.Buy(cost);
                            statTransformer.IncrementStat(selectedStatName);
                            break;
                        case "Minus":
                            var gain = statTransformer.GetCurrentGain();
                            if (string.IsNullOrEmpty(selectedStatName) || statTransformer.IsStatZero(selectedStatName)) return;
                            
                            SoulGmSettings.SellStat();
                            gm.Gain(gain);
                            statTransformer.ReduceStat(selectedStatName);
                            break;
                        default:
                            selectedStatName = resultName;
                            SwitchHoverStat(0);
                            break;
                    }

                    //statTransformer.IncrementStat(result.gameObject.name);
                }
                
                // Find the first (should be one) skill if available
                if (result.gameObject.CompareTag("SkillOption"))
                {
                    selectedSkill = result.gameObject;
                    var skill = result.gameObject.name.Split(' ');
                    var skillName = skill[0];
                    var skillLevel = skill[1];

                    nameSkill = skillName;

                    display.DisplayCard(skillName, int.Parse(skillLevel), false);
                    return;
                }


                // If clicked on the Upgrade/Buy/Downgrade
                if (result.gameObject.CompareTag("SoulCardUI"))
                {
                    int newLevel;
                    switch (result.gameObject.name)
                    {
                        case "Upgrade":
                            newLevel = display.ClickUpgrade();
                            break;
                        case "Downgrade":
                            newLevel = display.ClickDowngrade();
                            break;
                        case "Unlock":

                            // See if player has the affinity
                            print(nameSkill);
                            var requirements = HoldingOfSkills.LoadData(nameSkill).Requirements;

                            foreach (var requirement in requirements)
                            {
                                foreach (var affinity in affinityUI)
                                {
                                    if (affinity.name == requirement)
                                    {
                                        if (!affinity.GetComponent<AffinitiyUI>().HasBought()) return;
                                    }
                                }
                            }

                            newLevel = display.ClickUpgrade();
                            break;
                        default: throw new Exception("I don't know how you got here... But you are here since [" + result.gameObject.name + "] is not a button...");
                    }

                    UpdateCardText(newLevel);
                }

                // Affinities
                if (result.gameObject.CompareTag("AffinitiesIcon"))
                {
                    result.gameObject.GetComponent<AffinitiyUI>().OnClicked();

                    // Check if other affinities are still valid
                    foreach(var affinity in affinityUI)
                    {
                        affinity.GetComponent<AffinitiyUI>().CheckRequirementsSatisfaction();
                    }
                }

                // Stats - Buy
                if (result.gameObject.CompareTag("StatIconBUY"))
                {
                    var cost = result.gameObject.GetComponentInParent<StatUI_Updater>().BuyPoint(GetComponent<Soul_GM>().GetSP());

                    if (cost > 0) GetComponent<Soul_GM>().Buy(cost);
                }

                // Stats - Sell
                if (result.gameObject.CompareTag("StatIconSELL"))
                {
                    var gain = result.gameObject.GetComponentInParent<StatUI_Updater>().RemovePoint();

                    if (gain > 0) GetComponent<Soul_GM>().Gain(gain);
                }
            }

        }
    }

    private void UpdateCardText(int level)
    {
        var names = selectedSkill.name.Split(' ');
        selectedSkill.GetComponentInChildren<TMP_Text>().text = names[0] + " <color=red>Lv. " + level;
        selectedSkill.name = names[0] + " " + level;
    }


}
