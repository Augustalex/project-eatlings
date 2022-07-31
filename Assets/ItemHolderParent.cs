using System;
using Unity.Mathematics;
using UnityEngine;

public class ItemHolderParent : MonoBehaviour
{
    // Public
    [SerializeField] private Animator animator;
    [SerializeField] private ItemHolder itemHolder;


    private float _layerWeight;
    private float _layerWeightAnim;
    private static readonly int Planting = Animator.StringToHash("Planting");
    private FarmerMovement _movement;

    private void Awake()
    {
        _movement = GetComponent<FarmerMovement>();
    }

    private void OnEnable()
    {
        itemHolder.DidHoldItem += ItemHolderOnDidHoldItem;
        itemHolder.DidDropItem += ItemHolderOnDidDropItem;
        itemHolder.DidUseItem += ItemHolderOnDidUseItem;
    }

    private void ItemHolderOnDidUseItem(ItemHolder.ItemActivity activity)
    {
        if (activity == ItemHolder.ItemActivity.Plant)
        {
            _layerWeight = 0;
            animator.SetTrigger(Planting);
        }
        else
        {
            Debug.Log("No animation for activity: " + activity);
        }
    }

    private void OnDisable()
    {
        itemHolder.DidHoldItem -= ItemHolderOnDidHoldItem;
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
