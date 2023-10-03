using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    PlayerControls controls;
    InputAction move;
    Vector2 moveDirection;

    public float speed = 10;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        move = controls.Player.Move;
        move.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection = move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }
}
