using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerStateUpdater : MonoBehaviour
{
    // Private
    private ItemHolder _itemHolder;
    private FarmerState _farmerState;

    // Private

    void Awake()
    {
        _farmerState = GetComponent<FarmerState>();
        _itemHolder = GetComponentInChildren<ItemHolder>();

        _itemHolder.DidHoldItem += (ItemHolder.ItemActivity activity) => _farmerState.SetState(FarmerState.FarmerStates.HoldingItem);
        _itemHolder.DidDropItem += () =>
        {
            if (_farmerState.currentState == FarmerState.FarmerStates.HoldingItem)
            {
                _farmerState.SetState(FarmerState.FarmerStates.Idle);
            }
        };
    }
}
