using System;
using Source.Utility;
using UnityEngine;

namespace Source.Soul_Shop
{
    public class HoveringButtons : MouseInteraction
    {
        private readonly GameObject[] _buttons;

        public HoveringButtons(GameObject[] arr) : base("Group A")
        {
            _buttons = arr;
        }
        
        public override void OnLeftClick(string name)
        {
            switch (name)
            {
                case "Home Button":
                    OnSwitchClick(1);
                    break;
                case "Stats Button":
                    OnSwitchClick(2);
                    break;
                case "Affinities Button":
                    OnSwitchClick(3);
                    break;
                case "Transcend Button":
                    OnSwitchClick(4);
                    break;
                case "Skills Button":
                    OnSwitchClick(5);
                    break;
                case "Item Shop Button":
                    OnSwitchClick(6);
                    break;
                case "Curse Button":
                    OnSwitchClick(7);
                    break;
                default: throw new Exception("Could not find an item with the name of " + name);
            }
        }

        public override void OnLeftClick(GameObject item) { }

        public override void OnMouseHover(string name)
        {
            switch (name)
            {
                case "Home Button":
                    OnSwitch(1);
                    break;
                case "Stats Button":
                    OnSwitch(2);
                    break;
                case "Affinities Button":
                    OnSwitch(3);
                    break;
                case "Transcend Button":
                    OnSwitch(4);
                    break;
                case "Skills Button":
                    OnSwitch(5);
                    break;
                case "Item Shop Button":
                    OnSwitch(6);
                    break;
                case "Curse Button":
                    OnSwitch(7);
                    break;
                default: throw new Exception("Could not find an item with the name of " + name);
            }
        }

        public override void OnMouseHover(GameObject item) { }

        public override void OnSwitch(int index)
        {
            SelectedIndex = index;
            if (index is >= 1 and <= 7) _buttons[index - 1].transform.GetChild(2).gameObject.SetActive(true);
            
            for (var i = 0; i < _buttons.Length; i++)
            {
                if (SelectedIndex != i + 1)
                    _buttons[i].transform.GetChild(2).gameObject.SetActive(false);
            }
        }

        public override void OnSwitchClick(int index)
        {
            ClickedIndex = index;
            if (index is >= 1 and <= 7)
            {
                _buttons[index - 1].transform.GetChild(1).gameObject.SetActive(true);
                GameObject.FindWithTag("GameManager").GetComponent<Soul_GM>().SwitchCanvas(index - 1);
            }
            
            for (var i = 0; i < _buttons.Length; i++)
            {
                if (ClickedIndex != i + 1)
                    _buttons[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}
