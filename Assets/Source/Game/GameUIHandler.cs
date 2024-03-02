using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Source.Game
{
    public class GameUIHandler : MonoBehaviour
    {
        private GraphicRaycaster _caster;
        private List<RaycastResult> _pointerResults, _clickResults;
        private PointerEventData _pointerData, _clickData;
        private CardUiLogic _cardUiLogic;
        private PlayerInput _input;
        private EndTurnLogic _endTurnLogic;
        private HoveringStatsIcons _icons;

        private void Awake()
        {
            _input = GameObject.FindWithTag("GameManager").GetComponent<PlayerInput>();
            _input.onActionTriggered += OnClick;
            _cardUiLogic = new CardUiLogic(GameObject.FindWithTag("GameManager").GetComponent<GameGm>().cards, this);
            _endTurnLogic = new EndTurnLogic(GameObject.FindWithTag("GameManager").GetComponent<GameGm>());
            _icons = new HoveringStatsIcons(GameObject.FindWithTag("GameManager").GetComponent<GameGm>().icons, GameObject.FindWithTag("GameManager").GetComponent<GameGm>().enemyHp);
        }
        
        private void Start()
        {
            _caster = GetComponent<GraphicRaycaster>();
            _pointerData = new PointerEventData(EventSystem.current);
            _pointerResults = new List<RaycastResult>();

            _clickData = new PointerEventData(EventSystem.current);
            _clickResults = new List<RaycastResult>();
        }
        
        private void OnClick(InputAction.CallbackContext context)
        {
            if (context.action.name.Equals("Left Click") && context.performed) LeftClick();
        }

        private void FixedUpdate()
        {
            // Hovering
            _pointerData.position = Mouse.current.position.ReadValue();
            _pointerResults.Clear();
            _caster.Raycast(_pointerData, _pointerResults);
            var hasFoundItem = false;
            var hasFoundEnd = false;
            var hasFoundIcon = false;

            foreach (var result in _pointerResults)
            {
                if (result.gameObject.CompareTag(_cardUiLogic.Tag))
                {
                    _cardUiLogic.OnMouseHover(result.gameObject.transform.parent.gameObject);
                    hasFoundItem = true;
                    break;
                }

                if (result.gameObject.CompareTag(_endTurnLogic.Tag))
                {
                    _endTurnLogic.OnMouseHover(result.gameObject);
                    hasFoundEnd = true;
                }
                
                if (result.gameObject.CompareTag(_icons.Tag))
                {
                    _icons.OnMouseHover(result.gameObject);
                    hasFoundIcon = true;
                }

            }

            if (!hasFoundItem) if (_cardUiLogic.SelectedIndex != 0) _cardUiLogic.OnSwitch(0);
            
            if (!hasFoundEnd) if (_endTurnLogic.SelectedIndex != 0) _endTurnLogic.OnSwitch(0);
            
            if (!hasFoundIcon) if (_icons.SelectedIndex != 0) _icons.OnSwitch(0);
        }

        public void ReturnCardPosition(int index)
        {
            var gm = GameObject.FindWithTag("GameManager").GetComponent<GameGm>();
            
            gm.ReplaceCardUI(index);

            Invoke(nameof(MoveCardPosition), 0.15f);

        }

        private void MoveCardPosition()
        {
            var gm = GameObject.FindWithTag("GameManager").GetComponent<GameGm>();
            gm.cards[1].transform.DOMoveY(-230, 0.1f).SetEase(Ease.InOutSine);
            Invoke(nameof(EnableAnimationForCard), 0.1f);
        }

        private void EnableAnimationForCard()
        {
            _cardUiLogic.isAnimating = false;
        }

        private void LeftClick()
        {
            _clickData.position = Mouse.current.position.ReadValue();
            _clickResults.Clear();

            _caster.Raycast(_clickData, _clickResults);

            foreach (var result in _clickResults)
            {
                if (result.gameObject.CompareTag(_cardUiLogic.Tag))
                {
                    _cardUiLogic.OnLeftClick(result.gameObject.transform.parent.gameObject);
                    break;
                }
                
                if (result.gameObject.CompareTag(_endTurnLogic.Tag))
                {
                    _endTurnLogic.OnLeftClick(result.gameObject);
                    break;
                }
                
            }
        }
    }
}
