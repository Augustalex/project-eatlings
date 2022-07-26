using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeAnimator : MonoBehaviour
{
    public GameObject leftEye;
    public GameObject leftPupil;
    public GameObject rightEye;
    public GameObject rightPupil;

    private float _blinkCooldown;
    private float _blinkStarted;
    private GameObject[] _pupils;
    private float _moveCooldown;
    private Vector2 _moveTo;
    private float _moveToProgress;
    private Vector3 _moveStart;
    private float _stayCooldown;
    private float _openStarted;
    private GameObject[] _eyes;

    private const float PupilSpeed = 8f;
    private const float BlinkSpeed = 22f;

    private static readonly Vector3 EyeClosed = new Vector3(1, 0, 1);
    private static readonly Vector3 EyeOpen = new Vector3(1, 1, 1);

    void Start()
    {
        _pupils = new[]
        {
            leftPupil, rightPupil
        };
        _eyes = new[]
        {
            leftEye, rightEye
        };
    }

    void Update()
    {
        if (_blinkStarted > 0)
        {
            var progress = 1f - _blinkStarted;

            foreach (var eye in _eyes)
            {
                eye.transform.localScale = Vector3.Lerp(EyeOpen, EyeClosed, progress);
            }

            var pupilProgress = progress < .3f ? 0f : (progress - .3f) / .7f;
            foreach (var pupil in _pupils)
            {
                pupil.transform.localScale = Vector3.Lerp(EyeOpen, EyeClosed, pupilProgress);
            }

            _blinkStarted -= Time.deltaTime * BlinkSpeed;

            if (_blinkStarted <= 0)
            {
                _stayCooldown = Random.Range(.05f, .2f);
            }
        }
        else if (_stayCooldown > 0)
        {
            _stayCooldown -= Time.deltaTime;

            if (_stayCooldown <= 0)
            {
                _openStarted = 1f;
            }
        }
        else if (_openStarted > 0)
        {
            _openStarted -= Time.deltaTime * BlinkSpeed;
            var progress = 1f - _openStarted;

            foreach (var eye in _eyes)
            {
                eye.transform.localScale = Vector3.Lerp(EyeClosed, EyeOpen, progress);
            }

            var pupilProgress = progress < .3f ? 0f : (progress - .3f) / .7f;
            foreach (var pupil in _pupils)
            {
                pupil.transform.localScale = Vector3.Lerp(EyeClosed, EyeOpen, pupilProgress);
            }
        }
        else if (_blinkCooldown > 0)
        {
            _blinkCooldown -= Time.deltaTime;

            UpdatePupils();
        }
        else
        {
            Blink();
        }
    }

    private void UpdatePupils()
    {
        if (_moveToProgress > 0)
        {
            var progress = 1f - _moveToProgress;

            var target = new Vector3(
                _moveTo.x,
                _moveTo.y,
                _moveStart.z
            );

            foreach (var pupil in _pupils)
            {
                pupil.transform.localPosition = Vector3.Lerp(_moveStart, target, progress);
            }

            _moveToProgress -= Time.deltaTime * PupilSpeed;
        }
        else if (_moveCooldown > 0)
        {
            _moveCooldown -= Time.deltaTime;
        }
        else
        {
            MovePupils();
        }
    }

    private void MovePupils()
    {
        _moveCooldown = Random.Range(.5f, 2.5f);

        _moveStart = _pupils[0].transform.localPosition;
        _moveTo = Random.insideUnitCircle.normalized * .024f;
        _moveToProgress = 1f;
    }

    public void Blink()
    {
        _blinkCooldown = Random.Range(2, 5);

        _blinkStarted = 1f;
    }
}