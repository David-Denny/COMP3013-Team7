using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

/// <summary>
/// Class that controls a puzzle pattern.
/// Displays a sequence of patterns the players have to enter.
/// </summary>
public class PatternPuzzle : NetworkBehaviour
{
    [Header("Pattern")]
    [SerializeField] private int _sequenceLength;
    [SerializeField] private Sprite[] _patterns;
    [Header("Barrier Settings")]
    [SerializeField] private float _barrierOpenTime;
    [SerializeField] private float _upperStart;
    [SerializeField] private float _upperEnd;
    [SerializeField] private float _lowerStart;
    [SerializeField] private float _lowerEnd;
    [Header("References")]
    [SerializeField] private SpriteRenderer _targetScreen;
    [SerializeField] private Transform _upperBarrier;
    [SerializeField] private Transform _lowerBarrier;
    [SerializeField] private PatternPuzzleInteractable[] _interactables;

    private readonly NetworkVariable<int> _currentPattern = new();
    private int _currentMatches = 0;
    private bool _finished = false;
    private float _barrierTimer = 0.0f;

    public override void OnNetworkSpawn()
    {
        // Initial setup (Server only)
        if(IsServer)
        {
            // Set initial barrier positions
            _upperBarrier.localPosition = new Vector3(_upperBarrier.localPosition.x, _upperStart);
            _lowerBarrier.localPosition = new Vector3(_lowerBarrier.localPosition.x, _lowerStart);

            // Shuffle patterns
            System.Random rnd = new();
            List<int> patterns = Enumerable.Range(0, _patterns.Length).ToList();
            var shuffledPatterns = patterns.OrderBy(item => rnd.Next()).ToList();
            for(int i = 0; i < _interactables.Length; ++i)
                _interactables[i].SetPatternServerRpc(shuffledPatterns[i]);
            
            // Display target pattern
            ShowNewPatternServerRpc();
        }

        // Display target sprite
        _targetScreen.sprite = _patterns[_currentPattern.Value];
        // Register change listener
        _currentPattern.OnValueChanged += (prev, curr) => { _targetScreen.sprite = _patterns[curr]; };
    }

    private void Update()
    {
        // Open barriers if sequence finished
        if(IsServer && _finished && _barrierTimer < _barrierOpenTime)
        {
            // Update timer
            _barrierTimer += Time.deltaTime;
            float t = _barrierTimer / _barrierOpenTime;
            // Update positions
            _upperBarrier.localPosition = new Vector3(_upperBarrier.localPosition.x, Mathf.Lerp(_upperStart, _upperEnd, t));
            _lowerBarrier.localPosition = new Vector3(_lowerBarrier.localPosition.x, Mathf.Lerp(_lowerStart, _lowerEnd, t));
        }
    }

    /// <summary>
    /// Server RPC to attempt to match the target pattern
    /// </summary>
    /// <param name="pattern">The pattern to guess</param>
    [ServerRpc]
    public void AttemptMatchServerRpc(int pattern)
    {
        // Do nothing if already complete
        if (_finished)
            return;

        // If matched correctly
        if (pattern == _currentPattern.Value)
        {
            ++_currentMatches;
            // Check if finished
            if (_currentMatches == _sequenceLength)
                OnPatternSuccessClientRpc();
            else
                ShowNewPatternServerRpc();
        }
        else
        {
            // Penialize incorrect match
            _currentMatches = Math.Max(0, _currentMatches - 1);
        }
    }

    /// <summary>
    /// Server RPC to show a new target pattern to match
    /// </summary>
    [ServerRpc]
    private void ShowNewPatternServerRpc()
    {
        // Generate new random pattern
        int newPattern = UnityEngine.Random.Range(0, _patterns.Length);
        // Keep generating until the pattern is different than the last
        while (newPattern == _currentPattern.Value)
            newPattern = UnityEngine.Random.Range(0, _patterns.Length);
        // Update the target pattern
        _currentPattern.Value = newPattern;
    }

    /// <summary>
    /// Client RPC to clear target when puzzle has been solved
    /// </summary>
    [ClientRpc]
    private void OnPatternSuccessClientRpc()
    {
        _targetScreen.sprite = null;
        _finished = true;
    }
}
