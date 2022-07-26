using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerRotation : MonoBehaviour
{
    // Public
    public GameObject pivot;

    // Private
    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        var rigidbodyVelocity = _rigidbody.velocity;
        if (rigidbodyVelocity.magnitude < 2f) return;
        var currentDirection = rigidbodyVelocity.normalized;
        var angles = Mathf.Atan2(currentDirection.x, currentDirection.z) * Mathf.Rad2Deg - 90;

        pivot.transform.rotation = Quaternion.AngleAxis(angles, Vector3.up);
    }
}