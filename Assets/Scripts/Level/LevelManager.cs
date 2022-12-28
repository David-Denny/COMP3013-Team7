using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Singleton class <c>Level Manager</c> holds general level state
/// </summary>
public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance
    /// </summary>
    public static LevelManager Instance { get {  return _instance; } }
    private static LevelManager _instance;

    /// <summary>
    /// List of local player controllers
    /// </summary>
    public List<PlayerController2D> Players { get { return _players; } }
    /// <summary>
    /// Transform of the moving laser
    /// </summary>
    public Transform Laser { get { return _laser; } }
    /// <summary>
    /// Transform of the finish line
    /// </summary>
    public Transform Finish { get { return _finish; } }

    [SerializeField] private Transform _laser;
    [SerializeField] private Transform _finish;
    [SerializeField] private GameOverMenu _gameOverMenu;

    private readonly List<PlayerController2D> _players = new();

    private void Awake()
    {
        // Enforce singleton instance
        if (Instance == null)
            _instance  = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Register local player controller
    /// </summary>
    /// <param name="player">Local player controller</param>
    public void RegisterPlayer(PlayerController2D player)
    {
        _players.Add(player);
    }

    /// <summary>
    /// Handles the game being over.
    /// Either by a player dying or by all finishing
    /// </summary>
    /// <param name="finished">Whether the game ended by death or finish</param>
    public void GameOver(bool finished)
    {
        _gameOverMenu.ShowClientRpc();
    }
}
