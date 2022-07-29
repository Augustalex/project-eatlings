using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FarmerPivotAccess))]
public class FarmerRotation : MonoBehaviour
{
    // Private
    private Rigidbody _rigidbody;
    private FarmerPivotAccess _farmerPivotAccess;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _farmerPivotAccess = GetComponent<FarmerPivotAccess>();
    }

    void Update()
    {
        var rigidbodyVelocity = _rigidbody.velocity;
        if (rigidbodyVelocity.magnitude < .01f) return;
        var currentDirection = rigidbodyVelocity.normalized;
        var angles = Mathf.Atan2(currentDirection.x, currentDirection.z) * Mathf.Rad2Deg;

        _farmerPivotAccess.pivot.transform.rotation = Quaternion.Lerp(_farmerPivotAccess.pivot.transform.rotation,
            Quaternion.AngleAxis(angles, Vector3.up), .1f);
    }
}