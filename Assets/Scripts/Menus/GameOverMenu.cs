using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : NetworkBehaviour
{
    [SerializeField] private GameObject _menuPage;
    [SerializeField] private GameObject _menuButton;
    [SerializeField] private GameObject _restartButton;

    public override void OnNetworkSpawn()
    {
        _menuButton.SetActive(IsHost);
        _restartButton.SetActive(IsHost);

        _menuPage.SetActive(false);
    }

    [ClientRpc]
    public void ShowClientRpc()
    {
        _menuPage.SetActive(true);
    }

    [ServerRpc]
    public void RestartServerRpc()
    {
        NetworkManager.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

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
            NetworkManager.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);
            SceneManager.LoadScene("MainMenuScene");
        }
    }

    [ClientRpc]
    private void DisconnectClientsClientRpc()
    {
        if(!IsServer)
        {
            NetworkManager.Shutdown();
            SceneManager.LoadScene("MainMenuScene");
        }
    }
}
