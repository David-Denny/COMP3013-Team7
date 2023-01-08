using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public bool Moving {  get { return rb.velocity.x != 0.0f; } }
    public bool Grounded { get { return grounded; } }
    public float VelocityX { get { return rb.velocity.x; } }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider= GetComponent<BoxCollider2D>();
    }

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            // Create input map
            inputMap = new InputMap();
            inputMap.Enable();

            // Bind jump function to jump action
            inputMap.Player.Jump.performed += (InputAction.CallbackContext callback) => SubmitJumpRequestServerRPC();
        }

        // Remove rigidbody from client players
        if(!NetworkManager.Singleton.IsServer)
        {
            Destroy(rb);
            Destroy(boxCollider);
        }
    }

    private void Update()
    {
        if(NetworkManager.Singleton.IsServer)
        {
            // Check if the player is grounded
            Collider2D result = Physics2D.OverlapBox(transform.position, new Vector2(boxCollider.size.x * 0.95f, 0.1f), 0.0f, groundMask);
            grounded = result != null;
        }
        
        if(IsOwner)
            moveDirection.Value = inputMap.Player.Move.ReadValue<float>();
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
