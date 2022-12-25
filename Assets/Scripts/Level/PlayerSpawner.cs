using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Vector2[] _spawnPoints;

    private int _spawnIndex = 0;

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            foreach(ulong clientId in NetworkManager.ConnectedClientsIds)
                SpawnPlayer(clientId);
        }
        
        NetworkManager.OnClientConnectedCallback += OnClientConnectedCallback;
    }

    private void OnClientConnectedCallback(ulong clientId)
    {
        if (IsServer)
            SpawnPlayer(clientId);
    }

    private void SpawnPlayer(ulong clientId)
    {
        GameObject player = Instantiate(_playerPrefab, _spawnPoints[_spawnIndex], Quaternion.identity);
        player.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);

        _spawnIndex = (_spawnIndex + 1) % _spawnPoints.Length;
    }
}
