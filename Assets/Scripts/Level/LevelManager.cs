using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get {  return _instance; } }
    private static LevelManager _instance;

    public List<PlayerController2D> Players { get { return _players; } }
    public Transform Laser { get { return _laser; } }
    public Transform Finish { get { return _finish; } }

    [SerializeField] private Transform _laser;
    [SerializeField] private Transform _finish;

    private List<PlayerController2D> _players = new();

    private void Awake()
    {
        if (Instance == null)
            _instance  = this;
        else
            Destroy(gameObject);
    }

    public void RegisterPlayer(PlayerController2D player)
    {
        _players.Add(player);
    }

    public void GameOver(bool finished)
    {
        Debug.Log("Game over");
    }
}
