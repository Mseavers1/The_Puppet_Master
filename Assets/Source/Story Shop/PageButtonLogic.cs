using System;
using Source.Utility;
using UnityEngine;

namespace Source.Story_Shop
{
    public class PageButtonLogic : MouseInteraction
    {
        private DisplaySkills _displaySkills;
        
        public PageButtonLogic(DisplaySkills displaySkills) : base("Group C")
        {
            _displaySkills = displaySkills;
        }

        public override void OnLeftClick(string name)
        {
            switch (name)
            {
                case "Left Arrow":
                    _displaySkills.PageDown();
                    break;
                case "Right Arrow":
                    _displaySkills.PageUp();
                    break;
                default:
                    throw new Exception(name + " is not being searched for... Only left and right arrow...");
            }
        }

        public override void OnLeftClick(GameObject item)
        {
            throw new System.NotImplementedException();
        }

        public override void OnMouseHover(string name)
        {
            throw new System.NotImplementedException();
        }

        public override void OnMouseHover(GameObject item)
        {
            throw new System.NotImplementedException();
        }

        public override void OnSwitch(int index)
        {
            throw new System.NotImplementedException();
        }

        public override void OnSwitchClick(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}
