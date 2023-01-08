using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for managing the game score
/// </summary>
public class ScoreManager : NetworkBehaviour
{
    private static ScoreManager _instance;

    public static ScoreManager Instance { get { return _instance; } }

    public int Score { get { return _score.Value; } }

    [SerializeField] private int _distanceScoreWorth;
    [SerializeField] private Text _scoreText;

    private readonly NetworkVariable<int> _score = new();
    private float _maxT = 0f;
    private int _puzzleScore = 0;

    private void Awake()
    {
        // Setup singleton instance
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Setup text to update when score changes
        if (IsClient)
            _score.OnValueChanged += (prev, curr) => _scoreText.text = curr.ToString();
    }

    private void Update()
    {
        if(!IsServer) return;

        // Calculate the distance based score
        float finish = LevelManager.Instance.Finish.transform.position.x;
        foreach (var player in LevelManager.Instance.Players)
        {
            if (player != null)
            {
                // Calculate current distance of the player
                float t = player.transform.position.x / finish;
                // Update max distance reached
                _maxT = Mathf.Max(_maxT, t);
            }
        }
        // Recalculate total score
        RecalculateScoreServerRpc();
    }

    /// <summary>
    /// Add puzzle based score to the current score
    /// </summary>
    /// <param name="score">Amount of score to add</param>
    [ServerRpc]
    public void AddScoreServerRpc(int score)
    {
        // Increase current puzzle score
        _puzzleScore += score;
        // Recalculate total score
        RecalculateScoreServerRpc();
    }

    /// <summary>
    /// Recalculates the total score
    /// </summary>
    [ServerRpc]
    private void RecalculateScoreServerRpc()
    {
        int newScore = _puzzleScore + Mathf.CeilToInt(_distanceScoreWorth * _maxT);
        if(newScore != _score.Value)
            _score.Value = newScore;
    }
}
