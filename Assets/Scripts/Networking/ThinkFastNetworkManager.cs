using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class ThinkFastNetworkManager : MonoBehaviour
{

    [SerializeField] private NetworkManager networkManager;
    private string _inputFieldContent = "";

    private static ThinkFastNetworkManager _thinkFastNetworkManager;
    private GameObject _lobbyCanvas, _feedbackText;
    private bool _isConnected = false;
    
    public UnityTransport transport;

    private void Awake()
    {
        transport = GetComponent<UnityTransport>();
        _lobbyCanvas = GameObject.Find("LobbyCanvas");
        _feedbackText = GameObject.Find("Feedback");
        networkManager.OnServerStarted += OnHostStarted;
        networkManager.OnClientConnectedCallback += OnClientConnected;
        networkManager.OnClientDisconnectCallback += OnClientDisconnected;
    }

    public void SetIP(string ip) 
    {
        transport.ConnectionData.Address = ip;
        transport.ConnectionData.Port = 7777;
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
        if (!networkManager.StartHost())
        {
            ShowError("Error starting host");
        };
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
        Debug.Log(networkManager.StartClient());
    }
    
    private void OnHostStarted()
    {
        Debug.Log("Launched Host - " + transport.ConnectionData.ServerEndPoint);
        _lobbyCanvas.SetActive(false);
    }
    
    private void OnClientDisconnected(ulong id)
    {
        if (networkManager.IsHost && networkManager.ConnectedClients.Count == 2)
        {
            Debug.Log("Client left the game");
            networkManager.Shutdown();
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
        if (networkManager.IsConnectedClient)
        {
            _isConnected = true;
            _lobbyCanvas.SetActive(false);
        }

        if (networkManager.IsHost && networkManager.ConnectedClients.Count == 2)
        {
            Debug.Log("START GAME");
            // todo start game   
        }
    }
}
