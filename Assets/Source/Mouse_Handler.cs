using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Mouse_Handler : MonoBehaviour
{
    public PlayerInput input;

    private PlayerControls controls;

    private void Start()
    {
        controls = GetComponent<Player_Movement>().GetControls();
        input.onActionTriggered += OnClick;
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (context.action.name.Equals("Left Click") && context.performed) 
        {
            var pos = Mouse.current.position.ReadValue();
            var worldPos = Camera.main.ScreenToWorldPoint(pos);

            Collider2D detectedCollider = Physics2D.OverlapPoint(worldPos, 1<<6);

            if (detectedCollider != null)
            {
                Debug.Log(detectedCollider.name);

                if (detectedCollider.tag == "NPC")
                {
                    detectedCollider.GetComponent<Chatbox_Handler>().StartChat();
                }
            }

        }
    }

}
