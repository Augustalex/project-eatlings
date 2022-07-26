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
        Debug.Log("PICKUP ITEM: " + this);
        foreach (var collider1 in _colliders)
        {
            collider1.enabled = false;
        }

        _rigidbody.isKinematic = true;
    }

    public void Dropped()
    {
        foreach (var collider1 in _colliders)
        {
            collider1.enabled = true;
        }

        _rigidbody.isKinematic = false;
    }

    // Private
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _colliders = GetComponentsInChildren<Collider>();
    }
}