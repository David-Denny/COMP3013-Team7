using System.Collections;
using System.Collections.Generic;
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
}
