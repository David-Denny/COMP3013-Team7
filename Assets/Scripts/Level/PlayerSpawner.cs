using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Class to spawn in network players
/// </summary>
public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Vector2[] _spawnPoints;

    private int _spawnIndex = 0;

    public override void OnNetworkSpawn()
    {
        // Listen to client changes
        if(IsServer)
        {
            NetworkManager.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
            NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;
        }
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        SpawnPlayer(clientId);
    }

    private void OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if(IsServer)
        {
            // Spawn a player for each client
            foreach(var clientId in NetworkManager.ConnectedClientsIds)
                SpawnPlayer(clientId);

            // Stop listening as players have been spawned
            NetworkManager.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        // Create player object
        GameObject player = Instantiate(_playerPrefab, _spawnPoints[_spawnIndex], Quaternion.identity);
        // Spawn player on network
        player.GetComponent<NetworkObject>().SpawnWithOwnership(clientId, true);
        // Go to next spawn point
        _spawnIndex = (_spawnIndex + 1) % _spawnPoints.Length;
    }
}
