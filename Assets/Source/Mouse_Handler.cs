using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mouse_Handler : MonoBehaviour
{
    public PlayerInput input;

    private PlayerControls controls;
    private InputAction select; 

    private void Start()
    {
        controls = GetComponent<Player_Movement>().GetControls();
        select = controls.Player.LeftClick;
        select.Enable();
        input.onActionTriggered += OnClick;
    }

    private void OnDisable()
    {
        select.Disable();
    }

    private void Update()
    {
        
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (context.action.name.Equals("Left Click") && context.performed) 
        {
            Debug.Log("You Clicked!");
        }
    }

}
