using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to manage the lobby scene
/// </summary>
public class LobbyMenuManager : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    private void Start()
    {
        // Hide start button for everyone but the host
        if (!NetworkManager.Singleton.IsHost)
            _startButton.gameObject.SetActive(false);
        else
        {
            // Show the start button for the host
            _startButton.gameObject.SetActive(true);
            _startButton.interactable = false;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        }
    }

    private void OnClientConnectedCallback(ulong obj)
    {
        // Enable the start but when at least 2 clients are connected
        if(NetworkManager.Singleton.ConnectedClients.Count == 2)
            _startButton.interactable = true;
    }

    public void StartGame()
    {
        // Load game scene
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
