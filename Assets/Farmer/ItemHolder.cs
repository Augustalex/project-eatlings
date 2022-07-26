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
            _item.transform.position = pivot.transform.position + pivot.transform.forward * .7f;
        }
    }

    public void Drop()
    {
        _item.GetComponent<Pickupable>().Dropped();

        var pivotTransform = pivot.transform;
        var newPosition = pivotTransform.position + pivotTransform.forward.normalized * 1f + Vector3.up;
        var gridAlignedPosition = FarmGridUtils.GridAlign(newPosition);
        Debug.Log("DROP POSITION: " + newPosition + ", GRIDPOS: " + gridAlignedPosition);
        // _item.transform.position = gridAlignedPosition;
        _item.GetComponent<Rigidbody>().MovePosition(gridAlignedPosition);

        DidDropItem?.Invoke();

        _item = null;
    }
}