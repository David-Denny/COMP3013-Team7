using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// <c>CameraController</c> follows a target transform on the horizontal axis.
/// </summary>
public class CameraController : MonoBehaviour
{
    public PlayerController2D Target { get; set; }

    [SerializeField] private float _min = 0.0f;
    [SerializeField] private float _max = float.MaxValue;

    private void Update()
    {
        // Don't update if there is no target
        if (Target == null)
            return;

        // Cache current position
        var targetPosition = transform.position;
        // Calculate target position and restrict it to range
        targetPosition.x = Mathf.Clamp(Target.transform.position.x, _min, _max); 
        // Update position
        transform.position = targetPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        // Calculate min, max positions
        var minPosition = transform.position;
        minPosition.x = _min;
        var maxPosition = transform.position;
        maxPosition.x = _max;
        // Draw debug shapes to show camera's movement range
        Gizmos.DrawWireSphere(minPosition, 0.5f);
        Gizmos.DrawLine(minPosition, maxPosition);
        Gizmos.DrawWireSphere(maxPosition, 0.5f);
    }
}