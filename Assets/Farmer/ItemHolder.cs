using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    // Public
    public GameObject pivot;
    public Transform itemParent;

    public event Action DidHoldItem;
    public event Action DidDropItem;

    public event Action<ItemActivity> UsedItem;

    public enum ItemGrabMode
    {
    }

    public enum ItemActivity
    {
        Watering,
        Plant,
        MiscPickup
    }

    // Private
    private GameObject _itemGO;
    private Pickupable _item;
    private ItemUseTargetSystem _itemTargetSystem;
    private Transform _itemPreviousParent;

    // Public

    public void Pickup(Pickupable item)
    {
        _itemGO = item.gameObject;
        _item = item;
        _itemTargetSystem = item.GetComponent<ItemUseTargetSystem>();

        item.PickedUp();

        _itemPreviousParent = _itemGO.transform.parent;

        _itemGO.transform.SetParent(itemParent);
        _itemGO.transform.rotation = itemParent.transform.rotation;
        _itemGO.transform.position = itemParent.transform.position;

        // _itemGO.transform.position = -diff;


        // * Quaternion.Euler(-90f, 0f, 90f);
        // _itemGO.transform.position += new Vector3(0f, -0.169f, -0.169f);
        // _itemGO.transform.rotation = Quaternion.Euler(0f, itemParent.transform.rotation.eulerAngles.y, 0f);
        // _itemGO.transform.rotation = Quaternion.Euler(0f, pivot.transform.rotation.eulerAngles.y, 0f);
        // _itemGO.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        // _itemGO.transform.lossyScale = originalScale;

        DidHoldItem?.Invoke();
    }

    public bool HoldingItem()
    {
        return _itemGO != null;
    }

    // Private

    void Update()
    {
        if (HoldingItem())
        {
            if (_item.gridAligned)
            {
                // Todo: Replace with item target system
                // _itemGO.transform.position = NearestApplicablePosition();
            }
            else
            {
                // TODO: Verify new parent system then remove this code
                // _itemGO.transform.position = pivot.transform.position;
                // _itemGO.transform.rotation = Quaternion.Euler(0f, pivot.transform.rotation.eulerAngles.y, 0f);
            }

            if (_itemTargetSystem != null)
            {
                _itemTargetSystem.TargetNext();
            }
        }
    }

    public void Drop()
    {
        var eatlingRoot = _itemGO.GetComponent<EatlingBabyGrowth>();
        if (eatlingRoot)
        {
            // var itemRigidbody = _itemGO.GetComponent<Rigidbody>();
            // Destroy(itemRigidbody);
            var farmTile = _itemTargetSystem.GetCurrentTarget().GetComponent<FarmTile>();
            FinishDropItem();
            eatlingRoot.Plant(farmTile);
        }
        else
        {
            var item = _itemGO;
            var newPosition = _itemGO.transform.position + _itemGO.transform.forward;
            FinishDropItem();
            item.transform.position = newPosition;
        }
    }

    private void FinishDropItem()
    {
        _itemGO.transform.SetParent(_itemPreviousParent);

        var pickupable = _itemGO.GetComponent<Pickupable>();
        pickupable.Dropped();

        DidDropItem?.Invoke();

        _itemGO = null;
        _item = null;
        _itemPreviousParent = null;

        if (_itemTargetSystem) _itemTargetSystem.NullTarget();
        _itemTargetSystem = null;
    }
    
    
    public void FinishAnimatingPlanting() {
        Debug.Log("Now plant me!");
    }

    public void Use()
    {
        // TODO: Can we use some interface or something here to make this more Polymorphic? :) Or maybe simple is better?
        var waterCan = _itemGO.GetComponent<WateringCan>();
        if (waterCan)
        {
            var target = _itemTargetSystem.GetCurrentTarget();
            if (target)
            {
                waterCan.Water(target);
                UsedItem?.Invoke(ItemActivity.Watering);

                var farmerMovement = GetComponentInParent<FarmerMovement>();
                farmerMovement.StopAndFreeze();

                var current = farmerMovement.transform.position;
                var currentFlat = new Vector2(current.x, current.z);
                var next = target.transform.position;
                var nextFlat = new Vector2(next.x, next.z);

                var direction = (nextFlat - currentFlat).normalized;
                var angles = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

                var newRotation = Quaternion.AngleAxis(angles, Vector3.up);
                farmerMovement.GetComponent<FarmerRotation>().ForceRotation(newRotation);
            }
        }

        var eatling = _itemGO.GetComponent<EatlingBabyGrowth>();
        if (eatling)
        {
            var target = _itemTargetSystem.GetCurrentTarget();
            if (target)
            {
                var itemRigidbody = _itemGO.GetComponent<Rigidbody>();
                Destroy(itemRigidbody);

                FinishDropItem();

                var farmTile = target.GetComponent<FarmTile>();
                eatling.Plant(farmTile);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var pond = other.GetComponent<Pond>();
        if (pond)
        {
            pond.TryApplyItem(_itemGO);
        }
    }
}