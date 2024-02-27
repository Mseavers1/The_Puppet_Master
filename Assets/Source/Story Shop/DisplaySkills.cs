using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Source.Story_Shop
{
    public class DisplaySkills : MonoBehaviour
    {
        public GameObject[] slots;
        public TMP_Text pageCounterText;
        public TMP_Text[] slotsPlus, slotsMinus;

        private List<SkillType> _allSkills;
        private int _currentPage, _currentMaxPage;

        private GraphicRaycaster _caster;
        private List<RaycastResult> _pointerResults, _clickResults;
        private PointerEventData _pointerData, _clickData;
        private PlayerInput _input;
        private PageButtonLogic _pageButtonLogic;
        private PlaceholderLogic _placeholderLogic;

        public void PageUp()
        {
            if (_currentPage < _currentMaxPage)
                _currentPage++;
            
            UpdateCurrentPage();
            UpdatePageDisplay();
        }

        public void PageDown()
        {
            if (_currentPage > 1)
                _currentPage--;
            
            UpdateCurrentPage();
            UpdatePageDisplay();
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
                if (result.gameObject.CompareTag(_placeholderLogic.Tag))
                {
                    _placeholderLogic.OnMouseHover(result.gameObject);
                    hasFoundItem = true;
                    break;
                }
            }

            if (!hasFoundItem) if (_placeholderLogic.SelectedIndex != 0) _placeholderLogic.OnSwitch(0);
        }

        private void Awake()
        {
            _input = GameObject.FindWithTag("GameManager").GetComponent<PlayerInput>();
            _input.onActionTriggered += OnClick;
            _pageButtonLogic = new PageButtonLogic(this);
            _placeholderLogic = new PlaceholderLogic(this);
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
                if (result.gameObject.CompareTag(_pageButtonLogic.Tag))
                {
                    _pageButtonLogic.OnLeftClick(result.gameObject.name);
                    break;
                }

                if (result.gameObject.CompareTag(_placeholderLogic.Tag))
                {
                    _placeholderLogic.OnLeftClick(result.gameObject);
                }
            }
        }

        private void Start()
        {
            _caster = GetComponent<GraphicRaycaster>();
            _pointerData = new PointerEventData(EventSystem.current);
            _pointerResults = new List<RaycastResult>();

            _clickData = new PointerEventData(EventSystem.current);
            _clickResults = new List<RaycastResult>();
            
            _allSkills = HoldingOfSkills.GetAllSkills();
            UpdatePageCount();
            UpdateCurrentPage();
        }

        // Update the page count
        private void UpdatePageCount()
        {
            foreach (var slot in slots) slot.transform.parent.gameObject.SetActive(true);
            
            _currentPage = 1;
            _currentMaxPage = Mathf.CeilToInt(_allSkills.Count / 4f);

            UpdatePageDisplay();
        }

        private void UpdatePageDisplay()
        {
            pageCounterText.text = _currentPage + " / " + _currentMaxPage;
        }

        private void UpdateCurrentPage()
        {
            var lastIndex = _currentPage * 4 - 1;

            for (var slotIndex = 3; slotIndex >= 0; slotIndex--)
            {
                var getSkill = GetSkillOfID(lastIndex, slotIndex);
                
                if(getSkill == null) 
                    slots[slotIndex].transform.parent.gameObject.SetActive(false);
                else
                {
                    if (!slots[slotIndex].transform.parent.gameObject.activeSelf) { slots[slotIndex].transform.parent.gameObject.SetActive(true); }
                    UpdateSlot(slotIndex, GetSkillOfID(lastIndex, slotIndex));
                }
            }
        }

        private SkillType GetSkillOfID(int lastIndex, int slotIndex)
        {
            try
            {
                return _allSkills[lastIndex - (3 - slotIndex)];
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        // Changes the current slot with something else
        private void UpdateSlot(int slot, SkillType skill)
        {

            // Change name
            slots[slot].transform.GetChild(3).GetComponent<TMP_Text>().text = skill.Name;
            
            // Change level
            var level = 0;
            if (HoldingOfSkills.BoughtSkills.ContainsKey(skill.Name))
                level = HoldingOfSkills.BoughtSkills[skill.Name];
            
            slots[slot].transform.GetChild(4).GetComponent<TMP_Text>().text = "lv. " + level;
        }
    }
}
