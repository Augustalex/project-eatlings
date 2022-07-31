using eatlings;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlobBouncer : MonoBehaviour
{
    // Public

    public EatlingBounceSettings eatlingBounceSettings;

    // Private

    private const float NormalSquishHeight = 1f;
    private Pickupable _pickupable;
    private EatlingBabyGrowth _eatlingBabyGrowth;
    private Rigidbody _rigidbody;
    private Vector3 _originalScale;

    private float _forceDown;
    private bool _moved;
    private float _groundLevel;
    private Vector3 _frameScale;
    private float _recoilStarted = -100f;
    private float _jumpedAt;
    private double _waitTimeUntil;

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
            if (_waitTimeUntil < 0)
            {
                _waitTimeUntil = Time.time + Random.Range(0f, 3f);
            }
            else if (Time.time > _waitTimeUntil)
            {
                _rigidbody.AddForce(Vector3.up * Random.Range(2.5f, 5f), ForceMode.Impulse);
                MoveInRandomDirection();
                _moved = true;
                _jumpedAt = Time.time;
                _waitTimeUntil = -1f;
            }
        }

        if (RecoilDone() && _moved)
        {
            if (_rigidbody.velocity.y < 0f && Time.time - _jumpedAt > .1f && _forceDown >= 0f)
            {
                _rigidbody.AddForce(Vector3.down * _forceDown * Time.deltaTime, ForceMode.Force);

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
        var duration = (Time.time - _recoilStarted);
        var progress = duration / eatlingBounceSettings.animationDuration;

        return progress >= 1f;
    }

    private void TriggerImpact()
    {
        _recoilStarted = Time.time;
        _moved = false;
    }

    private void SquishOnRigidbody()
    {
        var peak = 4f;
        var delta = Mathf.Clamp(Mathf.Abs(_rigidbody.velocity.y) / peak, 0f, 1f);
        var progress = _rigidbody.velocity.y > 0 ? delta : 0f;

        var wideSqueeze = 1f - (progress) * .3f;
        var longSqueeze = 1f + (progress) * .7f;
        _frameScale = new Vector3(
            _frameScale.x * wideSqueeze,
            _frameScale.y * longSqueeze,
            _frameScale.z * wideSqueeze
        );
    }

    private void HandleSquishing()
    {
        var duration = (Time.time - _recoilStarted);
        var progress = Mathf.Clamp(duration / eatlingBounceSettings.animationDuration, 0f, 1f);
        var yScale = eatlingBounceSettings.impactSqueeze.Evaluate(progress);
        _frameScale = new Vector3(
            _frameScale.x + _frameScale.x * Mathf.Clamp(1f - yScale, 0f, 1f),
            _frameScale.y * yScale,
            _frameScale.z + _frameScale.z * Mathf.Clamp(1f - yScale, 0f, 1f)
        );
    }

    private void MoveInRandomDirection()
    {
        var direction2d = Random.insideUnitCircle;
        var direction = new Vector3(direction2d.x, 0f, direction2d.y);
        _rigidbody.AddForce(direction.normalized * Random.Range(.3f, .5f), ForceMode.Impulse);
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