using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringCan : MonoBehaviour
{
    // Public

    public float maxWater = 100f;

    // Private
    
    private float _waterLevel;

    // Public

    public void Water(EatlingBabyGrowth babyGrowth)
    {
        babyGrowth.Water(TakeWater());
    }

    private float TakeWater()
    {
        var take = Mathf.Min(_waterLevel, 10f);
        _waterLevel -= take;

        return take;
    }

    // Private
    
    private void OnTriggerEnter(Collider other)
    {
        var pond = other.GetComponent<Pond>();
        if (pond)
        {
            Refill();
        }
    }

    private void Refill()
    {
        _waterLevel = maxWater;
    }
}
