using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class Puzzle : NetworkBehaviour
{
    [SerializeField] private int _completionScore;

    [ServerRpc]
    protected void PuzzleCompletedServerRpc()
    {
        ScoreManager.Instance.AddScoreServerRpc(_completionScore);
    }
}
