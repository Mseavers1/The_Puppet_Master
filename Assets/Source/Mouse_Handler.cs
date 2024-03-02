using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Mouse_Handler : MonoBehaviour
{
    public PlayerInput input;
    public Canvas canvas;
    public GameObject[] cards;
    public BattleSimulator battleSimulator;

    public Color defaultItemBackgroundColor;
    public Color hoverItemSlot;

    private PlayerControls controls;
    private Gamemanager_World gm;
    private bool clickedOnCard = false, clickedOnSlot = false;
    private GameObject selectedSlot;

    private GraphicRaycaster uiCaster, clickCaster;
    private PointerEventData uiData, clickData;
    private List<RaycastResult> uiResults, clickResults;

    private CardDisplayInfo currentCard;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Gamemanager_World>();
        controls = GetComponent<Player_Movement>().GetControls();
        input.onActionTriggered += OnClick;

        uiCaster = canvas.GetComponent<GraphicRaycaster>();
        clickCaster = canvas.GetComponent<GraphicRaycaster>();

        uiData = new PointerEventData(EventSystem.current);
        clickData = new PointerEventData(EventSystem.current);

        uiResults = new List<RaycastResult>();
        clickResults = new List<RaycastResult>();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (gm.IsTutorialOn()) return;

        // Detect left clicks
        if (context.action.name.Equals("Left Click") && context.performed) 
        {
            var pos = Mouse.current.position.ReadValue();
            var uiHit = false;
            clickedOnSlot = false;

            // UI Clicking
            clickData.position = pos;
            clickResults.Clear();

            clickCaster.Raycast(clickData, clickResults);

            foreach (var result in clickResults) 
            {
                if (gm.Mode == "Battle Player" || gm.Mode == "Card Target")
                {
                    if (result.gameObject.CompareTag("Card"))
                    {
                        clickedOnCard = true;
                        var split = result.gameObject.name.Split(' ');
                        SwitchHover(int.Parse(split[1]));
                        gm.Mode = "Card Target";
                        uiHit = true;
                        currentCard = result.gameObject.GetComponent<CardDisplayInfo>();
                    }
                }

                if (gm.InventoryPanel.activeSelf)
                {
                    if (result.gameObject.CompareTag("InventorySlot"))
                    {

                        foreach (var ic in gm.ItemIcons.Values)
                        {
                            if (ic.GetComponent<SlotContainer>().SlotIndex != result.gameObject.transform.parent.gameObject.GetComponent<SlotContainer>().SlotIndex)
                                ic.transform.GetChild(3).gameObject.SetActive(false);
                        }

                        if (result.gameObject.transform.parent.gameObject.GetComponent<SlotContainer>().CurrentItem == null) return;

                        clickedOnSlot = true;
                        var menu = result.gameObject.transform.parent.GetChild(3);
                        menu.gameObject.SetActive(true);
                        var item = result.gameObject.transform.parent.gameObject.GetComponent<SlotContainer>().CurrentItem;


                        // Move to slotcontainer?
                        if (item.CanUse)
                        {
                            menu.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                        }
                        else if (item.CanEquip) menu.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                        else 
                            menu.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);

                        if (selectedSlot != menu.gameObject)
                        {
                            result.gameObject.transform.parent.SetParent(result.gameObject.transform.parent.parent.parent);
                            result.gameObject.transform.parent.SetParent(gm.InventoryPanel.transform);
                        }

                        selectedSlot = menu.gameObject;

                        SwitchHoverSlots(result.gameObject.GetComponentInParent<SlotContainer>().SlotIndex);
                    }

                    if (result.gameObject.CompareTag("BackgroundSlot"))
                    {
                        clickedOnSlot = true;
                    }

                    //if (result.gameObject != null) print(result.gameObject.name);
                }
            }

            if (!clickedOnSlot && selectedSlot != null) RemoveSlotHoverClick();

            // Non UI Clicking
            var worldPos = Camera.main.ScreenToWorldPoint(pos);

            Collider2D detectedCollider = Physics2D.OverlapPoint(worldPos, 1<<6);

            if (detectedCollider != null)
            {
                switch (detectedCollider.tag)
                {
                    case "Player":

                        print("player");
                        if (gm.Mode == "Card Target" && currentCard.GetCard().IsAllyOnly())
                        {
                            print("test");
                            PlayCard(detectedCollider);
                        }

                        break;
                    case "NPC":
                        // Prevents other actions occuring
                        if (gm.Mode != "None") break;

                        var handler = detectedCollider.GetComponent<Chatbox_Handler>();
                        if(!handler.playing)
                            handler.StartChat();

                        gm.Mode = "Chat";
                        break;
                    case "Chatbox":
                        if (detectedCollider.GetComponentInParent<Chatbox_Handler>().canContinue)
                            detectedCollider.transform.GetComponentInParent<Chatbox_Handler>().ContinueChat();
                        break;
                    case "Enemy":
                        // Find target if looking for card target
                        if (gm.Mode == "Card Target" && !currentCard.GetCard().IsAllyOnly())
                        {
                            PlayCard(detectedCollider);
                            break;
                        }

                        // Prevents battle starting if one is occuring
                        if (gm.Mode != "None") break;

                        // Find enemy and starts a battle
                        var enemy = detectedCollider.GetComponent<EnemyInfo>();
                        var playables = new GameObject[1]; // Temp
                        playables[0] = gameObject;
                        //enemy.StartBattle(playables);

                        if (battleSimulator.IsPlayerTurn()) gm.Mode = "Battle Player";
                        else
                        {
                            gm.Mode = "Battle Enemey";
                            enemy.PlayTurn();
                        }

                        break;
                }

            }

            // Remove card target if clicks on nothing
            if (!uiHit && gm.Mode == "Card Target")
            {
                gm.Mode = "Battle Player";
                SwitchHover(6);
                clickedOnCard = false;
            }

        }
    }

    private void RemoveSlotHoverClick()
    {
        clickedOnSlot = false;
        selectedSlot.SetActive(false);
        SwitchHoverSlots(-1);
        selectedSlot = null;
    }

    private void PlayCard(Collider2D target)
    {
        var card = currentCard.GetCard();
        var playerStats = StaticHolder.PlayerStats;
        Debug.Log(target.name);

        // Check if card is playable
        if (!playerStats.PlayCard(card.GetManaCost(), card.GetStaminaCost())) return; // TODO - Animation that declines buying

        // Check if there is combat
        if (!card.IsNoCombat())
        {
            // Calculate Damage of card
            var damage = currentCard.CalculateCardDamage(card);
            target.GetComponent<EnemyInfo>().TakeDamage(damage);
        }

        // Do Specials
        /*
        foreach(var c in card.GetSpellAttributes())
        {
            var command = c.Split(' ');
            switch (command[0])
            {
                case "HEAL":
                    StaticHolder.PlayerStats.Heal(float.Parse(command[1])); // TODO - Change for allies

                    break;
            }
        } */

        //currentCard.SetDesc(battleSimulator.DrawCard(currentCard.GetCardType()));

        //battleSimulator.NextTurn(); // Temp - Only end turn when player is ready
        // Update top bar
        gm.displays[0].UpdateText(playerStats.CurrentHealth + " / " + playerStats.GetStatValue("Health") + " HP");
        gm.displays[1].UpdateText(playerStats.CurrentMana + " / " + playerStats.GetStatValue("Mana") + " MP");
        gm.displays[2].UpdateText(playerStats.CurrentStamina + " / " + playerStats.GetStatValue("Stamina") + " S");

        SwitchHover(6);

        if (target.CompareTag("Enemy") && target.GetComponent<EnemyInfo>().IsDead()) battleSimulator.NextTurn();
    }

    private void FixedUpdate()
    {
        // Hovering
        if (gm.Mode == "Battle Player" && !clickedOnCard)
        {
            if (battleSimulator.IsPlayerTurn())
                OnHover(0);
        }

        if (gm.InventoryPanel.activeSelf) OnHover(1);
    }

    private void OnHover(int type)
    {
        if (gm.IsTutorialOn()) return;

        var pos = Mouse.current.position.ReadValue();
        uiData.position = pos;
        uiResults.Clear();

        uiCaster.Raycast(uiData, uiResults);
        var hasFound = false;
        var hasFoundSlot = false;

        foreach(var result in uiResults)
        {
            if (type == 0 && result.gameObject.CompareTag("Card"))
            {
                hasFound = true;
                var split = result.gameObject.name.Split(' ');
                SwitchHover(int.Parse(split[1]));
            }

            if (type == 1 && !clickedOnSlot && result.gameObject.CompareTag("InventorySlot"))
            {
                //print("found!");
                hasFoundSlot = true;
                SwitchHoverSlots(result.gameObject.GetComponentInParent<SlotContainer>().SlotIndex);
            }

            //if (result.gameObject != null) print (result.gameObject.name);
        }

        if (!hasFound) SwitchHover(6);
        if (!hasFoundSlot && !clickedOnSlot) SwitchHoverSlots(-1);
    }

    public void SwitchHoverSlots(int id)
    {
        foreach (var slot in gm.ItemIcons)
        {
            var box = slot.Value.transform.GetChild(2);
            if (slot.Key == id)
            {
                slot.Value.transform.GetChild(0).GetComponent<Image>().color = hoverItemSlot;

                if (slot.Value.GetComponent<SlotContainer>().CurrentItem != null)
                    box.gameObject.SetActive(true);
                
            } 
            else
            {
                slot.Value.transform.GetChild(0).GetComponent<Image>().color = defaultItemBackgroundColor;

                if (slot.Value.GetComponent<SlotContainer>().CurrentItem != null)
                    box.gameObject.SetActive(false);
            }
        }
    }

    private void SwitchHover(int cardID)
    {
        switch (cardID)
        {
            case 1:
                cards[0].GetComponent<Card_Scaling>().HoverCard(true);
                cards[1].GetComponent<Card_Scaling>().HoverCard(false);
                cards[2].GetComponent<Card_Scaling>().HoverCard(false);
                cards[3].GetComponent<Card_Scaling>().HoverCard(false);
                cards[4].GetComponent<Card_Scaling>().HoverCard(false);
                break;
            case 2:
                cards[0].GetComponent<Card_Scaling>().HoverCard(false);
                cards[1].GetComponent<Card_Scaling>().HoverCard(true);
                cards[2].GetComponent<Card_Scaling>().HoverCard(false);
                cards[3].GetComponent<Card_Scaling>().HoverCard(false);
                cards[4].GetComponent<Card_Scaling>().HoverCard(false);
                break;
            case 3:
                cards[0].GetComponent<Card_Scaling>().HoverCard(false);
                cards[1].GetComponent<Card_Scaling>().HoverCard(false);
                cards[2].GetComponent<Card_Scaling>().HoverCard(true);
                cards[3].GetComponent<Card_Scaling>().HoverCard(false);
                cards[4].GetComponent<Card_Scaling>().HoverCard(false);
                break;
            case 4:
                cards[0].GetComponent<Card_Scaling>().HoverCard(false);
                cards[1].GetComponent<Card_Scaling>().HoverCard(false);
                cards[2].GetComponent<Card_Scaling>().HoverCard(false);
                cards[3].GetComponent<Card_Scaling>().HoverCard(true);
                cards[4].GetComponent<Card_Scaling>().HoverCard(false);
                break;
            case 5:
                cards[0].GetComponent<Card_Scaling>().HoverCard(false);
                cards[1].GetComponent<Card_Scaling>().HoverCard(false);
                cards[2].GetComponent<Card_Scaling>().HoverCard(false);
                cards[3].GetComponent<Card_Scaling>().HoverCard(false);
                cards[4].GetComponent<Card_Scaling>().HoverCard(true);
                break;
            case 6:
                cards[0].GetComponent<Card_Scaling>().HoverCard(false);
                cards[1].GetComponent<Card_Scaling>().HoverCard(false);
                cards[2].GetComponent<Card_Scaling>().HoverCard(false);
                cards[3].GetComponent<Card_Scaling>().HoverCard(false);
                cards[4].GetComponent<Card_Scaling>().HoverCard(false);
                break;
            default: throw new Exception("Uh oh... how did you get here?? Wrong cardID but how?!? --> cardID of " + cardID);
        }
    }

}
