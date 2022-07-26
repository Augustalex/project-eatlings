using System;
using UnityEngine;

public class BlobBouncer : MonoBehaviour
{
    // Public
    public float minHeight = 0f;
    public float maxHeight = 1f;

    public float bounceLength = .75f;

    public float squishFactor = .2f;
    public float normalSquishHeight = 1f;
    public float minSquishHeight = .3f;

    // Private
    private int _direction = 1;
    private float _startAt;
    private Pickupable _pickupable;
    private EatlingBabyGrowth _eatlingBabyGrowth;
    private bool _bouncing;

    // Private

    private void Awake()
    {
        _pickupable = GetComponent<Pickupable>();
        _eatlingBabyGrowth = GetComponent<EatlingBabyGrowth>();
    }

    void Update()
    {
        return;
        
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

        var currentScale = transform.localScale;
        var newScale = new Vector3(
            currentScale.x,
            HeightScale(),
            currentScale.z
        );
        transform.localScale = newScale;
    }

    private float HeightScale()
    {
        var progress = (transform.position.y - minHeight) / maxHeight;

        return Mathf.Max(EaseOutBounce(progress) * normalSquishHeight, minSquishHeight);
        // if (_direction < 0)
        // {
        //     var squishProgressNonClamped = (progress - (progress - squishFactor));
        //     if (squishProgressNonClamped >= 0f)
        //     {
        //         var progressOfSquishUp = Mathf.Clamp(squishProgressNonClamped / squishFactor, 0f, 1f);
        //
        //         return (1 - SquishEase(1 - progressOfSquishUp)) * normalSquishHeight;
        //     }
        //     else
        //     {
        //         return minSquishHeight;
        //     }
        // }
        // else
        // {
        //     var progressOfSquishUp = Mathf.Clamp(progress / squishFactor, 0f, 1f);
        // }
    }

    private float SquishEase(float x)
    {
        return 1f;
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
    }
}