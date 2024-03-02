using DG.Tweening;
using Source.Utility;
using UnityEngine;

namespace Source.Game
{
    public class EndTurnLogic : MouseInteraction
    {
        private GameGm _gm;
        public EndTurnLogic(GameGm gm) : base("Group D")
        {
            _gm = gm;
        }

        public override void OnLeftClick(string name)
        {
            throw new System.NotImplementedException();
        }

        public override void OnLeftClick(GameObject item)
        {
            if (_gm.endSignLeaving) return;
            
            _gm.PlayerEndTurn();
        }

        public override void OnMouseHover(string name)
        {
            throw new System.NotImplementedException();
        }

        public override void OnMouseHover(GameObject item)
        {
            if (SelectedIndex == 1 || _gm.endSignLeaving) return;
            
            SelectedIndex = 1;
            item.transform.DOShakePosition(1, 10, 20, 20, false, false).SetLoops(-1).SetEase(Ease.InOutSine).timeScale = 0.2f;
        }

        public override void OnSwitch(int index)
        {
            if (index == SelectedIndex || _gm.endSignLeaving) return;

            SelectedIndex = index;
            
            if (index == 0)
            {
                _gm.endTurnSign.transform.DOKill();
            }
        }

        public override void OnSwitchClick(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}
