using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SoulShop_Controls : MonoBehaviour
{
    public GameObject canvas;
    public Card_Display display;

    private PlayerControls controls;
    private PlayerInput input;
    private GraphicRaycaster caster;

    private PointerEventData pointerData, hoveringData;
    private List<RaycastResult> pointerResults, hoveringResults;
    private GameObject selectedSkill;

    private byte hoveringItem = 0; // 0 = None, 1 = Upgrade, 2 = Downgrade, 3 = Unlock

    private void Awake()
    {
        controls = new PlayerControls();
        input = GetComponent<PlayerInput>();
        input.onActionTriggered += OnClick;
    }

    private void Start()
    {
        caster = canvas.GetComponent<GraphicRaycaster>();
        pointerData = new PointerEventData(EventSystem.current);
        pointerResults = new List<RaycastResult>();

        hoveringData = new PointerEventData(EventSystem.current);
        hoveringResults = new List<RaycastResult>();
    }

    private void FixedUpdate()
    {
        // Hovering
        hoveringData.position = Mouse.current.position.ReadValue();
        hoveringResults.Clear();
        caster.Raycast(hoveringData, hoveringResults);
        bool hasFound = false;

        foreach (var result in hoveringResults)
        {
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
        }

        if (!hasFound) if (hoveringItem != 0) SwitchHover(0);
        
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
    private void OnClick(InputAction.CallbackContext context)
    {
        if (context.action.name.Equals("Left Click") && context.performed)
        {
            // Raycast for UI
            pointerData.position = Mouse.current.position.ReadValue();
            pointerResults.Clear();

            caster.Raycast(pointerData, pointerResults);

            // Cycle through the results
            foreach (var result in pointerResults)
            {
                // Find the first (should be one) skill if available
                if (result.gameObject.CompareTag("SkillOption"))
                {
                    selectedSkill = result.gameObject;
                    var skill = result.gameObject.name.Split(' ');
                    var skillName = skill[0];
                    var skillLevel = skill[1];

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
                            newLevel = display.ClickUpgrade();
                            break;
                        default: throw new Exception("I don't know how you got here... But you are here since [" + result.gameObject.name + "] is not a button...");
                    }

                    UpdateCardText(newLevel);
                }

                // Affinities
                if (result.gameObject.CompareTag("AffinitiesIcon"))
                {
                    var color = result.gameObject.GetComponent<Image>().color;
                    result.gameObject.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 100);
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