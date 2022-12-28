using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Client side manager for the player
/// </summary>
public class ClientPlayer : NetworkBehaviour
{
    private PlayerController2D _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController2D>();
        _playerController.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        enabled = IsClient;

        // Register local player
        if(LevelManager.Instance != null)
            LevelManager.Instance.RegisterPlayer(_playerController);

        // Disable player controller if we are not the owner
        if (!IsOwner)
        {
            _playerController.enabled = false;
            return;
        }

        // Enable controller as we are the owner
        _playerController.enabled = true;

        // Set camera to target player
        var cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObject != null)
        {
            cameraObject.TryGetComponent(out CameraController cameraController);
            if (cameraController != null)
                cameraController.Target = _playerController;
        }
    }
}
