using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Source.Utility;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Soul_Shop
{
    public class AffinityIconsLogic : MouseInteraction
    {
        private readonly Color[] _colors;
        private readonly Color _deactivatedColor;
        private GameObject[] _affinities;

        private readonly Dictionary<string, string[]> _requirements = new();

        public AffinityIconsLogic(Color[] colors, Color deactivatedColor, GameObject[] affinities) : base("Group A")
        {
            _colors = colors;
            _deactivatedColor = deactivatedColor;
            _affinities = affinities;

            // Affinity Requirements
            _requirements.Add("Lightning", new [] {"Fire", "Nature"});
            _requirements.Add("Ice", new [] {"Water", "Wind"});
            _requirements.Add("Null", new [] {"Spacetime", "Lightning", "Ice", "Summoning", "Recovery"});
            _requirements.Add("Spacetime", new [] {"Fire", "Water", "Dark", "Light", "Nature", "Wind"});
            _requirements.Add("Recovery", new [] {"Water", "Light"});
            _requirements.Add("Summoning", new [] {"Dark", "Nature", "Light"});
            
        }

        public override void OnLeftClick(string name)
        {
            throw new System.NotImplementedException();
        }

        public override void OnLeftClick(GameObject item)
        {
            var index = GetIndexFromName(item.name);
            var cost = SoulGmSettings.GetAffinityCost(index);

            // Check if requirements are met
            if (!CheckRequirements(item.name) || GlobalResources.SoulEssences < cost) return;
            
            SoulGmSettings.FlipAffinityPosition(index);
            GameObject.FindWithTag("GameManager").GetComponent<Soul_GM>().UpdateSPText();
            
            item.transform.GetChild(0).GetComponent<Image>().color = SoulGmSettings.GetAffinityPosition(index) ? _colors[index] : _deactivatedColor;
            
            UpdateRequirements(item.name);
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

        private void UpdateRequirements(string name)
        {
            if (GetIndexFromName(name) >= 6) return;

            foreach (var requirement in from requirements in _requirements from requirement in requirements.Value where requirement == name select requirements.Key)
            {
                if (!SoulGmSettings.GetAffinityPosition(GetIndexFromName(requirement))) continue;
                
                UpdateAffinities(requirement);
            }

            if (SoulGmSettings.GetAffinityPosition(GetIndexFromName("Null"))) UpdateAffinities("Null");
        }

        private void UpdateAffinities(string name)
        {
            SoulGmSettings.SetAffinityPosition(GetIndexFromName(name), false);
            GlobalResources.SoulEssences += SoulGmSettings.GetAffinityGain(GetIndexFromName(name));
            SoulGmSettings.UpdateAffinityCosts();
            GameObject.FindWithTag("GameManager").GetComponent<Soul_GM>().UpdateSPText();

            _affinities[GetIndexFromName(name)-6].transform.GetChild(0).GetComponent<Image>().color = _deactivatedColor;
        }

        private bool CheckRequirements(string selected)
        {
            if (GetIndexFromName(selected) < 6) return true;

            var requirements = _requirements[selected];

            return requirements.Select(GetIndexFromName).All(SoulGmSettings.GetAffinityPosition);
        }

        private int GetIndexFromName(string name)
        {
            return name switch
            {
                "Fire" => 0,
                "Water" => 1,
                "Wind" => 2,
                "Nature" => 3,
                "Light" => 4,
                "Dark" => 5,
                "Lightning" => 6,
                "Ice" => 7,
                "Null" => 8,
                "Summoning" => 9,
                "Recovery" => 10,
                "Spacetime" => 11,
                _ => throw new Exception("Unable to find the correct index from the name: " + name)
            };
        }
    }
}
