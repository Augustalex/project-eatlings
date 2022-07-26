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
        // transform.position = _originalPosition;
    }

    // Private

    void Awake()
    {
        _originalPosition = transform.position;
    }

    void Update()
    {
        return;
        
        if (_highlighted)
        {
            transform.position = _originalPosition + Vector3.up * .5f;
        }
        else
        {
            _originalPosition = transform.position;
        }
    }
}