using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class to manage the game over menu
/// </summary>
public class GameOverMenu : NetworkBehaviour
{
    [SerializeField] private GameObject _menuPage;
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _menuButton;
    [SerializeField] private GameObject _restartButton;

    public override void OnNetworkSpawn()
    {
        // Only show buttons on host
        _menuButton.SetActive(IsHost);
        _restartButton.SetActive(IsHost);

        // Hide page on start
        _menuPage.SetActive(false);
    }

    /// <summary>
    /// Show the game over page on each client
    /// </summary>
    [ClientRpc]
    public void ShowClientRpc()
    {
        _scoreText.text = "Score: " + ScoreManager.Instance.Score.ToString();
        _menuPage.SetActive(true);
    }

    /// <summary>
    /// Restart the level
    /// </summary>
    [ServerRpc]
    public void RestartServerRpc()
    {
        NetworkManager.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    /// <summary>
    /// Return all clients to the main menu
    /// </summary>
    [ServerRpc]
    public void MainMenuServerRpc()
    {
        // Disconnect server when all other clients have disconnected
        NetworkManager.OnClientDisconnectCallback += OnClientDisconnected;

        // Disconnect clients
        DisconnectClientsClientRpc();
    }

    private void OnClientDisconnected(ulong obj)
    {
        if(IsServer)
        {
            // End connection
            NetworkManager.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);
            // Load main menu
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    /// <summary>
    /// Disconnect all clients
    /// </summary>
    [ClientRpc]
    private void DisconnectClientsClientRpc()
    {
        if(!IsServer)
        {
            // Disconnect client
            NetworkManager.Shutdown();
            // Load main menu
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
