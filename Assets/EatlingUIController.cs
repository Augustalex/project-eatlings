using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatlingUIController : MonoBehaviour
{
    // Public
    public EatlingBabyGrowth baby;
    public GameObject needWater;

    public GameObject uiRoot;

    // Private
    private EatlingModeController _root;

    private void Awake()
    {
        _root = GetComponentInParent<EatlingModeController>();

        // Disable UI elements initially
        needWater.SetActive(false);
    }

    private void Update()
    {
        if (_root.InBabyMode())
        {
            uiRoot.transform.position = _root.baby.transform.position;
        }
    }

    private void Start()
    {
        baby.NeedsWater += ShowNeedWater;
        baby.GotWater += HideNeedWater;
        baby.Died += HideNeedWater;
    }

    private void HideNeedWater()
    {
        needWater.SetActive(false);
    }

    private void ShowNeedWater()
    {
        needWater.SetActive(true);
    }
}