using System;
using Source.Utility;
using TMPro;
using UnityEngine;

namespace Source.Soul_Shop
{
    public class StatLogic : MouseInteraction
    {
        private readonly GameObject[] _buttons;
        private int _selectedClicked; 
        
        public StatLogic(GameObject[] buttons) : base("Group A")
        {
            _buttons = buttons;
        }

        public override void OnLeftClick(string name)
        {
            throw new System.NotImplementedException();
        }

        public override void OnLeftClick(GameObject item)
        {
            _selectedClicked = SelectedIndex;
        }

        public override void OnMouseHover(string name)
        {
            switch (name)
            {
                case "Vitality":
                    OnSwitch(1);
                    break;
                case "Intelligence":
                    OnSwitch(2);
                    break;
                case "Endurance":
                    OnSwitch(3);
                    break;
                case "Strength":
                    OnSwitch(4);
                    break;
                case "Agility":
                    OnSwitch(5);
                    break;
                case "Speed":
                    OnSwitch(6);
                    break;
                case "Luck":
                    OnSwitch(7);
                    break;
                default: throw new Exception("Could not find an item with the name of " + name);
            }
        }

        public override void OnMouseHover(GameObject item)
        {
            throw new System.NotImplementedException();
        }

        public override void OnSwitch(int index)
        {
            SelectedIndex = index;
            if (index is >= 1 and <= 7) _buttons[index - 1].transform.localScale = Vector3.one * 1.3f;
            
            for (var i = 0; i < _buttons.Length; i++)
            {
                if (SelectedIndex != i + 1 && _selectedClicked != i + 1)
                    _buttons[i].transform.localScale = Vector3.one;
            }
        }

        public override void OnSwitchClick(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}