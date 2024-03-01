using System;
using System.Globalization;
using DG.Tweening;
using Source.Soul_Shop;
using Source.Utility;
using TMPro;
using UnityEngine;
using static System.Int64;

namespace Source.Story_Shop
{
    public class PlaceholderLogic : MouseInteraction
    {
        public Transform selectedItem;

        private DisplaySkills _displaySkills;
        private GameObject _blankCard;
        private float _blankDefaultX;
        
        public PlaceholderLogic(DisplaySkills displaySkills, GameObject blankCard) : base("Group D")
        {
            _displaySkills = displaySkills;
            _blankCard = blankCard;
            _blankDefaultX = blankCard.transform.position.x;
        }

        public override void OnLeftClick(string name)
        {
            
        }

        public float GetDefaultX()
        {
            return _blankDefaultX;
        }

        public override void OnLeftClick(GameObject item)
        {
            var parent = item.transform.parent;
            var name = parent.GetChild(3).GetComponent<TMP_Text>().text;
            var level = int.Parse(parent.GetChild(4).GetComponent<TMP_Text>().text.Split(' ')[1]);
            
            switch (item.name)
            {
                case "But A": // --
                    if (level <= 0) break;
                    var gain = HoldingOfSkills.LoadData(parent.GetChild(3).GetComponent<TMP_Text>().text).Costs[level - 1] / 2;
                    level--;

                    parent.GetChild(4).GetComponent<TMP_Text>().text = "lv. " + level;
                    GlobalResources.SoulEssences += gain;
                    GameObject.FindWithTag("GameManager").GetComponent<Soul_GM>().UpdateSPText();
                    
                    if (HoldingOfSkills.BoughtSkills.ContainsKey(name))
                        HoldingOfSkills.BoughtSkills[name] = level;
                    else
                        HoldingOfSkills.BoughtSkills.Add(name, level);
                    
                    break;
                case "But B": // Info
                    selectedItem = parent;
                    _displaySkills.HandleInfoBut();
                    
                    break;
                case "But C": // ++
                    
                    if (level >= 10) break;
                    var cost = HoldingOfSkills.LoadData(parent.GetChild(3).GetComponent<TMP_Text>().text).Costs[level];
                    if (GlobalResources.SoulEssences < cost) break;
                    
                    level++;
                    
                    parent.GetChild(4).GetComponent<TMP_Text>().text = "lv. " + level;
                    GlobalResources.SoulEssences -= cost;
                    GameObject.FindWithTag("GameManager").GetComponent<Soul_GM>().UpdateSPText();
                    
                    if (HoldingOfSkills.BoughtSkills.ContainsKey(name))
                        HoldingOfSkills.BoughtSkills[name] = level;
                    else
                        HoldingOfSkills.BoughtSkills.Add(name, level);
                    
                    break;
                default: throw new Exception(item.name + " is not in the placeholder logic.");
            }
        }

        public override void OnMouseHover(string name)
        {
            throw new System.NotImplementedException();
        }

        public override void OnMouseHover(GameObject item)
        {
            SelectedIndex = 1;
            var parent = item.transform.parent;
            var level = int.Parse(parent.GetChild(4).GetComponent<TMP_Text>().text.Split(' ')[1]);
            
            switch (item.name)
            {
                case "But A": // --
                    
                    if (level == 0)
                    {
                        item.transform.GetChild(0).GetComponent<TMP_Text>().text = "MIN";
                        break;
                    }
                    
                    var cost = HoldingOfSkills.LoadData(parent.GetChild(3).GetComponent<TMP_Text>().text).Costs[level - 1] / 2;
                    item.transform.GetChild(0).GetComponent<TMP_Text>().text = "+" + cost + " SP";
                    break;
                case "But B": // Info
                    break;
                case "But C": // ++

                    if (level >= 10)
                    {
                        item.transform.GetChild(0).GetComponent<TMP_Text>().text = "MAX";
                        break;
                    }
                    
                    item.transform.GetChild(0).GetComponent<TMP_Text>().text = "-" + HoldingOfSkills.LoadData(parent.GetChild(3).GetComponent<TMP_Text>().text).Costs[level] + " SP";
                    break;
                default: throw new Exception(item.name + " is not in the placeholder logic.");
            }
        }

        public override void OnSwitch(int index)
        {
            if (index == 0)
            {
                SelectedIndex = 0;
                foreach (var plus in _displaySkills.slotsPlus) plus.text = "lv. ++";
                foreach (var minus in _displaySkills.slotsMinus) minus.text = "lv. --";
            }
        }

        public override void OnSwitchClick(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}
