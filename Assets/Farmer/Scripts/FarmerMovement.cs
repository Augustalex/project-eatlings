using System.Collections;
using System.Collections.Generic;
using Farmer.Scripts;
using UnityEngine;

public class FarmerMovement : MonoBehaviour
{
    // Public
    public FarmerSettings farmerSettings;

    // Private
    private Vector2 _movement;
    private Rigidbody _rigidbody;

    // Public methods
    public void SetMovementVector(Vector2 movement)
    {
        _movement = movement;
    }

    // Private methods
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        UpdateMovement();
        InertialDampening();
    }

    private void InertialDampening()
    {
        if (_movement.magnitude < .1f)
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }

    private void UpdateMovement()
    {
        var normalizedMovement = MovementVector();

        var finalMovementVector = normalizedMovement * CharacterSpeed();
        _rigidbody.velocity = finalMovementVector;
    }

    private Vector3 MovementVector()
    {
        var xyVector = _movement;
        var x = RoundMovementValue(xyVector.x);
        var z = xyVector.y < 0 ? -1f : 1f;
        return new Vector3(
            x,
            0,
            x == 0f ? z : 0f
        );
        return new Vector3(RoundMovementValue(xyVector.x), 0f, RoundMovementValue(xyVector.y));
    }

    private float RoundMovementValue(float x)
    {
        var scale = 1f;
        return Mathf.Round(x * scale) / scale;
    }

    private float CharacterSpeed()
    {
        return farmerSettings.baseMovementSpeed;
    }
}