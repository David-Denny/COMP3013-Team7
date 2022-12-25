using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NetworkManager))]
[RequireComponent(typeof(UnityTransport))]
public class ThinkFastNetworkManager : MonoBehaviour
{
    private NetworkManager _networkManager;
    private UnityTransport _transport;

    private void Awake()
    {
        _networkManager = GetComponent<NetworkManager>();
        _transport = GetComponent<UnityTransport>();
        _networkManager.OnServerStarted += OnHostStarted;
        _networkManager.OnClientConnectedCallback += OnClientConnected;
        _networkManager.OnClientDisconnectCallback += OnClientDisconnected;
    }
    
    private void OnHostStarted()
    {
        Debug.Log("Launched Host - " + _transport.ConnectionData.ServerEndPoint);
    }

    private void OnClientConnected(ulong obj)
    {
        if (_networkManager.IsHost && _networkManager.ConnectedClients.Count == 2)
        {
            Debug.Log("START GAME");
            // todo start game   
        }
    }
    
    private void OnClientDisconnected(ulong id)
    {
        if (_networkManager.IsHost && _networkManager.ConnectedClients.Count == 2)
        {
            Debug.Log("Client left the game");
            _networkManager.Shutdown();
            // todo Handle the client leaving the game
            return;
        }

        // todo Handle the host leaving the game for whatever reason
        Debug.Log("Host died");
    }
}
