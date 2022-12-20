using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Finish : NetworkBehaviour
{
    private readonly HashSet<ulong> _finishedPlayers = new();

    public void OnPlayerFinish(GameObject player)
    {
        NetworkObject playerObj = player.GetComponent<NetworkObject>();
        _finishedPlayers.Add(playerObj.OwnerClientId);
        playerObj.Despawn();
        if (_finishedPlayers.Count == NetworkManager.Singleton.ConnectedClientsIds.Count)
            LevelManager.Instance.GameOver(true);
    }
}
