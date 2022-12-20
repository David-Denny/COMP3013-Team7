using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Linq;

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

    private NetworkVariable<int> _currentPattern = new();
    private int _currentMatches = 0;
    private bool _finished = false;
    private float _barrierTimer = 0.0f;

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            _upperBarrier.localPosition = new Vector3(_upperBarrier.localPosition.x, _upperStart);
            _lowerBarrier.localPosition = new Vector3(_lowerBarrier.localPosition.x, _lowerStart);

            // Shuffle 
            System.Random rnd = new();
            List<int> patterns = Enumerable.Range(0, _patterns.Length).ToList();
            var shuffledPatterns = patterns.OrderBy(item => rnd.Next()).ToList();
            for(int i = 0; i < _interactables.Length; ++i)
                _interactables[i].SetPatternClientRpc(shuffledPatterns[i]);
            
            ShowNewPatternServerRpc();
        }

        _targetScreen.sprite = _patterns[_currentPattern.Value];
        _currentPattern.OnValueChanged += (prev, curr) => { _targetScreen.sprite = _patterns[curr]; };
    }

    private void Update()
    {
        if(IsServer && _finished && _barrierTimer < _barrierOpenTime)
        {
            _barrierTimer += Time.deltaTime;
            float t = _barrierTimer / _barrierOpenTime;
            _upperBarrier.localPosition = new Vector3(_upperBarrier.localPosition.x, Mathf.Lerp(_upperStart, _upperEnd, t));
            _lowerBarrier.localPosition = new Vector3(_lowerBarrier.localPosition.x, Mathf.Lerp(_lowerStart, _lowerEnd, t));
        }
    }

    [ServerRpc]
    public void AttemptMatchServerRpc(int pattern)
    {
        if (_finished)
            return;

        // If matched correctly
        if (pattern == _currentPattern.Value)
        {
            ++_currentMatches;
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

    [ServerRpc]
    private void ShowNewPatternServerRpc()
    {
        int newPattern = UnityEngine.Random.Range(0, _patterns.Length);
        while (newPattern == _currentPattern.Value)
            newPattern = UnityEngine.Random.Range(0, _patterns.Length);

        _currentPattern.Value = newPattern;
    }

    [ClientRpc]
    private void OnPatternSuccessClientRpc()
    {
        _targetScreen.sprite = null;
        _finished = true;
    }
}
