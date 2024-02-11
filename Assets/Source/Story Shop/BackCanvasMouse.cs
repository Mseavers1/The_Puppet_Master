using System.Collections.Generic;
using Source.Soul_Shop;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Source.Story_Shop
{
    public class BackCanvasMouse : MonoBehaviour
    {
        public GameObject[] menuButtons;

        private GraphicRaycaster _caster;
        private List<RaycastResult> _pointerResults, _clickResults;
        private PointerEventData _pointerData, _clickData;
        private HoveringButtons _hoveringButtons;
        private PlayerInput _input;

        private void Awake()
        {
            _input = GameObject.FindWithTag("GameManager").GetComponent<PlayerInput>();
            _input.onActionTriggered += OnClick;
            _hoveringButtons = new HoveringButtons(menuButtons);
        }

        private void Start()
        {
            _caster = GetComponent<GraphicRaycaster>();
            _pointerData = new PointerEventData(EventSystem.current);
            _pointerResults = new List<RaycastResult>();

            _clickData = new PointerEventData(EventSystem.current);
            _clickResults = new List<RaycastResult>();
        }


        private void FixedUpdate()
        {
            // Hovering
            _pointerData.position = Mouse.current.position.ReadValue();
            _pointerResults.Clear();
            _caster.Raycast(_pointerData, _pointerResults);
            var hasFoundItem = false;

            foreach (var result in _pointerResults)
            {
                if (result.gameObject.CompareTag(_hoveringButtons.Tag))
                {
                    _hoveringButtons.OnMouseHover(result.gameObject.name);
                    hasFoundItem = true;
                    break;
                }
            }

            if (!hasFoundItem) if (_hoveringButtons.SelectedIndex != 0) _hoveringButtons.OnSwitch(0);

        }

        private void OnClick(InputAction.CallbackContext context)
        {
            if (context.action.name.Equals("Left Click") && context.performed) LeftClick();
        }

        private void LeftClick()
        {
            _clickData.position = Mouse.current.position.ReadValue();
            _clickResults.Clear();

            _caster.Raycast(_clickData, _clickResults);
            var hasFoundItem = false;
            
            foreach (var result in _clickResults)
            {
                if (result.gameObject.CompareTag(_hoveringButtons.Tag))
                {
                    _hoveringButtons.OnLeftClick(result.gameObject.name);
                    hasFoundItem = true;
                    break;
                }
            }
            
            if (!hasFoundItem) if (_hoveringButtons.ClickedIndex != 0) _hoveringButtons.OnSwitchClick(0);
        }
    }
}
