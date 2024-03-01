using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Source.Soul_Shop;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Source.Story_Shop
{
    public class ButtonCanvasMouse : MonoBehaviour
    {
        public GameObject[] menuButtons;
        public GameObject coord, confirmation;
        public CurtainCommands curtain;
        public SignAnimator signAnimator;

        private GraphicRaycaster _caster;
        private List<RaycastResult> _pointerResults, _clickResults;
        private PointerEventData _pointerData, _clickData;
        private HoveringButtons _hoveringButtons;
        private PlayerInput _input;
        private CoordLogic _coordLogic;

        public void Confirmation(float delay)
        {
            StartCoroutine(ShowConfirmation(delay));
        }

        public void NoConfirmation()
        {
            confirmation.SetActive(false);
            curtain.OpenCurtains(1.5f);
        }

        public void YesConfirmation()
        {
            signAnimator.DeleteTween();
            SceneManager.LoadScene(1);
        }
        
        private IEnumerator ShowConfirmation(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            confirmation.SetActive(true);
        }

        private void Awake()
        {
            _input = GameObject.FindWithTag("GameManager").GetComponent<PlayerInput>();
            _input.onActionTriggered += OnClick;
            _hoveringButtons = new HoveringButtons(menuButtons, curtain);
            _coordLogic = new CoordLogic(coord, curtain, this);
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
            var hasFoundCoord = false;

            foreach (var result in _pointerResults)
            {
                if (result.gameObject.CompareTag(_hoveringButtons.Tag))
                {
                    _hoveringButtons.OnMouseHover(result.gameObject.name);
                    hasFoundItem = true;
                    break;
                }
                
                if (result.gameObject.CompareTag(_coordLogic.Tag))
                {
                    _coordLogic.OnMouseHover(result.gameObject.name);
                    hasFoundCoord = true;
                    break;
                }
            }

            if (!hasFoundItem) if (_hoveringButtons.SelectedIndex != 0) _hoveringButtons.OnSwitch(0);

            if (!hasFoundCoord) if (_coordLogic.SelectedIndex != 0) _coordLogic.OnSwitch(0);
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
                if (result.gameObject.CompareTag(_hoveringButtons.Tag))
                {
                    if (curtain.isAnimating) return; 
                    
                    _hoveringButtons.OnLeftClick(result.gameObject.name);
                    break;
                }
                
                if (result.gameObject.CompareTag(_coordLogic.Tag))
                {

                    _coordLogic.OnLeftClick(result.gameObject.name);
                    break;
                }
            }
        }
    }
}
