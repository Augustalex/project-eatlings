using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlightable : MonoBehaviour
{
    // Private 

    private bool _highlighted;
    private Vector3 _originalPosition;

    // Public

    public void Highlight()
    {
        _highlighted = true;
    }

    public void StopHighlight()
    {
        _highlighted = false;
    }

    // Private

    void Awake()
    {
        _originalPosition = transform.position;
    }

    void Update()
    {
        if (_highlighted)
        {
            transform.position = _originalPosition + Vector3.up * .5f;
        }
        else
        {
            transform.position = _originalPosition;
        }
    }
}