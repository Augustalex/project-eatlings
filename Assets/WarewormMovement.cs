using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WarewormMovement : MonoBehaviour
{
    public GameObject billboard;

    private Transform _target;
    private Rigidbody _rigidbody;
    private Vector3 _originalScale;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _originalScale = billboard.transform.localScale;
    }

    void Update()
    {
        if (_target == null) FindTarget();
        else
        {
            var direction = (_target.position - transform.position).normalized;
            Debug.Log("MOVE! " + _target.gameObject.name);

            var mode = Mathf.RoundToInt(Time.time * 4f) % 2 == 0;
            _rigidbody.AddForce(direction * (mode ? 100f : 600f) * Time.deltaTime, ForceMode.Force);

            var angles = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var body = billboard.transform;
            body.rotation = Quaternion.Lerp(body.rotation, Quaternion.Euler(0f, angles, 0f), .1f);
        }

        //
        // var currentScale = billboard.transform.localScale;
        // var inverted = (Mathf.Max(_rigidbody.velocity.x, _rigidbody.velocity.z) >= 0 ? -1f : 1f);
        // Debug.Log("INVERTD: " + inverted + ", VEL: " + _rigidbody.velocity);
        // billboard.transform.localScale = new Vector3(
        //     _originalScale.x * inverted,
        //     currentScale.y, currentScale.z);
    }

    private void FindTarget()
    {
        var farmers = FindObjectsOfType<FarmerMovement>();
        if (farmers.Length == 0) return;

        var farmer = farmers.OrderBy(_ => Random.value).First();
        _target = farmer.transform;
    }
}