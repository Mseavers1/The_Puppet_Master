using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Visual_Novel
{
    public class HistoryController : MonoBehaviour
    {
        public GameObject previousPrefab;
        public Slider slider;

        private VisualGameManager _gm;
        private readonly List<GameObject> _previousPrefabs = new ();
        private const int CharacterSpaces = 65;
        private float _previousTextYPos;
        public float _lastYPos;

        public void GenerateHistory()
        {
            foreach (var prefab in _previousPrefabs) Destroy(prefab);
            
            _previousPrefabs.Clear();
            var names = _gm.GetPreviousNames().ToArray();
            var previousText = _gm.GetPreviousText().ToArray();

            for (var index = 0; index < previousText.Length; index++)
            {
                if (Camera.main is null) continue;

                var prev = "";
                if (index != 0) prev = previousText[index - 1];
                
                var obj = Instantiate(previousPrefab, new Vector3(1010, CalYPosition(prev), 0), Quaternion.identity);
                obj.name = "Previous Text";
                
                var objText = obj.transform.GetChild(0).GetComponent<TMP_Text>();
                objText.text = "";

                for (var i = 0; i < (names[index].Length + 2) * 1.73f; i++) objText.text += " ";
                
                obj.transform.GetChild(0).GetComponent<TMP_Text>().text += previousText[index];
                obj.transform.GetChild(1).GetComponent<TMP_Text>().text = names[index] + ": ";
                obj.transform.SetParent(transform.GetChild(0).GetChild(0));

                _previousPrefabs.Add(obj);
                _lastYPos = obj.transform.position.y;
            }
            
            slider.value = slider.minValue;

            SliderMovement();
        }

        public void SliderMovement()
        {
            var yPos = Camera.main.WorldToScreenPoint(new Vector2(0, 0));
            transform.GetChild(0).GetChild(0).position = new Vector3(transform.GetChild(0).GetChild(0).position.x, yPos.y, 0);
        }

        private float CalcSliderPos()
        {
            var slope = 0 - (_lastYPos);
            return slope * slider.value;
        }

        private float CalYPosition(string prev)
        {
            float yPos;
            
            // Check if it is the starting point
            if (prev.Length == 0) yPos = 500;
            
            // Calculate if starting point is already complete
            else yPos = _previousTextYPos - CalYDifference(prev.Length);
            
            _previousTextYPos = yPos;
            return yPos;
        }

        private float CalYDifference(int prevCharCount)
        {
            return prevCharCount switch
            {
                <= CharacterSpaces => 100,
                <= CharacterSpaces * 2 => 180,
                _ => 250
            };
        }

        private void Awake()
        {
            _gm = GameObject.FindWithTag("GameManager").GetComponent<VisualGameManager>();
        }
    }
}
