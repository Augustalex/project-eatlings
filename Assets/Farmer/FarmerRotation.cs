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

        var currentRotation = _farmerPivotAccess.pivot.transform.rotation;
        var newRotation = Quaternion.AngleAxis(angles, Vector3.up);
        // var diff = (currentRotation.eulerAngles - newRotation.eulerAngles).magnitude;
        // _farmerPivotAccess.pivot.transform.rotation = Quaternion.Slerp(currentRotation,
        //     newRotation, .05f / (diff *.01f));
        //
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, 720f * Time.deltaTime);
    }

    public void ForceRotation(Quaternion rotation)
    {
        _farmerPivotAccess.pivot.transform.rotation = rotation;
    }
}