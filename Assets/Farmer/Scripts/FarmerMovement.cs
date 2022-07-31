using System;
using System.Collections;
using System.Collections.Generic;
using Farmer.Scripts;
using UnityEngine;

public class FarmerMovement : MonoBehaviour
{
    // Public
    public FarmerSettings farmerSettings;
    [SerializeField] private Animator animator;


    // Private
    private Vector2 _movement;
    private Rigidbody _rigidbody;
    private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
    private static readonly int MovementType = Animator.StringToHash("MovementType");
    private bool _forceRun;
    private float _freezeMovementUntil;

    // Public methods
    public void SetMovementVector(Vector2 movement)
    {
        _movement = movement;
    }

    public void StopAndFreeze()
    {
        _rigidbody.velocity = Vector3.zero;
        _freezeMovementUntil = Time.time + .08f;
    }

    public void StopAndFreezeUntilUnfreeze()
    {
        _rigidbody.velocity = Vector3.zero;
        _freezeMovementUntil = Mathf.Infinity;
    }

    public void Unfreeze()
    {
        _rigidbody.velocity = Vector3.zero;
        _freezeMovementUntil = -1f;
    }

    // Private methods
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Time.time < _freezeMovementUntil) return;

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
        if (velocity.magnitude > MaxSpeed())
        {
            _rigidbody.velocity = velocity.normalized * MaxSpeed();
        }
    }

    private void UpdateMovement()
    {
        var normalizedMovement = MovementVector();

        var finalMovementVector = normalizedMovement * CharacterSpeed();

        if (normalizedMovement.magnitude > .05f)
        {
            var direction = finalMovementVector.normalized;
            var speed = finalMovementVector.magnitude;
            var forceLeftToMax = Mathf.Max(0f, MaxSpeed() - _rigidbody.velocity.magnitude);
            var finalForceToAdd = Mathf.Min(speed, forceLeftToMax);
            var finalResult = direction * finalForceToAdd;

            if (finalForceToAdd < .01f)
            {
                var diff = (direction * MaxSpeed()) - _rigidbody.velocity;
                _rigidbody.AddForce(diff, ForceMode.Impulse);
            }
            else
            {
                _rigidbody.AddForce(finalResult, ForceMode.Impulse);
            }
        }
    }

    private float MaxSpeed()
    {
        return Running() ? farmerSettings.maxRunSpeed : farmerSettings.maxWalkSpeed;
    }

    private bool Running()
    {
        return _forceRun;
        //return _movement.magnitude > .8f || _forceRun;
    }

    public void SetForceRun(bool on)
    {
        _forceRun = on;
    }

    private Vector3 MovementVector()
    {
        return new Vector3(
            _movement.x,
            0f,
            _movement.y);

        var xyVector = _movement;
        // var x = RoundMovementValue(xyVector.x);
        // var roundedY = RoundMovementValue(xyVector.y);
        // var z = (roundedY > .1f || roundedY < -.1f) ? roundedY < 0 ? -1f : 1f : 0f;
        // return new Vector3(
        //     x,
        //     0,
        //     x == 0f ? z : 0f
        // );
        // return new Vector3(RoundMovementValue(xyVector.x), 0f, roundedY);
    }

    private float RoundMovementValue(float x)
    {
        return x;

        // var scale = 8f;
        // return Mathf.Round(x * scale) / scale;
    }

    private float CharacterSpeed()
    {
        return farmerSettings.baseMovementSpeed * _movement.magnitude;
    }

    public void TeleportTo(Vector3 spawnPoint)
    {
        transform.position = spawnPoint;
    }

    private void LateUpdate()
    {
        var isRunning = Running();
        var velocity = _rigidbody.velocity;
        var movementSpeedValue = velocity.magnitude /
                                 (isRunning ? farmerSettings.maxRunSpeed : farmerSettings.maxWalkSpeed) *
                                 (isRunning ? farmerSettings.runSpeedMultiplier : farmerSettings.walkSpeedMultiplier);
        var movementTypeValue = velocity.magnitude / farmerSettings.maxRunSpeed;
        animator.SetFloat(MovementSpeed, movementSpeedValue);
        animator.SetFloat(MovementType, movementTypeValue);
    }
}