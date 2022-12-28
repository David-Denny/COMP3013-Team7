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
            NetworkManager.SceneManager.OnLoadEventCompleted += OnLoadEventCompleted;
            NetworkManager.OnClientConnectedCallback +=OnClientConnectedCallback;
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
            foreach(var clientId in NetworkManager.ConnectedClientsIds)
                SpawnPlayer(clientId);

            NetworkManager.SceneManager.OnLoadEventCompleted -= OnLoadEventCompleted;
        }
    }

    private void SpawnPlayer(ulong clientId)
    {
        GameObject player = Instantiate(_playerPrefab, _spawnPoints[_spawnIndex], Quaternion.identity);
        player.GetComponent<NetworkObject>().SpawnWithOwnership(clientId, true);

        _spawnIndex = (_spawnIndex + 1) % _spawnPoints.Length;
    }
}
