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
        _rigidbody.velocity = EmptyWithGravity();
        _freezeMovementUntil = Time.time + .08f;
    }

    public void StopAndFreezeUntilUnfreeze()
    {
        _rigidbody.velocity = EmptyWithGravity();
        _freezeMovementUntil = Mathf.Infinity;
    }

    public void Unfreeze()
    {
        _rigidbody.velocity = EmptyWithGravity();
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

    private Vector3 EmptyWithGravity()
    {
        var velocity = _rigidbody.velocity;
        return new Vector3(
            0f,
            velocity.y,
            0f
        );
    }

    private void InertialDampening()
    {
        if (_movement.magnitude < .1f)
        {
            _rigidbody.velocity = EmptyWithGravity();
        }
    }

    private void ClampToMaxSpeed()
    {
        var velocity = _rigidbody.velocity;
        if (velocity.magnitude > MaxSpeed())
        {
            var targetVelocity = velocity.normalized * MaxSpeed();
            _rigidbody.velocity = new Vector3(
                targetVelocity.x,
                velocity.y,
                targetVelocity.z
            );
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
                _rigidbody.AddForce(StripGravity(diff), ForceMode.Impulse);
            }
            else
            {
                _rigidbody.AddForce(StripGravity(finalResult), ForceMode.Impulse);
            }
        }
    }

    private Vector3 StripGravity(Vector3 vector)
    {
        return new Vector3(
            vector.x,
            0f,
            vector.z
        );
    }

    private float MaxSpeed()
    {
        return Running() ? farmerSettings.maxRunSpeed : farmerSettings.maxWalkSpeed;
    }

    private bool Running()
    {
        return _forceRun;
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
        var rawVelocity = _rigidbody.velocity;
        var velocity = new Vector3(rawVelocity.x, 0f, rawVelocity.z);
        var movementSpeedValue = velocity.magnitude /
                                 (isRunning ? farmerSettings.maxRunSpeed : farmerSettings.maxWalkSpeed) *
                                 (isRunning ? farmerSettings.runSpeedMultiplier : farmerSettings.walkSpeedMultiplier);
        var movementTypeValue = velocity.magnitude / farmerSettings.maxRunSpeed;
        animator.SetFloat(MovementSpeed, movementSpeedValue);
        animator.SetFloat(MovementType, movementTypeValue);
    }
}