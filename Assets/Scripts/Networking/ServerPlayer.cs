using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class ServerPlayer : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if(!IsServer)
        {
            enabled = false; 
            return;
        }

        var client = GetComponent<ClientPlayer>();
        var spawnPosition = new Vector3(2, 2, 0);
        client.SetSpawnClientRpc(spawnPosition,
            new ClientRpcParams() { Send = new ClientRpcSendParams() { TargetClientIds = new[] { OwnerClientId } } });
    }
}
