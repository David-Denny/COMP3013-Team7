using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerGraphicsController : NetworkBehaviour
{
    private PlayerController2D playerController;
    private Animator animator;
    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private SpriteRenderer markerRenderer;
    [SerializeField] private Color ownerColor;
    [SerializeField] private Color otherColor;

    private void Awake()
    {
        playerController = GetComponent<PlayerController2D>();
        animator = GetComponent<Animator>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }

    private void Update()
    {
        markerRenderer.color = IsOwner ? ownerColor : otherColor;

        if (IsServer)
        {
            animator.SetBool("moving", playerController.Moving);
            animator.SetBool("grounded", playerController.Grounded);
            bool flip = playerController.VelocityX <= 0.0f && (playerController.VelocityX < 0.0f || playerRenderer.flipX);
            SetPlayerFacingClientRpc(flip);
        }
    }

    [ClientRpc]
    private void SetPlayerFacingClientRpc(bool flip)
    {
        playerRenderer.flipX = flip;
    }
}
