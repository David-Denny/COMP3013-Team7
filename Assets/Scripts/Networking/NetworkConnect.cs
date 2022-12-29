using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class to handle network connection setup
/// </summary>
public class NetworkConnect : MonoBehaviour
{
    [SerializeField] private InputField _ipInputField;
    [SerializeField] private Text _feedbackText;

    private UnityTransport _transport;

    private void Start()
    {
        // Setup default port
        _transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        _transport.ConnectionData.Port = 25566;

        ClearFeebackMessage();
    }

    public void OnHostButtonClicked()
    {
        // Start host
        NetworkManager.Singleton.StartHost();
        // Go to lobby
        NetworkManager.Singleton.SceneManager.LoadScene("LobbyScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void OnJoinButtonClicked()
    {
        ClearFeebackMessage();
        // Connect client if IP is valid
        if(UpdateIP())
            NetworkManager.Singleton.StartClient();
    }

    /// <summary>
    /// Read the IP inputed by the user
    /// </summary>
    /// <returns>Whether the IP was successfully parsed</returns>
    private bool UpdateIP()
    {
        // Validate entered IP
        if (_ipInputField.text.Length > 0)
        {
            if (IPAddress.TryParse(_ipInputField.text, out IPAddress address))
                _transport.ConnectionData.Address = address.ToString();
            else
            {
                // Report error to user
                SetFeedbackMessage("Invalid IP Address");
                return false;
            }
        }
        else
            // Default to local address
            _transport.ConnectionData.Address = "127.0.0.1";
        return true;
    }

    /// <summary>
    /// Display a feedback message to the user
    /// </summary>
    /// <param name="message">The message to display</param>
    private void SetFeedbackMessage(string message)
    {
        _feedbackText.text = message;
    }

    /// <summary>
    /// Clear the feedback message
    /// </summary>
    private void ClearFeebackMessage()
    {
        _feedbackText.text = "";
    }
}
