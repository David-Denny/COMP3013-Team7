using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : NetworkBehaviour
{
    private static ScoreManager _instance;

    public static ScoreManager Instance { get { return _instance; } }

    [SerializeField] private int _distanceScoreWorth;
    [SerializeField] private Text _scoreText;

    private readonly NetworkVariable<int> _score = new();
    private float _maxT = 0f;
    private int _puzzleScore = 0;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (IsClient)
            _score.OnValueChanged += (prev, curr) => _scoreText.text = curr.ToString();
    }

    private void Update()
    {
        if(!IsServer) return;

        float finish = LevelManager.Instance.Finish.transform.position.x;
        foreach (var player in LevelManager.Instance.Players)
        {
            if (player != null)
            {
                float t = player.transform.position.x / finish;
                _maxT = Mathf.Max(_maxT, t);
            }
        }
        RecalculateScoreServerRpc();
    }

    [ServerRpc]
    public void AddScoreServerRpc(int score)
    {
        _puzzleScore += score;
        RecalculateScoreServerRpc();
    }

    [ServerRpc]
    private void RecalculateScoreServerRpc()
    {
        int newScore = _puzzleScore + (int)(_distanceScoreWorth * _maxT);
        if(newScore != _score.Value)
            _score.Value = newScore;
    }
}
