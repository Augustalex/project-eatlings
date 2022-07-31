using UnityEngine;

public class FarmerGravity : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _grounded;
    private Vector3 _diff;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Debug.Log("GROUNDED: " + _grounded);
        if (!_grounded)
        {
            _rigidbody.AddForce(Vector3.down * 10000f * Time.deltaTime, ForceMode.Acceleration);
        }

        return;
        // var ground = Physics.OverlapBox(transform.position, new Vector3(.1f, .25f, .1f))
        //     .Where(h => h.gameObject.layer == LayerMask.NameToLayer("Ground"))
        //     .OrderBy(g => Vector3.Distance(transform.position, g.ContactPoint)).FirstOrDefault();
        //
        // if (ground != null)
        // {
        //     var diff = transform.position.y - ground.transform.position.y;
        //     if (diff > .1f)
        //     {
        //         _rigidbody.AddForce(Vector3.down * diff * Time.deltaTime * 2000f, ForceMode.Acceleration);
        //     }
        //
        //     Debug.Log("DIFF: " + diff);
        // }
        // else
        // {
        //     _rigidbody.AddForce(Vector3.down * 2000f * Time.deltaTime, ForceMode.Acceleration);
        // }
        //
        // Debug.Log("GROUND: " + ground);
        // var ray = new Ray(transform.position + Vector3.up * 2f, Vector3.down);
        // Debug.DrawRay(transform.position + Vector3.up * .5f, Vector3.down, Color.red);
        // Debug.DrawRay(transform.position + Vector3.up * .5f + Vector3.down * .7f + Vector3.left * .05f, Vector3.down,
        //     Color.blue);
        // if (Physics.Raycast(ray, out var hit, 2.5f, LayerMask.NameToLayer("Ground")))
        // {
        //     Debug.Log("GROUND! " + hit.collider);
        // }
        // else
        // {
        //     Debug.Log("DOWN! " + hit.collider);
        //     // _rigidbody.AddForce(Vector3.down * 10000f * Time.deltaTime, ForceMode.Acceleration);
        // }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _grounded = false;
            _diff = transform.position - other.contacts[0].point;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _grounded = true;
        }
    }
}