using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController2D : NetworkBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private LayerMask groundMask;

    private InputMap inputMap;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private bool grounded = false;

    private NetworkVariable<float> moveDirection = new(0.0f, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    private NetworkVariable<Vector3> position = new();

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

    public override void OnNetworkSpawn()
    {
        GetComponentInChildren<SpriteRenderer>().color = IsOwner ? Color.blue : Color.red;

        if(IsOwner)
        {
            // Bind jump function to jump action
            inputMap.Player.Jump.performed += (InputAction.CallbackContext callback) => SubmitJumpRequestServerRPC();
        }
    }

    private void Update()
    {
        if(NetworkManager.Singleton.IsServer)
        {
            // Check if the player is grounded
            Collider2D result = Physics2D.OverlapBox(transform.position, new Vector2(boxCollider.size.x * 0.95f, 0.1f), 0.0f, groundMask);
            grounded = result != null;

            position.Value = transform.position;
        }
        
        if(IsOwner)
            moveDirection.Value = inputMap.Player.Move.ReadValue<float>();
        
        transform.position = position.Value;
    } 

    private void FixedUpdate()
    {
        if(NetworkManager.Singleton.IsServer)
        {
            // Update the player's horizontal velocity
            rb.velocity = new Vector2(moveDirection.Value * moveSpeed, rb.velocity.y);
        }
    }

    [ServerRpc]
    public void SubmitJumpRequestServerRPC(ServerRpcParams rpcParams = default)
    {
        // Set the player's vertical velocity
        if(grounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
    }
}
