using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;
using System;

/// <summary>
/// Class for handling the player interacting with objects
/// </summary>
public class PlayerInteract : NetworkBehaviour
{
    [SerializeField] private Vector2 interactionOffset;
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask interactMask;

    private InputMap inputMap;

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            // Create input map
            inputMap = new InputMap();
            inputMap.Enable();

            // Bind to interact action
            inputMap.Player.Interact.performed += (InputAction.CallbackContext callback) => TryInteractServerRPC();
        }
    }

    /// <summary>
    /// Try to find an interactable in range, interact with it if there is
    /// </summary>
    [ServerRpc]
    private void TryInteractServerRPC()
    {
        // Query physics for overlapping objects in the interaction layer
        ContactFilter2D contactFilter = new()
        {
            useTriggers = true,
            useLayerMask = true,
            layerMask = interactMask
        };

        Collider2D[] results = new Collider2D[1];
        Vector2 center = (Vector2)transform.position + interactionOffset;
        int resultCount = Physics2D.OverlapCircle(center, interactionRadius, contactFilter, results);

        // If we found an object to interact with
        if(resultCount > 0)
        {
            // Try to interact with the object
            if (results[0].TryGetComponent(out IInteractable interactable))
                interactable.Interact();
            else
                Debug.LogWarning("Objects in layer \"Interactable\" should have an interactable component");
        }
    }

    private void OnDrawGizmos()
    {
        // Draw circle in editor to show interaction area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (Vector3)interactionOffset, interactionRadius);
    }
}
