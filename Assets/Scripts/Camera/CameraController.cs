using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController2D Target { get; set; }

    [SerializeField] private float _min = 0.0f;
    [SerializeField] private float _max = float.MaxValue;
    [SerializeField] [Range(0.0f, 1.0f)] private float _damping = 0.9f;
    [SerializeField] private float _aheadDistance = 5.0f;

    private void FixedUpdate()
    {
        if (Target == null)
            return;

        var targetPosition = transform.position;
        var offset = Target.VelocityX > 0.1f ? _aheadDistance : (Target.VelocityX < -0.1f ? -_aheadDistance : 0.0f);
        targetPosition.x = Target.transform.position.x + offset;
        var nextPosition = Vector3.Lerp(transform.position, targetPosition, _damping);
        nextPosition.x = Mathf.Clamp(nextPosition.x, _min, _max);
        transform.position = nextPosition;
    }
}
