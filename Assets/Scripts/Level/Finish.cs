using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Class <c>Finish</c> controls the finish line
/// </summary>
public class Finish : NetworkBehaviour
{
    private readonly HashSet<ulong> _finishedPlayers = new();

    /// <summary>
    /// Register a player that has touched the finish line.
    /// Player is despawned.
    /// Triggers game finish when all players are registered.
    /// </summary>
    /// <param name="player">The player that touched</param>
    public void OnPlayerFinish(GameObject player)
    {
        // Get player network information
        NetworkObject playerObj = player.GetComponent<NetworkObject>();
        // Register that the player has finished
        _finishedPlayers.Add(playerObj.OwnerClientId);
        // Despawn the player
        playerObj.Despawn();
        // Initiate game finished if all players have registered
        if (_finishedPlayers.Count == NetworkManager.Singleton.ConnectedClientsIds.Count)
            LevelManager.Instance.GameOver(true);
    }
}
