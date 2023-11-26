using System;
using System.Collections;
using System.Collections.Generic;
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

    private PlayerControls controls;
    private Gamemanager_World gm;
    private bool clickedOnCard = false;

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
        // Detect left clicks
        if (context.action.name.Equals("Left Click") && context.performed) 
        {
            var pos = Mouse.current.position.ReadValue();
            var uiHit = false;

            // UI Clicking
            if (gm.Mode == "Battle Player" || gm.Mode == "Card Target")
            {
                clickData.position = pos;
                clickResults.Clear();

                clickCaster.Raycast(clickData, clickResults);

                foreach (var result in clickResults) 
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
            }

            // Non UI Clicking
            var worldPos = Camera.main.ScreenToWorldPoint(pos);

            Collider2D detectedCollider = Physics2D.OverlapPoint(worldPos, 1<<6);

            if (detectedCollider != null)
            {
                //Debug.Log(detectedCollider.name);

                switch (detectedCollider.tag)
                {
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
                        if (gm.Mode == "Card Target")
                        {
                            Debug.Log(detectedCollider.name); // TODO - Do card to the enemy
                            detectedCollider.GetComponent<EnemyInfo>().TakeDamage(currentCard.GetCard().GetDamage());
                            currentCard.SetDesc(battleSimulator.DrawCard(currentCard.GetCardType()));
                            battleSimulator.NextTurn(); // Temp - Only end turn when player is ready
                            SwitchHover(6);
                            break;
                        }

                        // Prevents battle starting if one is occuring
                        if (gm.Mode != "None") break;

                        // Find enemy and starts a battle
                        var enemy = detectedCollider.GetComponent<EnemyInfo>();
                        var playables = new GameObject[1]; // Temp
                        playables[0] = gameObject;
                        enemy.StartBattle(playables);

                        if (battleSimulator.IsPlayerTurn()) gm.Mode = "Battle Player";
                        else gm.Mode = "Battle Enemey";

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

    private void FixedUpdate()
    {
        // Hovering
        if(gm.Mode == "Battle Player" && !clickedOnCard)
        {
            if (battleSimulator.IsPlayerTurn())
                OnHover();
        }
    }

    private void OnHover()
    {
        var pos = Mouse.current.position.ReadValue();
        uiData.position = pos;
        uiResults.Clear();

        uiCaster.Raycast(uiData, uiResults);
        var hasFound = false;

        foreach(var result in uiResults)
        {
            if (result.gameObject.CompareTag("Card"))
            {
                hasFound = true;
                var split = result.gameObject.name.Split(' ');
                SwitchHover(int.Parse(split[1]));
            }
        }

        if (!hasFound) SwitchHover(6);
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
