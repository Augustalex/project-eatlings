using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider[] _colliders;
    private bool _disabled;
    private bool _pickedUp;

    // Public

    public bool CanPickUp()
    {
        return !_disabled && !_pickedUp;
    }

    public void PickedUp()
    {
        _pickedUp = true;

        foreach (var collider1 in _colliders)
        {
            collider1.enabled = false;
        }

        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
    }

    public void Dropped()
    {
        _pickedUp = false;

        foreach (var collider1 in _colliders)
        {
            collider1.enabled = true;
        }

        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
    }

    // Private

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
    }

    public void Disable()
    {
        _disabled = true;
    }

    public bool IsPickedUp()
    {
        return _pickedUp;
    }
}