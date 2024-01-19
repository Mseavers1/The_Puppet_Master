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

        private const double StartingCost = 10;
        private double _currentCost, _currentRefund;
        private int _boughtPoints;

        private readonly int[] _sizes;
        private int _currentIndexOfSize;

        private readonly StatLogic _statLogic;
        private readonly StatTransformer _statTransformer;
        private readonly TMP_Text _cost, _refund;
        private EquationMaker _costEquation;

        public StatInputLogic(GameObject[] buttons, StatLogic statLogic, StatTransformer statTransformer, TMP_Text cost, TMP_Text refund) : base("Group B")
        {
            _buttons = buttons;
            _statLogic = statLogic;
            _statTransformer = statTransformer;
            _cost = cost;
            _refund = refund;
            _costEquation = new EquationMaker("Linear", new Vector2(0, 1), new Vector2(300, 301)); // x -> points, y -> cost
            _sizes = new[] {1, 5, 10, -1};

            _currentCost = StartingCost;
            _boughtPoints = 0;
            _currentRefund = 0;

            UpdateCostAndRefund();
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
            var length = _sizes[_currentIndexOfSize];
            var current = SoulGmSettings.GetStatPoints(_statLogic.ClickedIndex);
            
            switch (item.name)
            {
                case "Add":
                    const int max = SoulGmSettings.MAXValuePerStat;
                    
                    if (SoulGmSettings.GetStatPoints(_statLogic.ClickedIndex) >= max || _currentCost > GlobalResources.SoulEssences || _boughtPoints > SoulGmSettings.MAXBuyablePoints) return;
                    
                    if (_currentIndexOfSize != _sizes.Length - 1)
                    {
                        if (current + length > max) length = max - current;
                        
                        _boughtPoints += length;
                        _statTransformer.IncrementStat(_statLogic.GetCurrentIndexName(), length);
                        SoulGmSettings.AddStatPoints(_statLogic.ClickedIndex, length);
                        GlobalResources.SoulEssences -= _currentCost;
                    }
                    break;
                case "Remove":
                    if (SoulGmSettings.GetStatPoints(_statLogic.ClickedIndex) <= 0) return;
                    
                    if (_currentIndexOfSize != _sizes.Length - 1)
                    {
                        if (current - length < 0) length = current;
                        
                        _boughtPoints -= length;
                        _statTransformer.ReduceStat(_statLogic.GetCurrentIndexName(), length);
                        SoulGmSettings.SubtractStatPoints(_statLogic.ClickedIndex, length);
                        GlobalResources.SoulEssences += _currentRefund;
                    }
                    break;
                case "Quantity":
                    _currentIndexOfSize = (_currentIndexOfSize + 1) % _sizes.Length;
                    length = _sizes[_currentIndexOfSize];
                    UpdateQuantityDisplay(item);
                    break;
                default: throw new Exception("Could not find an item with the name of " + item.name);
            }
            
            UpdateCostAndRefund();
            GameObject.FindWithTag("GameManager").GetComponent<Soul_GM>().UpdateSPText();
        }

        public void UpdateCostAndRefund()
        {
            const int max = SoulGmSettings.MAXValuePerStat;
            var points = _boughtPoints;
            var length = _sizes[_currentIndexOfSize];
            var current = SoulGmSettings.GetStatPoints(_statLogic.ClickedIndex);
            
            if (current + length > max) length = max - current;
            
            // Logic for every multiplier other than max
            if (_currentIndexOfSize != _sizes.Length - 1)
            {
                _currentCost = 0;
                for (var i = 1; i <= length; i++)
                    _currentCost += Mathf.Floor((float) _costEquation.CalcValue(points + i - 1));
                
                _cost.text = "-" + _currentCost + " SP";

                
                // Refund
                length = _sizes[_currentIndexOfSize];
                if (current - length < 0) length = current;
                
                _currentRefund = 0;
                for (var i = 1; i <= length; i++)
                    _currentRefund += Mathf.Floor((float) _costEquation.CalcValue(points - i) / 2);

                if (_currentRefund < 0) _currentRefund = 0;
                
                _refund.text = "+" + _currentRefund + " SP";
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
