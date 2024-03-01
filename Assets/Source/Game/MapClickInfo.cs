using Source.Utility;
using UnityEngine;

namespace Source.Game
{
    public class MapClickInfo : MouseInteraction
    {
        public MapClickInfo() : base("Group A")
        {
        }

        public override void OnLeftClick(string name)
        {
            throw new System.NotImplementedException();
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
