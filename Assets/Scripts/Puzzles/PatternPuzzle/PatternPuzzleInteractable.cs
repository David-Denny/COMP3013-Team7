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

    private int _pattern = 0;

    [ClientRpc]
    public void SetPatternClientRpc(int pattern)
    {
        _pattern = pattern;
        _screen.sprite = _patterns[pattern];
    }

    public void Interact()
    {
        _patternPuzzle.AttemptMatchServerRpc(_pattern);
        _switchAnimator.SetTrigger("flip");
    }
}
