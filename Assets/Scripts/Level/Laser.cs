using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Laser : NetworkBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private Vector3 _endPosition;
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
            transform.position = Vector3.Lerp(_startPosition, _endPosition, t);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(_startPosition, 0.5f);
        Gizmos.DrawLine(_startPosition, _endPosition);
        Gizmos.DrawWireSphere(_endPosition, 0.5f);
    }
}
