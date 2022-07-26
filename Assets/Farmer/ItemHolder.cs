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
        var pickupable = _item.GetComponent<Pickupable>();
        pickupable.Dropped();

        var pivotTransform = pivot.transform;
        var newPosition = pivotTransform.position + pivotTransform.forward.normalized * 1.75f + Vector3.up;
        var gridAlignedPosition = FarmGridUtils.GridAlign(newPosition);

        var itemRigidbody = _item.GetComponent<Rigidbody>();
        itemRigidbody.MovePosition(gridAlignedPosition);

        var eatlingRoot = _item.GetComponent<EatlingBabyGrowth>();
        if (eatlingRoot)
        {
            eatlingRoot.Plant();
        }
        else
        {
            itemRigidbody.AddForce(Vector3.up * .1f, ForceMode.Impulse);
        }

        DidDropItem?.Invoke();

        _item = null;
    }
}