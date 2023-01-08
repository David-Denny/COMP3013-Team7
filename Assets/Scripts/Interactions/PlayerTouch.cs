using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class <c>PlayerTouch</c> invokes an event on the server when a player touches this object.
/// </summary>
public class PlayerTouch : NetworkBehaviour
{
    [SerializeField] private UnityEvent<GameObject> _onPlayerTouch;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(IsServer && collision.CompareTag("Player"))
            _onPlayerTouch.Invoke(collision.gameObject);
    }
}
