using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WarewormMovement : MonoBehaviour
{
    public AnimationCurve heightCurve;
    public AnimationCurve stretchCurve;
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
        var scale = .5f;

        var x = (Time.time * scale) % 1f;
        billboard.transform.localScale = new Vector3(
            _originalScale.x,
            heightCurve.Evaluate(x),
            stretchCurve.Evaluate(x)
        );

        if (_target == null) FindTarget();
        else
        {
            var direction = (_target.position - transform.position).normalized;
            var angles = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var body = transform;
            body.rotation = Quaternion.Lerp(body.rotation, Quaternion.Euler(0f, angles, 0f), .1f);

            var mode = x > .65f;
            Debug.Log("X: " + x);
            _rigidbody.AddForce(_rigidbody.transform.forward * (mode ? 260f : 0f) * Time.deltaTime, ForceMode.Force);
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