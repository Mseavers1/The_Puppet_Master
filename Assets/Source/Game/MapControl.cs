using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Source.Story_Shop;
using Source.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Source.Game
{
    public class MapControl : MonoBehaviour
    {
        public Vector2 clampVertical, clampHorizontal;
        public Vector2 verticalLimit, horizontalLimit;
        public float speed;
        public LevelGeneration levelGeneration;
        public CurtainCommands curtains;
        
        private PlayerInput _playerInput;
        private bool _mapMode = true;
        

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _playerInput.onActionTriggered += OnClick;
        }

        private void Update()
        {
            if (curtains.isAnimating || !_mapMode) return;

            var mouse = Mouse.current.position.ReadValue();
            var dirX = 0;
            var dirY = 0;

            if (mouse.x > horizontalLimit.y)
            {
                dirX = 1;
            } 
            else if (mouse.x < horizontalLimit.x)
            {
                dirX = -1;
            }

            if (mouse.y > verticalLimit.y)
            {
                dirY = 1;
            } 
            else if (mouse.y < verticalLimit.x)
            {
                dirY = -1;
            }
            
            Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x + (speed * dirX) * Time.deltaTime, clampHorizontal.x, clampHorizontal.y), Mathf.Clamp(Camera.main.transform.position.y + (speed * dirY) * Time.deltaTime, clampVertical.x, clampVertical.y), -10);

        }

        private void OnClick(InputAction.CallbackContext context)
        {
            if (context.action.name.Equals("Left Click") && context.performed) LeftClick();
        }

        private void LeftClick()
        {
            if (curtains.isAnimating || !_mapMode) return;
            
            var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()),Vector2.zero);
            
            if (hit)
            {
                var coordsStr = hit.transform.name.Split(' ');
                var coords = new Vector2(float.Parse(coordsStr[0]), float.Parse(coordsStr[1]));

                var rooms = levelGeneration.roomObjs;
                var found = false;

                var selectedRoom = rooms.FirstOrDefault(room => room.name == hit.transform.name);
                if (selectedRoom.GetComponent<MapSpriteSelector>().type == 8 || selectedRoom.GetComponent<MapSpriteSelector>().type == -1) return;

                foreach (var room in rooms)
                {
                    // Check top
                    if (room.name == (coords.x) + " " + (coords.y + 1) && selectedRoom.GetComponent<MapSpriteSelector>().up)
                    {
                        if (room.GetComponent<MapSpriteSelector>().type is -1 or 8)
                        {
                            found = true;
                            break;
                        }
                    }
                    
                    // Check Bot
                    if (room.name == (coords.x) + " " + (coords.y - 1) && selectedRoom.GetComponent<MapSpriteSelector>().down)
                    {
                        if (room.GetComponent<MapSpriteSelector>().type is -1 or 8)
                        {
                            found = true;
                            break;
                        }
                    }
                    
                    // Check left
                    if (room.name == (coords.x - 1) + " " + (coords.y) && selectedRoom.GetComponent<MapSpriteSelector>().left)
                    {
                        if (room.GetComponent<MapSpriteSelector>().type is -1 or 8)
                        {
                            found = true;
                            break;
                        }
                    }
                    
                    // Check right
                    if (room.name == (coords.x + 1) + " " + (coords.y) && selectedRoom.GetComponent<MapSpriteSelector>().right)
                    {
                        if (room.GetComponent<MapSpriteSelector>().type is -1 or 8)
                        {
                            found = true;
                            break;
                        }
                    }
                }
                
                if (found)
                {
                    var originalType = hit.transform.GetComponent<MapSpriteSelector>().type;
                    hit.transform.GetComponent<MapSpriteSelector>().type = -1;
                    hit.transform.GetComponent<SpriteRenderer>().color = new Color(0f, 0.46f, 0.27f);
                    _mapMode = false;
                    Invoke(nameof(TurnOffMap), 1.5f);
                    curtains.StartCurtainAnimation(1.5f);
                    StartCoroutine(StartEvent(originalType));

                }
            }
        }

        private bool CompletedAllRooms()
        {
            return levelGeneration.GetAllRooms().Cast<Room>().Where(room => room != null).All(room => room.Type is -1 or 8);
        }

        public float GetRoomSpPerRoom()
        {
            var completed = levelGeneration.GetAllRooms().Cast<Room>().Where(room => room != null).Count(room => room.Type is -1 or 8);

            return 50 * completed;
        }

        private IEnumerator StartEvent(int roomType)
        {
            yield return 1.6f;

            switch (roomType)
            {
                case 0:
                    GetComponent<GameGm>().LoadNormalEvent(GetNormalGameEvents());
                    break;
                case 1:
                    
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    GetComponent<GameGm>().LoadLootEvent(GetLootGameEvents());
                    break;
                case 6:
                    break;
                case 7:
                    break;
            }
        }
        
        public LootGameEvents GetLootGameEvents()
        {
            var txt = (TextAsset)Resources.Load("LootGameEvents", typeof(TextAsset));
            var events = JsonConvert.DeserializeObject<List<LootGameEvents>>(txt.text) ?? throw new Exception("Empty Json!");

            for (var i = 0; i < events.Count; i++)
            {
                var random = Random.Range(0, events.Count - i);
                
                var randomChance = Random.Range(0f, 100f);
                
                if (randomChance > events[random].Chance) continue;

                return events[random];
            }

            return events[0];
        }
        
        public NormalGameEvents GetNormalGameEvents()
        {
            var txt = (TextAsset)Resources.Load("NormalGameEvents", typeof(TextAsset));
            var events = JsonConvert.DeserializeObject<List<NormalGameEvents>>(txt.text) ?? throw new Exception("Empty Json!");

            for (var i = 0; i < events.Count; i++)
            {
                var random = Random.Range(0, events.Count - i);
                
                var randomChance = Random.Range(0f, 100f);
                
                if (randomChance > events[random].Chance) continue;

                return events[random];
            }

            return events[0];
        }

        private int GetIDFromName(string n)
        {
            // 0: normal, 1: story, 2: mini-boss, 3: secret, 4: trap, 5: loot, 6: special, 7: secret boss, 8: start
            return n switch
            {
                "Normal" => 0,
                "Story" => 1,
                "Mini-Boss" => 2,
                "Secret" => 3,
                "Trap" => 4,
                "Loot" => 5,
                "Special" => 6,
                "Secret Boss" => 7,
                _ => throw new Exception("The room typo of " + n + " does not exist!")
            };
        }
        
        private string GetNameFromID(int n)
        {
            // 0: normal, 1: story, 2: mini-boss, 3: secret, 4: trap, 5: loot, 6: special, 7: secret boss, 8: start
            return n switch
            {
                0 => "Normal",
                1 => "Story",
                2 => "Mini-Boss",
                3 => "Secret",
                4 => "Trap",
                5 => "Loot",
                6 => "Special",
                7 => "Secret Boss",
                _ => throw new Exception("The room typo of " + n + " does not exist!")
            };
        }
        
        public void TurnOnMap()
        {
            levelGeneration.gameObject.SetActive(true);
            _mapMode = true;
            
            // Check if all rooms are completed
            if (!CompletedAllRooms()) return;

            GlobalResources.SoulEssences += GetComponent<GameGm>().currentGameSP + (GetRoomSpPerRoom() * 1.5f);
            SceneManager.LoadScene(2);
        }
        
        private void TurnOffMap()
        {
            levelGeneration.gameObject.SetActive(false);
        }
    }
}
