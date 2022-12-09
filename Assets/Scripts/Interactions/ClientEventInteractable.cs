using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;

public class ClientEventInteractable : NetworkBehaviour, IInteractable
{
    [SerializeField] private UnityEvent onInteract;

    public void Interact()
    {
        PerformEventClientRpc();
    }

    [ClientRpc]
    private void PerformEventClientRpc()
    {
        onInteract.Invoke();
    }
}
