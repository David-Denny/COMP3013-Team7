using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController2D Target { get; set; }

    [SerializeField] private float _min = 0.0f;
    [SerializeField] private float _max = float.MaxValue;

    private void Update()
    {
        if (Target == null)
            return;

        var targetPosition = transform.position;
        targetPosition.x = Mathf.Clamp(Target.transform.position.x, _min, _max); 
        transform.position = targetPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        var minPosition = transform.position;
        minPosition.x = _min;
        var maxPosition = transform.position;
        maxPosition.x = _max;
        Gizmos.DrawWireSphere(minPosition, 0.5f);
        Gizmos.DrawLine(minPosition, maxPosition);
        Gizmos.DrawWireSphere(maxPosition, 0.5f);
    }
}