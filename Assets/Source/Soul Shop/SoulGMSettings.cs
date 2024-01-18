using System;
using System.Linq;
using Source.Utility;
using UnityEngine;

namespace Source.Soul_Shop
{
    public static class SoulGmSettings
    {
        public const int MAXValuePerStat = 150;
        public const int MAXBuyablePoints = 300;

        private static int[] _statPoints;

        private static readonly bool[] AffinitiesPositions; // False - Dont have ::: True - Have affinity
        private static double[] _affinityCost, _affinityGains, _defaultCosts;
        
        private const int MAXStatPointsBuyable = 750;
        private static int _currentStatPointsBuyable;
        private const float MultiplierForUpgrade = 1.5f, MultiplierForDowngrade = 0.8f;

        static SoulGmSettings()
        {
            _statPoints = new int[7];
            _currentStatPointsBuyable = 0;
            AffinitiesPositions = new bool[12];
            _defaultCosts = new double[]
            {
                10, 10, 10, 10, 10, 10,
                100, 100,
                500, // Null
                100, 100,
                250 // Spacetime
            };

            _affinityCost = new double[_defaultCosts.Length];
            _affinityGains = new double[_defaultCosts.Length];

            for (var i = 0; i < _defaultCosts.Length; i++)
            {
                _affinityCost[i] = _defaultCosts[i];
                _affinityGains[i] = _defaultCosts[i] / 2;
            }

            UpdateAffinityCosts();
        }

        public static int GetStatPoints(int index)
        {
            return _statPoints[index];
        }

        public static int AddStatPoints(int index, int value)
        {
            _statPoints[index] += value;

            return GetStatPoints(index);
        }
        
        public static int SubtractStatPoints(int index, int value)
        {
            _statPoints[index] -= value;
            
            return GetStatPoints(index);
        }
        
        public static double GetAffinityCost(int index)
        {
            return _affinityCost[index];
        }
        
        public static double GetAffinityGain(int index)
        {
            return _affinityGains[index];
        }

        public static void SetAffinityCost(int index, double value)
        {
            _affinityCost[index] = value;
        }

        public static bool GetAffinityPosition(int index)
        {
            return AffinitiesPositions[index];
        }

        public static void SetAffinityPosition(int index, bool value)
        {
            AffinitiesPositions[index] = value;
        }

        public static void FlipAffinityPosition(int index)
        {
            var pos = AffinitiesPositions[index];
            AffinitiesPositions[index] = !pos;

            if (pos)
            {
                GlobalResources.SoulEssences += GetAffinityGain(index);
            }
            else
            {
                GlobalResources.SoulEssences -= GetAffinityCost(index);
            }

            UpdateAffinityCosts();
        }

        public static bool IsBuyableMaxedOut()
        {
            return _currentStatPointsBuyable >= MAXStatPointsBuyable;
        }

        public static void BuyStat()
        {
            _currentStatPointsBuyable++;
        }
        
        public static void SellStat()
        {
            _currentStatPointsBuyable--;
        }

        public static int GetCurrentStatPoints()
        {
            return _currentStatPointsBuyable;
        }

        public static void UpdateAffinityCosts()
        {
            for (var i = 0; i < 12; i++)
            {
                _affinityCost[i] = Mathf.Floor((float) (_defaultCosts[i] + _defaultCosts[i] * MultiplierForUpgrade * GetNumberOfActiveAffinities()));
                _affinityGains[i] = Mathf.Floor((float) ((_defaultCosts[i] / 2) + (_defaultCosts[i] / 2) * MultiplierForDowngrade * GetNumberOfActiveAffinities()));
            }

            var gm = GameObject.FindWithTag("GameManager").GetComponent<Soul_GM>();
            gm.menuCanvases[gm.GetCanvasID("Affinity")].GetComponent<AffinitiesCanvas>().UpdateAffinityText();
        }

        private static int GetNumberOfActiveAffinities()
        {
            return AffinitiesPositions.Count(pos => pos);
        }
    }
}
