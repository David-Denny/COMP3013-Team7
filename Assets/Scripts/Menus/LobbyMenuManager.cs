using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenuManager : MonoBehaviour
{
    [SerializeField] private Button _startButton;

    private void Start()
    {
        if (!NetworkManager.Singleton.IsHost)
            _startButton.gameObject.SetActive(false);
        else
        {
            _startButton.gameObject.SetActive(true);
            _startButton.interactable = false;
            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        }
    }

    private void OnClientConnectedCallback(ulong obj)
    {
        if(NetworkManager.Singleton.ConnectedClients.Count == 2)
            _startButton.interactable = true;
    }

    public void StartGame()
    {
        NetworkManager.Singleton.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
