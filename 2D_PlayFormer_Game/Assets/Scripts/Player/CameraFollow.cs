using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script managing camera movement.
/// </summary>
public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform target;

    [SerializeField] [Range(0.1f,1f)]
    
    private float smoothSpeed = 0.125f;

    [SerializeField] private Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        }
    }
}
