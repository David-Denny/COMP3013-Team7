using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PatternPuzzleInteractable : NetworkBehaviour, IInteractable
{
    [SerializeField] private SpriteRenderer _screen;
    [SerializeField] private Sprite[] _patterns;
    [SerializeField] private PatternPuzzle _patternPuzzle;
    [SerializeField] private Animator _switchAnimator;

    private NetworkVariable<int> _pattern = new();

    public override void OnNetworkSpawn()
    {
        _screen.sprite = _patterns[_pattern.Value];
    }

    [ClientRpc]
    public void SetPatternClientRpc(int pattern)
    {
        _pattern.Value = pattern;
        _screen.sprite = _patterns[pattern];
    }

    public void Interact()
    {
        _patternPuzzle.AttemptMatchServerRpc(_pattern.Value);
        _switchAnimator.SetTrigger("flip");
    }
}
