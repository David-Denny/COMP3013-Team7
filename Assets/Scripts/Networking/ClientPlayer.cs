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
        base.OnNetworkSpawn();

        enabled = IsClient;

        // Register local player
        LevelManager.Instance.RegisterPlayer(_playerController);

        // Disable player controller if we are not the owner
        if (!IsOwner)
        {
            enabled = false;
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

    /// <summary>
    /// Client RPC to set the starting position of this player
    /// </summary>
    /// <param name="position">Spawn location</param>
    /// <param name="clientRpcParams">Additional parameters</param>
    [ClientRpc]
    public void SetSpawnClientRpc(Vector3 position, ClientRpcParams clientRpcParams = default)
    {
        transform.position = position;
    }
}
