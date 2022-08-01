using UnityEngine;

public class FarmerGravity : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _grounded;
    private Vector3 _diff;
    private bool _jumped;
    private float _jumpedAt;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // _grounded = Physics.Raycast(_rigidbody.position, Vector3.down, .5f);
        // Debug.Log("GROUNDED: " + _grounded);
        //
        //
        // if (!_grounded)
        // {
        //     var timeSinceLastJump = Time.time - _jumpedAt;
        //     if (!_jumped && timeSinceLastJump > 2f)
        //     {
        //         _jumped = true;
        //         _jumpedAt = Time.time;
        //         _rigidbody.AddForce(Vector3.up * 1000f, ForceMode.Impulse);
        //         Debug.Log("JUMP!");
        //     }
        //
        //     _rigidbody.AddForce(Vector3.down * 10f * Time.deltaTime, ForceMode.Acceleration);
        // }
        // else
        // {
        //     _jumped = false;
        // }

        // var supergrounded = Physics.Raycast(_rigidbody.position, -_rigidbody.transform.up, .3f);
        // if (!supergrounded)
        // {
        //     _rigidbody.AddForce(Vector3.down * 100f * Time.deltaTime, ForceMode.Acceleration);
        // }
    }
}