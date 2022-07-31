using eatlings;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlobBouncer : MonoBehaviour
{
    // Public

    public EatlingBounceSettings eatlingBounceSettings;

    // Private

    // Static
    private const float NormalSquishHeight = 1f;
    private Pickupable _pickupable;
    private EatlingBabyGrowth _eatlingBabyGrowth;
    private Rigidbody _rigidbody;
    private Vector3 _originalScale;

    // Squish animation state
    private float _startedSquish;
    private float _squishState;
    private int _squishIndex;

    // Internal
    private int _direction = 1;
    private float _forceDown;
    private bool _moved;
    private float _groundLevel;
    private Vector3 _frameScale;
    private float _recoilStarted = -100f;
    private float _jumpedAt;

    // Private

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _pickupable = GetComponent<Pickupable>();
        _eatlingBabyGrowth = GetComponent<EatlingBabyGrowth>();

        _originalScale = transform.localScale;
    }

    void Update()
    {
        if (!CanBounce())
        {
            transform.localScale = _originalScale;
            return;
        }

        if (_rigidbody.velocity.magnitude < .01f)
        {
            _groundLevel = _rigidbody.position.y;
        }

        _forceDown = Mathf.Max(_rigidbody.position.y - _groundLevel, 0f) * 10f;

        if (RecoilDone() && !_moved)
        {
            _rigidbody.AddForce(Vector3.up * 600f, ForceMode.Impulse);
            MoveInRandomDirection();
            _moved = true;
            _jumpedAt = Time.time;
        }

        if (RecoilDone())
        {
            if (_rigidbody.velocity.y > .1f)
            {
                _direction = 1;
            }
            else if (Time.time - _jumpedAt > .1f && _forceDown >= 0f)
            {
                _rigidbody.AddForce(Vector3.down * _forceDown * Time.deltaTime, ForceMode.Force);
                _direction = -1;

                if (_forceDown < 0.2f)
                {
                    TriggerImpact();
                }
            }
        }

        _frameScale = _originalScale;
        HandleSquishing();
        SquishOnRigidbody();
        transform.localScale = _frameScale;
    }

    private bool CanBounce()
    {
        if (_pickupable.IsPickedUp()) return false;
        if (_eatlingBabyGrowth.IsPlanted()) return false;
        if (_eatlingBabyGrowth.IsDead()) return false;

        return true;
    }

    private bool RecoilDone()
    {
        return _squishIndex >= eatlingBounceSettings.animation.Length || _squishIndex < 0;
    }

    private void TriggerImpact()
    {
        _recoilStarted = Time.time;
        _moved = false;

        StartSquish();
    }

    private void SquishOnRigidbody()
    {
        var progress = (_forceDown / 5.5f) * 2f;
        var wideSqueeze = 1f - (Mathf.Clamp(_forceDown / 5.5f, 0f, 1f)) * .3f;
        var longSqueeze =
            1f + (Mathf.Clamp(_forceDown / 5.5f, 0f, 1f)) * .5f; // Should be elastic up as well!
        _frameScale = new Vector3(
            _frameScale.x * wideSqueeze,
            _frameScale.y * longSqueeze,
            _frameScale.z * wideSqueeze
        );
    }

    private void HandleSquishing()
    {
        if (RecoilDone())
        {
            _squishIndex = -1;
        }

        UpdateSquish();
    }

    private void StartSquish()
    {
        _startedSquish = Time.time;
        _squishState = 1f;
        _squishIndex = 0;
    }

    private void UpdateSquish()
    {
        if (_squishIndex >= eatlingBounceSettings.animation.Length || _squishIndex < 0)
        {
            _squishState = 1f;
        }
        else
        {
            var squishTarget = eatlingBounceSettings.animation[_squishIndex].Squeeze;
            var squishDuration = eatlingBounceSettings.animation[_squishIndex].Duration *
                                 eatlingBounceSettings.timeScale;

            var time = Time.time - _startedSquish;
            if (time > squishDuration)
            {
                _squishIndex += 1;
                _squishState = squishTarget;
                _startedSquish = Time.time;
            }
            else
            {
                var previousTarget = _squishIndex == 0
                    ? NormalSquishHeight
                    : eatlingBounceSettings.animation[_squishIndex - 1].Squeeze;
                _squishState = Mathf.Lerp(previousTarget, squishTarget, time / squishDuration);
            }
        }

        _frameScale = new Vector3(
            _frameScale.x + _frameScale.x * Mathf.Clamp(1f - _squishState, 0f, 1f),
            _frameScale.y * _squishState,
            _frameScale.z + _frameScale.z * Mathf.Clamp(1f - _squishState, 0f, 1f)
        );
    }

    private void MoveInRandomDirection()
    {
        var direction2d = Random.insideUnitCircle;
        var direction = new Vector3(direction2d.x, 0f, direction2d.y);
        _rigidbody.AddForce(direction.normalized * 45f, ForceMode.Impulse);
    }

    // private float EaseOutBounce(float x)
    // {
    //     float n1 = 7.5625f;
    //     float d1 = 2.75f;
    //
    //     if (x < 1 / d1)
    //     {
    //         return n1 * x * x;
    //     }
    //     else if (x < 2 / d1)
    //     {
    //         return n1 * (x -= 1.5f / d1) * x + 0.75f;
    //     }
    //     else if (x < 2.5 / d1)
    //     {
    //         return n1 * (x -= 2.25f / d1) * x + 0.9375f;
    //     }
    //     else
    //     {
    //         return n1 * (x -= 2.625f / d1) * x + 0.984375f;
    //     }
    // }
}