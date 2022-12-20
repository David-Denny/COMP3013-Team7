using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ServerPlayer : NetworkBehaviour
{
    private static int _spawnIndex = 0;
    private static Vector3[] _spawnPoints = new Vector3[] { new(2.5f, 0.0f, 0.0f), new(2.5f, 7.0f, 0.0f) };

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(!IsServer)
        {
            enabled = false; 
            return;
        }

        var client = GetComponent<ClientPlayer>();
        var spawnPosition = NextSpawn();
        client.SetSpawnClientRpc(spawnPosition,
            new ClientRpcParams() { Send = new ClientRpcSendParams() { TargetClientIds = new[] { OwnerClientId } } });
    }

    private Vector3 NextSpawn()
    {
        Vector3 point = _spawnPoints[_spawnIndex];
        _spawnIndex = (_spawnIndex + 1) % _spawnPoints.Length;
        return point;
    }
}
