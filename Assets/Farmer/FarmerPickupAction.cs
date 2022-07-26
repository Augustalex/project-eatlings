using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FarmerItemSelector))]
[RequireComponent(typeof(FarmerState))]
public class FarmerPickupAction : MonoBehaviour
{
    private FarmerItemSelector _itemSelector;
    private ItemHolder _itemHolder;
    private FarmerState _farmerState;

    // Public
    public void PickupItem()
    {
        if (_itemHolder.HoldingItem())
        {
            _itemHolder.Drop();
        }
        else if (_farmerState.CanHoldItem() && _itemSelector.HasSelectedItem())
        {
            var item = _itemSelector.SelectedItem();
            _itemHolder.Pickup(item);
        }
    }

    // Private
    void Awake()
    {
        _itemSelector = GetComponent<FarmerItemSelector>();
        _itemHolder = GetComponentInChildren<ItemHolder>();
        _farmerState = GetComponentInChildren<FarmerState>();
    }
}