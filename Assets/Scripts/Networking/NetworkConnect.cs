using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

public class NetworkConnect : MonoBehaviour
{
    [SerializeField] private InputField _ipInputField;
    [SerializeField] private Text _feedbackText;

    private UnityTransport _transport;

    private void Start()
    {
        _transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        _transport.ConnectionData.Port = 25566;

        ClearFeebackMessage();
    }

    public void OnHostButtonClicked()
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene("LobbyScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void OnJoinButtonClicked()
    {
        ClearFeebackMessage();
        if(UpdateIP())
            NetworkManager.Singleton.StartClient();
    }

    private bool UpdateIP()
    {
        if (_ipInputField.text.Length > 0)
        {
            if (IPAddress.TryParse(_ipInputField.text, out IPAddress address))
                _transport.ConnectionData.Address = address.ToString();
            else
            {
                SetFeedbackMessage("Invalid IP Address");
                return false;
            }
        }
        else
            _transport.ConnectionData.Address = "127.0.0.1";
        return true;
    }

    private void SetFeedbackMessage(string message)
    {
        _feedbackText.text = message;
    }

    private void ClearFeebackMessage()
    {
        _feedbackText.text = "";
    }
}
