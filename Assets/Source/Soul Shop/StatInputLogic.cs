using System;
using Source.Utility;
using TMPro;
using UnityEngine;

namespace Source.Soul_Shop
{
    public class StatInputLogic : MouseInteraction
    {
        private readonly GameObject[] _buttons;
        private const float HoverSize = 1.1f;

        private int[] _sizes = {1, 5, 10, -1};
        private int _currentIndexOfSize;

        public StatInputLogic(GameObject[] buttons) : base("Group B")
        {
            _buttons = buttons;
        }

        public override void OnLeftClick(string name)
        {
            
        }

        private void UpdateQuantityDisplay(GameObject display)
        {
            var text = display.transform.GetChild(1).GetComponent<TMP_Text>();
            
            if (_currentIndexOfSize == _sizes.Length - 1)
                text.text = "MAX";
            else
                text.text = "x" + _sizes[_currentIndexOfSize];
        }

        public override void OnLeftClick(GameObject item)
        {
            switch (item.name)
            {
                case "Add":
                    
                    break;
                case "Remove":
                    
                    break;
                case "Quantity":
                    _currentIndexOfSize = (_currentIndexOfSize + 1) % _sizes.Length;
                    UpdateQuantityDisplay(item);
                    break;
                default: throw new Exception("Could not find an item with the name of " + item.name);
            }
        }

        public override void OnMouseHover(string name)
        {
            switch (name)
            {
                case "Add":
                    OnSwitch(1);
                    break;
                case "Remove":
                    OnSwitch(2);
                    break;
                case "Quantity":
                    OnSwitch(3);
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
            if (index is >= 1 and <= 7) _buttons[index - 1].transform.localScale = Vector3.one * HoverSize;
            
            for (var i = 0; i < _buttons.Length; i++)
            {
                if (SelectedIndex != i + 1)
                    _buttons[i].transform.localScale = Vector3.one;
            }
        }

        public override void OnSwitchClick(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}
