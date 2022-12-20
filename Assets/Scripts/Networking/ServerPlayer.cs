using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Server side manager of the player
/// </summary>
public class ServerPlayer : NetworkBehaviour
{
    private static int _spawnIndex = 0;
    private static readonly Vector3[] _spawnPoints = new Vector3[] { new(2.5f, 0.0f, 0.0f), new(2.5f, 7.0f, 0.0f) };

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Disable if we are not the server
        if(!IsServer)
        {
            enabled = false; 
            return;
        }

        // Get associated client
        var client = GetComponent<ClientPlayer>();

        // Set client spawn location
        var spawnPosition = NextSpawn();
        client.SetSpawnClientRpc(spawnPosition,
            new ClientRpcParams() { Send = new ClientRpcSendParams() { TargetClientIds = new[] { OwnerClientId } } });
    }

    /// <summary>
    /// Gets the next available spawn point
    /// </summary>
    /// <returns>Next spawn point</returns>
    private Vector3 NextSpawn()
    {
        // Get point
        Vector3 point = _spawnPoints[_spawnIndex];
        // Increment point
        _spawnIndex = (_spawnIndex + 1) % _spawnPoints.Length;
        return point;
    }
}
