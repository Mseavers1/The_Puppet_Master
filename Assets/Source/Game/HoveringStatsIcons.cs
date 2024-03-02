using Source.Utility;
using UnityEngine;

namespace Source.Game
{
    public class HoveringStatsIcons : MouseInteraction
    {
        private GameObject _icons;
        private GameObject _enemyIcon;
        
        public HoveringStatsIcons(GameObject icons, GameObject enemyIcon) : base("Group E")
        {
            _icons = icons;
            _enemyIcon = enemyIcon;
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
            var index = item.name switch
            {
                "SP" => 1,
                "MP" => 2,
                "HP" => 3,
                "EHP" => 4,
                _ => 0
            };
            
            OnSwitch(index);
        }

        public override void OnSwitch(int index)
        {
            if (SelectedIndex == index) return;
            
            SelectedIndex = index;

            switch (index)
            {
                case 1:
                    _icons.transform.GetChild(1).GetChild(3).gameObject.SetActive(true);
                    _icons.transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
                    _icons.transform.GetChild(3).GetChild(3).gameObject.SetActive(false);
                    _enemyIcon.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                    break;
                case 2:
                    _icons.transform.GetChild(2).GetChild(3).gameObject.SetActive(true);
                    _icons.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
                    _icons.transform.GetChild(3).GetChild(3).gameObject.SetActive(false);
                    _enemyIcon.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                    break;
                case 3:
                    _icons.transform.GetChild(3).GetChild(3).gameObject.SetActive(true);
                    _icons.transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
                    _icons.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
                    _enemyIcon.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                    break;
                case 4:
                    _icons.transform.GetChild(3).GetChild(3).gameObject.SetActive(false);
                    _icons.transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
                    _icons.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
                    _enemyIcon.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                    break;
                default:
                    _icons.transform.GetChild(3).GetChild(3).gameObject.SetActive(false);
                    _icons.transform.GetChild(2).GetChild(3).gameObject.SetActive(false);
                    _icons.transform.GetChild(1).GetChild(3).gameObject.SetActive(false);
                    _enemyIcon.transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                    break;
            }
        }

        public override void OnSwitchClick(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}
