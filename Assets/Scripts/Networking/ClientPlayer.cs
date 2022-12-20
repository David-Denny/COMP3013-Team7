using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;

public class ClientPlayer : NetworkBehaviour
{
    private PlayerController2D _playerController;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController2D>();
        _collider = GetComponent<BoxCollider2D>();

        _playerController.enabled = false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        enabled = IsClient;
        LevelManager.Instance.RegisterPlayer(_playerController);
        if (!IsOwner)
        {
            enabled = false;
            _playerController.enabled = false;
            return;
        }

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

    [ClientRpc]
    public void SetSpawnClientRpc(Vector3 position, ClientRpcParams clientRpcParams = default)
    {
        transform.position = position;
    }
}
