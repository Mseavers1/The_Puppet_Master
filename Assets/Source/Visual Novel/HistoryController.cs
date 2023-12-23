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

        public void GenerateHistory()
        {
            foreach (var prefab in _previousPrefabs) Destroy(prefab);
            
            _previousPrefabs.Clear();
            var index = 0;
            var names = _gm.GetPreviousNames().ToArray();
            foreach (var text in _gm.GetPreviousText())
            {
                if (Camera.main is null) continue;
                
                var obj = Instantiate(previousPrefab, Camera.main.WorldToScreenPoint(new Vector3(0, -1.5f * index, 0)), Quaternion.identity);
                obj.name = "Previous Text";
                
                var objText = obj.transform.GetChild(0).GetComponent<TMP_Text>();
                objText.text = "";

                for (var i = 0; i < names[index].Length * 2; i++) objText.text += " ";
                
                obj.transform.GetChild(0).GetComponent<TMP_Text>().text += text;
                obj.transform.GetChild(1).GetComponent<TMP_Text>().text = names[index++] + ": ";
                obj.transform.SetParent(transform.GetChild(0));

                _previousPrefabs.Add(obj);
            }
            
            slider.maxValue = _previousPrefabs.Count;
            slider.value = slider.maxValue;
            
            SliderMovement();
        }

        public void SliderMovement()
        {
            var index = 0;
            foreach (var prefab in _previousPrefabs)
            {
                if (Camera.main is null) return;
                
                var origPos = Camera.main.WorldToScreenPoint(new Vector3(0, -1.5f * index, 0));

                var position = prefab.transform.position;
                position = new Vector3(position.x, origPos.y + (60 * slider.value), position.z);
                prefab.transform.position = position;
                index++;
            }
        }

        private void Awake()
        {
            _gm = GameObject.FindWithTag("GameManager").GetComponent<VisualGameManager>();
        }
    }
}
