using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProgressBar : MonoBehaviour
{
    [SerializeField] private float _markerPadding = 2f;
    [SerializeField] private RectMask2D _progressBar;
    [SerializeField] private GameObject _markerPrefab;

    private RectTransform _transform;
    private readonly List<RectTransform> _playerMarkers = new();

    private void Awake()
    {
        _transform= GetComponent<RectTransform>();
    }

    private void Update()
    {
        List<PlayerController2D> players = LevelManager.Instance.Players;

        // Create marker foreach player
        int count = players.Count - _playerMarkers.Count;
        for (int i = 0; i < count; ++i)
        {
            GameObject playerMarker = Instantiate(_markerPrefab, transform);
            _playerMarkers.Add(playerMarker.GetComponent<RectTransform>());
        }

        if (LevelManager.Instance.Finish == null)
            return;

        // Update marker positions
        float finish = LevelManager.Instance.Finish.position.x;

        float maxT = 0f;
        for (int i = 0; i < players.Count; ++i)
        {
            if (players[i] != null)
            {
                float t = players[i].transform.position.x / finish;
                maxT = Mathf.Max(maxT, t);

                float markerX = Mathf.Lerp(_markerPadding, _transform.sizeDelta.x - _markerPadding, t);
                _playerMarkers[i].anchoredPosition = new Vector2(markerX, _playerMarkers[i].anchoredPosition.y);
            }
        }

        float barWidth = Mathf.Lerp(_transform.sizeDelta.x, 0f, maxT);
        _progressBar.padding = new Vector4(0f, 0f, barWidth, 0f);
    }
}
