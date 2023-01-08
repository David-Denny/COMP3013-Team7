using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Class to manage an interactable for the pattern puzzle.
/// </summary>
public class PatternPuzzleInteractable : NetworkBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer _screen;
    [SerializeField] private Sprite[] _patterns;
    [SerializeField] private PatternPuzzle _patternPuzzle;
    [SerializeField] private Animator _switchAnimator;

    private readonly NetworkVariable<int> _pattern = new();

    public override void OnNetworkSpawn()
    {
        RefreshDisplayedPattern();
        _pattern.OnValueChanged += (prev, curr) => RefreshDisplayedPattern();
    }

    /// <summary>
    /// Server RPC to set the pattern this interactable represents
    /// </summary>
    /// <param name="pattern">New pattern</param>
    [ServerRpc]
    public void SetPatternServerRpc(int pattern)
    {
        _pattern.Value = pattern;
    }

    /// <summary>
    /// Update the sprite to show the current pattern
    /// </summary>
    private void RefreshDisplayedPattern()
    {
        _screen.sprite = _patterns[_pattern.Value];
    }

    public void Interact()
    {
        // Try match with the associated pattern
        _patternPuzzle.AttemptMatchServerRpc(_pattern.Value);
        // Animate switch
        _switchAnimator.SetTrigger("flip");
    }
}
