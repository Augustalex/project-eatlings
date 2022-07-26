using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FarmerItemSelector))]
public class FarmerPickupAction : MonoBehaviour
{
    private FarmerItemSelector _itemSelector;
    private ItemHolder _itemHolder;

    // Public
    public void PickupItem()
    {
        if (!_itemSelector.HasSelectedItem()) return;

        var item = _itemSelector.SelectedItem();
        _itemHolder.Pickup(item);
    }

    // Private
    void Awake()
    {
        _itemSelector = GetComponent<FarmerItemSelector>();
        _itemHolder = GetComponentInChildren<ItemHolder>();
    }
}