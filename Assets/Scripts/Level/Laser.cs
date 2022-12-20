using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

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
        if(IsServer && _timing)
        {
            _currentTime += Time.deltaTime;
            float t = _currentTime / _time;

            var position = transform.position;
            position.x = Mathf.Lerp(_startPosition, _endPosition, t);
            transform.position = position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        var start = transform.position;
        start.x = _startPosition;
        var end = transform.position;
        end.x = _endPosition;
        Gizmos.DrawWireSphere(start, 0.5f);
        Gizmos.DrawLine(start, end);
        Gizmos.DrawWireSphere(end, 0.5f);
    }
}
