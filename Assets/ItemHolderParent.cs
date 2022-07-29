using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ItemHolderParent : MonoBehaviour
{
    // Public
    [SerializeField] private Animator animator;
    [SerializeField] private ItemHolder itemHolder;


    private float _layerWeight;
    private float _layerWeightAnim;
    
    private void OnEnable()
    {
        itemHolder.DidHoldItem += ItemHolderOnDidHoldItem;
        itemHolder.DidDropItem += ItemHolderOnDidDropItem;
    }

    private void OnDisable()
    {
        itemHolder.DidHoldItem -= ItemHolderOnDidHoldItem;
        itemHolder.DidDropItem -= ItemHolderOnDidDropItem;
    }

    private void ItemHolderOnDidDropItem()
    {
        _layerWeight = 0;
    }

    private void ItemHolderOnDidHoldItem()
    {
        _layerWeight = 1;
    }

    private void Update()
    {
        // TODO: optimize
        // if (_layerWeightAnim <= 0 || _layerWeightAnim >= 1)
        // {
        //     
        // }

        _layerWeightAnim = math.lerp(_layerWeightAnim, _layerWeight, 0.2f);
        
        animator.SetLayerWeight(1, _layerWeightAnim);
    }
}
