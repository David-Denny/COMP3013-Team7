using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Handles player animations and flip
/// </summary>
public class PlayerGraphicsController : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private SpriteRenderer markerRenderer;
    [SerializeField] private Color ownerColor;
    [SerializeField] private Color otherColor;

    private PlayerController2D playerController;
    private Animator animator;

    private readonly NetworkVariable<bool> _flip = new(false, writePerm: NetworkVariableWritePermission.Owner);

    private void Awake()
    {
        // Get components
        playerController = GetComponent<PlayerController2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Color marker depending on owner status
        markerRenderer.color = IsOwner ? ownerColor : otherColor;

        if (IsOwner)
        {
            // Update animator parameters
            animator.SetBool("moving", playerController.Moving);
            animator.SetBool("grounded", playerController.Grounded);

            // Calculate current flip direction if changed
            bool flip = playerRenderer.flipX;
            if (playerController.MoveDirection < -0.1f && !flip) flip = true;
            else if (playerController.MoveDirection > 0.1f && flip) flip = false;
            _flip.Value = flip;
        }

        // Update flip
        playerRenderer.flipX = _flip.Value;
    }
}
