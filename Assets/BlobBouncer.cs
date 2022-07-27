using UnityEngine;

public class BlobBouncer : MonoBehaviour
{
    // Public
    public float minHeight = 0f;
    public float maxHeight = 1f;
    public float bounceLength = .75f;

    // Private
    private const float NormalSquishHeight = 1f;
    private int _direction = 1;
    private float _startAt;
    private Pickupable _pickupable;
    private EatlingBabyGrowth _eatlingBabyGrowth;
    private bool _bouncing;
    private float _startedSquish;
    private float _squishState;
    private int _squishIndex;
    private readonly float[] _squishSequence = new[] {.2f, 1.5f, .5f, 1f, .8f, 1f};
    private readonly float[] _squishSequenceLengths = new[] {.2f, .15f, .1f, .05f, .02f, .01f};
    private Vector3 _originalScale;
    private Rigidbody _rigidbody;

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
        if (_pickupable.IsPickedUp() || _eatlingBabyGrowth.IsPlanted())
        {
            if (_bouncing)
            {
                transform.localScale = Vector3.one;
                _bouncing = false;
            }

            return;
        }

        _bouncing = true;
        if (BounceProgress() >= 1f) SwitchDirection();

        var currentPosition = transform.position;
        var newPosition = new Vector3(
            currentPosition.x,
            Mathf.Lerp(_direction < 0 ? maxHeight : minHeight, _direction < 0 ? minHeight : maxHeight,
                BounceProgress()),
            currentPosition.z
        );
        transform.position = newPosition;

        HandleSquishing();
    }

    private void HandleSquishing()
    {
        if (_direction < 0)
        {
            var progress = (Time.time - _startAt) / BounceLength();
            Debug.Log(progress);
            if (progress > .25f && (_squishIndex < 0))
            {
                StartSquish();
            }
        }
        else
        {
            _squishIndex = -1;
        }

        UpdateSquish();
    }

    private float BounceProgress()
    {
        var progress = (Time.time - _startAt) / BounceLength();
        return Mathf.Clamp(_direction < 0 ? EaseOutBounce(progress) : 1 - EaseInQuart(1 - progress), 0f, 1f);
    }

    private float EaseInQuart(float x)
    {
        return x * x * x * x;
    }

    private void StartSquish()
    {
        _startedSquish = Time.time;
        _squishState = 1f;
        _squishIndex = 0;
    }

    private void UpdateSquish()
    {
        if (_squishIndex >= _squishSequence.Length || _squishIndex < 0)
        {
            _squishState = 1f;
        }
        else
        {
            var squishTarget = _squishSequence[_squishIndex];
            var squishDuration = _squishSequenceLengths[_squishIndex];

            var time = Time.time - _startedSquish;
            if (time > squishDuration)
            {
                _squishIndex += 1;
                _squishState = squishTarget;
                _startedSquish = Time.time;
            }
            else
            {
                var previousTarget = _squishIndex == 0 ? NormalSquishHeight : _squishSequence[_squishIndex - 1];
                _squishState = Mathf.Lerp(previousTarget, squishTarget, time / squishDuration);
            }
        }

        transform.localScale = new Vector3(
            _originalScale.x + _originalScale.x * Mathf.Clamp(1f - _squishState, 0f, 1f),
            NormalSquishHeight * _squishState,
            _originalScale.z + _originalScale.z * Mathf.Clamp(1f - _squishState, 0f, 1f)
        );
    }

    private float EaseOutBounce(float x)
    {
        float n1 = 7.5625f;
        float d1 = 2.75f;

        if (x < 1 / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2 / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5 / d1)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }

    private float BounceLength()
    {
        return bounceLength;
    }

    private void SwitchDirection()
    {
        _direction *= -1;
        _startAt = Time.time;

        if (_direction > 0)
        {
            MoveInRandomDirection();
        }
    }

    private void MoveInRandomDirection()
    {
        var direction2d = Random.insideUnitCircle;
        var direction = new Vector3(direction2d.x, 0f, direction2d.y);
        _rigidbody.AddForce(direction.normalized * 10f, ForceMode.Impulse);
    }
}