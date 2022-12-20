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

    private NetworkVariable<bool> _flip = new(false, writePerm: NetworkVariableWritePermission.Owner);

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

        if (IsOwner)
        {
            animator.SetBool("moving", playerController.Moving);
            animator.SetBool("grounded", playerController.Grounded);
            bool flip = playerRenderer.flipX;
            if (playerController.MoveDirection < -0.1f && !flip) flip = true;
            else if (playerController.MoveDirection > 0.1f && flip) flip = false;
            _flip.Value = flip;
        }

        playerRenderer.flipX = _flip.Value;
    }
}
