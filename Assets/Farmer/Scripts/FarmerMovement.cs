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
        ClampToMaxSpeed();
        InertialDampening();
    }

    private void InertialDampening()
    {
        if (_movement.magnitude < .1f)
        {
            _rigidbody.velocity = Vector3.zero;
        }
    }

    private void ClampToMaxSpeed()
    {
        var velocity = _rigidbody.velocity;
        if (velocity.magnitude > farmerSettings.maxSpeed)
        {
            _rigidbody.velocity = velocity.normalized * farmerSettings.maxSpeed;
        }
    }

    private void UpdateMovement()
    {
        var normalizedMovement = MovementVector();

        var finalMovementVector = normalizedMovement * CharacterSpeed();
        // _rigidbody.velocity = finalMovementVector;
        if (normalizedMovement.magnitude > .25f)
        {
            _rigidbody.AddForce(finalMovementVector * .05f, ForceMode.Impulse);
        }
    }

    private Vector3 MovementVector()
    {
        var xyVector = _movement;
        var x = RoundMovementValue(xyVector.x);
        var roundedY = RoundMovementValue(xyVector.y);
        var z = (roundedY > .1f || roundedY < -.1f) ? roundedY < 0 ? -1f : 1f : 0f;
        return new Vector3(
            x,
            0,
            x == 0f ? z : 0f
        );
        return new Vector3(RoundMovementValue(xyVector.x), 0f, roundedY);
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

    public void TeleportTo(Vector3 spawnPoint)
    {
        transform.position = spawnPoint;
    }
}