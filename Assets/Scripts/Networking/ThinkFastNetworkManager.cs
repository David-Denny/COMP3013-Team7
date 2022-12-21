using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NetworkManager))]
[RequireComponent(typeof(UnityTransport))]
public class ThinkFastNetworkManager : MonoBehaviour
{
    [SerializeField] private GameObject _lobbyCanvas, _feedbackText;

    private NetworkManager _networkManager;
    private UnityTransport _transport;

    private string _inputFieldContent = "";
    private bool _isConnected = false;

    private void Awake()
    {
        _networkManager = GetComponent<NetworkManager>();
        _transport = GetComponent<UnityTransport>();
        _networkManager.OnServerStarted += OnHostStarted;
        _networkManager.OnClientConnectedCallback += OnClientConnected;
        _networkManager.OnClientDisconnectCallback += OnClientDisconnected;
    }

    public void SetIP(string ip) 
    {
        _transport.ConnectionData.Address = ip;
        _transport.ConnectionData.Port = 25566;
    }

    private void ShowError(string message)
    {
        _feedbackText.GetComponent<Text>().text = message;
        _feedbackText.SetActive(true);
    }

    public void SetInputFieldContent(string content)
    {
        _inputFieldContent = content;
    } 

    public void OnHostButtonClicked()
    {
        SetIP("0.0.0.0");
        if (!_networkManager.StartHost())
            ShowError("Error starting host");
    }

    public void OnJoinButtonClicked()
    {
        if (_inputFieldContent.Length > 0)
        {
            if (IPAddress.TryParse(_inputFieldContent, out IPAddress address))
            {
                Debug.Log(address);
                SetIP(address.ToString());   
            }
            else
            {
                ShowError("Invalid IP Address");
                return;
            }
        }
        else
        {
            SetIP("127.0.0.1");
        }
        Debug.Log(_networkManager.StartClient());
    }
    
    private void OnHostStarted()
    {
        Debug.Log("Launched Host - " + _transport.ConnectionData.ServerEndPoint);
        _lobbyCanvas.SetActive(false);
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
        if (!_isConnected)
        {
            ShowError("Failed to connect to Server");           
        }
        // todo Handle the host leaving the game for whatever reason
        Debug.Log("Host died");
    }

    private void OnClientConnected(ulong obj)
    {
        if (_networkManager.IsConnectedClient)
        {
            _isConnected = true;
            _lobbyCanvas.SetActive(false);
        }

        if (_networkManager.IsHost && _networkManager.ConnectedClients.Count == 2)
        {
            Debug.Log("START GAME");
            // todo start game   
        }
    }
}
