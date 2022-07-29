using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera _camera;

    void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        Vector3 v = _camera.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(_camera.transform.position - v);
        transform.Rotate(0, 180, 0);
    }
}