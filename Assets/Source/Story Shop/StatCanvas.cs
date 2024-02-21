using System.Collections.Generic;
using Source.Soul_Shop;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Source.Story_Shop
{
    public class StatCanvas : MonoBehaviour
    {
        public GameObject[] buttons, buttons2;
        public StatTransformer statTransformer;
        public TMP_Text cost, refund;
        public Color selectedColor;

        private GraphicRaycaster _caster;
        private List<RaycastResult> _clickResults, _pointerResults;
        private PointerEventData _clickData, _pointerData;
        private PlayerInput _input;
        private StatLogic _statLogic;
        private StatInputLogic _statInputLogic;
    
        private void Awake()
        {
            _input = GameObject.FindWithTag("GameManager").GetComponent<PlayerInput>();
            _input.onActionTriggered += OnClick;
            _statLogic = new StatLogic(buttons);
            _statInputLogic = new StatInputLogic(buttons2, _statLogic, statTransformer, cost, refund, selectedColor);
            _statLogic.SetInputLogic(_statInputLogic);
        }

        private void Start()
        {
            _caster = GetComponent<GraphicRaycaster>();

            _clickData = new PointerEventData(EventSystem.current);
            _clickResults = new List<RaycastResult>();
        
            _pointerResults = new List<RaycastResult>();
            _pointerData = new PointerEventData(EventSystem.current);
        }
    
        private void FixedUpdate()
        {
            // Hovering
            _pointerData.position = Mouse.current.position.ReadValue();
            _pointerResults.Clear();
            _caster.Raycast(_pointerData, _pointerResults);
            var hasFoundItem = false;
            var hasFoundItemTwo = false;

            foreach (var result in _pointerResults)
            {
                if (result.gameObject.CompareTag(_statLogic.Tag))
                {
                    _statLogic.OnMouseHover(result.gameObject.name);
                    hasFoundItem = true;
                    break;
                }
            
                if (result.gameObject.CompareTag(_statInputLogic.Tag))
                {
                    var parent = result.gameObject.transform.parent;
                    _statInputLogic.OnMouseHover(result.gameObject.name == "Tail"
                        ? parent.transform.parent.gameObject.name
                        : parent.gameObject.name);

                    hasFoundItemTwo = true;
                    break;
                }
            }

            if (!hasFoundItem) if (_statLogic.SelectedIndex != 0) _statLogic.OnSwitch(0);
            if (!hasFoundItemTwo) if (_statInputLogic.SelectedIndex != 0) _statInputLogic.OnSwitch(0);

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
            
            foreach (var result in _clickResults)
            {
                if (result.gameObject.CompareTag(_statLogic.Tag))
                {
                    _statLogic.OnLeftClick(result.gameObject.transform.parent.gameObject);
                    break;
                }
            
                if (result.gameObject.CompareTag(_statInputLogic.Tag))
                {
                    var parent = result.gameObject.transform.parent;
                    _statInputLogic.OnLeftClick(result.gameObject.name == "Tail"
                        ? parent.transform.parent.gameObject
                        : parent.gameObject);
                    break;
                }
            }
        }
    }
}
