using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    // Public
    public GameObject pivot;

    public event Action DidHoldItem;
    public event Action DidDropItem;

    // Private
    private GameObject _item;

    // Public

    public void Pickup(Pickupable item)
    {
        _item = item.gameObject;

        item.PickedUp();
        
        DidHoldItem?.Invoke();
    }

    public bool HoldingItem()
    {
        return _item != null;
    }

    // Private

    void Update()
    {
        if (HoldingItem())
        {
            _item.transform.position = pivot.transform.position;
        }
    }
}