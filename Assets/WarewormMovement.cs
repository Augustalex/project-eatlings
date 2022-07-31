using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WarewormMovement : MonoBehaviour
{
    private Transform _target;
    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (_target == null) FindTarget();
        else
        {
            var direction = (_target.position - transform.position).normalized;
            _rigidbody.AddForce(direction * 10f * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    private void FindTarget()
    {
        var farmers = FindObjectsOfType<FarmerMovement>();
        if (farmers.Length == 0) return;
        
        var farmer = farmers.OrderBy(_ => Random.value).First();
        _target = farmer.transform;
    }
}