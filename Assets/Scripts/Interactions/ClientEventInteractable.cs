using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

/// <summary>
/// Class <c>ClientEventInteractable</c> is an interactable object.
/// An event is invoked on each client when interacted with.
/// </summary>
public class ClientEventInteractable : NetworkBehaviour, IInteractable
{
    [SerializeField] private UnityEvent onInteract;

    public void Interact()
    {
        PerformEventClientRpc();
    }

    /// <summary>
    /// <c>PerformEventClientRpc</c> invokes the associated on each client.
    /// </summary>
    [ClientRpc]
    private void PerformEventClientRpc()
    {
        onInteract.Invoke();
    }
}
