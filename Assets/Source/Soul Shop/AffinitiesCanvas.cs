using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Source.Soul_Shop
{
    public class AffinitiesCanvas : MonoBehaviour
    {
        public Color[] affinityColors;
        public Color defaultAffinityColor;
        public GameObject[] advancedAffinities, beginningAffinities;

        private GraphicRaycaster _caster;
        private List<RaycastResult> _clickResults;
        private PointerEventData _clickData;
        private PlayerInput _input;
        private AffinityIconsLogic _affinityIconsLogic;

        private void Awake()
        {
            _input = GameObject.FindWithTag("GameManager").GetComponent<PlayerInput>();
            _input.onActionTriggered += OnClick;
            _affinityIconsLogic = new AffinityIconsLogic(affinityColors, defaultAffinityColor, advancedAffinities);
        }

        private void Start()
        {
            _caster = GetComponent<GraphicRaycaster>();

            _clickData = new PointerEventData(EventSystem.current);
            _clickResults = new List<RaycastResult>();

            UpdateAffinityText();
        }

        public void UpdateAffinityText()
        {
            var index = 0;
            foreach (var beginning in beginningAffinities)
            {
                if (!SoulGmSettings.GetAffinityPosition(index))
                    beginning.transform.GetChild(3).GetComponent<TMP_Text>().text = "-" + SoulGmSettings.GetAffinityCost(index++) + " SP";
                else
                    beginning.transform.GetChild(3).GetComponent<TMP_Text>().text = "+" + SoulGmSettings.GetAffinityGain(index++) + " SP";
            }

            foreach (var advanced in advancedAffinities)
            {
                if (!SoulGmSettings.GetAffinityPosition(index))
                    advanced.transform.GetChild(3).GetComponent<TMP_Text>().text = "-" + SoulGmSettings.GetAffinityCost(index++) + " SP";
                else
                    advanced.transform.GetChild(3).GetComponent<TMP_Text>().text = "+" + SoulGmSettings.GetAffinityGain(index++) + " SP";
            }
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
                if (result.gameObject.CompareTag(_affinityIconsLogic.Tag))
                {
                    _affinityIconsLogic.OnLeftClick(result.gameObject.transform.parent.gameObject);
                    break;
                }
            }
        }
    }
}
