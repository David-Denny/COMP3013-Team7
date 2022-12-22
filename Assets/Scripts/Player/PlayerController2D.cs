using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class for controlling the player's movement
/// </summary>
public class PlayerController2D : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private LayerMask _groundMask;

    private InputMap _inputMap;
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _collider;

    private bool _grounded = false;

    /// <summary>
    /// Is the player moving horizontally
    /// </summary>
    public bool Moving {  get { return _rigidbody.velocity.x != 0.0f; } }
    /// <summary>
    /// Is the player on the ground
    /// </summary>
    public bool Grounded { get { return _grounded; } }
    /// <summary>
    /// The horizontal direction of the player
    /// </summary>
    public float MoveDirection { get { return _rigidbody.velocity.x; } }

    private void Awake()
    {
        // Get required components
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        // Create input map
        _inputMap = new InputMap();
        _inputMap.Enable();

        // Bind jump function to jump action
        _inputMap.Player.Jump.performed += (InputAction.CallbackContext callback) => Jump();
    }

    private void Update()
    {
        // Check if the player is grounded
        Collider2D result = Physics2D.OverlapBox(transform.position, new Vector2(_collider.size.x * 0.95f, 0.1f), 0.0f, _groundMask);
        _grounded = result != null;

        // Update the player's horizontal velocity
        var moveDirection = _inputMap.Player.Move.ReadValue<float>();
        _rigidbody.velocity = new Vector2(moveDirection * _moveSpeed, _rigidbody.velocity.y);
    } 

    /// <summary>
    /// Initiate a jump
    /// </summary>
    private void Jump()
    {
        // Set the player's vertical velocity
        if(_grounded)
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpSpeed);
    }
}
