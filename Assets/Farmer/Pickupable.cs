using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider[] _colliders;

    // Public

    public void PickedUp()
    {
        foreach (var collider1 in _colliders)
        {
            collider1.enabled = false;
        }

        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
    }

    public void Dropped()
    {
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
}