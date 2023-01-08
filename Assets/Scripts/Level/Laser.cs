using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// Class <c>Laser</c> controls the movement of the laser.
/// </summary>
public class Laser : NetworkBehaviour
{
    [SerializeField] private float _startPosition;
    [SerializeField] private float _endPosition;
    [SerializeField] private float _time;

    private bool _timing = false;
    private float _currentTime = 0.0f;

    private void Start()
    {
        _timing = true;
    }

    private void Update()
    {
        // If we have authority and should be moving
        if(IsServer && _timing)
        {
            // Calculate new position (%)
            _currentTime += Time.deltaTime;
            float t = _currentTime / _time;

            // Calcaulte new position (world space)
            var position = transform.position;
            position.x = Mathf.Lerp(_startPosition, _endPosition, t);

            // Update position
            transform.position = position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        // Calculate start and end points
        var start = transform.position;
        start.x = _startPosition;
        var end = transform.position;
        end.x = _endPosition;

        // Draw debug shapes showing the laser's movement range
        Gizmos.DrawWireSphere(start, 0.5f);
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(end, 0.5f);
    }
}
