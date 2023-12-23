using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Source.Visual_Novel
{
    public class VisualGameManager : MonoBehaviour
    {
        public TMP_Text textBox;
        public GameObject[] buttons;
        public Canvas canvas;
        public string textFile;
        public Color defaultTextColor, highlightedTextColor;

        [Range(0, 1)]
        public float textSpeed;

        [Range(0.001f, 3)]
        public float autoSpeed;
        public bool isSkimming, isAuto;

        private readonly Queue<string> _loadedTexts = new();
        private double _timer, _autoTimer;
        private float _savedTextSpeed, _savedAutoSpeed;
        private bool _isRunning, _isOverButton, _savedIsAuto;
        private int _currentIndex;

        private PlayerInput _input;
        private GraphicRaycaster _caster;
        private PointerEventData _pointerEventData;
        private List<RaycastResult> _results;
        private int _currentHoverButton;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _caster = canvas.GetComponent<GraphicRaycaster>();
            _pointerEventData = new PointerEventData(EventSystem.current);
            _results = new List<RaycastResult>();
        }

        private void Start()
        {
            _input.onActionTriggered += MouseHandler;
            
            LoadTextFile();
            PlayText();
        }

        private void Update()
        {
            // Checks if auto is enabled
            if (isAuto && !_isRunning)
            {
                _autoTimer += Time.deltaTime;
                
                if (_autoTimer >= autoSpeed) PlayText();
            }
            
            // Checks if the system is running
            if (!_isRunning) return;
            
            _timer += Time.deltaTime;

            if (_timer >= textSpeed) NextCharacter();
        }

        private void FixedUpdate()
        {
            var pos = Mouse.current.position.ReadValue();
            _pointerEventData.position = pos;
            _results.Clear();
            
            _caster.Raycast(_pointerEventData, _results);

            _isOverButton = false;
            foreach (var result in _results.Where(result => result.gameObject.CompareTag("Button")))
            {
                var obj = result.gameObject.name.Split(' ');
                SwitchHover(int.Parse(obj[1]));
                _isOverButton = true;
            }
            
            if (!_isOverButton) SwitchHover(-1);
        }

        private void SwitchHover(int id)
        {
            if (id == -1) _isOverButton = false;
            _currentHoverButton = id;
            
            for (var index = 0; index < buttons.Length; index++)
            {
                buttons[index].GetComponent<TMP_Text>().color = index == id ? highlightedTextColor : defaultTextColor;
            }
        }

        private void LoadTextFile()
        {
            try
            {
                _loadedTexts.Clear();

                var path = "Assets/Resources/VisualNovel Texts/" + textFile + ".txt";
                var reader = new StreamReader(path);
                var line = reader.ReadLine();

                while (line != null)
                {
                    _loadedTexts.Enqueue(line);
                    line = reader.ReadLine();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private void MouseHandler(InputAction.CallbackContext context)
        {
            if (context.action.name.Equals("Left Click") && context.performed) OnLeftClick();
        }

        private void OnLeftClick()
        {
            // Check if it is clicking a button
            if (_isOverButton)
            {
                switch (_currentHoverButton)
                {
                    case 0:
                        // History
                        break;
                    case 1:
                        // Auto
                        isAuto = !isAuto;
                        break;
                    case 2:
                        // Skim
                        isSkimming = !isSkimming;
                        SkimmingUpdate();
                        
                        break;
                    case 3:
                        // Skip
                        break;
                }
                return;
            }
            
            // Checks if text is running, if so, skips to the end.
            if (_isRunning)
            {
                textBox.text = _loadedTexts.Peek();
                EndText();
            }
            
            // Checks if no text is running, if so, moves to the next line
            else PlayText();
        }

        private void SkimmingUpdate()
        {
            if (isSkimming)
            {
                _savedTextSpeed = textSpeed;
                _savedAutoSpeed = autoSpeed;
                _savedIsAuto = isAuto;

                textSpeed = 0;
                autoSpeed = 0.001f;

                isAuto = true;
            }
            else
            {
                textSpeed = _savedTextSpeed;
                autoSpeed = _savedAutoSpeed;
                isAuto = _savedIsAuto;
            }
        }

        // Loads the next set of text into the system
        private void PlayText()
        {
            if (_loadedTexts.Count <= 0) return;
            
            _timer = 0;
            _autoTimer = 0;
            _currentIndex = 0;
            _isRunning = true;

            textBox.text = "";
            
            NextCharacter();
        }

        // Loads the current char of the loaded text into the text box
        private void NextCharacter()
        {
            if (textSpeed == 0)
            {
                textBox.text += _loadedTexts.Peek();
                EndText();
                return;
            }
            
            textBox.text += _loadedTexts.Peek()[_currentIndex];

            _timer = 0;
            _currentIndex++;
            
            if (_currentIndex >= _loadedTexts.Peek().Length) EndText();
        }

        // Ends the system
        private void EndText()
        {
            _isRunning = false;
            _loadedTexts.Dequeue();
        }
    }
}
