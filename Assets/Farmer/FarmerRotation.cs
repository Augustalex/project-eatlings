using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FarmerPivotAccess))]
public class FarmerRotation : MonoBehaviour
{
    // Private
    private Rigidbody _rigidbody;
    private FarmerPivotAccess _farmerPivotAccess;

    private bool _forcedTarget;
    private Quaternion _forceTarget;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _farmerPivotAccess = GetComponent<FarmerPivotAccess>();
    }

    void Update()
    {
        if (_forcedTarget)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _forceTarget, 720f * Time.deltaTime);
        }
        else
        {
            var rigidbodyVelocity = _rigidbody.velocity;
            if (rigidbodyVelocity.magnitude < .01f) return;

            var currentDirection = rigidbodyVelocity.normalized;
            var angles = Mathf.Atan2(currentDirection.x, currentDirection.z) * Mathf.Rad2Deg;
            var newRotation = Quaternion.AngleAxis(angles, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 720f * Time.deltaTime);
        }
    }

    public void ForceRotationTarget(Quaternion newRotation)
    {
        _forceTarget = newRotation;
        _forcedTarget = true;
    }

    public void NullForceTarget()
    {
        _forcedTarget = false;
    }
}