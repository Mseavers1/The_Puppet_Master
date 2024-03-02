using System.Collections.Generic;
using System.Linq;
using Source.Story_Shop;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Debug = System.Diagnostics.Debug;

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
                    hit.transform.GetComponent<MapSpriteSelector>().type = -1;
                    hit.transform.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.4f, 0.9f);
                    _mapMode = false;
                    Invoke(nameof(TurnOffMap), 1.5f);
                    curtains.StartCurtainAnimation(1.5f);
                    Invoke(nameof(StartEvent), 1.6f);
                    
                }
            }
        }

        private void StartEvent()
        {
            GetComponent<GameGm>().LoadEvent(0);
        }
        
        public void TurnOnMap()
        {
            levelGeneration.gameObject.SetActive(true);
            _mapMode = true;
        }
        
        private void TurnOffMap()
        {
            levelGeneration.gameObject.SetActive(false);
        }
    }
}
