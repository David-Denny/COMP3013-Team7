using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;

    private InputMap inputMap;
    private Rigidbody2D rb;

    private float moveDirection;

    private void Awake()
    {
        inputMap = new InputMap();
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        inputMap.Enable();
    }

    private void OnDisable()
    {
        inputMap.Disable();
    }

    private void Start()
    {
        // Bind jump function to jump action
        inputMap.Player.Jump.performed += (InputAction.CallbackContext callback) => Jump();
    }

    private void Update()
    {
        // Read the users current move input
        moveDirection = inputMap.Player.Move.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        // Update the player's horizontal velocity
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        // Set the player's vertical velocity
        rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }
}
