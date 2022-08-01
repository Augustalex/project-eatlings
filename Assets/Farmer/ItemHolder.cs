using System;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    // Public
    public GameObject pivot;
    public Transform itemParent;

    public event Action<ItemActivity> DidHoldItem;
    public event Action DidDropItem;

    public event Action<ItemActivity> UsedItem;

    private Action _doWhenAnimationDone = null;

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
    private bool _hideTarget;

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
        var waterCan = _itemGO.GetComponent<WateringCan>();

        if (waterCan)
        {
            DidHoldItem?.Invoke(ItemActivity.Watering);
        }
        else
        {
            DidHoldItem?.Invoke(ItemActivity.MiscPickup);
        }
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
            if (_itemTargetSystem != null)
            {
                if (_hideTarget)
                {
                    _itemTargetSystem.NullTarget();
                }
                else
                {
                    _itemTargetSystem.TargetNext();
                }
            }
        }
    }

    public void Drop()
    {
        var item = _itemGO;

        var groundRay = new Ray(_itemGO.transform.position, Vector3.down);
        if (Physics.Raycast(groundRay, out var groundHit, 10f, LayerMask.NameToLayer("Ground")))
        {
            var newPosition = groundHit.point + Vector3.up;
            var rotation = _itemGO.transform.rotation.eulerAngles;
            ResetItemParent();
            TellItemItWasDropped();
            DidDropItem?.Invoke();
            ResetItemVariables();

            item.transform.position = newPosition;
            item.transform.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
        else
        {
            Debug.Log("How about, no?");
        }
    }

    private void ResetItemParent()
    {
        _itemGO.transform.SetParent(_itemPreviousParent);
    }

    private void TellItemItWasDropped()
    {
        var pickupable = _itemGO.GetComponent<Pickupable>();
        pickupable.Dropped();
    }

    private void ResetItemVariables()
    {
        _itemGO = null;
        _item = null;
        if (_itemTargetSystem) _itemTargetSystem.NullTarget();
        _itemTargetSystem = null;
    }

    public void TriggerAnimationDone()
    {
        Debug.Log("Now plant me!");
        _doWhenAnimationDone?.Invoke();
        _doWhenAnimationDone = null;
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
                _hideTarget = true;
                UsedItem?.Invoke(ItemActivity.Watering);

                var farmerMovement = GetComponentInParent<FarmerMovement>();
                farmerMovement.StopAndFreezeUntilUnfreeze();

                var current = farmerMovement.transform.position;
                var next = target.transform.position;
                var direction = (next - current).normalized;
                var angles = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                var newRotation = Quaternion.AngleAxis(angles, Vector3.up);
                var farmerRotation = GetComponentInParent<FarmerRotation>();
                farmerRotation.ForceRotationTarget(newRotation);

                _doWhenAnimationDone = () =>
                {
                    _hideTarget = false;
                    waterCan.Water(target);
                    farmerMovement.Unfreeze();
                    farmerRotation.NullForceTarget();
                };
            }
        }

        var eatling = _itemGO.GetComponent<EatlingBabyGrowth>();
        if (eatling)
        {
            var target = _itemTargetSystem.GetCurrentTarget();
            if (target)
            {
                UsedItem?.Invoke(ItemActivity.Plant);
                // Todo: Make it better :)
                GetComponentInParent<FarmerMovement>().StopAndFreezeUntilUnfreeze();
                _hideTarget = true;

                _doWhenAnimationDone = () =>
                {
                    var itemRigidbody = _itemGO.GetComponent<Rigidbody>();
                    Destroy(itemRigidbody);

                    ResetItemParent();
                    TellItemItWasDropped();
                    DidDropItem?.Invoke();
                    _hideTarget = false;
                    ResetItemVariables();

                    var farmTile = target.GetComponent<FarmTile>();
                    eatling.Plant(farmTile);

                    GetComponentInParent<FarmerMovement>().Unfreeze();
                };
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