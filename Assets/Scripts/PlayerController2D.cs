using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private LayerMask groundMask;

    private InputMap inputMap;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private float moveDirection = 0.0f;
    private bool grounded = false;

    private void Awake()
    {
        inputMap = new InputMap();
        rb = GetComponent<Rigidbody2D>();
        boxCollider= GetComponent<BoxCollider2D>();
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

        // Check if the player is grounded
        Collider2D result = Physics2D.OverlapBox(transform.position, new Vector2(boxCollider.size.x * 0.95f, 0.1f), 0.0f, groundMask);
        grounded = result != null;
    }

    private void FixedUpdate()
    {
        // Update the player's horizontal velocity
        rb.velocity = new Vector2(moveDirection * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        // Set the player's vertical velocity
        if(grounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }
}
