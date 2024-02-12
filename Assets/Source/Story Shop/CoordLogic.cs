using System;
using DG.Tweening;
using Source.Utility;
using UnityEngine;

namespace Source.Story_Shop
{
    public class CoordLogic : MouseInteraction
    {
        private GameObject _coord;
        private bool _isCordPulled = false;
        
        public CoordLogic(GameObject coord) : base("Group B")
        {
            _coord = coord;
            _coord.transform.DOShakePosition(1, 5, 10, 50, false, false).SetLoops(-1).timeScale = 0.35f;
            _coord.transform.DOPause();
        }

        public override void OnLeftClick(string name)
        {
            OnSwitchClick(name == "Start Story Coord" ? 1 : 0);
        }

        public override void OnLeftClick(GameObject item)
        {
            throw new System.NotImplementedException();
        }

        public override void OnMouseHover(string name)
        {
            OnSwitch(name == "Start Story Coord" ? 1 : 0);
        }

        public override void OnMouseHover(GameObject item)
        {
            throw new System.NotImplementedException();
        }

        public override void OnSwitch(int index)
        {
            SelectedIndex = index;
            if (_isCordPulled) return;
            
            if (SelectedIndex == ClickedIndex && ClickedIndex != 0) return;

            if (index == 1) _coord.transform.DOPlay();
            else _coord.transform.DOPause();
        }

        public override void OnSwitchClick(int index)
        {
            ClickedIndex = index;
            if (_isCordPulled) return;
            
            if (index == 1)
            {
                _isCordPulled = true;
                _coord.transform.DOPause();
                _coord.transform.DOMoveY(1100, 1).SetEase(Ease.InBack);
            }
        }
    }
}
