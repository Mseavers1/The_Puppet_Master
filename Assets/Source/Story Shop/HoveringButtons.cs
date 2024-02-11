using System;
using Source.Utility;
using UnityEngine;

namespace Source.Story_Shop
{
    public class HoveringButtons : MouseInteraction
    {
        private readonly GameObject[] _buttons;
        private const float HoverSize = 1.1f, SelectedSize = 1.2f;

        public HoveringButtons(GameObject[] arr) : base("Group A")
        {
            _buttons = arr;
        }
        
        public override void OnLeftClick(string name)
        {
            switch (name)
            {
                case "Skills Button":
                    OnSwitchClick(1);
                    break;
                case "Stats Button":
                    OnSwitchClick(2);
                    break;
                case "Start Story Button":
                    OnSwitchClick(3);
                    break;
                case "Curses Button":
                    OnSwitchClick(4);
                    break;
                case "Item Shop Button":
                    OnSwitchClick(5);
                    break;
                default: throw new Exception("Could not find an item with the name of " + name);
            }
        }

        public override void OnLeftClick(GameObject item) { }

        public override void OnMouseHover(string name)
        {
            switch (name)
            {
                case "Skills Button":
                    OnSwitch(1);
                    break;
                case "Stats Button":
                    OnSwitch(2);
                    break;
                case "Start Story Button":
                    OnSwitch(3);
                    break;
                case "Curses Button":
                    OnSwitch(4);
                    break;
                case "Item Shop Button":
                    OnSwitch(5);
                    break;
                default: throw new Exception("Could not find an item with the name of " + name);
            }
        }

        public override void OnMouseHover(GameObject item) { }

        public override void OnSwitch(int index)
        {
            SelectedIndex = index;
            
            if (SelectedIndex == ClickedIndex) return;
            
            if (index is >= 1 and <= 5) _buttons[index - 1].transform.localScale = new Vector3(HoverSize, HoverSize);
            
            for (var i = 0; i < _buttons.Length; i++)
            {
                if (SelectedIndex != i + 1 && (i + 1) != ClickedIndex)
                    _buttons[i].transform.localScale = new Vector3(1, 1);
            }
        }

        public override void OnSwitchClick(int index)
        {
            ClickedIndex = index;
            if (index is >= 1 and <= 5)
            {
                _buttons[index - 1].transform.localScale = new Vector3(SelectedSize, SelectedSize);
                //GameObject.FindWithTag("GameManager").GetComponent<Soul_GM>().SwitchCanvas(index - 1);
            }
            
            for (var i = 0; i < _buttons.Length; i++)
            {
                if (ClickedIndex != i + 1)
                    _buttons[i].transform.localScale = new Vector3(1, 1);
            }
        }
    }
}
