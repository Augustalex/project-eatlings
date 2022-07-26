using System;
using UnityEngine;

public class Highlightable : MonoBehaviour
{
    // Public

    public GameObject highlighter;
    public bool updatePosition = true;

    // Private 

    private Vector3 _originalPosition;

    // Public

    private void Awake()
    {
        highlighter.SetActive(
            false); // Always disable highlighter on first frame, no matter how many highlighters are connected to it.
    }

    public void Highlight()
    {
        highlighter.SetActive(true);
    }

    public void StopHighlight()
    {
        highlighter.SetActive(false);
    }

    private void Update()
    {
        if (updatePosition)
        {
            highlighter.transform.position = transform.position;
        }
    }
}