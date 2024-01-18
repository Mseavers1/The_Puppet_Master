using Unity.VisualScripting;
using UnityEngine;

namespace Source.Utility
{
    public abstract class MouseInteraction
    {
        public bool HasButtonClicked { set; protected get; }
        public int SelectedIndex { protected set; get; }
        public string SelectedName { protected set; get; }
        public int ClickedIndex { protected set; get; }

        public string Tag { private set; get; }

        protected MouseInteraction(string tag)
        {
            Tag = tag;
        }

        public abstract void OnLeftClick(string name);
        
        public abstract void OnLeftClick(GameObject item);

        public abstract void OnMouseHover(string name);
        
        public abstract void OnMouseHover(GameObject item);

        public abstract void OnSwitch(int index);
        
        public abstract void OnSwitchClick(int index);

    }
}
