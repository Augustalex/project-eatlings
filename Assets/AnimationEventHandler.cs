using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHandler : MonoBehaviour
{
    public UnityEvent onPerformed;

    public void Trigger(float e)
    {
        onPerformed.Invoke();
    }
}
